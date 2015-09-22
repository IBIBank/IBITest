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
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM Customer WHERE Token = '{0}' ", token), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    c.CustomerID = Convert.ToInt64(reader[0]);
                    c.CustomerName = String.Copy(reader[1].ToString());
                    c.DOB = Convert.ToDateTime(reader[2].ToString());
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

        public bool IsUniqueUserID(string userID)
        {
            bool result;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT UserID FROM UserRoles WHERE UserID = '{0}' ", userID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                    result = false;
                else
                    result = true;

                reader.Close();
            }

            return result;
        }



        public bool FinishReg(Customer c)
        {
            bool res;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                string cmdtxt = String.Format("UPDATE Customer SET UserID = '{0}', Password = '{1}', CommunicationAddress = '{2}', TransactionPassword = '{3}', PhotoIDProof = '{4}' WHERE CustomerID = {5}", c.UserID, c.Password, c.CommunicationAddress, c.TransactionPassword, c.PhotoIDProof, c.CustomerID);


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

                command.CommandText = String.Format("INSERT INTO UserRoles VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', 'A') ", id, c.UserID, c.Password, "Customer", DateTime.Now.ToString(), "0");
                command.ExecuteNonQuery();

            }

            return res;
        }


        public bool AddNewAccountRequest(NewAccountRequestView request)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                int requestID;

                SqlCommand command = new SqlCommand("SELECT MAX(RequestID) FROM NewAccountRequest ", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    requestID = Convert.ToInt16(reader[0]) + 1;
                }
                else
                    requestID = 1;

                reader.Close();

                command.CommandText = String.Format("SELECT CustomerName FROM Customer WHERE CustomerID = " + request.CustomerID);
                reader = command.ExecuteReader();
                reader.Read();

                string customerName = reader[0].ToString();
                reader.Close();

                command.CommandText = String.Format("INSERT INTO NewAccountRequest(RequestID, BranchCode, CustomerID, SubmissionDate, Status, AddressProof, CustomerName) VALUES('{0}', '{1}', '{2}', '{3}', 'S', '{4}', '{5}') ", requestID, request.Branch, request.CustomerID, DateTime.Now.ToString(), request.AddresProof, customerName);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }
            return result;
        }


        public long GetCustomerIDbyUserID(string userID)
        {
            long customerID;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT CustomerID FROM Customer WHERE UserID = '{0}' ", userID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                customerID = Convert.ToInt64(reader[0]);
                reader.Close();
            }

            return customerID;
        }


        public List<AccountListViewModel> GetAccountsListByCustomerID(long customerID)
        {
            List<AccountListViewModel> accountsList = new List<AccountListViewModel>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT AccountNumber, Balance FROM Account WHERE CustomerID = {0} ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                else
                {
                    while (reader.Read())
                    {
                        AccountListViewModel account = new AccountListViewModel();

                        account.accountNumber = Convert.ToInt64(reader[0]);
                        account.balance = Convert.ToDecimal(reader[1]);

                        accountsList.Add(account);
                    }
                    reader.Close();
                }
            }

            return accountsList;
        }


        public List<long> GetAccountsListAsLongListByCustomerID(long customerID)
        {
            List<long> accountsList = new List<long>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT AccountNumber FROM Account WHERE CustomerID = {0} ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                else
                {
                    while (reader.Read())
                    {
                        long account = Convert.ToInt64(reader[0]);
                        
                        accountsList.Add(account);
                    }
                    reader.Close();
                }
            }

            return accountsList;
        }



        public int GetNoOfApprovedRequestsByCustomerID(long customerID)
        {
            int numberOfRequests = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                // Transfer requests
                SqlCommand command = new SqlCommand(String.Format("SELECT COUNT(*) FROM BranchTransferRequest WHERE CustomerID = {0} AND Status = 'A' ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // Closure requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM ClosingRequest WHERE CustomerID = {0} AND Status = 'A' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // Loan requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM LoanRequest WHERE CustomerID = {0} AND Status = 'A' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // New Account requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM NewAccountRequest WHERE CustomerID = {0} AND Status = 'A' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }
            }

            return numberOfRequests;
        }




        public int GetNoOfRejectedRequestsByCustomerID(long customerID)
        {
            int numberOfRequests = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                // Transfer requests
                SqlCommand command = new SqlCommand(String.Format("SELECT COUNT(*) FROM BranchTransferRequest WHERE CustomerID = {0} AND Status = 'R' ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // Closure requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM ClosingRequest WHERE CustomerID = {0} AND Status = 'R' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // Loan requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM LoanRequest WHERE CustomerID = {0} AND Status = 'R' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // New Account requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM NewAccountRequest WHERE CustomerID = {0} AND Status = 'R' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }
            }

            return numberOfRequests;
        }



        public int GetNoOfPendingRequestsByCustomerID(long customerID)
        {
            int numberOfRequests = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                // Transfer requests
                SqlCommand command = new SqlCommand(String.Format("SELECT COUNT(*) FROM BranchTransferRequest WHERE CustomerID = {0} AND Status = 'S' ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // Closure requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM ClosingRequest WHERE CustomerID = {0} AND Status = 'S' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // Loan requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM LoanRequest WHERE CustomerID = {0} AND Status = 'S' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }


                // New Account requests

                command.CommandText = String.Format("SELECT COUNT(*) FROM NewAccountRequest WHERE CustomerID = {0} AND Status = 'S' ", customerID);
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    numberOfRequests += Convert.ToInt16(reader[0]);
                    reader.Close();
                }
            }

            return numberOfRequests;
        }


        public AddPayeeViewModel ValidatePayeeAccountNumber(long payeeAccountNumber, long sourceCustomerID)
        {
            AddPayeeViewModel payeeDetails = new AddPayeeViewModel();
            long payeeCustomerID, branchCode;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT PayeeID FROM Payee WHERE CustomerID = " + sourceCustomerID.ToString() + " AND PayeeAccountNumber = " + payeeAccountNumber.ToString(), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    return null;
                }

            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT CustomerID, BranchCode FROM Account WHERE AccountNumber = " + payeeAccountNumber.ToString() + " AND NOT Status = 'Closed' ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                    return null;
                else
                {
                    reader.Read();
                    payeeCustomerID = Convert.ToInt64(reader[0]);
                    branchCode = Convert.ToInt64(reader[1]);

                    reader.Close();
                }

                command.CommandText = String.Format("SELECT CustomerName FROM Customer WHERE CustomerID = " + payeeCustomerID.ToString());
                reader = command.ExecuteReader();
                reader.Read();

                payeeDetails.payeeName = reader[0].ToString();
                reader.Close();

                command.CommandText = String.Format("SELECT BranchName FROM Branch WHERE BranchCode = " + branchCode.ToString());
                reader = command.ExecuteReader();
                reader.Read();

                payeeDetails.branchName = reader[0].ToString();
                reader.Close();

                payeeDetails.payeeAccountNumber = payeeAccountNumber;
            }

            return payeeDetails;
        }



        public bool AddPayee(AddPayeeViewModel payeeDetails, long customerID)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                int payeeID;
                SqlCommand command = new SqlCommand("SELECT COUNT(PayeeID) FROM Payee", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                payeeID = Convert.ToInt16(reader[0]) + 1;

                reader.Close();

                command.CommandText = String.Format("INSERT INTO Payee VALUES('{0}', '{1}', '{2}', '{3}') ", payeeID, payeeDetails.payeeNickName, customerID, payeeDetails.payeeAccountNumber);

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }

            return result;
        }




        public List<long> GetAllSavingsAccountByCustomerID(long customerID)
        {
            List<long> savingsAccountList = new List<long>();
            long accountNumber;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT AccountNumber FROM Account WHERE CustomerID = {0} AND AccountType = 'S' ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                else
                {
                    while (reader.Read())
                    {
                        accountNumber = Convert.ToInt64(reader[0]);
                        savingsAccountList.Add(accountNumber);
                    }
                    reader.Close();
                }
            }

            return savingsAccountList;
        }



        public List<FundTransferPayeeModel> GetAllPayeeAccountByCustomerID(long customerID)
        {
            List<FundTransferPayeeModel> payeeAccountList = new List<FundTransferPayeeModel>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT PayeeNickName, PayeeAccountNumber FROM Payee WHERE CustomerID = {0} ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                else
                {
                    while (reader.Read())
                    {
                        FundTransferPayeeModel payeeDetails = new FundTransferPayeeModel();

                        payeeDetails.payeeNickName = reader[0].ToString();
                        payeeDetails.payeeAccountNumber = Convert.ToInt64(reader[1]);

                        payeeAccountList.Add(payeeDetails);
                    }
                    reader.Close();
                }
            }

            return payeeAccountList;
        }



        public bool ValidateTransactionPassword(long customerID, string password)
        {
            bool result = false;


            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT TransactionPassword FROM Customer WHERE CustomerID = {0} ", customerID), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    reader.Read();
                else
                    return false;

                string storedPassword = reader[0].ToString();
                reader.Close();

                return password.Equals(storedPassword);
            }
            return result;
        }


        public bool DoFundTransfer(FundTransferViewModel fundTransferDetails)
        {
            bool result = false;
            char accountType;

            // read balance of from account
            Decimal balance = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT Balance FROM Account WHERE AccountNumber = " + fundTransferDetails.FromAccount.ToString(), connection);
                SqlDataReader reader = command.ExecuteReader();


                reader.Read();
                balance = Convert.ToDecimal(reader[0]);

                reader.Close();
            }

            // debit amount from FromAccount

            balance -= fundTransferDetails.Amount;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(String.Format("UPDATE Account SET Balance = {0} WHERE AccountNumber = {1}", balance.ToString(), fundTransferDetails.FromAccount.ToString()), connection);
                command.ExecuteNonQuery();

            }

            //read balance and type of ToAccount

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT Balance, AccountType FROM Account WHERE AccountNumber = " + fundTransferDetails.ToAccount.ToString(), connection);
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                balance = Convert.ToDecimal(reader[0]);
                accountType = Convert.ToChar(reader[1]);


                reader.Close();
            }


            // credit based on type of ToAccount

            if (accountType == 'S')
                balance += fundTransferDetails.Amount;
            else
                balance -= fundTransferDetails.Amount;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(String.Format("UPDATE Account SET Balance = {0} WHERE AccountNumber = {1}", balance.ToString(), fundTransferDetails.ToAccount.ToString()), connection);
                command.ExecuteNonQuery();

            }


            if (balance == 0 && accountType == 'L')
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(String.Format("UPDATE Account SET Status = 'Closed' WHERE AccountNumber = {0} ", fundTransferDetails.ToAccount.ToString()), connection);
                    if (command.ExecuteNonQuery() > 0)
                        result = true;
                }
            }

            // Insert into Transactions Table

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                int transactionID;

                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Transactions", connection);
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                transactionID = Convert.ToInt16(reader[0]) + 1;
                reader.Close();

                command.CommandText = String.Format("INSERT INTO Transactions(TransactionID, Type, TransactionDate, Amount, TransactionRemarks, SrcAccount, DestAccount) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' )", transactionID.ToString(), "Fund Transfer", DateTime.Now.ToString(), fundTransferDetails.Amount.ToString(), fundTransferDetails.remarks, fundTransferDetails.FromAccount.ToString(), fundTransferDetails.ToAccount.ToString());

                if (command.ExecuteNonQuery() > 0)
                    result = true;

            }


            return result;
        }



        public int GetNoOfApprovedRequests(long customerID)
        {
            int approvedRequests = 0;

            //  account transfer requests
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(RequestID) FROM BranchTransferRequest WHERE Status = 'A' AND CustomerID = " + customerID.ToString(), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests = Convert.ToInt16(reader[0]);
                }

                reader.Close();


                // Closing requests

                command.CommandText = "SELECT COUNT(RequestID) FROM ClosingRequest WHERE Status = 'A' AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();



                // Loan Requests

                command.CommandText = "SELECT COUNT(RequestID) FROM LoanRequest WHERE Status = 'A' AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();


                // New Account Request

                command.CommandText = "SELECT COUNT(RequestID) FROM NewAccountRequest WHERE Status = 'A' AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();
            }

            return approvedRequests;
        }




        public int GetNoOfRejectedRequests(long customerID)
        {
            int approvedRequests = 0;

            //  account transfer requests
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(RequestID) FROM BranchTransferRequest WHERE Status = 'R' AND CustomerID = " + customerID.ToString(), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests = Convert.ToInt16(reader[0]);
                }

                reader.Close();


                // Closing requests

                command.CommandText = "SELECT COUNT(RequestID) FROM ClosingRequest WHERE Status = 'R' AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();



                // Loan Requests

                command.CommandText = "SELECT COUNT(RequestID) FROM LoanRequest WHERE Status = 'R' AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();


                // New Account Request

                command.CommandText = "SELECT COUNT(RequestID) FROM NewAccountRequest WHERE Status = 'R' AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();
            }

            return approvedRequests;
        }



        public int GetNoOfPendingRequests(long customerID)
        {
            int approvedRequests = 0;

            //  account transfer requests
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(RequestID) FROM BranchTransferRequest WHERE (Status = 'S' OR Status = 'T') AND CustomerID = " + customerID.ToString(), connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests = Convert.ToInt16(reader[0]);
                }

                reader.Close();


                // Closing requests

                command.CommandText = "SELECT COUNT(RequestID) FROM ClosingRequest WHERE (Status = 'S' OR Status = 'T') AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();



                // Loan Requests

                command.CommandText = "SELECT COUNT(RequestID) FROM LoanRequest WHERE (Status = 'S' OR Status = 'T') AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();


                // New Account Request

                command.CommandText = "SELECT COUNT(RequestID) FROM NewAccountRequest WHERE (Status = 'S' OR Status = 'T') AND CustomerID = " + customerID.ToString();

                reader = command.ExecuteReader();

                // Call Read before accessing data. 
                if (reader.HasRows)
                {
                    reader.Read();
                    approvedRequests += Convert.ToInt16(reader[0]);
                }

                reader.Close();
            }

            return approvedRequests;
        }




        public List<TransactionStatementViewModel> GetLastFiveTransactions(long accountNumber)
        {
            List<TransactionStatementViewModel> accountsList = new List<TransactionStatementViewModel>();
            int count = 0;
            

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Transactions WHERE SrcAccount = " + accountNumber.ToString() + " OR DestAccount = " + accountNumber.ToString() + " ORDER BY(TransactionID) DESC ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                else
                {
                    while (reader.Read() && (++count) <= 5)
                    {
                        TransactionStatementViewModel transactionDetails = new TransactionStatementViewModel();

                        transactionDetails.transactionID = Convert.ToInt16(reader[0]);

                        if (!reader.IsDBNull(6))
                        {
                            if (Convert.ToInt64(reader[5]) == accountNumber)
                                transactionDetails.transactionType = "Debit";
                            else
                                transactionDetails.transactionType = "Credit";
                        }
                        else
                            transactionDetails.transactionType = reader[1].ToString();

                        transactionDetails.transactionDate = Convert.ToDateTime(reader[2]);
                        transactionDetails.amount = Convert.ToDecimal(reader[3]);
                        transactionDetails.transactionRemarks = reader[4].ToString();

                        accountsList.Add(transactionDetails);
                    }
                    reader.Close();

                }
            }

            return accountsList;
        }


        public List<TransactionStatementViewModel> GetDetailedTransactions(long accountNumber, DateTime startDate, DateTime endDate)
        {
            List<TransactionStatementViewModel> accountsList = new List<TransactionStatementViewModel>();
            

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Transactions WHERE (SrcAccount = "+ accountNumber.ToString() + " OR DestAccount = " + accountNumber.ToString() + ") AND (TransactionDate >= '"+startDate + "' AND TransactionDate <= '" + endDate + "') ", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                else
                {
                    while (reader.Read())
                    {
                        TransactionStatementViewModel transactionDetails = new TransactionStatementViewModel();

                        transactionDetails.transactionID = Convert.ToInt16(reader[0]);

                        if (!reader.IsDBNull(6))
                        {
                            if (Convert.ToInt64(reader[5]) == accountNumber)
                                transactionDetails.transactionType = "Debit";
                            else
                                transactionDetails.transactionType = "Credit";
                        }
                        else
                            transactionDetails.transactionType = reader[1].ToString();

                        transactionDetails.transactionDate = Convert.ToDateTime(reader[2]);
                        transactionDetails.amount = Convert.ToDecimal(reader[3]);
                        transactionDetails.transactionRemarks = reader[4].ToString();

                        accountsList.Add(transactionDetails);
                    }
                    reader.Close();

                }
            }

            return accountsList;
        }




        public string ValidateAndSetPassword(long customerID, string oldPassword, string newPassword, string passwordType)
        {
            if (oldPassword.Equals(newPassword))
                return "Old and New Passwords cannot be same !";

            if(passwordType.Equals("userPassword"))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
                {
                    SqlCommand command = new SqlCommand(String.Format("SELECT Password, UserID FROM Customer WHERE CustomerID = {0} ", customerID), connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();                    
                    var storedPassword = reader[0].ToString();
                    var userID = reader[1].ToString();
                    reader.Close();

                    if (!storedPassword.Equals(oldPassword))
                        return "Old Password entered is not correct !";

                    // update password in Customer and User Profile tables

                    command.CommandText = String.Format("UPDATE Customer SET Password = '{0}' WHERE CustomerID = {1}", newPassword, customerID.ToString());
                    command.ExecuteNonQuery();                    

                    command.CommandText = String.Format("UPDATE UserRoles SET Password = '{0}' WHERE UserID = '{1}' ", newPassword, userID);
                    command.ExecuteNonQuery();

                }
            }

            else
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database1ConnectionString"].ToString()))
                {
                    SqlCommand command = new SqlCommand(String.Format("SELECT TransactionPassword FROM Customer WHERE CustomerID = {0} ", customerID), connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();
                    var storedPassword = reader[0].ToString();
                    reader.Close();

                    if (!storedPassword.Equals(oldPassword))
                        return "Password entered is not correct !";

                    // update password in Customer and User Profile tables

                    command.CommandText = String.Format("UPDATE Customer SET TransactionPassword = '{0}' WHERE CustomerID = {1}", newPassword, customerID.ToString());
                    command.ExecuteNonQuery();                 
                }
            }

            return "Success";
        }



    }
}