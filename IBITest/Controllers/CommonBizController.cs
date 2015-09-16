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
            CommonDAL cd = new CommonDAL();

            cd.Sync();

            List<string> cityList = cd.GetCityList();
            
            //foreach (string v in cityList)
            //      MessageBox.Show(v);
            ViewBag.cityList = cityList;
            return View();
        }
        public JsonResult GetBranchForCity(string cityName)
        {
            //CommonDAL cd = new CommonDAL();
            MessageBox.Show("Hey Ajax method called me");
            //List<BranchMiniViewModel> bmvm = new List<BranchMiniViewModel>();
            List<string> branchList = new List<string>{"branch3","branch4"};
            //bmvm = cd.GetBranchesInCity(cityName);
            return Json(branchList);

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
