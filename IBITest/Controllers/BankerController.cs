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

            if (Session["User"] == null)
                return RedirectToAction("Login", "CommonBiz");
            else
                ViewBag.user = (Session["User"] as UserRole).userID;

            ViewBag.acctrf = bd.GetNoOfBranchTransferRequests();
            ViewBag.loan = bd.GetNoOfLoanRequests();
            ViewBag.newacc = bd.GetNoOfNewAccountRequests();
            ViewBag.accclo = bd.GetNoOfClosureRequests();

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
            return View(new GenerateTokenViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateToken(GenerateTokenViewModel model)
        {
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
            
            mdL = obj.GetAllRequests();

            Session["AllRequests"] = mdL;

            return View(mdL);
        }

        public ActionResult GetRequestsByType(int typeid)
        {
            BankerDAL obj = new BankerDAL();
            List<RequestViewModel> md = new List<RequestViewModel>();

            switch (typeid)
            {
                case 1:
                    md = obj.GetNewAccountRequests();
                    break;

                case 2:
                    md = obj.GetLoanRequests();
                    break;

                case 3:
                    md = obj.GetBranchTransferRequests();
                    break;

                case 4:
                    md = obj.GetClosingRequests();
                    break;
            }
            return Json(md);
        }

        public ActionResult GetRequestsByStatus(int typeid)
        {
            BankerDAL obj = new BankerDAL();
            List<RequestViewModel> md = new List<RequestViewModel>();
            List<RequestViewModel> mdSorted = new List<RequestViewModel>();

            if (Session["AllRequests"] != null)
                md = (Session["AllRequests"] as List<RequestViewModel>);
            else
                System.Windows.Forms.MessageBox.Show("Session expired");


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
        public string GetAccountType(int accountNumber)
        {
            
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

        }
        public bool SetBalance(int accountNumber, decimal amount, char accountType, char CreditOrDebit, string remarks)
        {
            return true;
        }
    }
}
