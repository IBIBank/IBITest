using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using IBITest.Models;
using System.Text;
using System.Security.Cryptography;
using System.Web.SessionState;

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


        public BranchDetailsViewModel GetBranchDetails(long BranchCode)
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


        public string CheckValidation(string allowedRole, HttpSessionStateBase Session)
        {
            if (Session["User"] == null)
                return "LogIn";

            string currentRole = (Session["User"] as UserRole).role;

            if (allowedRole.Contains(currentRole))
                return "Authorised";
            else
                return "Unauthorised";
        }



        public string CheckRole(string UserID, string Password)
        {
            string res;
            CommonDAL commonDALObj = new CommonDAL();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString());
            SqlCommand command = new SqlCommand(String.Format( "SELECT Password, Role,FailCount,Status FROM UserRoles WHERE UserID = '{0}' ",  UserID), connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            int failCount;

            if (!reader.HasRows)
            {
                res = String.Copy("DoesNotExist");
            }
            else
            { // account exists
                reader.Read();

                if (reader[3].ToString().Equals("L"))  // Account is locked
                {
                    res = String.Copy("Locked");
                }
                else
                {
                    //Account is active
                    if (reader[0].ToString().Equals(commonDALObj.GetHashedText(Password)))
                    {
                        //correct password
                        res = String.Copy(reader[1].ToString());
                        
                        //update last log in in UserRoles and set FailCount = 0
                        reader.Close();

                        command.CommandText = "UPDATE UserRoles SET FailCount = 0, LastLogInDate = '" + DateTime.Now.ToString() + "' WHERE UserID = '" + UserID + "'";
                        command.ExecuteNonQuery();  
                    }

                    else
                    {
                        //Account is active but wrong password !
                        if (reader[1].ToString().Equals("Customer"))
                        {
                            failCount = Convert.ToInt16(reader[2]) + 1;

                            if (failCount == 3)
                            {
                                // lock customer account and report
                                reader.Close();

                                command.CommandText = "UPDATE UserRoles SET FailCount = 3, Status = 'L' WHERE UserID = '" + UserID + "'";
                                command.ExecuteNonQuery();

                                res = String.Copy("Account has been locked. Contact your banker to unlock !");


                            }
                            else
                            {
                                // update failcount in UserRoles and warn
                                reader.Close();

                                command.CommandText = "UPDATE UserRoles SET FailCount = " + failCount.ToString() + " WHERE UserID = '" + UserID + "'";
                                command.ExecuteNonQuery();

                                res = String.Copy("Incorrect Password ! " + (3 - failCount).ToString() + " more attempts remaining.");

                            }
                        }
                        else
                        { // Admin or banker entered incorrect password
                            res = String.Copy("Invalid");
                        }

                    }
                } 
                
            }
            connection.Close();

            return res;
        }


        public void Sync()
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                string cmdtxt = "DELETE FROM UserRoles";
                SqlCommand command = new SqlCommand(cmdtxt, connection);
                connection.Open();
                CommonDAL commonDALObj = new CommonDAL();
                int rowaff = command.ExecuteNonQuery();


                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('1','Adminnnn', '"+commonDALObj.GetHashedText("A1@nnnnn")+"','Admin','2015/2/2','0','A')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('2','Banker11','" + commonDALObj.GetHashedText("B1@nnnnn") + "','Banker','2015/2/2','0','A')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('3','Customer1','" + commonDALObj.GetHashedText("C1@nnnnn") + "','Customer','2015/2/2','0','A')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO UserRoles VALUES('4','Banker22','" + commonDALObj.GetHashedText("B1@nnnnn") + "','Banker','2015/2/2','0','A')");
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

                command.CommandText = String.Copy("INSERT INTO Branch VALUES('1','Branch1','City1','Address1','11','Banker11','Banker11','"+commonDALObj.GetHashedText("B1@nnnnn")+"','e@m.l')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO Branch VALUES('2','Branch2','City2','Address1','11','Banker22','Banker22','" + commonDALObj.GetHashedText("B1@nnnnn") + "','e@m.l')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM Customer");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("INSERT INTO Customer (CustomerID, CustomerName,DOB, UserID, Password, PermanentAddress, CommunicationAddress, ContactNumber, Email, TransactionPassword, Token) VALUES('1001','Customer','2015/12/12','Customer1','"+commonDALObj.GetHashedText("customer1") + "','PAddress','CAddress','11','e@m.l','"+commonDALObj.GetHashedText("tpassword")+"','IBI1234')");
                rowaff = command.ExecuteNonQuery();

                command.CommandText = String.Copy("DELETE FROM BranchTransferRequest");
                rowaff = command.ExecuteNonQuery();

                
                //insert into user profile too !!
            }
        }

        public string GetHashedText(string inputData)
        {
            byte[] tmpSource;
            byte[] tmpData;
            tmpSource = ASCIIEncoding.ASCII.GetBytes(inputData);
            tmpData = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return Convert.ToBase64String(tmpData);
        }


    }
}