using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using IBITest.Models;

namespace IBITest.Layers.DAL
{
    public class CommonDAL
    {
        public List<String> GetCityList()
        {
            SqlConnection cn = new SqlConnection(); //("Data Source=(localdb)\\Projects;Initial Catalog=Database1;Integrated Security=True");//windows auth
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString();
            
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT CityName FROM Branch", cn);
            cn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            List <string> CityList = new List<string>();

            while (rd.Read())
                CityList.Add(rd[0].ToString());

            cn.Close();

            return CityList;
        }


        public List<BranchMiniViewModel> GetBranchesInCity(string CityName)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT BranchCode, BranchName FROM Branch WHERE CityName = " + CityName, cn);
            cn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            List <BranchMiniViewModel> BranchList = new List<BranchMiniViewModel>();

            while (rd.Read())
                BranchList.Add(new BranchMiniViewModel{BranchCode = Convert.ToInt64(rd[0]), BranchName = rd[1].ToString()});

            cn.Close();

            return BranchList;
        }


        public BranchDetailsViewModel GetBranchDetails(Int64 BranchCode)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());
            SqlCommand cmd = new SqlCommand("SELECT BranchName,CityName,Address,ContactNumber, Email FROM Branch WHERE BranchCode = " + BranchCode, cn);
            cn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            BranchDetailsViewModel bd = new BranchDetailsViewModel();


            if (rd.Read())
            {
                bd.BranchName = rd[0].ToString();
                bd.CityName = rd[1].ToString();
                bd.Address = rd[2].ToString();
                bd.ContactNumber = rd[3].ToString();
                bd.Email = rd[4].ToString();
            }
            else
                System.Windows.Forms.MessageBox.Show("No Data in Branch Table !");

            cn.Close();

            return bd;
        }

        public string CheckRole(string UserID, string Password)
        {
            string res;
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());
            SqlCommand cmd = new SqlCommand(String.Format( "SELECT Role FROM UserRoles WHERE UserID = '{0}' AND Password = '{1}'",  UserID,Password), cn);
            cn.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            rd.Read();

            

            if (!rd.HasRows)
            {
                res =  String.Copy("Invalid");
            }
            else
                res = String.Copy(rd[0].ToString());

            cn.Close();

            return res;
        }


    }
}