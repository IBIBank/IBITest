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
                ViewBag.user = (Session["User"] as UserRole).UserID;

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

        


    }
}
