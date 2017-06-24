using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Media.Media3D;

namespace packingapi
{
    [Serializable]
    public class itembox
    {
        
        private double _width;
        private double _lenght;
        private double _height;
        private double _flag;
        private string _type;
        private Point3D _point;
        private string _inbox;
        private string _fnumber;
        /// <summary>
        /// 宽度
        /// </summary>
        public double width
        {
            get { return _width; }
            set { _width = value; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public double lenght
        {
            get { return _lenght; }
            set { _lenght = value; }
        }
        /// <summary>
        /// 高度
        /// </summary>
        public double height
        {
            get { return _height; }
            set { _height = value; }
        }
        /// <summary>
        /// 装箱状态，0为未入箱
        /// </summary>
        public double flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        /// <summary>
        /// 箱子的种类
        /// </summary>
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 装箱坐标
        /// </summary>
        public Point3D point
        {
            get { return _point; }
            set { _point = value; }
        }
        /// <summary>
        /// 转入外箱名称
        /// </summary>
        public string inbox
        {
            get { return _inbox; }
            set { _inbox = value; }
        }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string fnumber
        {
            get { return _fnumber; }
            set { _fnumber = value; }
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
