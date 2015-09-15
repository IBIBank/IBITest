using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IBITest.Models;


namespace IBITest.Layers.DAL
{
    public class AdminDAL
    {
        public int GetNoOfAccTransferReq()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM NewAccountRequest ", cn);
            cn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            cn.Close();
            return Convert.ToInt16(rd[0]);

        }


        public int GetNoOfAccClosureferReq()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ClosingRequest ", cn);
            cn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();

            cn.Close();
            return Convert.ToInt16(rd[0]);
        }



        public bool AddBranch(BranchDetails bd)
        {

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT MAX(BranchCode) FROM Branch ", cn);
            cn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            Int64 BranchCode;

            if (rd.HasRows)
                BranchCode = Convert.ToInt64(rd[0]) + 1;
            else
                BranchCode = 1;

            cn.Close();
            SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());
            string command = String.Format("INSERT INTO Branch VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", BranchCode, bd.BranchName, bd.CityName, bd.Address, bd.ContactNumber, bd.BankerName, bd.BranchLogInID, bd.BranchLogInPassword, bd.Email);

            cn2.Open();

            SqlCommand cmd2 = new SqlCommand(command,cn2);            
            int res = cmd2.ExecuteNonQuery();

            cn.Close();
            if (res == 0)
                return false;
            else
                return true;
        }



    }
}