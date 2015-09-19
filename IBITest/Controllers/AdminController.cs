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
            if (!ModelState.IsValid)
                return View(model);

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
            AdminDAL adminObj = new AdminDAL();
            List<TranferOfAccountAdminView> obj = adminObj.GetTransferAccountRequests();
            return Json(obj);
        }
        public JsonResult GetClosureOfAccountAdminRequest(string requestType)
        {
            AdminDAL objAdminDal = new AdminDAL();
            List<ClosureOfAccountAdminView> obj = objAdminDal.GetClosureOfAccountRequests();
            return Json(obj);
        }
        public void ApproveTOC(List<int> approveRequestList)
        {
           return;
        }
        public void ApproveCOA(List<int> approveRequestList)
        {
            return;
        }
        public void RejectTOC(List<int> rejectRequestList)
        {
            return;
        }
        public void RejectCOA(List<int> rejectRequestList)
        {
            return;
        }

    }
}
