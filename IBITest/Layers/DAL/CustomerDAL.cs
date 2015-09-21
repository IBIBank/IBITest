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

                command.CommandText = String.Format("INSERT INTO UserRoles VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', 'A') ", id, c.UserID, c.Password, "Customer",DateTime.Now.ToString(),"0");
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
                
                if(reader.HasRows)
                {
                    reader.Read();
                    requestID = Convert.ToInt16(reader[0]) + 1;
                }
                else
                    requestID = 1;
                
                reader.Close();

                command.CommandText = String.Format("SELECT CustomerName FROM Customer WHERE CustomerID = "+ request.CustomerID);
                reader = command.ExecuteReader();
                reader.Read();

                string customerName = reader[0].ToString();
                reader.Close();

                command.CommandText = String.Format("INSERT INTO NewAccountRequest(RequestID, BranchCode, CustomerID, SubmissionDate, Status, AddressProof, CustomerName) VALUES('{0}', '{1}', '{2}', '{3}', 'S', '{4}', '{5}') ", requestID,request.Branch, request.CustomerID, DateTime.Now.ToString(), request.AddresProof, customerName);
                
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
                    return accountsList;

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



        public int GetNoOfApprovedRequestsByCustomerID(long customerID)
        {
            int numberOfRequests=0;

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

                command.CommandText = String.Format("SELECT BranchName FROM Branch WHERE BranchCode = " + branchCode.ToString() );
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


    }
}