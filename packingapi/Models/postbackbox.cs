using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Media.Media3D;

namespace packingapi.Controllers
{
    /// <summary>
    /// 返回信息
    /// </summary> 
   
    public class postbackbox
    {
        private string _number;
        private string _name;
        private int _qty;
        private string _group;
        /// <summary>
        /// 编码
        /// </summary>
        public string number
        {
            get { return _number; }
            set { _number = value; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int qty
        {
            get { return _qty; }
            set { _qty = value; }
        }
        /// <summary>
        /// 分组
        /// </summary>
        public string group
        {
            get { return _group; }
            set { _group = value; }
        }

        public object Clone()
        {
            BinaryFormatter formatter = new BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            object clonedObj = formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }
    }
}
