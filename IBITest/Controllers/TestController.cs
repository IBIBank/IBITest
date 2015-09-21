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

            List<TransactionStatementViewModel> list = newObj.GetDetailedTransactions(2,new DateTime (DateTime.Today.Year-1, DateTime.Today.Month, DateTime.Today.Day), new DateTime (DateTime.Today.Year+1, DateTime.Today.Month, DateTime.Today.Day));

            foreach(var v in list)
                MessageBox.Show(v.transactionID.ToString());
            
            return View(new TestModel());
        }

        [HttpPost]
        public ActionResult Index(TestModel model)
        {
            

            return View(model);
        }



    }
}
