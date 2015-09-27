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
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");


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
            Customer customer = obj.GetUserByTokenID(token);
            customer.DOB = Convert.ToDateTime(customer.DOB.ToShortDateString());

            return View(customer);
        }


        [HttpPost]
        public ActionResult FinishRegistration(Customer model, HttpPostedFileBase Image)
        {
            //MessageBox.Show("I am in POST");
            CustomerDAL obj = new CustomerDAL();

            if (obj.IsUniqueUserID(model.UserID) && ModelState.IsValid)
            {
                if (Image != null)
                {
                   // product.ImageMimeType = image.ContentType;
                    model.PhotoIDProof = new byte[Image.ContentLength];
                    Image.InputStream.Read(model.PhotoIDProof, 0, Image.ContentLength);
                }
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
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            CommonDAL obj = new CommonDAL();
            NewAccountRequestView model = new NewAccountRequestView();

            var customerID = (Session["User"] as UserRole).customerID;

            ViewBag.cityList = obj.GetCityList();

            model.CustomerID = customerID;
            return View(model);
        }





        [HttpPost]
        public JsonResult UploadImage()
        {
            string imgPath = string.Empty;
            long requestID;
            CustomerDAL customerDALObj = new CustomerDAL();

           
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    //Use the following properties to get file's name, size and MIMEType
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    requestID = customerDALObj.GetNextNewAccountRequestID();
                    //To save file, use SaveAs method
                    imgPath = "Images/NewAccountCreation/" + "NAC" + requestID + ".jpg";
                    file.SaveAs(Server.MapPath("~/") + imgPath); //File will be saved in directory
                }
                return Json("Image uploaded successfully !");
           
        }









        [HttpPost]
        public ActionResult CreateNewAccount(NewAccountRequestView model, HttpPostedFileBase Image)
        {
            CustomerDAL customerDALobject = new CustomerDAL();
           // ModelState.Clear();           

            CommonDAL obj = new CommonDAL();
            ViewBag.cityList = obj.GetCityList();

            if (Image == null)
            {
                ViewBag.message = "Please upload image first ! ";
                return View(model);
            }
            

            if (customerDALobject.AddNewAccountRequest(model))
            {
                ViewBag.message = "Request Submitted. Thank You !";
                return View(model);
            }

            else
            {
                ViewBag.message = "Oops... Error in submitting request.";
                return View(model);
            }
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

            obj = (new CustomerDAL()).GetAccountsListByCustomerID((this.Session["User"] as UserRole).customerID);

            return Json(obj);
        }
        public JsonResult GetListOfLoanAccounts()
        {
            List<AccountListViewModel> obj = new List<AccountListViewModel>();

            obj = (new CustomerDAL()).GetLoanAccountsListByCustomerID((this.Session["User"] as UserRole).customerID);

            return Json(obj);
        }


        public ActionResult MiniDetailedstatements()
        {
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            List<TransactionStatementViewModel> transactionsList = new List<TransactionStatementViewModel>();
            List<long> accountsList = new List<long>();


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
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

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

            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            ModelState.Clear();

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
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            return View();
        }


        public ActionResult TransferFunds()
        {
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            long customerID = (Session["User"] as UserRole).customerID;
            CustomerDAL objCustomerDAL = new CustomerDAL();
            //List<int> savingsAccounts = new List<int>(){4,8,9};
            //ViewBag.savingsAccountList = savingsAccounts;
            List<long>  savingsAccountList = objCustomerDAL.GetAllSavingsAccountByCustomerID(customerID);
            if (savingsAccountList == null)
            {
                ViewBag.savingsAccountList = new List<long>();
            }
            else
            {
                ViewBag.savingsAccountList = savingsAccountList;
            }
            //ViewBag.savingsAccountList = objCustomerDAL.GetAllSavingsAccountByCustomerID(customerID);

            //List<SelectListItem> payeeAccounts = new List<SelectListItem>()
            //{
            //    new SelectListItem(){Text = "sham",Value="4"},
            //    new SelectListItem(){Text = "ram",Value="8"},
            //    new SelectListItem(){Text = "suresh",Value="12"},
            //};
            //ViewBag.payeeAccounts = payeeAccounts;
            List<FundTransferPayeeModel> payeeAccounts = objCustomerDAL.GetAllPayeeAccountByCustomerID(customerID);
            if (payeeAccounts == null)
            {
                ViewBag.payeeAccounts = new List<FundTransferPayeeModel>();
            }
            else
            {
                ViewBag.payeeAccounts = payeeAccounts;   
            }
            //ViewBag.payeeAccounts = objCustomerDAL.GetAllPayeeAccountByCustomerID(customerID);
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
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            long customerID = (Session["User"] as UserRole).customerID;
            CustomerDAL objCustomerDAL = new CustomerDAL();
            ViewBag.savingsAccountList = objCustomerDAL.GetAccountsListAsLongListByCustomerID(customerID);
            ViewBag.message = "";
            return View();
        }
        [HttpPost]
        public ActionResult RequestClosureOfAccount(CloseAccountCustomerViewModel model)
        {
            long customerID = (Session["User"] as UserRole).customerID;
            CustomerDAL objCustomerDAL = new CustomerDAL();
            //List<long> savingsAccountList = new List<long>() { 121212, 12123123421 };
            ViewBag.savingsAccountList = objCustomerDAL.GetAccountsListAsLongListByCustomerID(customerID);
            if (objCustomerDAL.AddCloseAccountRequest(model.accountNumber, customerID))
            {
                ViewBag.message = "Request for closure of account is successful";
            }
            else
            {
                ViewBag.message = "Request already exist";
            }   
            
            return View();
        }

        public ActionResult ViewRequestStatus()
        {
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            return View();
        }


        public ActionResult ViewOrUpdateProfile()
        {
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            return View();
        }

        public ActionResult UpdateUserOrTransactionPassword()
        {
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

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
            CommonDAL objCommonDal = new CommonDAL();
            ViewBag.cityList = objCommonDal.GetCityList();
            long customerID = (Session["User"] as UserRole).customerID;
            CustomerDAL objCustomerDal = new CustomerDAL();
            LoanRequestViewModel objLoanRequest = new LoanRequestViewModel();
            objLoanRequest.customerID = customerID;
            objLoanRequest.age = objCustomerDal.GetAgeByCustomerID(customerID);
            objLoanRequest.salaryProof = new byte[1024];
            objLoanRequest.addressProof = new byte[1024];
            @ViewBag.loanTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Personal",Value="P"},
                new SelectListItem(){Text = "Home",Value="H"},
                new SelectListItem(){Text = "Vehicle",Value="V"},
            };

            return View(objLoanRequest);
        }


        [HttpPost]
        public JsonResult UploadAddressProof()
        {
            string imgPath = string.Empty;
            long requestID;
            CustomerDAL customerDALObj = new CustomerDAL();


            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                requestID = customerDALObj.GetNextLoanAccountRequestID();
                //To save file, use SaveAs method
                imgPath = "Images/ApplyForLoan/" + "AFL_AP_" + requestID + ".jpg";
                file.SaveAs(Server.MapPath("~/") + imgPath); //File will be saved in directory
            }
            return Json("Image uploaded successfully !");

        }

        [HttpPost]
        public JsonResult UploadSalaryProof()
        {
            string imgPath = string.Empty;
            long requestID;
            CustomerDAL customerDALObj = new CustomerDAL();


            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                requestID = customerDALObj.GetNextLoanAccountRequestID();
                //To save file, use SaveAs method
                imgPath = "Images/ApplyForLoan/" + "AFL_SP_" + requestID + ".jpg";
                file.SaveAs(Server.MapPath("~/") + imgPath); //File will be saved in directory
            }
            return Json("Image uploaded successfully !");

        }



        [HttpPost]
        public ActionResult ApplyForLoan(LoanRequestViewModel model)
        {
            
            CommonDAL objCommonDal = new CommonDAL();
            CustomerDAL objCustomerDal = new CustomerDAL();
            ViewBag.cityList = objCommonDal.GetCityList();
            @ViewBag.loanTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Personal",Value="P"},
                new SelectListItem(){Text = "Home",Value="H"},
                new SelectListItem(){Text = "Vehicle",Value="V"},
            };
            if(objCustomerDal.AddLoanAccountRequest(model) )
            {
                ModelState.AddModelError("", "You request for loan was successful. You may apply for yet another loan");
            }
            else
            {
                ModelState.AddModelError("", "Operation failed");
            }
            

            return View(model);
        }

        public ActionResult MoneyManagaer()
        {
            string result = (new CommonDAL()).CheckValidation("Customer", this.Session);

            if (result.Equals("LogIn"))
                return RedirectToAction("Login", "CommonBiz");
            else if (result.Equals("Unauthorised"))
                return RedirectToAction("Unauthorised", "CommonBiz");

            return View();
        }
        public ActionResult EmiCalculator()
        {
            return View();
        }
        public ActionResult RestService()
        {
            return View();
        }
        
    }

}
/*
    Money Managaer   
 */
