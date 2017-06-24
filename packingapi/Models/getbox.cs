using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace packingapi
{
    /// <summary>
    /// 商品的详细信息
    /// </summary>
    public class getbox
    {
        private double _width;
        private double _lenght;
        private double _height;
        private string _name;
        private string _fnumber;
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string fnumber
        {
            get { return _fnumber; }
            set { _fnumber = value; }
        }
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
        
    }
}