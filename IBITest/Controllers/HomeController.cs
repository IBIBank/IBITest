using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBITest.Layers.DAL;
using IBITest.Models;

//sgaggaagabg

namespace IBITest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            AdminDAL a = new AdminDAL();

            //bool res = a.AddBranch(new BranchDetails { CityName = "Chenn", Address="add",BranchName="br12",BankerName="som",BranchLogInID="fdaf",BranchLogInPassword="pasa",ContactNumber="sfs",Email="sfd@.co"});
            bool res = a.IsUniqueBranchLogInID("sadadaca");
            System.Windows.Forms.MessageBox.Show(res.ToString());
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
