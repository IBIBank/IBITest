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

            MessageBox.Show(newObj.DoFundTransfer(new FundTransferViewModel { FromAccount = 2, ToAccount = 5, Amount = 4, remarks = "First FT", TransactionPassword = "nocheck" }).ToString());
            
            return View(new TestModel());
        }

        [HttpPost]
        public ActionResult Index(TestModel model)
        {
            

            return View(model);
        }



    }
}
