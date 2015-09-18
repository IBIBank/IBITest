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
            MessageBox.Show("some one called me");
            AdminDAL adminObj = new AdminDAL();

            List<TranferOfAccountAdminView> obj = adminObj.GetTransferAccountRequests();
            //return Json(obj);
            return Json(obj);
        }
        public JsonResult GetClosureOfAccountAdminRequest(string requestType)
        {
            //Console.WriteLine("some one called me");
            //Console.ReadKey();
            //MessageBox.Show("some one called me");
            List<ClosureOfAccountAdminView> obj = new List<ClosureOfAccountAdminView>(){
                new ClosureOfAccountAdminView() {requestID=1,customerID=1,accountNumber=1},
                new ClosureOfAccountAdminView() { requestID=2,customerID=2,accountNumber=2}
            };
            //return Json(obj);
            return Json(obj);
        }
        public void Approve(List<int> approveRequestList)
        {
            MessageBox.Show("In approve method");
            MessageBox.Show(approveRequestList[0].ToString());
            //MessageBox.Show(((int)approveRequestList[0]).ToString() + " " + ((int)approveRequestList[1]).ToString());
            //string r = "";
            //for (int i = 0; i < approveRequestList.Length; i++)
            //{
            //    r = approveRequestList[i] + ",";
            //}
            //MessageBox.Show(r);
            return;
        }

    }
}
