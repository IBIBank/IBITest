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
        public long GetBranchCodeByBankerID(string branchLogInID)
        {
            long branchCode = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT BranchCode FROM Branch WHERE BranchLogInID =  '" + branchLogInID + "' ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                                
                reader.Read();
                branchCode = Convert.ToInt64(reader[0]);          

                reader.Close();
            }

            return branchCode;

        }




        public int GetNoOfBranchTransferRequests(long branchCode)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM BranchTransferRequest WHERE Status = 'S' AND FromBranch = " + branchCode.ToString(), connection);
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


        public int GetNoOfLoanRequests(long branchCode)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM LoanRequest WHERE Status = 'S' AND BranchCode = " + branchCode.ToString(), connection);
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



        public int GetNoOfNewAccountRequests(long branchCode)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM NewAccountRequest WHERE Status = 'S' AND BranchCode = " + branchCode.ToString(), connection);
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




        public int GetNoOfClosureRequests(long branchCode)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM ClosingRequest WHERE Status = 'S' AND BranchCode = " + branchCode.ToString(), connection);
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


        public List<RequestViewModel> GetBranchTransferRequests(long branchCode)
        {
            List<RequestViewModel> mdL = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM BranchTransferRequest WHERE FromBranch = " + branchCode.ToString(), connection);
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





        public List<RequestViewModel> GetClosingRequests(long branchCode)
        {
            List<RequestViewModel> mdL = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM ClosingRequest WHERE BranchCode = "+branchCode.ToString(), connection);
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





        public List<RequestViewModel> GetLoanRequests(long branchCode)
        {
            List<RequestViewModel> mdL = null;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM LoanRequest WHERE BranchCode = " + branchCode.ToString(), connection);
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






        public List<RequestViewModel> GetNewAccountRequests(long branchCode)
        {
            List<RequestViewModel> mdL = null;
            

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM NewAccountRequest WHERE BranchCode = "+branchCode.ToString(), connection);
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





        public List<RequestViewModel> GetAllRequests(long branchCode)
        {
            char fl = 'f';
            List<RequestViewModel> mdLA = new List<RequestViewModel>();

            List<RequestViewModel> mdLtemp = null;

            mdLtemp = GetLoanRequests(branchCode);

            if (mdLtemp != null)
            {
                fl = 't';

                foreach (var v in mdLtemp)
                    mdLA.Add(v);
                mdLtemp.Clear();
            }



            mdLtemp = GetClosingRequests(branchCode);

            if (mdLtemp != null)
            {
                fl = 't';

                foreach (var v in mdLtemp)
                    mdLA.Add(v);
                mdLtemp.Clear();

            }


            mdLtemp = GetNewAccountRequests(branchCode);

            if (mdLtemp != null)
            {
                fl = 't';

                foreach (var v in mdLtemp)
                    mdLA.Add(v);
                mdLtemp.Clear();

            }


            mdLtemp = GetBranchTransferRequests(branchCode);

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

                SqlCommand command = new SqlCommand("SELECT AccountType FROM Account WHERE AccountNumber = " + accountNumber.ToString() + " AND NOT Status = 'Closed' ", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    accountType = Convert.ToChar(reader[0]);
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
                    balance = Convert.ToDecimal(reader[0]);
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

                SqlCommand command = new SqlCommand(String.Format("UPDATE Account SET Balance = {0} WHERE AccountNumber = {1}" , finalBalance.ToString(), accountNumber.ToString()),connection);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }

            return result;
        }




        public bool CreateAccountTransactionByBanker(long sourceAccount, string transactionType, Decimal amount, string transactionRemarks)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                int transactionID;

                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Transactions",connection);
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                transactionID = Convert.ToInt16(reader[0]) + 1;
                reader.Close();                

                command.CommandText = String.Format("INSERT INTO Transactions(TransactionID, Type, TransactionDate, Amount, TransactionRemarks, SrcAccount) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}' )",transactionID.ToString(), transactionType, DateTime.Now.ToString(), amount, transactionRemarks, sourceAccount.ToString()  );

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }

            return result;
        }





        public bool CreditSavingsAccountByBanker(long sourceAccount, Decimal amount, string transactionRemarks)
        {
            bool result = false;
            Decimal currentBalance;


            currentBalance = GetAccountBalance(sourceAccount);

            currentBalance += amount;
            result = (SetAccountBalance(sourceAccount, currentBalance) && CreateAccountTransactionByBanker(sourceAccount, "Credit", amount, transactionRemarks));
                
            return result;
        }

        public bool DebitSavingsAccountByBanker(long sourceAccount, Decimal amount, string transactionRemarks)
        {
            bool result = false;
            Decimal currentBalance;
            
            currentBalance = GetAccountBalance(sourceAccount);

            currentBalance -= amount;
            result = (SetAccountBalance(sourceAccount, currentBalance) && CreateAccountTransactionByBanker(sourceAccount, "Debit", amount, transactionRemarks));
                
            return result;
        }

        public bool CreditLoanAccountByBanker(long sourceAccount, Decimal amount, string transactionRemarks)
        {
            bool result = false;
            Decimal currentBalance;
            
            currentBalance = GetAccountBalance(sourceAccount);

            currentBalance -= amount;
            result = (SetAccountBalance(sourceAccount, currentBalance) && CreateAccountTransactionByBanker(sourceAccount, "Credit", amount, transactionRemarks));
                
            return result;
        }


        public bool CloseLoanAccount(long accountNumber)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();
                                
                SqlCommand command = new SqlCommand(String.Format("UPDATE Account SET Status = 'Closed' WHERE AccountNumber = {0} ", accountNumber.ToString() ), connection);
                if (command.ExecuteNonQuery() > 0)
                    result = true;
            }

            return result;
        }


        public List<SearchCustomerViewModel> GetCustomerByName(string customerName, long branchCode)
        {
            List<SearchCustomerViewModel> customersList = new List<SearchCustomerViewModel>();
            long customerID;
            

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()), connection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();
                connection2.Open();

                SqlCommand command = new SqlCommand(String.Format("SELECT CustomerID, CustomerName, PermanentAddress, CommunicationAddress, ContactNumber, Email FROM Customer WHERE CustomerName LIKE '%{0}%' ", customerName), connection);
                SqlDataReader reader = command.ExecuteReader(), reader2;
                SqlCommand command2 = new SqlCommand("", connection2);



                if (!reader.HasRows)
                    return null;

                while (reader.Read())
                {
                    customerID = Convert.ToInt64(reader[0]);
                    string actualCustomerName = reader[1].ToString();
                    string permanentAddress = reader[2].ToString();
                    string communicationAddress = reader[3].ToString();
                    string contactNumber = reader[4].ToString();
                    string email = reader[5].ToString();

                    
                    command2.CommandText = String.Format("SELECT AccountNumber FROM Account WHERE CustomerID = {0} AND BranchCode = {1} AND Status = 'Active' ",customerID.ToString(), branchCode.ToString());
                    reader2 = command2.ExecuteReader();

                    if(reader2.HasRows)
                        while (reader2.Read())
                        {
                            SearchCustomerViewModel customerDetails = new SearchCustomerViewModel();

                            customerDetails.accountNumber = reader2[0].ToString();
                            customerDetails.customerName = String.Copy(actualCustomerName);
                            customerDetails.permanentAddress = String.Copy(permanentAddress);
                            customerDetails.communicationAddress = String.Copy(communicationAddress);
                            customerDetails.contactNumber = String.Copy(contactNumber);
                            customerDetails.email = String.Copy(email);

                            customersList.Add(customerDetails);
                        }
                      
                 
                    reader2.Close();
                }

            }

            return customersList;
        }


    }
}