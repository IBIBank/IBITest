﻿using System;
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

            SqlCommand cmd = new SqlCommand(String.Format( "SELECT BranchCode, BranchName FROM Branch WHERE CityName = '{0}'" ,CityName), cn);
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


        public void Sync()
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                string cmdtxt = "DELETE FROM UserRoles";
                SqlCommand command = new SqlCommand(cmdtxt, connection);
                connection.Open();
                int rowaff = command.ExecuteNonQuery();


                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('1','admin','admin','admin')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('2','banker1','banker1','banker')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('3','customer1','customer1','customer')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('4','banker2','banker2','banker')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM BranchTransferRequest");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM ClosingRequest");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM LoanRequest");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM NewAccountRequest");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM Branch");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO Branch VALUES('1','Branch1','City1','Address1','11','Banker1','banker1','banker1','e@m.l')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO Branch VALUES('2','Branch2','City2','Address1','11','Banker1','banker2','banker2','e@m.l')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM Customer");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO Customer (CustomerID, CustomerName,DOB, UserID, Password, PermanentAddress, CommunicationAddress, ContactNumber, Email, TransactionPassword, Token) VALUES('1','Customer','2015/12/12','customer1','customer1','PAddress','CAddress','11','e@m.l','tpassword','IBI1234')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM BranchTransferRequest");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO BranchTransferRequest (RequestID, SubmissionDate,Status, AccountNumber, FromBranch, ToBranch, CustomerID) VALUES(1,'2015/12/12','T',1,1,2,1)");
                rowaff = command.ExecuteNonQuery();
                command.CommandText = String.Copy("INSERT INTO BranchTransferRequest (RequestID, SubmissionDate,Status, AccountNumber, FromBranch, ToBranch, CustomerID) VALUES(2,'2015/12/12','T',2,2,1,2)");
                rowaff = command.ExecuteNonQuery();
                command.CommandText = String.Copy("INSERT INTO BranchTransferRequest (RequestID, SubmissionDate,Status, AccountNumber, FromBranch, ToBranch, CustomerID) VALUES(3,'2015/12/12','S',3,3,1,3)");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO NewAccountRequest (RequestID, BranchCode,CustomerID, SubmissionDate, ServiceDate, Status, CustomerName) VALUES('1','1','1','2015/9/17', '2015/2/2', 'S', 'Customer')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO NewAccountRequest (RequestID, BranchCode,CustomerID, SubmissionDate, ServiceDate, Status, CustomerName) VALUES('2','2','2','2016/9/17', '2016/2/2', 'A', 'Customer')");
                rowaff = command.ExecuteNonQuery();

                if(rowaff==0)
                    System.Windows.Forms.MessageBox.Show(rowaff.ToString());

                //insert into user profile too !!
            }
        }
    }
}