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
            CommonDAL obj = new CommonDAL();
            string role = obj.CheckRole(model.UserName, model.Password);
            //MessageBox.Show(role);
            //MessageBox.Show(role.Length.ToString());

            if (role.Equals("Invalid"))
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
                //return RedirectToAction("Login");
            }

            else if (role.Equals("admin"))
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
