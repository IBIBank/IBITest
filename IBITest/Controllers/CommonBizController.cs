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
        static List<Int64> BranchCode;
        public ActionResult Index()
        {
            CommonDAL syncobj = new CommonDAL();
            syncobj.Sync();
            BankCountryModel objbankcountrymodel = new BankCountryModel();
            objbankcountrymodel.CityModel = new List<City>();
            objbankcountrymodel.BranchModel = new List<BranchL>();
            objbankcountrymodel.CityModel = GetAllCities();

            return View(objbankcountrymodel);
        }

        [HttpPost]
        public ActionResult GetBranchNamebyCity(int cityid)
        {
            List<BranchL> objbranch = new List<BranchL>();
            objbranch = GetAllBranch(cityid);
           // SelectList obgbranch = new SelectList(objbranch, "Id", "BranchName", 0);
            return Json(objbranch);
        }

        [HttpPost]
        public ActionResult GetBranchDetails()
        {
            CommonDAL cd = new CommonDAL();
            BranchDetailsViewModel objbranch = new BranchDetailsViewModel();
            objbranch = cd.GetBranchDetails(BranchCode[0]);
            return Json(objbranch);
        }

        public List<City> GetAllCities()
        {
            List<City> objcity = new List<City>();
            CommonDAL cd = new CommonDAL();
            List<string> listcity = cd.GetCityList();
            int id = 1;

            foreach (string v in listcity)
            {
                objcity.Add(new City { Id = id++, CityName = v });
            }

            return objcity;
        }

        public List<BranchL> GetAllBranch(int cityid)
        {
            List<BranchL> objbranch = new List<BranchL>();
            BranchCode = new List<Int64>();
            CommonDAL cd = new CommonDAL();
            List<string> listcity = cd.GetCityList();

            string city = listcity[cityid-1];

            List<BranchMiniViewModel> bmvm = new List<BranchMiniViewModel>();
            int id =1;
            bmvm = cd.GetBranchesInCity(city);

            foreach (BranchMiniViewModel b in bmvm)
            {
                objbranch.Add(new BranchL {Id = id++, BranchName = b.BranchName });
                BranchCode.Add(b.BranchCode);
            }

            return objbranch;

        }

         
        

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginCredentials model)
        {
            CommonDAL obj = new CommonDAL();
            string role = obj.CheckRole(model.UserName, model.Password);

            UserRole ur = new UserRole { UserID=model.UserName, Role = role};
           

            if (role.Equals("Invalid"))
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
                //return RedirectToAction("Login");
            }

            Session["User"] = ur;

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
