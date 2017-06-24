using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using packingapi;
using packingapi.Controllers;

namespace packingapi.test.Controllers
{
    [TestClass]
    public class PackControllerTest
    {

       
        [TestMethod]
        public void Post()
        {
            // 排列
            PackController controller = new PackController();
            List<PackController.boxes> box = new List<PackController.boxes>();
            box.Add(new PackController.boxes { fnumber="mj201",qty=31 });
            box.Add(new PackController.boxes { fnumber = "KACM1002", qty = 96 });

            // 操作
            var result = controller.packing(box);

            // 断言
            Assert.IsNotNull(result);
           
        }
        [TestMethod]
        public void getallbox()
        {
            PackController controller = new PackController();

            var result = controller.getallbox();

            Assert.IsNotNull(result);
        }

       
    }
}
