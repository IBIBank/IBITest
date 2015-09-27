using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBITest.Models;
using IBITest.Layers.DAL;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace IBITest.Controllers
{
    public class TestController : Controller
    {

        public static string ImagePath = "Images/"; //"App_Data/Images/";

        public ActionResult Index()
        {
           CustomerDAL newObj = new CustomerDAL();
           AdminDAL adminObj = new AdminDAL();

           List<BranchViewModel> list= adminObj.GetAllBranchDetails();

           foreach(var v in list)
               MessageBox.Show(v.branchCode);

           return View();
        }

        [HttpPost]
        public ActionResult Index(TestModel model)
        {
            

            return View(model);
        }



        [HttpGet]
        public ActionResult UploadImage(string id)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult UploadImage()
        {
            string imgPath = string.Empty;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                imgPath = TestController.ImagePath + "TempImage.jpg";
                file.SaveAs(Server.MapPath("~/") + imgPath); //File will be saved in directory
            }
            return Json(imgPath);
        }



        [HttpGet]
        public ActionResult GetBase64Image()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images") + "\\" + "TempImage.jpg";

            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[(int)fileStream.Length];
            fileStream.Read(data, 0, data.Length);

            return Json(new { base64imgage = Convert.ToBase64String(data) }  , JsonRequestBehavior.AllowGet);
        } 





    }
}
