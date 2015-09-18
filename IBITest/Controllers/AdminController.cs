﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBITest.Models;
using IBITest.Layers.DAL;
using System.Windows.Forms;
namespace IBITest.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult DashBoard()
        {
            ViewBag.BranchTransferRequests = 7;
            ViewBag.AccountClosureRequests = 0;
            return View();
        }
        public ActionResult AddBranch()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBranch(BranchDetails model)
        {
            AdminDAL obj = new AdminDAL();
            if (!obj.IsUniqueBranchLogInID(model.BranchLogInID))
            {
                ModelState.AddModelError("", "Login ID is alreay taken.");
                return View(model);
            }
            obj.AddBranch(model);
            return View("Dashboard");
        }
        public ActionResult ViewOrUpdateBranchDetails()
        {
            return View();
        }
        public ActionResult ApproveOrRejectRequests()
        {

            List<SelectListItem> obj = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Transfer of Account Request",Value="TOC"},
                new SelectListItem(){Text = "Closure of Account",Value="COA"}
            };
            ViewBag.requestTypeList = obj;
            return View();
        }
        public JsonResult GetTransferOfAccountAdminRequest(string requestType)
        {
            Console.WriteLine("some one called me");
            //Console.ReadKey();
            MessageBox.Show("some one called me");
            List<TranferOfAccountAdminView> obj = new List<TranferOfAccountAdminView>(){
                new TranferOfAccountAdminView() {requestID=1,customerID=1,accountNumber=1,fromBranch=1,toBranch=2},
                new TranferOfAccountAdminView() { requestID=2,customerID=2,accountNumber=2,fromBranch=2,toBranch=1}
            };
            //return Json(obj);
            return Json(obj);
        }

    }
}
