using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
namespace IBITest.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult DashBoard()
        {
            return View();
        }
        public ActionResult FinishRegistration(string token)
        {
            MessageBox.Show(token);
            return View();
        }

    }
}
