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
        public ActionResult ValidateToken(string token)
        {
            bool IsValid = true;
            CustomerDAL cd = new CustomerDAL();
            
            IsValid = cd.ValidateToken(token);

            if (IsValid)
            {
                return RedirectToAction("FinishRegistration", "Customer", new { token=token});
            }
            else
            {
                ModelState.AddModelError("", "Token is not valid.");
                return RedirectToAction("Index");
            }
            //MessageBox.Show("Token is: " + token);
            
            
        }
       

        [HttpPost]
        public ActionResult GetBranchNamebyCity(int cityid)
        {
            List<BranchL> objbranch = new List<BranchL>();
            objbranch = GetAllBranch(cityid);
            SelectList obgbranch = new SelectList(objbranch, "Id", "BranchName");
            return Json(obgbranch);
        }
         
        [HttpPost]
        public ActionResult GetBranchDetails(int branchid )
        {
            CommonDAL cd = new CommonDAL();
            BranchDetailsViewModel objbranch = new BranchDetailsViewModel();
           // MessageBox.Show(branchid);
            objbranch = cd.GetBranchDetails(BranchCode[branchid-1]);
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
            if (Session["User"] != null)
            {
                return View("InvalidSession");
            }
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


            if (!ModelState.IsValid)
                return View(model);

            if (role.Equals("DoesNotExist"))
            {
                ModelState.AddModelError("", "The user name does not exist !");
                return View(model);
                //return RedirectToAction("Login");
            }
            

            if (role.Equals("Invalid"))
            {
                ModelState.AddModelError("", "The password provided is incorrect.");
                return View(model);
                //return RedirectToAction("Login");
            }
            
            UserRole ur = new UserRole {userID = model.UserName, role = role};
            
            
            if(role.Equals("Customer"))
                ur.customerID = (new CustomerDAL()).GetCustomerIDbyUserID(model.UserName);
            else if(role.Equals("Banker"))
                ur.branchCode = (new BankerDAL()).GetBranchCodeByBankerID(model.UserName);

            Session["User"] = ur;

            if (role.Equals("Admin"))
            {
                return RedirectToAction("DashBoard", "Admin");
            }
            else if (role.Equals("Banker"))
            {
                return RedirectToAction("DashBoard", "Banker");
            }
            else
            {
                return RedirectToAction("DashBoard", "Customer");
            }
            

        }


        public ActionResult LogOut()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }

    }
}
