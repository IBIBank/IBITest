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
            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");

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
            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");

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

        public JsonResult GetListOfSavingAccounts()
        {
            List<AccountListViewModel> obj = new List<AccountListViewModel>()
            {
                new AccountListViewModel(){accountNumber=1,balance=9},
                new AccountListViewModel(){accountNumber=2,balance=10 }
            };
            return Json(obj);
        }
        public ActionResult MiniDetailedstatements()
        {
            return View();
        }
        public ActionResult AddPayee()
        {
            //AddPayeeViewModel obj = new AddPayeeViewModel();
            return View();
        }
        public JsonResult ValidateAccountNumber(int accountNumber)
        {
            //CustomerDAL objOfCustomerDAL = new CustomerDAL();
            //AddPayeeViewModel obj = objOfCustomerDAL.ValidatePayeeAccountNumber(accountNumber);
            
            if(accountNumber==0)
                return null;
            AddPayeeViewModel obj = new AddPayeeViewModel {payeeName="virat",branchName="kormangala" };
            
            return Json(obj);
    
        }
        public ActionResult RequestForAccountTransfer()
        {
            return View();
        }
        public ActionResult TransferFunds()
        {
            return View();
        }
        public ActionResult RequestClosureOfAccount()
        {
            return View();
        }
        public ActionResult ViewRequestStatus()
        {
            return View();
        }
        public ActionResult ViewOrUpdateProfile()
        {
            return View();
        }
        public ActionResult UpdateUserOrTransactionPassword()
        {
            return View();
        }
        public ActionResult ApplyForLoan()
        {
            return View();
        }
        public ActionResult MoneyManagaer()
        {
            return View();
        }
    }

}
/*
    
    EMI Calculator
    Money Managaer
   
 */
