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

namespace IBITest.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            CustomerDAL newObj = new CustomerDAL();
            BankerDAL banObj = new BankerDAL();
            CommonDAL commonDALObj = new CommonDAL();



            int branchCode = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(PayeeID) FROM Payee ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();

                if(!reader.HasRows)
                    branchCode = Convert.ToInt16(reader[0]);
                else
                    MessageBox.Show("No data");

                MessageBox.Show(branchCode.ToString());

                reader.Close();
            }



           return View(new TestModel());
        }

        [HttpPost]
        public ActionResult Index(TestModel model)
        {
            

            return View(model);
        }



    }
}
