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
        public void GetNewAccountRequests()
        {
           
        }

        public string GenerateToken(GenerateTokenViewModel tv)
        {
            Customer c = new Customer();

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

            c.Token = String.Copy("IBI" + c.CustomerID);

            c.UserID = null;
            c.Password = null;
            c.CommunicationAddress = null;
            c.TransactionPassword = null;
            c.PhotoIDProof = null;
/*
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("INSERT INTO Customer VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", BranchCode, bd.BranchName, bd.CityName, bd.Address, bd.ContactNumber, bd.BankerName, bd.BranchLogInID, bd.BranchLogInPassword, bd.Email), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                    res = false;
                else
                    res = true;

                reader.Close();
            }*/

            return c.Token;
        }
    }
}