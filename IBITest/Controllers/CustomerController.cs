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

            CustomerDAL customerDALObject = new CustomerDAL();
            
            long customerID = (Session["User"] as UserRole).customerID;

            ViewBag.approvedRequests = customerDALObject.GetNoOfApprovedRequests(customerID);
            ViewBag.rejectedRequests = customerDALObject.GetNoOfRejectedRequests(customerID);
            ViewBag.pendingRequests = customerDALObject.GetNoOfPendingRequests(customerID);

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
        public string CreateNewAccount(NewAccountRequestView model)
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
            bmvm = cd.GetBranchesInCity(cityName);
            //MessageBox.Show(bmvm[0].BranchCode.ToString()+" "+bmvm[0].BranchName);
            SelectList obj = new SelectList(bmvm, "BranchCode", "BranchName");

            return Json(obj);
        }

        public JsonResult GetListOfSavingAccounts()
        {
            List<AccountListViewModel> obj = new List<AccountListViewModel>();

            obj = (new CustomerDAL()).GetAccountsListByCustomerID((Session["User"] as UserRole).customerID);

            return Json(obj);
        }



        public ActionResult MiniDetailedstatements()
        {
            List<TransactionStatementViewModel> transactionsList = new List<TransactionStatementViewModel>();
            List<long> accountsList = new List<long>();

            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");

            CustomerDAL customerDALObject = new CustomerDAL();
            long customerID = (Session["User"] as UserRole).customerID;


            accountsList = customerDALObject.GetAccountsListAsLongListByCustomerID(customerID);

            ViewBag.accountsList = accountsList;

            if (accountsList != null)
                ViewBag.transactionDetails = customerDALObject.GetLastFiveTransactions(accountsList[0]);

            return View();
        }


        public ActionResult GetLastFiveTransactions(long accountNumber)
        {
            List<TransactionStatementViewModel> transactionsList = new List<TransactionStatementViewModel>();
            
            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");

            CustomerDAL customerDALObject = new CustomerDAL();
            long customerID = (Session["User"] as UserRole).customerID;

            return Json(customerDALObject.GetLastFiveTransactions(accountNumber));
        }

        [HttpPost]
        public ActionResult GetDetailedTransactions(long accountNumber, int fromDate, int fromMonth, int fromYear, int toDate, int toMonth, int toYear)
        {
            List<TransactionStatementViewModel> transactionsList = new List<TransactionStatementViewModel>();

            DateTime startDate = new DateTime(fromYear, fromMonth, fromDate);
            DateTime endDate = new DateTime(toYear, toMonth, toDate);

            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");

            CustomerDAL customerDALObject = new CustomerDAL();
            long customerID = (Session["User"] as UserRole).customerID;

            return Json(customerDALObject.GetDetailedTransactions(accountNumber, startDate, endDate));
        }
        
        
        public ActionResult AddPayee()
        {
            //AddPayeeViewModel obj = new AddPayeeViewModel();
            return View();
        }
        public JsonResult ValidateAccountNumber(int accountNumber)
        {
            CustomerDAL objOfCustomerDAL = new CustomerDAL();
            long customerID = (Session["User"] as UserRole).customerID;

            AddPayeeViewModel obj = objOfCustomerDAL.ValidatePayeeAccountNumber(accountNumber, customerID);

            //if(accountNumber==0)
            //    return null;
            //AddPayeeViewModel obj = new AddPayeeViewModel {payeeName="virat",branchName="kormangala" };
            ViewBag.message = "";
            return Json(obj);

        }
        [HttpPost]
        public ActionResult AddPayee(AddPayeeViewModel model)
        {
            CustomerDAL objOfCustomerDAL = new CustomerDAL();

            long customerID = (Session["User"] as UserRole).customerID;
            if (objOfCustomerDAL.ValidatePayeeAccountNumber(model.payeeAccountNumber, customerID) != null)
            {
                objOfCustomerDAL.AddPayee(model, customerID);
                ViewBag.message = "success";
                return View();
            }
            else
            {
                ViewBag.message = "Operation failed";
                return View();
            }
            
        }
        public ActionResult RequestForAccountTransfer()
        {
            return View();
        }
        public ActionResult TransferFunds()
        {
            long customerID = (Session["User"] as UserRole).customerID;
            CustomerDAL objCustomerDAL = new CustomerDAL();
            //List<int> savingsAccounts = new List<int>(){4,8,9};
            //ViewBag.savingsAccountList = savingsAccounts;
            ViewBag.savingsAccountList = objCustomerDAL.GetAllSavingsAccountByCustomerID(customerID);

            //List<SelectListItem> payeeAccounts = new List<SelectListItem>()
            //{
            //    new SelectListItem(){Text = "sham",Value="4"},
            //    new SelectListItem(){Text = "ram",Value="8"},
            //    new SelectListItem(){Text = "suresh",Value="12"},
            //};
            //ViewBag.payeeAccounts = payeeAccounts;
            ViewBag.payeeAccounts = objCustomerDAL.GetAllPayeeAccountByCustomerID(customerID);
            ViewBag.message = "";
            return View();
        }
        
        [HttpPost]
        public ActionResult TransferFunds(FundTransferViewModel model)
        {
            
            long customerID = (Session["User"] as UserRole).customerID;
            BankerDAL objBDAL = new BankerDAL();
            CustomerDAL objCDAL = new CustomerDAL();
            ViewBag.savingsAccountList = objCDAL.GetAllSavingsAccountByCustomerID(customerID);
            ViewBag.payeeAccounts = objCDAL.GetAllPayeeAccountByCustomerID(customerID);
            string message="";
            if (model.Amount <= 0)
            {
                //ModelState.AddModelError("", "Source and Dest cant be same");
                message =  "Amount must be positive";
                @ViewBag.message = message;
                return View(model);
            }
            if (model.FromAccount == model.ToAccount)
            {
                //ModelState.AddModelError("", "Source and Dest cant be same");
                message =  "Source and Destination account can't be same";
                @ViewBag.message = message;
                return View(model);
            }
            if (model.Amount > objBDAL.GetAccountBalance(model.FromAccount))
            {
                message =  "Insufficient funds. Please check balance";
                @ViewBag.message = message;
                return View(model);
            }
            if (objCDAL.ValidateTransactionPassword(customerID, model.TransactionPassword) == false)
            {
                message =  "Password is not valid";
                @ViewBag.message = message;
                return View(model);
            }
            if (objBDAL.GetAccountType(model.ToAccount) == 'L')
            {
                if (model.Amount > objBDAL.GetAccountBalance(model.ToAccount))
                {
                    message =  "Invalid transaction";
                    @ViewBag.message = message;
                    return View(model);
                }
            }
            objCDAL.DoFundTransfer(model);
            message =  "Transaction Successful";
            @ViewBag.message = message;
            return View(model);
        }
       
        public ActionResult RequestClosureOfAccount()
        {
            long customerID = (Session["User"] as UserRole).customerID;
            CustomerDAL objCustomerDAL = new CustomerDAL();
            ViewBag.savingsAccountList = objCustomerDAL.GetAllSavingsAccountByCustomerID(customerID);
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
            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");

            return View();
        }
        
        [HttpPost]
        public ActionResult UpdateUserOrTransactionPassword(string oldPassword, string newPassword, string passwordType)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");

            long customerID = (Session["User"] as UserRole).customerID;

            string result = (new CustomerDAL()).ValidateAndSetPassword(customerID, oldPassword, newPassword, passwordType);

            return Json(result);
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
