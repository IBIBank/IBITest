using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBITest.Models;
using IBITest.Layers.DAL;
using System.Windows.Forms;

namespace IBITest.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            CustomerDAL newObj = new CustomerDAL();

            List<FundTransferPayeeModel> accList = newObj.GetAllPayeeAccountByCustomerID(1);

            MessageBox.Show(newObj.ValidateTransactionPassword(04,"abhi123").ToString());

            return View(new TestModel());
        }

        [HttpPost]
        public ActionResult Index(TestModel model)
        {
            

            return View(model);
        }



    }
}
