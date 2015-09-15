﻿using System;
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

        public bool IsUniqueBranchLogInID(string BranchLogInID)
        {
            bool res;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT UserID FROM UserRoles WHERE UserID = '{0}' " , BranchLogInID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                    res = false;
                else
                    res = true;

                reader.Close();

            }

            return res;
        }

        public BranchDetails GetBranchDetails(Int64 BranchCode)
        {
            BranchDetails bd = new BranchDetails();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Branch WHERE BranchCode = '{0}' " + BranchCode, connection);
                connection.Open();

                SqlDataReader rd = command.ExecuteReader();

                // Call Read before accessing data. 
                if (rd.HasRows)
                {
                    rd.Read();
                    bd.BranchCode = Convert.ToInt64(rd[0]);
                    bd.BranchName = rd[1].ToString();
                    bd.CityName = rd[2].ToString();
                    bd.Address = rd[3].ToString();
                    bd.ContactNumber = rd[4].ToString();
                    bd.BankerName = rd[5].ToString();
                    bd.BranchLogInID = rd[6].ToString();
                    bd.BranchLogInPassword = rd[7].ToString();
                    bd.Email = rd[8].ToString();
                }

                else
                    System.Windows.Forms.MessageBox.Show("Branch Details Read Failed");

                rd.Close();

            }

            return bd;
        }

        

        public bool AddBranch(BranchDetails bd)
        {

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT MAX(BranchCode) FROM Branch ", cn);
            cn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();

            if (rd.HasRows)
                bd.BranchCode = Convert.ToInt64(rd[0]) + 1;
            else
                bd.BranchCode = 1;

            rd.Close();

            cmd.CommandText = String.Format("SELECT MAX(Id) FROM UserRoles ");
            
            rd = cmd.ExecuteReader();
            rd.Read();
            int id = Convert.ToInt16(rd[0]) + 1;

            cn.Close();


            SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());
            string command = String.Format("INSERT INTO Branch VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", bd.BranchCode, bd.BranchName, bd.CityName, bd.Address, bd.ContactNumber, bd.BankerName, bd.BranchLogInID, bd.BranchLogInPassword, bd.Email);

            cn2.Open();

            SqlCommand cmd2 = new SqlCommand(command,cn2);            
            int res = cmd2.ExecuteNonQuery();

            SqlCommand cmd3 = new SqlCommand(String.Format("INSERT INTO UserRoles VALUES('{0}','{1}', '{2}', '{3}'", id, bd.BranchLogInID, bd.BranchLogInPassword, "Banker"), cn2);
            cmd3.ExecuteNonQuery();

            cn2.Close();
            
            if (res == 0)
                return false;
            else
                return true;
        }


        public bool UpdateBranchDetails(BranchDetails bd)
        {                       
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());
            string command = String.Format("UPDATE Branch SET Address = '{0}', ContactNumber = '{1}', Email = '{2}', BranchPassword = '{3}' WHERE BranchCode = '{4}'",bd.Address,bd.ContactNumber,bd.Email,bd.BranchLogInPassword,bd.BranchCode) ;
           
            // update password in UserProfile
            cn.Open();

            SqlCommand cmd2 = new SqlCommand(command, cn);
            int res = cmd2.ExecuteNonQuery();
            
            cn.Close();


            if (res == 0)
                return false;
            else
                return true;
        }



    }
}