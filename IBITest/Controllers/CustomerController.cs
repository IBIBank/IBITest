using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using IBITest.Layers.DAL;
using IBITest.Models;
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
            //MessageBox.Show("I am in GET Now get user details according to token"+token);
            CustomerDAL obj = new CustomerDAL();

            return View(obj.GetUserByTokenID(token));
        }
        [HttpPost]
        public ActionResult FinishRegistration(Customer model)
        {
            //MessageBox.Show("I am in POST");
            CustomerDAL obj = new CustomerDAL();

            if (obj.IsUniqueUserID(model.UserID))
            {
                obj.FinishReg(model);
                return RedirectToAction("Login", "CommonBiz");
            }
            else
            {
                ModelState.AddModelError("", "User Id not available");
                return View(model);
            }
            
        }
        public ActionResult CreateNewAccount()
        {
            CommonDAL obj = new CommonDAL();
            NewAccountRequestView model = new NewAccountRequestView();
            
            var customerID = (Session["User"] as UserRole).customerID;

            ViewBag.cityList = obj.GetCityList();

            model.CustomerID = customerID;
            return View(model);
        }

        [HttpPost]
        public string CreateNewAccount( NewAccountRequestView model)
        {
            CustomerDAL customerDALobject = new CustomerDAL();

            if (customerDALobject.AddNewAccountRequest(model))
                return "Request Submitted. Thank You !";
            else
                return "Oops... Error in submitting request.";
        }



        public JsonResult GetBranchesInCity(string cityName)
        {
            List<BranchMiniViewModel> bmvm = new List<BranchMiniViewModel>();
            CommonDAL cd = new CommonDAL();
            bmvm = cd.GetBranchesInCity(cityName );
            //MessageBox.Show(bmvm[0].BranchCode.ToString()+" "+bmvm[0].BranchName);
            SelectList obj = new SelectList(bmvm, "BranchCode", "BranchName");

            return Json(obj);
        }

    }
}
