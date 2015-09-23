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
            BankerDAL banObj = new BankerDAL();

            List<TransactionStatementViewModel> list = newObj.GetDetailedTransactions(2,new DateTime (DateTime.Today.Year-1, DateTime.Today.Month, DateTime.Today.Day), new DateTime (DateTime.Today.Year+1, DateTime.Today.Month, DateTime.Today.Day));
            AccountDetailsViewModel customerList = new AccountDetailsViewModel();

            customerList = banObj.GetAccountDetails(1);

           // foreach(var v in customerList)
                MessageBox.Show(customerList.accountNumber);
                MessageBox.Show(customerList.cityName);
            
            return View(new TestModel());
        }

        [HttpPost]
        public ActionResult Index(TestModel model)
        {
            

            return View(model);
        }



    }
}
