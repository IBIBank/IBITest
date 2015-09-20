using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IBITest.Models;
using System.Configuration;
using System.Globalization;

namespace IBITest.Layers.DAL
{
    public class BankerDAL
    {
        public int GetNoOfBranchTransferRequests()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM BranchTransferRequest WHERE Status = 'S' ", connection);
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
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM LoanRequest WHERE Status = 'S' ", connection);
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
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM NewAccountRequest WHERE Status = 'S' ", connection);
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
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM ClosingRequest WHERE Status = 'S' ", connection);
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




        public TokenInfo GenerateToken(GenerateTokenViewModel tv)
        {
            Customer c = new Customer();
            TokenInfo res = new TokenInfo();

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
                {
                    res.CustomerID = c.CustomerID;
                    res.Token = String.Copy(c.Token);

                }
                else
                    res.Token = String.Copy("Error");
            }

            return res;
        }


        public List<RequestViewModel> GetBranchTransferRequests()
        {
            List<RequestViewModel> mdL = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM BranchTransferRequest ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    mdL = new List<RequestViewModel>();
                    while (reader.Read())
                    {
                        RequestViewModel md = new RequestViewModel();

                        md.RequestID = String.Copy("TOA" + reader[0].ToString());
                        md.RequestType = String.Copy("TOA");
                        md.SubmissionDate = Convert.ToDateTime(reader[1].ToString());
                        if (reader[2].ToString() != "")
                            md.ServiceDate = Convert.ToDateTime(reader[2].ToString(), new DateTimeFormatInfo { FullDateTimePattern = "yyyy-mm-dd" });
                        md.Status = Convert.ToChar(reader[3].ToString());
                        md.CustomerName = reader[8].ToString();

                        mdL.Add(md);
                    }
                }                

                reader.Close();
            }

            return mdL;            
        }





        public List<RequestViewModel> GetClosingRequests()
        {
            List<RequestViewModel> mdL = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM ClosingRequest ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    mdL = new List<RequestViewModel>();
                    while (reader.Read())
                    {
                        RequestViewModel md = new RequestViewModel();

                        md.RequestID = String.Copy("COA" + reader[0].ToString());
                        md.RequestType = String.Copy("COA");
                        md.SubmissionDate = Convert.ToDateTime(reader[1].ToString());
                       
                        if(reader[2].ToString() != "")
                            md.ServiceDate = Convert.ToDateTime(reader[2].ToString());
                       
                        md.Status = Convert.ToChar(reader[3].ToString());
                        md.CustomerName = reader[6].ToString();

                        mdL.Add(md);
                    }
                }

                reader.Close();
            }

            return mdL;
        }





        public List<RequestViewModel> GetLoanRequests()
        {
            List<RequestViewModel> mdL = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM LoanRequest ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {

                    mdL = new List<RequestViewModel>();
                    while (reader.Read())
                    {
                        RequestViewModel md = new RequestViewModel();

                        md.RequestID = String.Copy("AFL" + reader[0].ToString());
                        md.RequestType = String.Copy("AFL");
                        md.SubmissionDate = Convert.ToDateTime(reader[3].ToString());

                        if (reader[2].ToString() != "")
                               md.ServiceDate = Convert.ToDateTime(reader[4].ToString());
                        md.Status = Convert.ToChar(reader[5].ToString());
                        md.CustomerName = reader[12].ToString();

                        mdL.Add(md);
                    }
                }

                reader.Close();
            }

            return mdL;
        }






        public List<RequestViewModel> GetNewAccountRequests()
        {
            List<RequestViewModel> mdL = null;
            

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM NewAccountRequest", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    mdL = new List<RequestViewModel>();
                    while (reader.Read())
                    {
                        RequestViewModel md = new RequestViewModel();

                        md.RequestID = "NAC" + reader[0].ToString();
                        md.RequestType = String.Copy("NAC");
                        md.SubmissionDate = Convert.ToDateTime(reader[3].ToString());

                        if (reader[4].ToString() != "")
                             md.ServiceDate = Convert.ToDateTime(reader[4].ToString());
                        md.Status = Convert.ToChar(reader[5].ToString());
                        md.CustomerName = reader[7].ToString();

                        mdL.Add(md);
                    }
                }

                reader.Close();
            }

            return mdL;
        }





        public List<RequestViewModel> GetAllRequests()
        {
            char fl = 'f';
            List<RequestViewModel> mdLA = new List<RequestViewModel>();

            List<RequestViewModel> mdLtemp = null;

            mdLtemp = GetLoanRequests();

            if (mdLtemp != null)
            {
                fl = 't';

                foreach (var v in mdLtemp)
                    mdLA.Add(v);
                mdLtemp.Clear();
            }

           

            mdLtemp = GetClosingRequests();

            if (mdLtemp != null)
            {
                fl = 't';

                foreach (var v in mdLtemp)
                    mdLA.Add(v);
                mdLtemp.Clear();

            }


            mdLtemp = GetNewAccountRequests();

            if (mdLtemp != null)
            {
                fl = 't';

                foreach (var v in mdLtemp)
                    mdLA.Add(v);
                mdLtemp.Clear();

            }


            mdLtemp = GetBranchTransferRequests();

            if (mdLtemp != null)
            {
                fl = 't';

                foreach (var v in mdLtemp)
                    mdLA.Add(v);
                mdLtemp.Clear();

            }


            if (fl == 'f')
                return null;

            return mdLA;
        }


        public Byte[] GetProofImageByNewAccountRequestID(int requestID)
        {            
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT AddressProof FROM NewAccountRequest WHERE RequestID = " + requestID.ToString(), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    Byte[] image = (Byte[])reader[0];

                    reader.Close();
                    return image;
                }
                else
                    return null;

            }
        }



        public bool ApproveNewAccountCreationRequest(int requestID)
        {
            long branchCode, customerID, accountNumber;
            bool result = false;
            Byte[] addressProof;
            
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();
                              
                SqlCommand command = new SqlCommand("SELECT BranchCode, CustomerID, AddressProof FROM NewAccountRequest WHERE RequestID = " + requestID.ToString(), connection);
                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                
                reader.Read();
                branchCode = Convert.ToInt64(reader[0]);
                customerID = Convert.ToInt64(reader[1]);

                /*
                if (reader[2] != null)
                    addressProof = (Byte[])reader[2];
                else */
                    addressProof = null;
               
                reader.Close();


                command.CommandText = String.Format("SELECT COUNT(*) FROM Account");
                reader = command.ExecuteReader();

                reader.Read();

                if (Convert.ToInt64(reader[0]) != 0)
                {
                    accountNumber = Convert.ToInt64(reader[0]) + 1;
                }
                else
                    accountNumber = 1;

                reader.Close();

                // create account in account table

                command.CommandText = String.Format("INSERT INTO Account VALUES('{0}','S','{1}','Active','500','{2}','{3}','{4}')",accountNumber,DateTime.Now.ToString(),branchCode,customerID,addressProof);
                command.ExecuteNonQuery();

                // set request as serviced

                command.CommandText = String.Format("UPDATE NewAccountRequest SET Status = 'A' WHERE RequestID = {0}",requestID);
                if (command.ExecuteNonQuery() > 0)
                    result = true;


            }
                       
            return result;
        }


        public bool ApproveLoanRequest(int requestID)
        {
            bool result = false;




            return result;
        }



        public bool TransferTransferOfAccountRequest(int requestID)
        {

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE BranchTransferRequest SET Status = 'T' WHERE RequestID = " +requestID.ToString(), connection);
                                
                if (command.ExecuteNonQuery() > 0)
                    result = true;
                
            }


            return result;
        }


      
        public bool TransferClosingOfAccountRequest(int requestID)
        {

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE ClosingRequest SET Status = 'T' WHERE RequestID = " + requestID.ToString(), connection);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }


            return result;
        }



        public bool RejectNewAccountCreationRequest(int requestID)
        {

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE NewAccountRequest SET Status = 'R' WHERE RequestID = " + requestID.ToString(), connection);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }



            return result;
        }


        public bool RejectLoanRequest(int requestID)
        {

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE LoanRequest SET Status = 'R' WHERE RequestID = " + requestID.ToString(), connection);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }



            return result;
        }


        public bool RejectTransferOfAccountRequest(int requestID)
        {

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE BranchTransferRequest SET Status = 'R' WHERE RequestID = " + requestID.ToString(), connection);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }



            return result;
        }


        public bool RejectClosingOfAccountRequest(int requestID)
        {

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE ClosingRequest SET Status = 'R' WHERE RequestID = " + requestID.ToString(), connection);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }



            return result;
        }



        public char GetAccountType(long accountNumber)
        {
            char accountType = 'I' ;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT AccountType FROM Account WHERE AccountNumber = " + accountNumber.ToString(), connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    accountType = (char)reader[0];
                }
                reader.Close();
            }                      
  
            return accountType;
        }


        public Decimal GetAccountBalance(long accountNumber)
        {
            Decimal balance = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT Balance FROM Account WHERE AccountNumber = " + accountNumber.ToString(), connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    balance = (Decimal)reader[0];
                }
                reader.Close();
            }          


            return balance;
        }




        public bool SetAccountBalance(long accountNumber, Decimal finalBalance)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(String.Format("UPDATE Account SET Balance = {0} WHERE AccountNumber = {1}" , finalBalance.ToString(), accountNumber.ToString()));

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }

            return result;
        }
        
        
    }
}