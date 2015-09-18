using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBITest.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace IBITest.Layers.DAL
{
    public class CustomerDAL
    {
        public Customer GetUserByTokenID(string token)
        {
            Customer c = new Customer();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format( "SELECT * FROM Customer WHERE Token = '{0}' ",token), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    c.CustomerID = Convert.ToInt64( reader[0] );
                    c.CustomerName = String.Copy(reader[1].ToString());
                    c.DOB = Convert.ToDateTime(reader[2].ToString()) ;
                    c.PermanentAddress = String.Copy(reader[5].ToString());
                    c.ContactNumber = String.Copy(reader[7].ToString());
                    c.Email = String.Copy(reader[8].ToString());
                }

                else
                    System.Windows.Forms.MessageBox.Show("Could Not access Customer Table !");

                reader.Close();
            }

            return c;
        }


        public bool ValidateToken(string token)
        {
            bool res;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM Customer WHERE Token = '{0}' ", token), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                    res = true;
     
                else
                    res = false;

                reader.Close();
            }

            return res;
        }

        public bool IsUniqueUserID(string UserID)
        {
            bool res;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT UserID FROM UserRoles WHERE UserID = '{0}' ", UserID), connection);
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



        public bool FinishReg(Customer c)
        {
            bool res;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                string cmdtxt = String.Format("UPDATE Customer SET UserID = '{0}', Password = '{1}', CommunicationAddress = '{2}', TransactionPassword = '{3}', PhotoIDProof = '{4}' WHERE CustomerID = {5}", c.UserID, c.Password,c.CommunicationAddress,c.TransactionPassword, c.PhotoIDProof, c.CustomerID);

                
                SqlCommand command = new SqlCommand(cmdtxt, connection);
                connection.Open();
                int rowaff = command.ExecuteNonQuery();


                // Call Read before accessing data. 
                if (rowaff == 0)
                    res = false;
                else
                    res = true;

                command.CommandText = String.Format("SELECT MAX(Id) FROM UserRoles ");

                SqlDataReader rd = command.ExecuteReader();
                rd.Read();
                int id = Convert.ToInt16(rd[0]) + 1;
                rd.Close();                

                command.CommandText = String.Format("INSERT INTO UserRoles VALUES('{0}', '{1}', '{2}', '{3}') ", id, c.UserID, c.Password, "Customer");
                command.ExecuteNonQuery();
                
                //insert into user profile too !!
            }

            return res;
        }



        public 


    }
}