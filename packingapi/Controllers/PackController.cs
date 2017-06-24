using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace packingapi.Controllers
{
    /// <summary>
    /// 装箱接口
    /// </summary>
   
    public class PackController : ApiController
    {
       
        /// <summary>
        /// boxlist
        /// </summary>
        public class boxes {
            /// <summary>
            /// boxes
            /// </summary>
            public List<box> box = new List<box>();
        }
        /// <summary>
        /// 箱子
        /// </summary>
        public class box
        {
            /// <summary>
            /// 商品编码
            /// </summary>
           public string fnumber { get; set; }
            /// <summary>
            /// 商品数量
            /// </summary>
           public int qty { get; set; }
        }
        /// <summary>
        /// 返修拆分信息
        /// </summary>
        public class postbackboxes
        {
            /// <summary>
            /// 返回拆分信息
            /// </summary>
            public List<postbackbox> postbackbox = new List<postbackbox>();
        }
        /// <summary>
        /// 商品详细信息
        /// </summary>
        public class getboxinfo
        {
            /// <summary>
            /// 商品详细信息
            /// </summary>
            public List<getbox> boxinfo = new List<getbox>();
        }
        /// <summary>
        /// 根据请求的商品编码和数量拆分订单
        /// </summary>
        ///<param name="box" >箱子数据</param> 
        /// <returns>返回拆分后的订单分组信息</returns>
        [System.Web.Http.HttpPost]
        public postbackboxes packing([FromBody]boxes box)
        {
            postbackboxes bxnomal = new postbackboxes();
            List<itembox> bxmix = new List<itembox>();
            List<itembox> packbox = new List<itembox>();
            SQLHelper con = new SQLHelper();
            string sql = "select Fname,Fnumber,WIDTH,LENGHT,HEIGHT,Target_quantity,FUNIT from XAY_MATERIEL";
            DataTable dt = con.sqldataset(sql);
            int count = 0;
            foreach (var q in box.box )
            {
                DataRow[] dr = dt.Select("Fnumber = '"+q.fnumber+"'");
                if (dr.Count() != 0)
                {

                    int tqty = Convert.ToInt32(dr[0]["Target_quantity"]);//物流订单起始数量
                    int unitqty = Convert.ToInt32(dr[0]["FUNIT"]);//整箱数量                    
                    if (q.qty < tqty)
                    {
                        int Integer = q.qty / unitqty;
                        int remainderqty = q.qty % unitqty;

                        for (int i = 0; i < Integer; i++)
                        {
                            bxnomal.postbackbox.Add(new postbackbox() { name = dr[0]["Fname"].ToString(), number = dr[0]["Fnumber"].ToString(), qty = unitqty, group = count.ToString() });
                            count++;

                        }
                        if (remainderqty != 0)
                        {
                            for (int j = 0; j < remainderqty; j++)
                            {
                                bxmix.Add(new itembox() { type = dr[0]["Fname"].ToString(), fnumber = dr[0]["Fnumber"].ToString(), width = Convert.ToDouble(dr[0]["WIDTH"]), lenght = Convert.ToDouble(dr[0]["LENGHT"]), height = Convert.ToDouble(dr[0]["HEIGHT"]), flag = 0, point = new System.Windows.Media.Media3D.Point3D(0, 0, 0) });//组合散件

                            }

                        }

                    }
                    else
                    {

                        bxnomal.postbackbox.Add(new postbackbox() { name = dr[0]["Fname"].ToString(), number = dr[0]["Fname"].ToString(), qty = q.qty, group = count.ToString() });
                        count++;
                    }
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("商品编码{0}不存在", q.fnumber)),
                        ReasonPhrase = "Fnumber Not Found"
                    };
                     throw new HttpResponseException(resp);
                }
              
                //bx.Add(new itembox() { width =Convert.ToInt32( dt.Rows[i].ItemArray[2]), height = Convert.ToInt32(dt.Rows[i].ItemArray[4]), flag = 0, point = new System.Windows.Media.Media3D.Point3D(0, 0, 0), lenght = Convert.ToInt32(dt.Rows[i].ItemArray[3]), type = dt.Rows[i].ItemArray[0].ToString() });

            }
            sql = "select FBOXNAME,WIDTH,LENGHT,HEIGHT from XAY_MATERIELBOX";
            DataTable boxdt = con.sqldataset(sql);
            for (int i=0;i<boxdt.Rows.Count;i++)
            {
                packbox.Add(new itembox() {type= boxdt.Rows[i]["FBOXNAME"].ToString(),width=Convert.ToDouble(boxdt.Rows[i]["WIDTH"]),lenght= Convert.ToDouble(boxdt.Rows[i]["LENGHT"]),height= Convert.ToDouble(boxdt.Rows[i]["HEIGHT"]) });
            }
            sa3dpacking pack = new sa3dpacking(); //组合退火装箱算法           
            //packing pack = new packing(); //装箱算法
            if (bxmix.Count!=0)
            { 
            pack.packing(bxmix, packbox);
            bxmix = pack.getbox();

            var newbox = bxmix.GroupBy(x => new {  x.inbox,x.type, x.fnumber }).Select(y => new { name=y.Key.type,fnumber=y.Key.fnumber, qty = y.Count(),inbox=y.Key.inbox});
            foreach (var q in newbox)
            {
                bxnomal.postbackbox.Add(new postbackbox  { name = q.name, number = q.fnumber, qty = q.qty,group=q.inbox } );
            }
            }
            return bxnomal;
        }
        /// <summary>
        /// 通过商品编号获取外箱大小
        /// </summary>
        /// <param name="Fnumber">商品编码</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public getboxinfo getallbox(string Fnumber)
        {
            getboxinfo b = new getboxinfo();            
            string sql = "select Fname,Fnumber,WIDTH,LENGHT,HEIGHT from XAY_MATERIEL";
            if (!string.IsNullOrEmpty(Fnumber))
                sql = string.Format("{0} where Fnumber='{1}'",sql,Fnumber);
            SQLHelper con = new SQLHelper();
            DataTable dt= con.sqldataset(sql);
            if (dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    b.boxinfo.Add(new getbox { fnumber = dt.Rows[i]["Fnumber"].ToString(), name = dt.Rows[i]["Fname"].ToString(), width = Convert.ToDouble(dt.Rows[i]["WIDTH"]), lenght = Convert.ToDouble(dt.Rows[i]["LENGHT"]), height = Convert.ToDouble(dt.Rows[i]["HEIGHT"]) });
                }
            }
            else
            {
               
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("商品编码{0}不存在", Fnumber)),
                        ReasonPhrase = "Fnumber Not Found"
                    };
                    throw new HttpResponseException(resp);
              
            }
            return b;
        }

    }
}