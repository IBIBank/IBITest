using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IBITest.Models;
using System.Configuration;

namespace IBITest.Layers.DAL
{
    public class BankerDAL
    {
        public int GetNoOfBranchTransferRequests()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM BranchTransferRequest", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    count = Convert.ToInt16(reader[0]);
                }

                else
                    count = 0;

                reader.Close();
            }
            return count;
        }


        public int GetNoOfLoanRequests()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM LoanRequest", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    count = Convert.ToInt16(reader[0]);
                }

                else
                    count = 0;

                reader.Close();
            }
            return count;
        }



        public int GetNoOfNewAccountRequests()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM NewAccountRequest", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    count = Convert.ToInt16(reader[0]);
                }

                else
                    count = 0;

                reader.Close();
            }
            return count;
        }




        public int GetNoOfClosureRequests()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM ClosingRequest", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    count = Convert.ToInt16(reader[0]);
                }

                else
                    count = 0;

                reader.Close();
            }
            return count;
        }


        public BranchDetailsViewModel GetSelfBranchDetails(string BankerID)
        {            
            BranchDetailsViewModel bd = new BranchDetailsViewModel();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT BranchName,CityName,Address,ContactNumber, Email FROM Branch WHERE BranchLogInID = '{0}'",BankerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    bd.BranchName = reader[0].ToString();
                    bd.CityName = reader[1].ToString();
                    bd.Address = reader[2].ToString();
                    bd.ContactNumber = reader[3].ToString();
                    bd.Email = reader[4].ToString();
                }

                else
                    System.Windows.Forms.MessageBox.Show("Error in accessing Branch Table");

                reader.Close();
            }

            return bd;
        }




        public string GenerateToken(GenerateTokenViewModel tv)
        {
            Customer c = new Customer();
            string res;

            c.CustomerName = String.Copy(tv.CustomerName);
            c.DOB = tv.DOB;
            c.PermanentAddress = String.Copy(tv.PermanentAddress);
            c.ContactNumber = String.Copy(tv.ContactNumber);
            c.Email = String.Copy(tv.Email);
            
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT MAX(CustomerID) FROM Customer", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    c.CustomerID = Convert.ToInt64(reader[0]) + 1;
                }

                else
                    System.Windows.Forms.MessageBox.Show("Could Not access Customer Table !");

                reader.Close();
            }

            Random rnd = new Random();
            int num = rnd.Next(9999);
            c.Token = String.Copy("IBI" + c.CustomerID + num);

            c.UserID = null;
            c.Password = null;
            c.CommunicationAddress = null;
            c.TransactionPassword = null;
            c.PhotoIDProof = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("INSERT INTO Customer VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", c.CustomerID, c.CustomerName, c.DOB, c.UserID,c.Password, c.PermanentAddress, c.CommunicationAddress, c.ContactNumber, c.Email, c.TransactionPassword, c.Token, c.PhotoIDProof), connection);
                connection.Open();

                int rowsaffected = command.ExecuteNonQuery();

                // Call Read before accessing data. 
                if (rowsaffected > 0)
                    res = String.Copy(c.Token);
                else
                    res = String.Copy("Error");
            }

            return res;
        }
    }
}