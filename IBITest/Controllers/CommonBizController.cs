using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBITest.Models;

namespace IBITest.Controllers
{
    public class CommonBizController : Controller
    {
        //
        // GET: /CommonBiz/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }
        //
        // GET: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginCredentials model)
        {
            string role = "admin";
            if (role.Equals("admin"))
            {
                return RedirectToAction("DashBoard", "Admin");
            }
            else if (role.Equals("banker"))
            {
                return RedirectToAction("DashBoard", "Banker");
            }
            else
            {
                return RedirectToAction("DashBoard", "Customer");
            }
            

        }

    }
}
