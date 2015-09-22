using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBITest.Models;
using IBITest.Layers.DAL;

namespace IBITest.Controllers
{
    public class BankerController : Controller
    {
        //
        // GET: /Banker/

        public ActionResult DashBoard()
        {
            BankerDAL bd = new BankerDAL();
            long branchCode;

            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");
            else
                ViewBag.user = (Session["User"] as UserRole).userID;

            branchCode = (Session["User"] as UserRole).branchCode;


            ViewBag.acctrf = bd.GetNoOfBranchTransferRequests(branchCode);
            ViewBag.loan = bd.GetNoOfLoanRequests(branchCode);
            ViewBag.newacc = bd.GetNoOfNewAccountRequests(branchCode);
            ViewBag.accclo = bd.GetNoOfClosureRequests(branchCode);

            BranchDetailsViewModel bm = bd.GetSelfBranchDetails(ViewBag.user);

            ViewBag.selfbranchname = bm.BranchName;
            ViewBag.selfbranchAdddress = bm.Address;
            ViewBag.selfbranchnameCity = bm.CityName;
            ViewBag.selfbranchnameContactNo = bm.ContactNumber;
            ViewBag.selfbranchMailId = bm.Email;


            return View();
        }

        [HttpGet]
        public ActionResult GenerateToken()
        {
            return View(/*new GenerateTokenViewModel()*/);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateToken(GenerateTokenViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            BankerDAL obj = new BankerDAL();
            TokenInfo tf = new TokenInfo();
                      
            tf = obj.GenerateToken(model);

            ViewBag.CustomerID = String.Format("Customer ID : {0}", tf.CustomerID);
            ViewBag.token = String.Format("Token : {0}", tf.Token);

            ModelState.Clear();
            return View(new GenerateTokenViewModel());
        }

        [HttpGet]
        public ActionResult Requests()
        {
            BankerDAL obj = new BankerDAL();
            List<RequestViewModel> mdL = new List<RequestViewModel>();
            long branchCode;

            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");
            else
                ViewBag.user = (Session["User"] as UserRole).userID;

            branchCode = (Session["User"] as UserRole).branchCode;

            mdL = obj.GetAllRequests(branchCode);

            Session["AllRequests"] = mdL;

            return View(mdL);
        }

        public ActionResult GetRequestsByType(int typeid)
        {
            BankerDAL obj = new BankerDAL();
            List<RequestViewModel> requestListTemp = new List<RequestViewModel>();
            List<RequestViewModel> requestList = new List<RequestViewModel>();

            long branchCode; ;


            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");
            else
                branchCode = (Session["User"] as UserRole).branchCode;



            switch (typeid)
            {
                case 1:
                    requestListTemp = obj.GetNewAccountRequests(branchCode);

                    foreach (var v in requestListTemp)
                        if (v.Status == 'S')
                            requestList.Add(v);

                    break;

                case 2:
                    requestListTemp = obj.GetLoanRequests(branchCode);

                    foreach (var v in requestListTemp)
                        if (v.Status == 'S')
                            requestList.Add(v);

                    break;

                case 3:
                    requestListTemp = obj.GetBranchTransferRequests(branchCode);

                    foreach (var v in requestListTemp)
                        if (v.Status == 'S')
                            requestList.Add(v);

                    break;

                case 4:
                    requestListTemp = obj.GetClosingRequests(branchCode);

                    foreach (var v in requestListTemp)
                        if (v.Status == 'S')
                            requestList.Add(v);
                    
                    break;
            }
            return Json(requestList);
        }

        public ActionResult GetRequestsByStatus(int typeid)
        {
            BankerDAL obj = new BankerDAL();
            List<RequestViewModel> md = new List<RequestViewModel>();
            List<RequestViewModel> mdSorted = new List<RequestViewModel>();


            long branchCode; ;


            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");
            else
                branchCode = (Session["User"] as UserRole).branchCode;

            md = (Session["AllRequests"] as List<RequestViewModel>);
            

            switch (typeid)
            {
                case 1:
                    return Json(md);

                case 2:                   
                    foreach (var v in md)
                    {
                        if (v.Status == 'S')
                            mdSorted.Add(v);
                    }

                    break;

                case 3:

                    foreach (var v in md)
                    {
                        if (v.Status == 'A')
                            mdSorted.Add(v);
                    }                  
                    break;

                case 4:
                    foreach (var v in md)
                    {
                        if (v.Status == 'R')
                            mdSorted.Add(v);
                    }
                    break;


                case 5:
                    foreach (var v in md)
                    {
                        if (v.Status == 'T')
                            mdSorted.Add(v);
                    }
                    break;

            }

            return Json(mdSorted);
        }


        public ActionResult GetProofImage(string val)
        {
            int requestID;
            BankerDAL bankerDALObj = new BankerDAL();

            if (val[0] == 'N')
            {
                requestID = Convert.ToInt16(val.Substring(3));
                Byte[] image = bankerDALObj.GetProofImageByNewAccountRequestID(requestID);

                return Json(new { base64imgage = Convert.ToBase64String(image) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                requestID = Convert.ToInt16(val.Substring(3));
                Byte[] image = bankerDALObj.GetProofImageByNewAccountRequestID(requestID);

                return Json(new { base64imgage = Convert.ToBase64String(image) });
            }

        }

        public ActionResult CreditDebitCustomerAccount()
        {
            return View();
        }
        public char GetAccountType(int accountNumber)
        {
            BankerDAL objBankerDal = new BankerDAL();
            char accountType = objBankerDal.GetAccountType(accountNumber);
            ViewBag.accountType = accountType;
            return accountType;
            /*
            if (accountNumber == 2011)
            {
                ViewBag.accountType = "S";
                return "S";
            }
            else if (accountNumber == 2012)
            {
                ViewBag.accountType = "L";
                return "L";
            }
            else
            {
                ViewBag.accountType = "I";
                return "I";
            }
             */

        }
        public string SetBalance(int accountNumber, decimal amount, char accountType, char CreditOrDebit, string remarks)
        {
            BankerDAL objBankerDAL = new BankerDAL();
            if (accountType == 'S')
            {
                if (CreditOrDebit == 'C')//credit
                {
                    objBankerDAL.CreditSavingsAccountByBanker(accountNumber,amount,remarks);
                    return "Credit Successful";
                }
                else//debit
                {
                    if (amount <= objBankerDAL.GetAccountBalance(accountNumber))
                    {
                        objBankerDAL.DebitSavingsAccountByBanker(accountNumber, amount, remarks);
                        return "Debit successful";
                    }
                    else
                    {
                        return "Amount exceeds available balance";
                    }
                }
            }
            else
            {

                decimal loanBalance = objBankerDAL.GetAccountBalance(accountNumber);
                if (amount == loanBalance)
                {
                    objBankerDAL.CreditLoanAccountByBanker(accountNumber, amount, remarks);
                    objBankerDAL.CloseLoanAccount(accountNumber);
                    return "Credit Successful and Closed account";
                }
                else if (amount > loanBalance)
                {
                    return "Amount exceeds available balance";
                }
                else
                {
                    objBankerDAL.CreditLoanAccountByBanker(accountNumber, amount, remarks);
                    return "Credit Successful";
                }
            }
            
        }
        public ActionResult ApproveRequest(string val)
        {
            int requestID;
            BankerDAL bankerDALObj = new BankerDAL();
           
            System.Windows.Forms.MessageBox.Show(val.Length.ToString());

            if (val[0] == 'N')
            {
                requestID = Convert.ToInt16(val.Substring(3));

                return Json(bankerDALObj.ApproveNewAccountCreationRequest(requestID));
            }
            else
            {
                requestID = Convert.ToInt16(val.Substring(3));
                
                return Json(bankerDALObj.ApproveLoanRequest(requestID));
            }            
        }


        public ActionResult TransferRequest(string val)
        {
            int requestID;
            BankerDAL bankerDALObj = new BankerDAL();

            if (val[0] == 'T')
            {
                requestID = Convert.ToInt16(val.Substring(3));

                return Json(bankerDALObj.TransferTransferOfAccountRequest(requestID));
            }
            else
            {
                requestID = Convert.ToInt16(val.Substring(3).ToString());

                return Json(bankerDALObj.TransferClosingOfAccountRequest(requestID));
            }
        }



        public ActionResult RejectRequest(string val)
        {
            int requestID;
            BankerDAL bankerDALObj = new BankerDAL();

            if (val[0] == 'N')
            {
                requestID = Convert.ToInt16(val.Substring(3));

                return Json(bankerDALObj.RejectNewAccountCreationRequest(requestID));
            }
            else if(val[0] == 'A')
            {
                requestID = Convert.ToInt16(val.Substring(3));

                return Json(bankerDALObj.RejectLoanRequest(requestID));
            }
            else if (val[0] == 'T')
            {
                requestID = Convert.ToInt16(val.Substring(3));

                return Json(bankerDALObj.RejectTransferOfAccountRequest(requestID));
            }
            else
            {
                requestID = Convert.ToInt16(val.Substring(3));

                return Json(bankerDALObj.RejectClosingOfAccountRequest(requestID));
            }
        }

        public ActionResult SearchCustomerDetails()
        {
            //query
            return View();
        }
        [HttpPost]
        public JsonResult GetCustomersByName(string queryName)
        {
            BankerDAL objBankerDal = new BankerDAL();
            long branchCode = (Session["User"] as UserRole).branchCode;
            return Json(objBankerDal.GetCustomerByName(queryName,branchCode));
        }
        [HttpPost]
        public JsonResult GetCustomerByAccountNo(int accountNo)
        {
            BankerDAL objBankerDal = new BankerDAL();
            long branchCode = (Session["User"] as UserRole).branchCode;
            return Json(objBankerDal.GetCustomerByAccountNumber(accountNo, branchCode));
        }
        [HttpPost]
        public JsonResult GetCustomersWithLoan()
        {
            BankerDAL objBankerDal = new BankerDAL();
            long branchCode = (Session["User"] as UserRole).branchCode;
            return Json(objBankerDal.GetCustomersHavingLoan(branchCode));
        }
        

    }
}
