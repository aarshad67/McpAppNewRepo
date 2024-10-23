using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MCPApp
{
    public class MeltonData
    {
        Logger logger = new Logger();
        public string connStr = ConfigurationManager.ConnectionStrings["MCPApp.Properties.Settings.MeltonDoncreteDBConnectionString"].ToString();
        string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
        public char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        #region Connection and Security

        public int SwitchPasswordEncryption()
        {
            Crypto c = new Crypto();
            DataTable dt = new DataTable();
            int counter = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {

                    try
                    {
                        conn.Open();
                        string qry = "SELECT username FROM dbo.Users ORDER BY userName";

                        SqlCommand cmd = new SqlCommand(qry, conn);
                       
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                        string username = "";
                        string encryptedPwd1 = "";
                        string encryptedPwd2 = "";
                        string decryptedPwd = "";
                        
                        foreach (DataRow dr in dt.Rows)
                        {
                            username = dr["username"].ToString();

                            encryptedPwd1 = GetPassword(username);
                            decryptedPwd = c.Decrypt(GetPassword(username));
                            encryptedPwd2 = c.WebEncrypt(decryptedPwd);
                            if(UpdatePassword(username, encryptedPwd2))
                            {
                                counter++;
                            }
                            
                        }
                        return counter;


                    }
                    catch (Exception ex)
                    {
                        string audit = CreateErrorAudit("MeltonData.cs", "SwitchPasswordEncryption()", ex.Message);
                        return counter;
                        
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int CheckSQLConnection()
        {
            int maxSeqNo = 0;
            string qry = "SELECT 1";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    return 0;
                }

                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                maxSeqNo = 1;
                            }
                        }
                    }
                    return maxSeqNo;
                }
                catch (Exception ex)
                {
                    string msg = "CheckSQLConnection() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CheckSQLConnection()", msg);
                    return 0;
                }

            }
        }

        public bool UpdatePassword(string userID, string newPwd)
        {

            string updateCommand = "UPDATE dbo.Users SET Password=@newPwd WHERE username = @userid";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = updateCommand;
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@newPwd", newPwd);
                        command.Parameters.AddWithValue("@userid", userID);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    string msg = "UpdatePassword() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdatePassword(...)", msg);
                    return false;
                }

            }
        }

        public bool UpdateWebPassword(string userName,string newPwd)
        {

            string updateCommand = "UPDATE dbo.WebCredentials SET webPassword=@webPassword WHERE webUserName = @webUserName";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = updateCommand;
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@webPassword", newPwd);
                        command.Parameters.AddWithValue("@webUserName", userName);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    string msg = "UpdateWebPassword() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateWebPassword(...)", msg);
                    return false;
                }

            }
        }

        public string GetPassword(string username)
        {

            string pwd = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT Password FROM dbo.Users WHERE Username = '{0}'", username), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pwd = reader["Password"].ToString();
                            }
                        }
                    }
                    return pwd;
                }
                catch (Exception ex)
                {
                    string msg = "GetPassword() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetPassword({0})", username), msg);
                    return "";
                }

            }
        }

        #endregion

        #region Users

        public DataTable GetAllUsers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Users ORDER BY userID";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllUsers()", ex.Message);
                    return null;
                }

            }

        }

        public string CreateMCPUser(string userID, string username, string fullName, string emailAddress, string telNo, string managerFlag, string guestFlag, string ipAddress)
        {

            string insertQry = "INSERT INTO dbo.Users("
                                            + "userID,username,fullName,emailAddress,telNo,managerFlag,ipAddress,guestFlag) "
                                            + "VALUES("
                                            + "@userID,"
                                            + "@username,"
                                            + "@fullName,"
                                            + "@emailAddress,"
                                            + "@telNo,"
                                            + "@managerFlag,"
                                            + "@ipAddress,"
                                            + "@guestFlag)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("userID", userID));
                        command.Parameters.Add(new SqlParameter("username", username));
                        command.Parameters.Add(new SqlParameter("fullName", fullName));
                        command.Parameters.Add(new SqlParameter("emailAddress", emailAddress));
                        command.Parameters.Add(new SqlParameter("telNo", telNo));
                        command.Parameters.Add(new SqlParameter("managerFlag", managerFlag));
                        command.Parameters.Add(new SqlParameter("guestFlag", guestFlag));
                        command.Parameters.Add(new SqlParameter("ipAddress", ipAddress));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateMCPUser(....)", ex.Message.ToString());
                    return String.Format("Message = {0}", ex.Message.ToString());
                }

            }
        }

        public string UpdateMCPUser(string userID, string username, string fullName, string emailAddress, string telNo, string managerFlag, string guestFlag, string ipAddress)
        {

            string updateQry = "UPDATE dbo.Users "
                                    + "SET username = @username, "
                                    + "fullName = @fullName, "
                                    + "emailAddress = @emailAddress, "
                                    + "telNo = @telNo, "
                                    + "managerFlag = @managerFlag, "
                                    + "ipAddress = @ipAddress, "
                                    + "guestFlag = @guestFlag"
                                    + " WHERE userID = @userID";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(updateQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("userID", userID));
                        command.Parameters.Add(new SqlParameter("username", username));
                        command.Parameters.Add(new SqlParameter("fullName", fullName));
                        command.Parameters.Add(new SqlParameter("emailAddress", emailAddress));
                        command.Parameters.Add(new SqlParameter("telNo", telNo));
                        command.Parameters.Add(new SqlParameter("managerFlag", managerFlag));
                        command.Parameters.Add(new SqlParameter("guestFlag", guestFlag));
                        command.Parameters.Add(new SqlParameter("ipAddress", ipAddress));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateMCPUser(....)", ex.Message.ToString());
                    return String.Format("Message = {0}", ex.Message.ToString());
                }

            }
        }

        public int DeleteUser(string userID)
        {

            string qry = String.Format("DELETE FROM dbo.Users WHERE userID = '{0}'", userID);

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = qry;
            cmd.Connection = conn;
            conn.Open();
            int numDeleted = cmd.ExecuteNonQuery();
            conn.Close();
            return numDeleted;


        }

        public bool IsUserExists(string userID)
        {
            string error;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Users WHERE userID = '{0}'", userID), conn))
                    {
                        Int32 numUsersFound = (Int32)command.ExecuteScalar();

                        if (numUsersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsUserExists({0})", userID), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsUserManager(string userID)
        {
            string error;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT count(*) FROM dbo.Users WHERE userID = '{userID}' and managerFlag = 'Y'", conn))
                    {
                        Int32 numUsersFound = (Int32)command.ExecuteScalar();

                        if (numUsersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", $"IsUserManager({userID})", ex.Message.ToString());
                    return false;
                }

            }
        }

        public string GetUserFullNameFromUserName(string username)
        {
            string error;
            string fullname = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT FullName FROM dbo.Users WHERE username = '{0}'", username), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fullname = reader["FullName"].ToString();
                            }
                        }
                    }
                    return fullname;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetUserFullNameFromUserName({0})", username), ex.Message.ToString());
                    return "*** UNKWOWN USERNAME***";
                }

            }
        }

        public string GetUserFullNameFromUserID(string userID)
        {
            string error;
            string fullname = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT FullName FROM dbo.Users WHERE userID = '{0}'", userID), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fullname = reader["FullName"].ToString();
                            }
                        }
                    }
                    return fullname;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetUserFullNameFromUserID({0})", userID), ex.Message.ToString());
                    return "*** UNKWOWN USER ID***";
                }

            }
        }

        public string GetUserIDFromUserName(string username)
        {
            string error;
            string userID = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT userID FROM dbo.Users WHERE username = '{0}'", username), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userID = reader["userID"].ToString();
                            }
                        }
                    }
                    return userID;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetUserIDFromUserName({0})", username), ex.Message.ToString());
                    return "*** UNKWOWN USERNAME***";
                }

            }
        }

        public bool UserNameExists(string username)
        {
            string error;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Users WHERE username = '{0}'", username), conn))
                    {
                        Int32 numUsersFound = (Int32)command.ExecuteScalar();

                        if (numUsersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UserNameExists({0})", username), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool UpdateUserLastLoggedOn(string userID)
        {

            string error = "";
            try
            {
                string qry = "UPDATE dbo.Users SET lastLoggedOn = @timeNow WHERE userID = @userID";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("userID", userID));
                        command.Parameters.Add(new SqlParameter("timeNow", DateTime.Now));
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                error = String.Format("Error Message = {0}", ex.Message.ToString());
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateUserLastLoggedOn({0})", userID), ex.Message.ToString());
                return false;
            }



        }

        public string ResetPassword(string userID)
        {
            try
            {
                string qry = "UPDATE dbo.Users SET password = '' WHERE userID = @userID ";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("userID", userID));
                        command.ExecuteNonQuery();
                    }
                }
                return "OK";
            }
            catch (Exception ex)
            {
                string error = CreateErrorAudit("MeltonData.cs", $"ResetPassword{userID}", ex.Message.ToString());
                return $"Error resetting password - {error}";
            }



        }

        public DataTable GetUserByUserID(string userID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = String.Format("SELECT * FROM dbo.Users WHERE userID = '{0}'", userID);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetUserByUserID({0})", userID), ex.Message.ToString());
                    return null;
                }

            }

        }

        #endregion

        #region Customers

        public string CreateCustomer(string custCode, string custName, string contactName, string contactEmail, string contactTel, string contactMobile, string tempCustCode, string nonExistingCustomer)
        {

            string insertQry = "INSERT INTO dbo.Customer("
                                            + "custCode,custName,contactName,contactEmail,contactTel,contactMobile,tempCustCode,nonExistingCustomer) "
                                            + "VALUES("
                                            + "@custCode,"
                                            + "@custName,"
                                            + "@contactName,"
                                            + "@contactEmail,"
                                            + "@contactTel,"
                                            + "@contactMobile,"
                                            + "@tempCustCode,"
                                            + "@nonExistingCustomer)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("custName", custName));
                        command.Parameters.Add(new SqlParameter("contactName", contactName));
                        command.Parameters.Add(new SqlParameter("contactEmail", contactEmail));
                        command.Parameters.Add(new SqlParameter("contactTel", contactTel));
                        command.Parameters.Add(new SqlParameter("contactMobile", contactMobile));
                        command.Parameters.Add(new SqlParameter("tempCustCode", tempCustCode));
                        command.Parameters.Add(new SqlParameter("nonExistingCustomer", nonExistingCustomer));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = "CreateCustomer() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateCustomer(...)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateCustomer(string custCode, string custName, string contactName, string contactEmail, string contactTel, string contactMobile, string tempCustCode, string nonExistingCustomer)
        {
            string updateQry = "UPDATE dbo.Customer "
                                    + "SET custName = @custName, "
                                    + "contactName = @contactName, "
                                    + "contactEmail = @contactEmail, "
                                    + "contactTel = @contactTel, "
                                    + "contactMobile = @contactMobile, "
                                    + "tempCustCode = @tempCustCode, "
                                    + "nonExistingCustomer = @nonExistingCustomer"
                                    + " WHERE custCode = @custCode";





            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    // insert into QuoteHeader table
                    using (SqlCommand command = new SqlCommand(updateQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("custName", custName));
                        command.Parameters.Add(new SqlParameter("contactName", contactName));
                        command.Parameters.Add(new SqlParameter("contactEmail", contactEmail));
                        command.Parameters.Add(new SqlParameter("contactTel", contactTel));
                        command.Parameters.Add(new SqlParameter("contactMobile", contactMobile));
                        command.Parameters.Add(new SqlParameter("tempCustCode", tempCustCode));
                        command.Parameters.Add(new SqlParameter("nonExistingCustomer", nonExistingCustomer));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = "UpdateCustomer() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateCustomer(...)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateCustomerOnStopFlag(string custCode, string onStopFlag)
        {
            string updateQry = "UPDATE dbo.Customer SET onStopFlag = @onStopFlag, onStopUser = @onStopUser, onStopDateTime = @onStopDateTime WHERE custCode = @custCode";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    // insert into QuoteHeader table
                    using (SqlCommand command = new SqlCommand(updateQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("onStopFlag", onStopFlag));
                        command.Parameters.Add(new SqlParameter("onStopDateTime", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("onStopUser", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = $"UpdateCustomerOnStopFlag({custCode},{onStopFlag}) ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"UpdateCustomerOnStopFlag({custCode},{onStopFlag})", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public int DeleteCustomer(string custCode)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.Customer WHERE custCode = '{0}'", custCode);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = "DeleteCustomer() ERROR : " + ex.Message.ToString();
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteCustomer({0})", custCode), ex.Message.ToString());
                return 0;
            }



        }

        public bool IsCustomerExists(string custCode)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Customer WHERE custCode = '{0}'", custCode), conn))
                    {
                        Int32 numUsersFound = (Int32)command.ExecuteScalar();

                        if (numUsersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsCustomerExists() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsCustomerExists({0})", custCode), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsJobCustomerOnStop(string custCode)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Customer WHERE custCode = '{0}' and onStopFlag = 'Y'", custCode), conn))
                    {
                        Int32 numUsersFound = (Int32)command.ExecuteScalar();

                        if (numUsersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"IsJobCustomerOnStop({custCode}) ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsJobCustomerOnStop({0})", custCode), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsCustomerTempOrNonExisting(string custCode)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Customer WHERE custCode = '{0}' AND ( tempCustCode = 'YES' or nonExistingCustomer = 'YES' ) ", custCode), conn))
                    {
                        Int32 numUsersFound = (Int32)command.ExecuteScalar();

                        if (numUsersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsCustomerTempOrNonExisting() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsCustomerTempOrNonExisting({0})", custCode), ex.Message.ToString());
                    return false;
                }

            }
        }

        public DataTable GetAllCustomer()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Customer ORDER BY custCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = "GetAllCustomer() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllCustomer()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllOnStopCustomersDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Customer WHERE onStopFlag = 'Y' ORDER BY custCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = "GetAllOnStopCustomersDT() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllOnStopCustomersDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllCustomerForCombo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT custCode,custName FROM dbo.Customer ORDER BY custCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = "GetAllCustomerForCombo() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllCustomerForCombo()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetCustomerByCode(string custCode)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = String.Format("SELECT * FROM dbo.Customer WHERE custCode = '{0}'", custCode);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = "GetCustomerByCode() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCustomerByCode({0})", custCode), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetCustomersByKeyDT(string custKey)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = String.Format("SELECT custCode,custName FROM dbo.Customer WHERE UPPER(custName) LIKE '%{0}%' ORDER BY custCode", custKey.ToUpper());

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = "GetCustomersByKeyDT() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCustomersByKeyDT({0})", custKey), ex.Message.ToString());
                    return null;
                }

            }
        }

        public string GetCustomerCodeByParentJob(int parentJob)
        {
            string custCode = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.ParentJob WHERE parentJobNo = {0}", parentJob), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custCode = reader["custCode"].ToString();
                            }
                        }
                    }
                    return custCode;
                }
                catch (Exception ex)
                {
                    string msg = "GetCustomerCodeByParentJob() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCustomerCodeByParentJob({0})", parentJob.ToString()), ex.Message.ToString());
                    return msg;
                }

            }

        }

        public string GetCustomerCodeByJobNo(string jobNo)
        {
            string custCode = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.ParentJob WHERE parentJobNo = {0}", Convert.ToInt32(jobNo.Substring(0, 5))), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custCode = reader["custCode"].ToString();
                            }
                        }
                    }
                    return custCode;
                }
                catch (Exception ex)
                {
                    string msg = "GetCustomerCodeByParentJob() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCustomerCodeByJobNo({0})", jobNo), ex.Message.ToString());
                    return msg;
                }

            }

        }

        public string GetCustomerNameByJobNo(string jobNo)
        {
            string custName = "";
            string qry =
                "select cust.custName as customerName from dbo.JobPlanner jp " +
                "inner join dbo.ParentJob pjob " +
                "on jp.parentJobNo = pjob.parentJobNo " +
                "inner join dbo.Customer cust " +
                "on pjob.custCode = cust.custCode " +
                $"where jp.jobNo = '{jobNo}'";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custName = reader["customerName"].ToString();
                            }
                        }
                    }
                    return custName;
                }
                catch (Exception ex)
                {
                    string msg = "GetCustomerNameByJobNo() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCustomerNameByJobNo({0})", jobNo), ex.Message.ToString());
                    return msg;
                }

            }

        }


        public string GetCustName(string custCode)
        {
            string error;
            string accName = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT custName FROM dbo.Customer WHERE custCode = '{0}'", custCode), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accName = reader["custName"].ToString();
                            }
                        }
                    }
                    return accName;
                }
                catch (Exception ex)
                {
                    string msg = "GetCustName() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCustName({0})", custCode), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string GetCustCodeByCustName(string custName)
        {
            string error;
            string custCode = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT custCode FROM dbo.Customer WHERE custName = '{0}'", custName), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custCode = reader["custCode"].ToString();
                            }
                        }
                    }
                    return custCode;
                }
                catch (Exception ex)
                {
                    string msg = "GetCustCodeByCustName() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCustCodeByCustName({0})", custName), ex.Message.ToString());
                    return msg;
                }

            }
        }

        #endregion

        #region Suppliers

        public string CreateSupplier(string suppCode, string suppName, string shortname, string suppAddress, string productType, string contactName, string contactEmail, string contactTel, string colourCode, int rgb1, int rgb2, int rgb3)
        {

            string insertQry = "INSERT INTO dbo.Supplier("
                                            + "suppCode,suppName,shortname,suppAddress,productType,contactName,contactEmail,contactTel,colourCode,rgb1,rgb2,rgb3) "
                                            + "VALUES("
                                            + "@suppCode,"
                                            + "@suppName,"
                                            + "@shortname,"
                                            + "@suppAddress,"
                                            + "@productType,"
                                            + "@contactName,"
                                            + "@contactEmail,"
                                            + "@contactTel,"
                                            + "@colourCode,"
                                            + "@rgb1,"
                                            + "@rgb2,"
                                            + "@rgb3)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("suppCode", suppCode));
                        command.Parameters.Add(new SqlParameter("suppName", suppName));
                        command.Parameters.Add(new SqlParameter("shortname", shortname));
                        command.Parameters.Add(new SqlParameter("suppAddress", suppAddress));
                        command.Parameters.Add(new SqlParameter("productType", productType));
                        command.Parameters.Add(new SqlParameter("contactName", contactName));
                        command.Parameters.Add(new SqlParameter("contactEmail", contactEmail));
                        command.Parameters.Add(new SqlParameter("contactTel", contactTel));
                        command.Parameters.Add(new SqlParameter("colourCode", colourCode));
                        command.Parameters.Add(new SqlParameter("rgb1", rgb1));
                        command.Parameters.Add(new SqlParameter("rgb2", rgb2));
                        command.Parameters.Add(new SqlParameter("rgb3", rgb3));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = "CreateSupplier() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateSupplier(....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateSupplier(string suppCode, string suppName, string shortname, string suppAddress, string productType, string contactName, string contactEmail, string contactTel, string colourCode, int rgb1, int rgb2, int rgb3)
        {
            string updateQry = "UPDATE dbo.Supplier "
                                    + "SET suppName = @suppName, "
                                    + "shortname = @shortname, "
                                    + "suppAddress = @suppAddress, "
                                    + "productType = @productType, "
                                    + "contactName = @contactName, "
                                    + "contactEmail = @contactEmail, "
                                    + "contactTel = @contactTel, "
                                    + "colourCode = @colourCode, "
                                    + "rgb1 = @rgb1, "
                                    + "rgb2 = @rgb2, "
                                    + "rgb3 = @rgb3 "
                                    + " WHERE suppCode = @suppCode";




            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    // insert into QuoteHeader table
                    using (SqlCommand command = new SqlCommand(updateQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("suppCode", suppCode));
                        command.Parameters.Add(new SqlParameter("suppName", suppName));
                        command.Parameters.Add(new SqlParameter("shortname", shortname));
                        command.Parameters.Add(new SqlParameter("suppAddress", suppAddress));
                        command.Parameters.Add(new SqlParameter("productType", productType));
                        command.Parameters.Add(new SqlParameter("contactName", contactName));
                        command.Parameters.Add(new SqlParameter("contactEmail", contactEmail));
                        command.Parameters.Add(new SqlParameter("contactTel", contactTel));
                        command.Parameters.Add(new SqlParameter("colourCode", colourCode));
                        command.Parameters.Add(new SqlParameter("rgb1", rgb1));
                        command.Parameters.Add(new SqlParameter("rgb2", rgb2));
                        command.Parameters.Add(new SqlParameter("rgb3", rgb3));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("Error Message = {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateSupplier(....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public int DeleteSupplier(string suppCode)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.Supplier WHERE suppCode = '{0}'", suppCode);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteSupplier() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteSupplier({0})", suppCode), ex.Message.ToString());
                return 0;
            }



        }

        public bool IsSupplierExists(string suppCode)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Supplier WHERE suppCode = '{0}'", suppCode), conn))
                    {
                        Int32 numSuppliersFound = (Int32)command.ExecuteScalar();

                        if (numSuppliersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsSupplierExists() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsSupplierExists({0})", suppCode), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsSupplierShortNameExists(string shortname)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT count(*) FROM dbo.Supplier WHERE shortname = '{shortname}'", conn))
                    {
                        Int32 numSuppliersFound = (Int32)command.ExecuteScalar();

                        if (numSuppliersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"IsSupplierShortNameExists({shortname}) Error : {ex.Message.ToString()}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"IsSupplierShortNameExists({shortname})", ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsStairsSupplier(string suppCode)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Supplier WHERE shortname = '{0}' AND productType = 'STAIRS'", suppCode), conn))
                    {
                        Int32 numSuppliersFound = (Int32)command.ExecuteScalar();

                        if (numSuppliersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsSupplierExists() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsSupplierExists({0})", suppCode), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsSuppShortnameUsedInJobPlanner(string shortname)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.JobPlanner WHERE productSupplier = '{0}'", shortname), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsSuppShortnameUsedInJobPlanner() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsSuppShortnameUsedInJobPlanner({0})", shortname), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsSuppShortnameUsedInWhiteboard(string shortname)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Whiteboard WHERE productSupplier = '{0}'", shortname), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsSuppShortnameUsedInWhiteboard() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsSuppShortnameUsedInWhiteboard({0})", shortname), ex.Message.ToString());
                    return false;
                }

            }
        }

        public string GetSupplierProductTypeFromShortname(string shortname)
        {
            //string error;
            string productType = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT productType FROM dbo.Supplier WHERE shortname = '{0}'", shortname), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productType = reader["productType"].ToString();
                            }
                        }
                    }
                    return productType;
                }
                catch (Exception ex)
                {
                    string msg = "GetSupplierProductTypeFromShortname() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSupplierProductTypeFromShortname({0})", shortname), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string GetSupplierCodeFromShortname(string shortname)
        {
            //string error;
            string productType = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT suppCode FROM dbo.Supplier WHERE shortname = '{0}'", shortname), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productType = reader["suppCode"].ToString();
                            }
                        }
                    }
                    return productType;
                }
                catch (Exception ex)
                {
                    string msg = "GetSupplierCodeFromShortname() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetSupplierCodeFromShortname({shortname})", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public DataTable GetAllSupplierShortnamesForCombo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT shortname FROM dbo.Supplier WHERE LEN(shortname) > 0 ORDER BY shortname";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllSupplierShortnamesForCombo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllSupplierShortnamesForCombo()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllWhiteboardSupplierShortnamesForCombo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = @"SELECT DISTINCT suppShortname 
                                FROM dbo.Whiteboard 
                                WHERE LEN(suppShortname) > 0 
                                AND suppShortname NOT LIKE '%SLAB%' 
                                AND suppShortname NOT LIKE '%BEAMS%' 
                                AND suppShortname NOT LIKE '%STAIRS%' 
                                ORDER BY suppShortname";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllWhiteboardSupplierShortnamesForCombo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllWhiteboardSupplierShortnamesForCombo()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllSuppliersForComboOLD()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT suppCode, suppCode + ' - ' + suppName as fullSuppName FROM dbo.Supplier ORDER BY suppCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllSuppliersForCombo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllSuppliersForCombo()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllSuppliers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Supplier ORDER BY suppCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllSuppliers() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllSuppliers()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllNonStairsSuppliers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Supplier WHERE productType IN {'BEAM','SLAB'} ORDER BY suppCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllSuppliers() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllSuppliers()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetSupplierByCode(string suppCode)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = String.Format("SELECT * FROM dbo.Supplier WHERE suppCode = '{0}'", suppCode);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetSupplierByCode() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSupplierByCode({0})", suppCode), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetSupplierByShortname(string shortname)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = String.Format("SELECT * FROM dbo.Supplier WHERE shortname = '{0}'", shortname);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetSupplierByShortname() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSupplierByShortname({0})", shortname), ex.Message.ToString());
                    return null;
                }

            }

        }



        public void GetSupplierColour(string suppCode, out int rgb1, out int rgb2, out int rgb3)
        {
            string error;


            rgb1 = 255;
            rgb2 = 255;
            rgb3 = 255;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.Supplier WHERE suppCode = '{0}'", suppCode), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rgb1 = reader["rgb1"] == null ? 255 : Convert.ToInt16(reader["rgb1"].ToString());
                                rgb2 = reader["rgb2"] == null ? 255 : Convert.ToInt16(reader["rgb2"].ToString());
                                rgb3 = reader["rgb3"] == null ? 255 : Convert.ToInt16(reader["rgb3"].ToString());
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSupplierColour({0})", suppCode), ex.Message.ToString());
                    return;
                }

            }
        }

        public void GetSupplierColourByShortname(string shortname, out int rgb1, out int rgb2, out int rgb3)
        {
            string error;


            rgb1 = 255;
            rgb2 = 255;
            rgb3 = 255;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT * FROM dbo.Supplier WHERE shortname = '{shortname}'", conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rgb1 = reader["rgb1"] == null ? 255 : Convert.ToInt16(reader["rgb1"].ToString());
                                rgb2 = reader["rgb2"] == null ? 255 : Convert.ToInt16(reader["rgb2"].ToString());
                                rgb3 = reader["rgb3"] == null ? 255 : Convert.ToInt16(reader["rgb3"].ToString());
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetSupplierColourByShortname({shortname})", ex.Message.ToString());
                    return;
                }

            }
        }

        public void GetSupplierDetailsByShortname(string shortname, out string suppCode, out string suppName)
        {
            string error;


            suppCode = "";
            suppName = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT * FROM dbo.Supplier WHERE shortname = '{shortname}'", conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suppCode = reader["suppCode"] == null ? "" : reader["suppCode"].ToString();
                                suppName = reader["suppName"] == null ? "" : reader["suppName"].ToString();
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetSupplierDetailsByShortname({suppCode})", ex.Message.ToString());
                    return;
                }

            }
        }

        //public void GetSupplierColourByShortname(string suppShortname, out int rgb1, out int rgb2, out int rgb3)
        //{
        //    string error;


        //    rgb1 = 255;
        //    rgb2 = 255;
        //    rgb3 = 255;

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();
        //        try
        //        {
        //            using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.Supplier WHERE shortname = '{0}'", suppShortname), conn))
        //            {
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        rgb1 = reader["rgb1"] == null ? 255 : Convert.ToInt16(reader["rgb1"].ToString());
        //                        rgb2 = reader["rgb2"] == null ? 255 : Convert.ToInt16(reader["rgb2"].ToString());
        //                        rgb3 = reader["rgb3"] == null ? 255 : Convert.ToInt16(reader["rgb3"].ToString());
        //                    }
        //                }
        //            }
        //            return;
        //        }
        //        catch (Exception ex)
        //        {
        //            error = ex.Message.ToString();
        //            string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSupplierColourByShortname({0})", suppShortname), ex.Message.ToString());
        //            return;
        //        }

        //    }
        //}

        public string GetSuppShortnameFromJob(string jobNo)
        {
            string productSupplier = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT productSupplier FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productSupplier = reader["productSupplier"] == DBNull.Value ? "" : reader["productSupplier"].ToString();
                            }
                        }
                    }
                    return productSupplier;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetSuppShortnameFromJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSuppShortnameFromJob({0})", jobNo), ex.Message.ToString());
                    return productSupplier;
                }

            }
        }

        #endregion

        #region Jobs

        public DataTable GetOutOfSyncDateJobsDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.DatesOutOfSyncView ORDER BY WB_jobNo";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetCancelledJobsDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetCancelledJobsDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetCancelledJobsDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.CancelledJobsView ORDER BY jobNum";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetCancelledJobsDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetCancelledJobsDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetDeletedJobsAuditDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.JobDeletionAudit ORDER BY jobNo";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetDeletedJobsAuditDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetDeletedJobsAuditDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public bool UpdateJobPlannerSupplier(string jobNo, string productSupplier)
        {


            string updateQry = "UPDATE dbo.JobPlanner "
                                + "SET productSupplier = @productSupplier,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy "
                                + "WHERE LEFT(jobNo,8) = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(updateQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo.Substring(0, 8)));
                        command.Parameters.Add(new SqlParameter("productSupplier", productSupplier));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    string msg = $"UpdateJobPlannerSupplier() Error : {ex.Message}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"UpdateJobPlannerSupplier({jobNo},{productSupplier})", ex.Message.ToString());
                    return false;
                }

            }
        }


        public void GetCancelledJobDetails(string jobNo, out DateTime reqDate, out string custName, out string siteAddress, out decimal invValue, out string lastComment)
        {
            string error;


            string custCode = "";
            custName = "";
            siteAddress = "";
            invValue = 0;
            lastComment = "";
            reqDate = DateTime.MinValue;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reqDate = reader["requiredDate"] == null ? DateTime.MinValue : Convert.ToDateTime(reader["requiredDate"].ToString());
                                custCode = GetCustomerCodeByJobNo(jobNo);
                                custName = GetCustName(custCode);
                                siteAddress = reader["siteAddress"] == null ? "" : reader["siteAddress"].ToString();
                                invValue = reader["phaseInvValue"] == null ? 0 : Convert.ToDecimal(reader["phaseInvValue"].ToString());
                                lastComment = GetLastComment(jobNo);
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCancelledJobDetails({0})", jobNo), ex.Message.ToString());
                    return;
                }

            }
        }

        public string CreateJobDeletionAudit(string jobNo, DateTime reqDate, string custName, string siteAddress, decimal invValue, string lastComment, string deleteTrigger)
        {
            string query = "";
            string insertQry = $"INSERT INTO dbo.JobDeletionAudit("
                                            + "jobNo,reqDate,custName,siteAddress,invValue,lastComment,deleteTrigger,auditDate,auditUser) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@reqDate,"
                                            + "@custName,"
                                            + "@siteAddress,"
                                            + "@invValue,"
                                            + "@lastComment,"
                                            + "@deleteTrigger,"
                                            + "@auditDate,"
                                            + "@auditUser)";
                                            

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("reqDate", reqDate));
                        command.Parameters.Add(new SqlParameter("custName", custName));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("invValue", invValue));
                        command.Parameters.Add(new SqlParameter("lastComment", lastComment));
                        command.Parameters.Add(new SqlParameter("deleteTrigger", deleteTrigger));
                        command.Parameters.Add(new SqlParameter("auditDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("auditUser", ConfigurationManager.AppSettings["LoggedInUser"]));
                        
                        //query = command.CommandText;

                        //foreach (SqlParameter p in command.Parameters)
                        //{
                        //    query = query.Replace(p.ParameterName, p.Value.ToString());
                        //}
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    
                    string msg = $"CreateJobDeletionAudit() Error : {ex.Message.ToString()}";
                    logger.LogLine(msg);
                    //string audit = CreateErrorAudit("MeltonData.cs", $"PopulateDGV({jobNo},{reqDate},{custName},{siteAddress},{invValue},{lastComment},{deleteTrigger})", msg);
                    return msg;
                }

            }
        }

        public string CreateJobDayAudit(string jobNo, DateTime requiredDate, string source)
        {

            string insertQry = "INSERT INTO dbo.JobDateAudit("
                                            + "jobNo,requiredDate,auditDate,auditUser,source) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@requiredDate,"
                                            + "@auditDate,"
                                            + "@auditUser,"
                                            + "@source)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("auditDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("auditUser", ConfigurationManager.AppSettings["LoggedInUser"]));
                        command.Parameters.Add(new SqlParameter("source", source));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateWBDayCommentAudit() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("CreateWBDayCommentAudit({0})", jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string CreateJobLock(string lockType, string jobNo, string trigger)
        {

            string insertQry = "INSERT INTO dbo.JobLock("
                                            + "jobNo,jobLockType,lockUser,lockDateTime,isActive,lockTrigger) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@jobLockType,"
                                            + "@lockUser,"
                                            + "@lockDateTime,"
                                            + "@isActive,"
                                            + "@lockTrigger)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("jobLockType", lockType));
                        command.Parameters.Add(new SqlParameter("isActive", "Y"));
                        command.Parameters.Add(new SqlParameter("lockDateTime", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("lockUser", ConfigurationManager.AppSettings["LoggedInUser"]));
                        command.Parameters.Add(new SqlParameter("lockTrigger", trigger));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    //string msg = String.Format("CreateJobLock() Error : {0}", ex.Message.ToString());
                    string msg = $"CreateJobLock({jobNo},{lockType},{ConfigurationManager.AppSettings["LoggedInUser"]},{DateTime.Now},Y) Error : {ex.Message}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"CreateJobLock({jobNo},{lockType},{ConfigurationManager.AppSettings["LoggedInUser"]},{DateTime.Now},Y)", ex.Message);
                    return msg;
                }

            }
        }

        public string CreateCancelledLock(string jobNo)
        {

            string insertQry = $"INSERT INTO dbo.CancelledJob VALUES('{jobNo}')";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {                        
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    //string msg = String.Format("CreateJobLock() Error : {0}", ex.Message.ToString());
                    string msg = $"CreateCancelledLock({jobNo}) Error : {ex.Message}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"CreateCancelledLock({jobNo})", ex.Message);
                    return msg;
                }

            }
        }

        public string CreateDummyJobLock(string jobNo, string jobLockType, string lockUser)
        {

            string insertQry = "INSERT INTO dbo.JobLock("
                                            + "jobNo,jobLockType,lockUser,lockDateTime,isActive,lockTrigger) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@jobLockType,"
                                            + "@lockUser,"
                                            + "@lockDateTime,"
                                            + "@isActive,"
                                            + "@lockTrigger)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("jobLockType", jobLockType));
                        command.Parameters.Add(new SqlParameter("isActive", "Y"));
                        command.Parameters.Add(new SqlParameter("lockDateTime", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("lockUser", ConfigurationManager.AppSettings["LoggedInUser"]));
                        command.Parameters.Add(new SqlParameter("lockTrigger", "dummy_trigger"));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    //string msg = String.Format("CreateJobLock() Error : {0}", ex.Message.ToString());
                    string msg = $"CreateDummyJobLock({jobNo},Y,{ConfigurationManager.AppSettings["LoggedInUser"]},{DateTime.Now},Y) Error : {ex.Message}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"CreateDummyJobLock({jobNo},Y,{ConfigurationManager.AppSettings["LoggedInUser"]},{DateTime.Now},Y)", ex.Message);
                    return msg;
                }

            }
        }

        public string GetJobLockedUser(string jobNo, string lockType)
        {

            string lockUser = "n/a";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT lockUser FROM dbo.JobLock WHERE jobNo = '{0}' and jobLockType = '{1}' and isActive = 'Y'", jobNo, lockType), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lockUser = reader["lockUser"] == null ? "n/a" : reader["lockUser"].ToString();
                            }
                        }
                    }
                    return lockUser;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobLockedUser() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobLockedUser({0})", jobNo), ex.Message.ToString());
                    return lockUser;
                }

            }
        }

        public string DisableJobLock(string lockType, string jobNo)
        {
            string qry = "UPDATE dbo.JobLock "
                                + "SET isActive = 'N' "
                                + "WHERE jobLockType = @jobLockType AND jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("jobLockType", lockType));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("DisableJobLock({0},{1}) Error : {2}", lockType, jobNo, ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("DisableJobLock({0},{1})", lockType, jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public int DeleteUserLocks(string lockType, string loggedUser)
        {
            try
            {
                string qry = $"DELETE FROM dbo.JobLock WHERE jobLockType = '{lockType}' AND lockUser = '{loggedUser}'";

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteUserLocks() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", $"DeleteUserLocks({lockType},{loggedUser})", ex.Message.ToString());
                return 0;
            }




        }

        public int DeleteJobLocks(string lockType, string jobNo)
        {
            try
            {
                string qry = $"DELETE FROM dbo.JobLock WHERE jobLockType = '{lockType}' AND jobNo = '{jobNo}'";

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteJobLock() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", $"DeleteJobLock({lockType},{jobNo})", ex.Message.ToString());
                return 0;
            }




        }

        public int DeleteCancelledJob(string jobNo)
        {
            try
            {
                string qry = $"DELETE FROM dbo.CancelledJob WHERE jobNo = '{jobNo}'";

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteCancelledJob() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", $"DeleteCancelledJob({jobNo})", ex.Message.ToString());
                return 0;
            }




        }

        public DataTable GetJobLocksDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.JobLock WHERE isActive = 'Y' ORDER BY jobLockType,jobNo";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobLocksDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobLocksDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobLocksDT(string lockType)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = $"SELECT * FROM dbo.JobLock WHERE jobLockType = '{lockType}' ORDER BY jobNo";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = $"GetJobLocksDT({lockType}) Error : {ex.Message.ToString()}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetJobLocksDT({lockType})", ex.Message.ToString());
                    return null;
                }

            }

        }

        public bool IsJobLockExist()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("SELECT count(*) FROM dbo.JobLock WHERE isActive = 'Y'", conn))
                    {
                        Int32 numSuppliersFound = (Int32)command.ExecuteScalar();

                        if (numSuppliersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsJobLockExist() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "IsJobLockExist()", ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool AnyCancelledJobsExist()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("SELECT count(*) FROM dbo.CancelledJob", conn))
                    {
                        Int32 num = (Int32)command.ExecuteScalar();

                        if (num > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("AnyCancelledJobsExist() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "AnyCancelledJobsExist()", ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool AnyJobsDeleted()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("SELECT count(*) FROM dbo.JobDeletionAudit", conn))
                    {
                        Int32 num = (Int32)command.ExecuteScalar();

                        if (num > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("AnyJobsDeleted() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "AnyJobsDeleted()", ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsJobLockExist(string lockType, string jobNo, string trigger, string loggedinUser)
        {
            string qry = $"SELECT count(*) FROM dbo.JobLock WHERE isActive = 'Y' AND jobLockType = '{lockType}' AND jobNo = '{jobNo}' AND lockTrigger = '{trigger}' and lockUser = '{loggedinUser}'";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        Int32 numFound = (Int32)command.ExecuteScalar();

                        if (numFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsJobLockExist(string lockType, string jobNo, string trigger) Error : {0}", qry + Environment.NewLine + ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "IsJobLockExist(string lockType, string jobNo, string trigger)", qry + Environment.NewLine + ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsJobLockExistByType(string jobLockType)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT count(*) FROM dbo.JobLock WHERE jobLockType = '{jobLockType}'", conn))
                    {
                        Int32 numLocks = (Int32)command.ExecuteScalar();

                        if (numLocks > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsJobLockExist() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "IsJobLockExist()", ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsJobLockExistByOtherUser(string jobLockType, string jobNo, string loggedinUser)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT count(*) FROM dbo.JobLock WHERE jobLockType = '{jobLockType}' AND jobNo = '{jobNo}' AND lockUser != '{loggedinUser}' ", conn))
                    {
                        Int32 numSuppliersFound = (Int32)command.ExecuteScalar();

                        if (numSuppliersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"IsJobLockExist({jobLockType},{jobNo}) Error : {ex.Message.ToString()}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"IsJobLockExist({jobLockType},{jobNo})", ex.Message.ToString());
                    return false;
                }

            }
        }

        public DataTable GetJobDayAuditDT(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = string.Format("SELECT * FROM dbo.JobDateAudit WHERE jobNo LIKE '{0}%' ORDER BY jobNo, auditDate", jobNo.Substring(0, 8));

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobDayAuditDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobDayAuditDT({0})", jobNo), ex.Message.ToString());
                    return null;
                }

            }

        }

        public bool IsJobExists(string jobNo)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsJobExists() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsJobExists({0})", jobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsParentJobExists(int parentJobNo)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.ParentJob WHERE parentJobNo = {0}", parentJobNo), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsParentJobExists() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsParentJobExists({0})", parentJobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsJobCompleted(string jobNo)
        {
            string qry = $"SELECT count(*) FROM dbo.JobPlanner WHERE LEFT(jobNo,8) = '{jobNo.Substring(0, 8)}' and completedFlag = 'Y'";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsJobCompleted() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsJobCompleted({0})", jobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsJobCancelled(string jobNo)
        {
            string qry = $"SELECT count(*) FROM dbo.CancelledJob WHERE LEFT(jobNo,8) = '{jobNo.Substring(0, 8)}'";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "IsJobCancelled() ERROR : " + ex.Message.ToString();
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsJobCancelled({0})", jobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        public void SyncCompletedNonSuffixedJobsWithTheirExtendedJobs(string completedFlag)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = $"SELECT jobNo FROM dbo.WhiteboardJobInvoicedToday WHERE completedFlag = '{completedFlag}' OR isInvoiced = '{completedFlag}' ";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    string job = "";

                    foreach (DataRow dr in dt.Rows)
                    {
                        job = dr["jobNo"].ToString();
                        string result = CompleteJobPlanner(job, completedFlag);
                        string result2 = CompleteWhiteboardJob(job, completedFlag);
                    }

                    return;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("SyncCompletedNonSuffixedJobsWithTheirExtendedJobs() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("SyncCompletedNonSuffixedJobsWithTheirExtendedJobs({0})", completedFlag), ex.Message.ToString());
                    return;
                }

            }
        }

        public uint GetNextParentJobNo()
        {

            uint seq = 0;
            // string qryOld = "SELECT MAX VALUE FOR dbo.ParentJobNoSeq AS nextParentJobNo";
            string qry = "SELECT nextParentJobNo = MAX(parentJobNo)  from dbo.ParentJob";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                seq = Convert.ToUInt32(reader["nextParentJobNo"].ToString()) + 1;
                            }
                        }
                    }
                    return seq;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetNextParentJobNo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetNextParentJobNo()", ex.Message.ToString());
                    return 0;
                }

            }
        }

        public DataTable GetAllFloorLevels()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.FloorLevels ORDER BY seq";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllFloorLevels() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllFloorLevels()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllParentJobsDT()
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    qry = "SELECT * FROM dbo.ParentJob ORDER BY parentJobNo";


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllParentJobsDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllParentJobsDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetParentJobsDT(int parentJobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = String.Format("SELECT * FROM dbo.ParentJob WHERE parentJobNo = {0}", parentJobNo);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetParentJobsDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetParentJobsDT({0})", parentJobNo), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetParentJobsDTByJobKey(string jobKey)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    int parent = Convert.ToInt32(jobKey.Substring(0, 5));
                    conn.Open();
                    string qry = String.Format("SELECT DISTINCT * FROM dbo.ParentJob WHERE parentJobNo = {0}", parent);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetParentJobsDTByJobKey() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetParentJobsDTByJobKey({0})", jobKey), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobPlannerDT()
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    //qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' ORDER BY supplyType,requiredDate";
                    qry = "SELECT * FROM dbo.ListJobsNotCompletedOrCancelledViewV2 ORDER BY supplyType,requiredDate";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobPlannerDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobsNotOnShopDT()
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    qry = @"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag != 'Y' 
                            AND OnShop != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY sortType,requiredDate";


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobsNotOnShopDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobsNotOnShopDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobPlannerDTByShortName(string shortname)
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    qry = String.Format("SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND productSupplier = '{0}' ORDER BY supplyType,requiredDate", shortname);


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobPlannerDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetOnShopJobPlannerDTByShortNameDT(string shortname)
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    qry = String.Format("SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND onShop = 'Y' AND productSupplier = '{0}' ORDER BY sortType,requiredDate", shortname);


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetOnShopJobPlannerDTByShortNameDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetOnShopJobPlannerDTByShortNameDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetBeamJobPlannerDT()
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) ORDER BY supplyType,requiredDate";


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobPlannerDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetSlabJobPlannerDT()
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' )  ORDER BY supplyType,requiredDate";


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobPlannerDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobPlannerDT(string rptMode,DateTime startDate, DateTime endDate, int year)
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    switch (rptMode)
                    {
                        case "BeamLM":
                            qry = "SELECT jobNo,floorLevel,requiredDate,beamLM,supplyType,productSupplier FROM dbo.JobPlanner " 
                                + "WHERE YEAR(requiredDate) = @year AND completedFlag = 'Y' AND beamLm > 0 "
                                + "AND ( requiredDate between @startDate and @endDate ) AND LEN(productSupplier) > 0 "
                                + "ORDER BY productSupplier,requiredDate";
                            break;
                        case "BeamM2":
                            qry = "SELECT jobNo,floorLevel,requiredDate,beamM2,supplyType,productSupplier FROM dbo.JobPlanner "
                                + "WHERE YEAR(requiredDate) = @year AND completedFlag = 'Y' AND beamM2 > 0 "
                                + "AND ( requiredDate between @startDate and @endDate ) AND LEN(productSupplier) > 0 "
                                + "ORDER BY productSupplier,requiredDate";
                            break;
                        case "SlabM2":
                            qry = "SELECT jobNo,floorLevel,requiredDate,slabM2,supplyType,productSupplier FROM dbo.JobPlanner "
                                + "WHERE YEAR(requiredDate) = @year AND completedFlag = 'Y' AND slabM2 > 0 "
                                + "AND ( requiredDate between @startDate and @endDate ) AND LEN(productSupplier) > 0 "
                                + "ORDER BY productSupplier,requiredDate";
                            break;
                        default:
                            break;
                    }
                    


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    cmd.Parameters.Add(new SqlParameter("year", year));
                    cmd.Parameters.Add(new SqlParameter("startDate", startDate.Date));
                    cmd.Parameters.Add(new SqlParameter("endDate", endDate.Date));
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobPlannerDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobPlannerMIssingDataDT(string rptMode)
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    switch (rptMode)
                    {
                        case "BEAMZERO": // beam jobs
                            qry = "SELECT jobNo,floorLevel,siteAddress,requiredDate,beamLM,beamM2,slabM2,supplyType,productSupplier,supplierRef,phaseInvValue,jobMgnValue FROM dbo.JobPlanner "
                                + "WHERE completedFlag = 'Y' AND (beamLm = 0 or beamM2 = 0) AND slabM2 = 0 and stairsIncl != 'Y' "
                                + "ORDER BY supplyType,requiredDate";
                            break;
                        case "SLABZERO": //slab jobs
                            qry = "SELECT jobNo,floorLevel,siteAddress,requiredDate,beamLM,beamM2,slabM2,supplyType,productSupplier,supplierRef,phaseInvValue,jobMgnValue FROM dbo.JobPlanner "
                                + "WHERE completedFlag = 'Y' AND slabM2 = 0 AND beamLm = 0 and beamM2 = 0 and stairsIncl != 'Y' "
                                + "ORDER BY supplyType,requiredDate";
                            break;
                        case "ALLZERO": // beam and slab jobs
                            qry = "SELECT jobNo,floorLevel,siteAddress,requiredDate,beamLM,beamM2,slabM2,supplyType,productSupplier,supplierRef,phaseInvValue,jobMgnValue FROM dbo.JobPlanner "
                                + "WHERE completedFlag = 'Y' AND slabM2 = 0 AND beamLm = 0 and beamM2 = 0 and stairsIncl != 'Y'"
                                //  + "AND productSupplier NOT IN ('LEROC','RightCast','Kalisto') and stairsIncl != 'Y'"
                                + "ORDER BY supplyType,requiredDate";
                            break;
                        case "MISSINGSUPPLIER": // beam and slab jobs
                            qry = "SELECT jobNo,floorLevel,siteAddress,requiredDate,beamLM,beamM2,slabM2,supplyType,productSupplier,supplierRef,phaseInvValue,jobMgnValue FROM dbo.JobPlanner "
                                + "WHERE LEN(productSupplier) < 1 AND completedFlag = 'Y' "
                                + "ORDER BY productSupplier,requiredDate";
                            break;
                        default:
                            break;
                    }



                    SqlCommand cmd = new SqlCommand(qry, conn);
                    
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobPlannerDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public int GetNumMissingData(string rptMode)
        {
            string error;
            string qry = "";
            switch (rptMode)
            {
                case "BEAMZERO": // beam jobs
                    qry = "SELECT COUNT(*) FROM dbo.JobPlanner "
                        + "WHERE completedFlag = 'Y' AND (beamLm = 0 or beamM2 = 0) AND slabM2 = 0 and stairsIncl != 'Y' ";
                    break;
                case "SLABZERO": //slab jobs
                    qry = "SELECT COUNT(*) FROM dbo.JobPlanner "
                        + "WHERE completedFlag = 'Y' AND slabM2 = 0 AND beamLm = 0 and beamM2 = 0 and stairsIncl != 'Y' ";
                    break;
                case "ALLZERO": // beam and slab jobs
                    qry = "SELECT COUNT(*) FROM dbo.JobPlanner "
                        + "WHERE completedFlag = 'Y' AND slabM2 = 0 AND beamLm = 0 and beamM2 = 0 and stairsIncl != 'Y' ";
                    break;
                case "MISSINGSUPPLIER": // beam and slab jobs
                    qry = "SELECT COUNT(*) FROM dbo.JobPlanner "
                        + "WHERE completedFlag = 'Y' AND LEN(productSupplier) < 1 ";
                    break;
                case "MISSINGPRODUCTS1": // whiteboard jobs
                    qry = "SELECT COUNT(*) FROM dbo.Whiteboard "
                        + "WHERE completedFlag = 'Y' AND LEN(products) < 1 AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )";
                    break;
                case "MISSINGPRODUCTS2": // whiteboard jobs
                    qry = "SELECT COUNT(*) FROM dbo.Whiteboard "
                        + "WHERE completedFlag != 'Y' AND LEN(products) < 1 AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )";
                    break;
                default:
                    break;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {

                        int numRows = (Int32)command.ExecuteScalar();
                        return numRows;
                    }
                }
                catch (Exception ex)
                {
                    error = ex.InnerException.ToString();
                    return 0;
                }

            }
        }

        public DataTable GetSupplierSummaryByYearDT(string rptMode, int year)
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    switch (rptMode)
                    {
                        case "BeamLM":
                            qry = "SELECT YEAR(requiredDate) as year,productSupplier,SUM(beamLm)  as 'BeamLM' FROM dbo.JobPlanner "
                                + $"WHERE completedFlag = 'Y' AND YEAR(requiredDate) = {year} "
                                + "AND COALESCE (productSupplier, '') <> '' "
                               // + "AND beamLm > 0 "
                                + "GROUP BY YEAR(requiredDate),productSupplier "
                                + "ORDER BY YEAR(requiredDate),productSupplier ";
                            break;
                        case "BeamM2":
                            qry = "SELECT YEAR(requiredDate) as year,productSupplier,SUM(beamM2)  as 'BeamM2' FROM dbo.JobPlanner "
                                + $"WHERE completedFlag = 'Y' AND YEAR(requiredDate) = {year} "
                                + "AND COALESCE (productSupplier, '') <> '' "
                             //   + "AND beamM2 > 0 "
                                + "GROUP BY YEAR(requiredDate),productSupplier "
                                + "ORDER BY YEAR(requiredDate),productSupplier ";
                            break;
                        case "SlabM2":
                            qry = "SELECT YEAR(requiredDate) as year,productSupplier,SUM(slabM2)  as 'SlabM2' FROM dbo.JobPlanner "
                                + $"WHERE completedFlag = 'Y' AND YEAR(requiredDate) = {year} "
                                + "AND COALESCE (productSupplier, '') <> '' "
                           //     + "AND slabM2 > 0 "
                                + "GROUP BY YEAR(requiredDate),productSupplier "
                                + "ORDER BY YEAR(requiredDate),productSupplier ";
                            break;
                        default:
                            break;
                    }
                    

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    cmd.Parameters.Add(new SqlParameter("year", year));
                    
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetSupplierSummaryByYearDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetSupplierSummaryByYearDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetYearsDT(string rptMode,DateTime startDate, DateTime endDate)
        {
            string qry = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    switch (rptMode)
                    {
                        case "BeamLM":
                            qry = "SELECT DISTINCT YEAR(requiredDate) as year FROM dbo.JobPlanner " 
                                + "WHERE completedFlag = 'Y' AND beamLm > 0 AND ( requiredDate between @startDate and @endDate ) "
                                + "AND LEN(productSupplier) > 0 ORDER BY YEAR(requiredDate)";
                            break;
                        case "BeamM2":
                            qry = "SELECT DISTINCT YEAR(requiredDate) as year FROM dbo.JobPlanner "
                                + "WHERE completedFlag = 'Y' AND beamM2 > 0 AND ( requiredDate between @startDate and @endDate ) "
                                + "AND LEN(productSupplier) > 0 ORDER BY YEAR(requiredDate)";
                            break;
                        case "SlabM2":
                            qry = "SELECT DISTINCT YEAR(requiredDate) as year FROM dbo.JobPlanner "
                                + "WHERE completedFlag = 'Y' AND slabM2 > 0 AND ( requiredDate between @startDate and @endDate ) "
                                + "AND LEN(productSupplier) > 0 ORDER BY YEAR(requiredDate)";
                            break;
                        default:
                            break;
                    }
                    


                    SqlCommand cmd = new SqlCommand(qry, conn);
                    cmd.Parameters.Add(new SqlParameter("startDate", startDate.Date));
                    cmd.Parameters.Add(new SqlParameter("endDate", endDate.Date));
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetJobPlannerDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public int GetTotalBySupplier(string rptMode, string productSupplier, DateTime startDate, DateTime endDate, int year)
        {
            string error;
            decimal total = 0;
            string qry = "";
            switch (rptMode)
            {
                case "BeamLM":
                    qry = "SELECT beamLm FROM dbo.JobPlanner WHERE completedFlag = 'Y' "
                        + "AND beamLm > 0 AND ( requiredDate between @startDate and @endDate ) " 
                        + "AND productSupplier = @productSupplier AND YEAR(requiredDate) = @year ";
                    break;
                case "BeamM2":
                    qry = "SELECT beamM2 FROM dbo.JobPlanner WHERE completedFlag = 'Y' "
                        + "AND beamM2 > 0 AND ( requiredDate between @startDate and @endDate ) "
                        + "AND productSupplier = @productSupplier AND YEAR(requiredDate) = @year ";
                    break;
                case "SlabM2":
                    qry = "SELECT slabM2 FROM dbo.JobPlanner WHERE completedFlag = 'Y' "
                        + "AND slabM2 > 0 AND ( requiredDate between @startDate and @endDate ) "
                        + "AND productSupplier = @productSupplier AND YEAR(requiredDate) = @year ";
                    break;
                default:
                    break;
            }
            

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("startDate", startDate.Date));
                        command.Parameters.Add(new SqlParameter("endDate", endDate.Date));
                        command.Parameters.Add(new SqlParameter("productSupplier", productSupplier));
                        command.Parameters.Add(new SqlParameter("year", year));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                switch (rptMode)
                                {
                                    case "BeamLM":
                                        total += Convert.ToDecimal(reader["beamLm"].ToString());
                                        break;
                                    case "BeamM2":
                                        total += Convert.ToDecimal(reader["beamM2"].ToString());
                                        break;
                                    case "SlabM2":
                                        total += Convert.ToDecimal(reader["slabM2"].ToString());
                                        break;
                                    default:
                                        break;
                                }
                                
                            }
                        }
                    }
                    return Convert.ToInt32(total);
                }
                catch (Exception ex)
                {
                    error = ex.InnerException.ToString();
                    return 0;
                }

            }
        }


        public DataTable GetJobPlannerDTByQry(string myQuery)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = myQuery;

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobPlannerDTByQry({0})", myQuery), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobPlannerDT(int parentJobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = $"select * from dbo.JobPlanner where parentJobNo = '{parentJobNo}' and LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY jobNo";
                  //  string qry = String.Format("SELECT * FROM dbo.JobPlanner WHERE parentJobNo = {0} ORDER BY jobNo", parentJobNo);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobPlannerDT({0})", parentJobNo.ToString()), ex.Message.ToString());
                    return null;
                }

            }

        }

        

        public void GetSiteDetailFromParentJob(int parentJob, out string siteContact, out string siteContactTel, out string siteContactEmail)
        {
            string error;


            siteContact = "";
            siteContactTel = "";
            siteContactEmail = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.ParentJob WHERE parentJobNo = {0}", parentJob), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                siteContact = reader["siteContact"] == null ? "" : reader["siteContact"].ToString();
                                siteContactTel = reader["siteContactTel"] == null ? "" : reader["siteContactTel"].ToString();
                                siteContactEmail = reader["siteContactEmail"] == null ? "" : reader["siteContactEmail"].ToString();
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSiteDetailFromParentJob({0})", parentJob.ToString()), ex.Message.ToString());
                    return;
                }

            }
        }

        public void GetCusomerCodeAndSiteAddrFromWBJob(string jobNo, out string custCode, out string siteAddress)
        {
            string error;


            custCode = "";
            siteAddress = "";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.Whiteboard WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custCode = reader["custCode"] == null ? "" : reader["custCode"].ToString();
                                siteAddress = reader["siteAddress"] == null ? "" : reader["siteAddress"].ToString();

                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCusomerCodeAndSiteAddrFromWBJob({0})", jobNo), ex.Message.ToString());
                    return;
                }

            }
        }

        public void GetParentJobDetail(int parentJob, out string siteAddress, out string custCode, out string dateCreated, out string createdBy)
        {
            string error;


            siteAddress = "";
            custCode = "";
            createdBy = "";
            dateCreated = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.ParentJob WHERE parentJobNo = {0}", parentJob), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                siteAddress = reader["siteAddress"] == null ? "" : reader["siteAddress"].ToString();
                                custCode = reader["custCode"] == null ? "" : reader["custCode"].ToString();
                                createdBy = reader["createdBy"] == null ? "" : reader["createdBy"].ToString();
                                dateCreated = reader["dateCreated"] == null ? "" : Convert.ToDateTime(reader["dateCreated"].ToString()).ToLongDateString();
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {

                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetParentJobDetail({0})", parentJob.ToString()), ex.Message.ToString());
                    return;
                }

            }
        }

        public string GetCompletedFlagFromJob(string jobNo)
        {
            string completedFlag = "N";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT completedFlag FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                completedFlag = reader["completedFlag"] == null ? "" : reader["completedFlag"].ToString();
                            }
                        }
                    }
                    return completedFlag;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetCompletedFlagFromJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetCompletedFlagFromJob({0})", jobNo), ex.Message.ToString());
                    return completedFlag;
                }

            }
        }

        public string GetSiteAddressFromParentJob(int parentJobNo)
        {
            string siteAddress = "";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT siteAddress FROM dbo.ParentJob WHERE parentJobNo = {0}", parentJobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                siteAddress = reader["siteAddress"] == null ? "" : reader["siteAddress"].ToString();
                            }
                        }
                    }
                    return siteAddress;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetSiteAddressFromParentJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSiteAddressFromParentJob({0})", parentJobNo.ToString()), ex.Message.ToString());
                    return siteAddress;
                }

            }
        }

        public decimal GetJobMgnValueFromJobNo(string jobNo)
        {
            decimal value = 0;


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT jobMgnValue FROM dbo.JobPlanner WHERE jobNo = '{jobNo}'", conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                value = reader["jobMgnValue"] == null ? 0 : Convert.ToDecimal(reader["jobMgnValue"].ToString());
                            }
                        }
                    }
                    return value;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobMgnValueFromJobNo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetJobMgnValueFromJobNo({jobNo})", ex.Message.ToString());
                    return value;
                }

            }
        }

        public string GetSiteAddressFromJobNo(string jobNo)
        {
            string siteAddress = "";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT siteAddress FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                siteAddress = reader["siteAddress"] == null ? "" : reader["siteAddress"].ToString();
                            }
                        }
                    }
                    return siteAddress;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetSiteAddressFromJobNo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSiteAddressFromJobNo({0})", jobNo), ex.Message.ToString());
                    return siteAddress;
                }

            }
        }

        public string GetSupplierRefFromJobNo(string jobNo)
        {
            string suppRef = "";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT supplierRef FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suppRef = reader["supplierRef"] == null ? "" : reader["supplierRef"].ToString();
                            }
                        }
                    }
                    return suppRef;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetSupplierRefFromJobNo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetSupplierRefFromJobNo({0})", jobNo), ex.Message.ToString());
                    return suppRef;
                }

            }
        }

        public string CreateParentJob(int parentJobNo, string custCode, string siteAddress, string siteContact, string siteContactTel, string siteContactEmail)
        {

            string insertQry = "INSERT INTO dbo.ParentJob("
                                            + "parentJobNo,custCode,siteAddress,siteContact,siteContactTel,siteContactEmail,dateCreated,createdBy) "
                                            + "VALUES("
                                            + "@parentJobNo,"
                                            + "@custCode,"
                                            + "@siteAddress,"
                                            + "@siteContact,"
                                            + "@siteContactTel,"
                                            + "@siteContactEmail,"
                                            + "@dateCreated,"
                                            + "@createdBy)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("parentJobNo", parentJobNo));
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("siteContact", siteContact));
                        command.Parameters.Add(new SqlParameter("siteContactTel", siteContactTel));
                        command.Parameters.Add(new SqlParameter("siteContactEmail", siteContactEmail));
                        command.Parameters.Add(new SqlParameter("dateCreated", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("createdBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateParentJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateParentJob(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateParentJob(int parentJobNo, string custCode, string siteAddress, string siteContact, string siteContactTel, string siteContactEmail)
        {

            string insertQry = "UPDATE dbo.ParentJob "
                                    + "SET custCode = @custCode, "
                                    + "siteAddress = @siteAddress, "
                                    + "siteContact = @siteContact, "
                                    + "siteContactTel = @siteContactTel, "
                                    + "siteContactEmail = @siteContactEmail"
                                    + " WHERE parentJobNo = @parentJobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("parentJobNo", parentJobNo));
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("siteContact", siteContact));
                        command.Parameters.Add(new SqlParameter("siteContactTel", siteContactTel));
                        command.Parameters.Add(new SqlParameter("siteContactEmail", siteContactEmail));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateParentJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateParentJob(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateParentJobWithSiteAddr(int parentJobNo, string siteContact, string siteContactTel, string siteContactEmail)
        {

            string insertQry = "UPDATE dbo.ParentJob "
                                    + "SET siteContact = @siteContact, "
                                    + "siteContactTel = @siteContactTel, "
                                    + "siteContactEmail = @siteContactEmail"
                                    + " WHERE parentJobNo = @parentJobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("parentJobNo", parentJobNo));
                        command.Parameters.Add(new SqlParameter("siteContact", siteContact));
                        command.Parameters.Add(new SqlParameter("siteContactTel", siteContactTel));
                        command.Parameters.Add(new SqlParameter("siteContactEmail", siteContactEmail));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateParentJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateParentJobWithSiteAddr(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string CreateDesignBoardJob( 
            string jobNo, 
            DateTime designDate,
            string designStatus,
            DateTime requiredDate,
            string floorlevel,
            string suppShortname,
            string supplierRef,
            string stairsIncluded,
            string supplyType,
            string salesman,
            int slabM2, 
            int beamM2, 
            int beamLM
            )
        {

            string insertQry = "INSERT INTO dbo.DesignBoard("
                    + "jobNo,designDate, designStatus, requiredDate,dateJobCreated, floorlevel, suppShortname, supplierRef, stairsIncluded, salesman, supplyType, slabM2, beamM2, beamLM, modifiedDate, modifiedBy) "
                    + "VALUES("
                    + "@jobNo,"
                    + "@designDate,"
                    + "@designStatus,"
                    + "@requiredDate,"
                    + "@dateJobCreated,"
                    + "@floorlevel,"
                    + "@suppShortname,"
                    + "@supplierRef,"
                    + "@stairsIncluded,"
                    + "@salesman,"
                    + "@supplyType,"
                    + "@slabM2,"
                    + "@beamM2,"
                    + "@beamLM,"
                    + "@modifiedDate,"
                    + "@modifiedBy)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("designDate", designDate));
                        command.Parameters.Add(new SqlParameter("designStatus", designStatus));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("dateJobCreated", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("floorlevel", floorlevel));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("supplierRef", supplierRef));
                        command.Parameters.Add(new SqlParameter("stairsIncluded", stairsIncluded));
                        command.Parameters.Add(new SqlParameter("salesman", salesman));
                        command.Parameters.Add(new SqlParameter("supplyType", supplyType));
                        command.Parameters.Add(new SqlParameter("slabM2", slabM2));
                        command.Parameters.Add(new SqlParameter("beamM2", beamM2));
                        command.Parameters.Add(new SqlParameter("beamLM", beamLM));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        
                        command.ExecuteNonQuery();
                    }
                    string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"CreateDesignBoardJob(....{requiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = $"CreateDesignBoardJob() Error : {ex.Message.ToString()}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateDesignBoardJob(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }


        public string CreateJobPlanner(int parentJobNo, string jobNo, string phaseNo, string floorLevel, DateTime requiredDate, string siteAddress, string approved, string OnShop, string stairsIncl,
                                                                 int slabM2, int beamM2, int beamLm, string productSupplier, string supplyType, string supplierRef, string lastComment, decimal phaseInvValue, decimal jobMgnValue, string sortType)
        {

            string insertQry = "INSERT INTO dbo.JobPlanner("
                                            + "parentJobNo,jobNo,phaseNo,floorLevel,requiredDate,siteAddress,approved,OnShop,stairsIncl,slabM2,beamM2, beamLm,supplyType, productSupplier, supplierRef, lastComment, phaseInvValue,jobMgnValue,completedFlag,modifiedDate,modifiedBy,jobCreatedBy,jobCreatedDate,sortType) "
                                            + "VALUES("
                                            + "@parentJobNo,"
                                            + "@jobNo,"
                                            + "@phaseNo,"
                                            + "@floorLevel,"
                                            + "@requiredDate,"
                                            + "@siteAddress,"
                                            + "@approved,"
                                            + "@OnShop,"
                                            + "@stairsIncl,"
                                            + "@slabM2,"
                                            + "@beamM2,"
                                            + "@beamLm,"
                                            + "@supplyType,"
                                            + "@productSupplier,"
                                            + "@supplierRef,"
                                            + "@lastComment,"
                                            + "@phaseInvValue,"
                                            + "@jobMgnValue,"
                                            + "@completedFlag,"
                                            + "@modifiedDate,"
                                            + "@modifiedBy,"
                                            + "@jobCreatedBy,"
                                            + "@jobCreatedDate,"
                                            + "@sortType)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("parentJobNo", parentJobNo));
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("phaseNo", phaseNo));
                        command.Parameters.Add(new SqlParameter("floorLevel", floorLevel));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("approved", approved));
                        command.Parameters.Add(new SqlParameter("OnShop", OnShop));
                        command.Parameters.Add(new SqlParameter("stairsIncl", stairsIncl));
                        command.Parameters.Add(new SqlParameter("slabM2", slabM2));
                        command.Parameters.Add(new SqlParameter("beamM2", beamM2));
                        command.Parameters.Add(new SqlParameter("beamLm", beamLm));
                        command.Parameters.Add(new SqlParameter("supplyType", supplyType));
                        command.Parameters.Add(new SqlParameter("productSupplier", productSupplier));
                        command.Parameters.Add(new SqlParameter("supplierRef", supplierRef));
                        command.Parameters.Add(new SqlParameter("lastComment", lastComment));
                        command.Parameters.Add(new SqlParameter("phaseInvValue", phaseInvValue));
                        command.Parameters.Add(new SqlParameter("jobMgnValue", jobMgnValue));
                        command.Parameters.Add(new SqlParameter("completedFlag", "N"));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("jobCreatedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("jobCreatedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("sortType", sortType));
                        command.ExecuteNonQuery();
                    }
                    string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"CreateJobPlanner(....{requiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateJobPlanner() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateJobPlanner(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }



        public string UpdateJobPlanner(int parentJobNo, string jobNo, string phaseNo, string floorLevel, DateTime requiredDate, string siteAddress, string stairsIncl,
                                                                 int slabM2, int beamM2, int beamLm, string productSupplier, string supplyType, string supplierRef, string lastComment, decimal phaseInvValue, decimal jobMgnValue, string sortType)
        {
            //NOTE :    [designDate] does NOT get updated at all in Job Planner - only in DesignBoard
            //          [requiredDate] only gets updated in Job PLanner via double click event
            string insertQry = "UPDATE dbo.JobPlanner "
                                    + "SET floorLevel = @floorLevel, "
                                //    + "requiredDate = @requiredDate, "
                                    + "siteAddress = @siteAddress, "
                                 //   + "approved = @approved, "
                                 //   + "OnShop = @OnShop, "
                                    + "stairsIncl = @stairsIncl, "
                                    + "slabM2 = @slabM2, "
                                    + "beamM2 = @beamM2, "
                                    + "beamLm = @beamLm, "
                                    + "supplyType = @supplyType, "
                                    + "productSupplier = @productSupplier, "
                                    + "supplierRef = @supplierRef, "
                                    + "lastComment = @lastComment, "
                                    + "modifiedDate = @modifiedDate, "
                                    + "modifiedBy = @modifiedBy, "
                                    + "phaseInvValue = @phaseInvValue,"
                                    + "jobMgnValue = @jobMgnValue,"
                                    + "sortType = @sortType "
                                    + " WHERE parentJobNo = @parentJobNo AND jobNo = @jobNo AND phaseNo = @phaseNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("parentJobNo", parentJobNo));
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("phaseNo", phaseNo));
                        command.Parameters.Add(new SqlParameter("floorLevel", floorLevel));
                     //   command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                     //   command.Parameters.Add(new SqlParameter("approved", approved));
                     //   command.Parameters.Add(new SqlParameter("OnShop", OnShop));
                        command.Parameters.Add(new SqlParameter("stairsIncl", stairsIncl));
                        command.Parameters.Add(new SqlParameter("slabM2", slabM2));
                        command.Parameters.Add(new SqlParameter("beamM2", beamM2));
                        command.Parameters.Add(new SqlParameter("beamLm", beamLm));
                        command.Parameters.Add(new SqlParameter("supplyType", supplyType));
                        command.Parameters.Add(new SqlParameter("productSupplier", productSupplier));
                        command.Parameters.Add(new SqlParameter("supplierRef", supplierRef));
                        command.Parameters.Add(new SqlParameter("lastComment", lastComment));
                        command.Parameters.Add(new SqlParameter("phaseInvValue", phaseInvValue));
                        command.Parameters.Add(new SqlParameter("jobMgnValue", jobMgnValue));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("sortType", sortType));
                        command.ExecuteNonQuery();
                    }
                    //string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"UpdateJobPlanner(....{requiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateJobPlanner() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateJobPlanner(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateJobPlannerJobDate(string jobNo, DateTime requiredDate)
        {
            //string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.JobPlanner "
                                    + "SET requiredDate = @requiredDate, "
                                    + "modifiedDate = @modifiedDate, "
                                    + "modifiedBy = @modifiedBy "
                                    + " WHERE jobNo = @jobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"UpdateJobPlannerJobDate(....{requiredDate.ToShortDateString()}......)");
                    return "OK";

                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateJobPlannerJobDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateJobPlannerJobDate(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateJobPlannerQty(string jobNo, int beamLM, int beamM2, int slabM2)
        {
            string insertQry = $"UPDATE dbo.JobPlanner SET beamLm = {beamLM}, beamM2 = {beamM2}, slabM2 = {slabM2} WHERE jobNo = '{jobNo}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateJobPlannerQty() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateJobPlannerQty(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateMissingJobPlannerBeamLM(string jobNo,int beamLM)
        {
            string insertQry = $"UPDATE dbo.JobPlanner SET beamLm = {beamLM} WHERE jobNo = '{jobNo}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateMissingJobPlannerBeamLM() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateMissingJobPlannerBeamLM(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateMissingJobPlannerBeamM2(string jobNo, int beamM2)
        {
            string insertQry = $"UPDATE dbo.JobPlanner SET beamM2 = {beamM2} WHERE jobNo = '{jobNo}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateMissingJobPlannerBeamM2() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateMissingJobPlannerBeamM2(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateMissingJobPlannerSlabM2(string jobNo, int slabM2)
        {
            string insertQry = $"UPDATE dbo.JobPlanner SET slabM2 = {slabM2} WHERE jobNo = '{jobNo}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateMissingJobPlannerSlabM2() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateMissingJobPlannerSlabM2(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateMissingJobPlannerSupplier(string jobNo, string supplier)
        {
            string insertQry = $"UPDATE dbo.JobPlanner SET productSupplier = '{supplier}' WHERE jobNo = '{jobNo}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateMissingJobPlannerSupplier() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateMissingJobPlannerSupplier(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateJobPlannerSupplierShortName(string jobNo, string shortname)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.JobPlanner "
                                + "SET productSupplier = @productSupplier,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy "
                                + "WHERE LEFT(jobNo,8) = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo.Substring(0, 8)));
                        command.Parameters.Add(new SqlParameter("productSupplier", shortname));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateJobPlannerSupplierShortName() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateJobPlannerSupplierShortName({0},{1})", jobNo, shortname), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string RemoveSupplierFromJobPlanner(string jobNo)
        {
            //string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.JobPlanner "
                                    + "SET productSupplier = @productSupplier, "
                                    + "modifiedDate = @modifiedDate, "
                                    + "modifiedBy = @modifiedBy "
                                    + " WHERE jobNo = @jobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("productSupplier", String.Empty));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("RemoveSupplierFromJobPlanner() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("RemoveSupplierFromJobPlanner({0})", jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string RemoveSupplierFromWB(string jobNo)
        {
            //string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.Whiteboard "
                                    + "SET suppShortname = @suppShortname, "
                                    + "modifiedDate = @modifiedDate, "
                                    + "modifiedBy = @modifiedBy "
                                    + " WHERE jobNo = @jobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("suppShortname", String.Empty));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("RemoveSupplierFromWB() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("RemoveSupplierFromWB({0})", jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateJobPlannerSiteAddrByParentJob(string pJobNo, string siteAddress)
        {
            //string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.JobPlanner "
                                    + "SET siteAddress = @siteAddress, "
                                    + "modifiedDate = @modifiedDate, "
                                    + "modifiedBy = @modifiedBy "
                                    + " WHERE jobNo LIKE '%' + @jobNo + '%'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", pJobNo));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateJobPlannerSiteAddrByParentJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateJobPlannerSiteAddrByParentJob({0},...)", pJobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string GetExtendedJobsList(string jobNo)
        {
            string jobList = "";
            string phaseJob = jobNo.Trim().Substring(0, 8);
            string qry = $"SELECT * FROM dbo.Whiteboard WHERE LEFT(jobNo,8) = '{phaseJob}' ";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            jobList += dr["jobNo"].ToString() + Environment.NewLine;
                        }
                    }

                    return jobList;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetExtendedJobsList() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetExtendedJobsList()", ex.Message.ToString());
                    return null;
                }

            }
        }

        public string CompleteJobPlanner(string jobNo, string completedFlag)
        {
            //string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];


            //string insertQry = "UPDATE dbo.JobPlanner "
            //                        + "SET completedFlag = @completedFlag, modifiedDate = @modifiedDate, modifiedBy = @modifiedBy"
            //                        + " WHERE jobNo = @jobNo";
            string phaseJob = jobNo.Trim().Substring(0, 8);

            //  string selectQry = $"SELECT * FROM dbo.JobPlanner WHERE LEFT(jobNo,8) = '{phaseJob}' ";

            string insertQry = "UPDATE dbo.JobPlanner "
                                    + "SET completedFlag = @completedFlag, modifiedDate = @modifiedDate, modifiedBy = @modifiedBy"
                                    + " WHERE LEFT(jobNo,8) = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        //  command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("jobNo", phaseJob));
                        command.Parameters.Add(new SqlParameter("completedFlag", completedFlag));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CompleteJobPlanner() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("CompleteJobPlanner({0},...)", jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }


        public int DeleteJobPlannerByParentJob(int parentJobNo)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.JobPlanner WHERE parentJobNo = {0}", parentJobNo);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteJobPlannerByParentJob() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteJobPlannerByParentJob({0})", parentJobNo.ToString()), ex.Message.ToString());
                return 0;
            }




        }

        public int DeleteJobPlannerByJobNo(string jobNo)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteJobPlannerByJobNo() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteJobPlannerByJobNo({0})", jobNo), ex.Message.ToString());
                return 0;
            }



        }

        public int DeleteParentJob(int parentJobNo)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.ParentJob WHERE parentJobNo = {0}", parentJobNo);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteParentJob() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteParentJob({0})", parentJobNo.ToString()), ex.Message.ToString());
                return 0;
            }



        }

        public int DeleteDesignBoardByParentJob(string parentJob)
        {
            try
            {
                string qry = $"DELETE FROM dbo.DesignBoard WHERE jobNo LIKE '{parentJob}%'";

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = $"DeleteDesignBoardJob() Error : {ex.Message.ToString()}";
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", $"DeleteDesignBoardJob({parentJob})", ex.Message.ToString());
                return 0;
            }



        }

        public DataTable GetJobCommentsDT(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = string.Format("SELECT * FROM dbo.JobComments WHERE jobNo = '{0}' ORDER BY seq DESC", jobNo);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobCommentsDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobCommentsDT({0})", jobNo), ex.Message.ToString());
                    return null;
                }

            }

        }

        public int GetNextCommentSeq(string jobNo)
        {
            int nextSeq = 0;
            string qry = String.Format("SELECT MAX(seq) FROM dbo.JobComments WHERE jobNo = '{0}'", jobNo);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        object dataReturned = command.ExecuteScalar();
                        if (!Convert.IsDBNull(dataReturned))
                        {
                            nextSeq = Convert.ToInt32(dataReturned) + 1;
                        }
                        else
                        {
                            nextSeq = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetNextCommentSeq() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetNextCommentSeq({0})", jobNo), ex.Message.ToString());
                    return 0;
                }

            }
            return nextSeq;
        }

        public string GetLastPhaseFromJobPlanner(int parentJobNo)
        {
            string error;
            string phase = "";
            string qry = String.Format("SELECT phaseNo FROM dbo.JobPlanner WHERE parentJobNo = {0} order by phaseNo", parentJobNo);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                phase = reader["phaseNo"].ToString();
                            }
                        }
                    }
                    return phase;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetLastPhaseFromJobPlanner({0})", parentJobNo.ToString()), ex.Message.ToString());
                    return "00";
                }

            }
        }

        public string GetLastComment(string jobNo)
        {
            string comment = "";

            string oldQry = String.Format("SELECT LAST comment FROM dbo.JobComments WHERE jobNo = '{0}' ORDER BY seq", jobNo);
            string qry = $"SELECT TOP 1 * FROM dbo.JobComments WHERE jobNo = '{jobNo}' ORDER BY seq DESC";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comment = reader["comment"].ToString();
                            }
                        }
                    }

                    return comment;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetLastComment() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetLastComment({0})", jobNo), ex.Message.ToString());
                    return "";
                }

            }
        }

        public string CreateJobComment(string jobNo, string comment)
        {

            string insertQry = "INSERT INTO dbo.JobComments("
                                            + "jobNo,seq,comment,dateCreated,createdBy) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@seq,"
                                            + "@comment,"
                                            + "@dateCreated,"
                                            + "@createdBy)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("seq", GetNextCommentSeq(jobNo)));
                        command.Parameters.Add(new SqlParameter("comment", comment));
                        command.Parameters.Add(new SqlParameter("dateCreated", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("createdBy", ConfigurationManager.AppSettings["LoggedInUser"]));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateJobComment() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("CreateJobComment({0},{1})", jobNo, comment), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateJobComment(string jobNo, int seq, string comment)
        {

            string insertQry = "UPDATE dbo.JobComments "
                                    + "SET comment = @comment, "
                                    + "dateCreated = @dateCreated, "
                                    + "createdBy = @createdBy "
                                    + " WHERE jobNo = @jobNo AND seq = @seq";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("seq", seq));
                        command.Parameters.Add(new SqlParameter("comment", comment));
                        command.Parameters.Add(new SqlParameter("dateCreated", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("createdBy", ConfigurationManager.AppSettings["LoggedInUser"]));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateJobComment() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateJobComment({0},{1},{2})", jobNo, seq.ToString(), comment), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public DataTable GetParentJobsByCustCodeDT(string custCode)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = string.Format("SELECT * FROM dbo.ParentJob WHERE custCode = '{0}' ORDER BY parentJobNo DESC", custCode);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetParentJobsByCustCodeDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetParentJobsByCustCodeDT({0})", custCode), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetParentJobsBySiteKeyDT(string siteKey)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = string.Format("SELECT * FROM dbo.ParentJob WHERE UPPER(siteAddress) LIKE '%{0}%' ORDER BY parentJobNo DESC", siteKey.ToUpper());

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetParentJobsBySiteKeyDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetParentJobsBySiteKeyDT({0})", siteKey), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DateTime GetFirstPlannerDate()
        {
            DateTime date = DateTime.MinValue;
            //string qry = "SELECT MIN(requiredDate) as firstDate FROM dbo.JobPlanner WHERE completedFlag = 'N' AND ( OnShop = 'Y' AND Approved = 'Y' )";
            string qry = "SELECT MIN(requiredDate) as firstDate FROM dbo.JobPlanner WHERE  OnShop = 'Y' AND Approved = 'Y'";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                date = Convert.ToDateTime(reader["firstDate"].ToString());
                            }
                        }
                    }

                    return date;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetFirstPlannerDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetFirstPlannerDate()", ex.Message.ToString());
                    return DateTime.Now;
                }

            }
        }

        public DateTime GetLastPlannerDate()
        {
            DateTime date = DateTime.MinValue;
            //  string qry = "SELECT MAX(requiredDate) as lastDate FROM dbo.JobPlanner WHERE completedFlag = 'N' AND ( OnShop = 'Y' AND Approved = 'Y' )";
            string qry = "SELECT MAX(requiredDate) as lastDate FROM dbo.JobPlanner WHERE OnShop = 'Y' AND Approved = 'Y'";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                date = Convert.ToDateTime(reader["lastDate"].ToString());
                            }
                        }
                    }

                    return date;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetLastPlannerDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetLastPlannerDate()", ex.Message.ToString());
                    return DateTime.Now;
                }

            }
        }

        public DateTime GetFirstWhiteboardDate()
        {
            DateTime date = DateTime.MinValue;
            //string qry = "SELECT MIN(requiredDate) as firstDate FROM dbo.JobPlanner WHERE completedFlag = 'N' AND ( OnShop = 'Y' AND Approved = 'Y' )";
            string qry = "SELECT MIN(requiredDate) as firstDate FROM dbo.Whiteboard";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                date = Convert.ToDateTime(reader["firstDate"].ToString());
                            }
                        }
                    }

                    return date;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetFirstWhiteboardDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetFirstWhiteboardDate()", ex.Message.ToString());
                    return DateTime.Now;
                }

            }
        }

        public DateTime GetLastWhiteboardDate()
        {
            DateTime date = DateTime.MinValue;
            //  string qry = "SELECT MAX(requiredDate) as lastDate FROM dbo.JobPlanner WHERE completedFlag = 'N' AND ( OnShop = 'Y' AND Approved = 'Y' )";
            string qry = "SELECT MAX(requiredDate) as lastDate FROM dbo.Whiteboard";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                date = Convert.ToDateTime(reader["lastDate"].ToString());
                            }
                        }
                    }

                    return date;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetLastWhiteboardDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetLastWhiteboardDate()", ex.Message.ToString());
                    return DateTime.Now;
                }

            }
        }

        public DateTime GetPlannerDateByJobNo(string jobNo)
        {
            DateTime date = DateTime.MinValue;

            string qry = String.Format("SELECT requiredDate FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                date = Convert.ToDateTime(reader["requiredDate"].ToString());
                            }
                        }
                    }

                    return date;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetFirstPlannerDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetPlannerDateByJobNo({0})", jobNo), ex.Message.ToString());
                    return DateTime.Now;
                }

            }
        }

        public string GetJobPlannerSupplier(string jobNo)
        {
            string shortname = "";

            string qry = String.Format("SELECT productSupplier FROM dbo.JobPlanner WHERE LEFT(jobNo,8) = '{0}'", jobNo.Substring(0, 8));
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                shortname = reader["productSupplier"].ToString();
                            }
                        }
                    }

                    return shortname;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerSupplier() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobPlannerSupplier({0})", jobNo), ex.Message.ToString());
                    return shortname;
                }

            }
        }

        public DateTime GetJobCreatedDate(string jobNo)
        {
            DateTime date = DateTime.MinValue;

            string qry = String.Format("SELECT jobCreatedDate FROM dbo.JobPlanner WHERE jobNo = '{0}'", jobNo);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                date = Convert.ToDateTime(reader["jobCreatedDate"].ToString());
                            }
                        }
                    }

                    return date;


                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobCreatedDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobCreatedDate({0})", jobNo), ex.Message.ToString());
                    return DateTime.Now;
                }

            }
        }

        public int EmptyTheJobPlanner()
        {

            string qry = "TRUNCATE TABLE dbo.JobPlanner";

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = qry;
            cmd.Connection = conn;
            conn.Open();
            int numDeleted = cmd.ExecuteNonQuery();
            conn.Close();
            return numDeleted;


        }

        #endregion

        #region WHITEBOARD

        public DataTable GetKeyWhiteboardDetailsBySupplier(string supplier)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = $@"
                        select wb.jobNo as jobNo,jp.beamLm as beamLm,jp.beamM2 as beamM2, 
                        jp.slabM2 as slabM2,wb.totalM2 as totalM2,  wb.stairsIncl as stairsIncl,
                        wb.suppShortname as supplier, 
                        wb.products as product
                        from dbo.Whiteboard wb  
                        inner join dbo.JobPlanner jp 
                        on wb.jobNo = jp.jobNo 
                        where wb.suppShortname = '{supplier}'";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetKeyWhiteboardDetailsBySupplier() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetKeyWhiteboardDetailsBySupplier({supplier})", ex.Message.ToString());
                    return null;
                }

            }
        }

        public DataTable GetKeyJobPlannerDetailsBySupplier(string supplier)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = $@"
                        select jobNo,beamLm,beamM2, slabM2,stairsIncl, productSupplier FROM dbo.JobPlanner WHERE productSupplier = '{supplier}'";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetKeyJobPlannerDetailsBySupplier() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetKeyJobPlannerDetailsBySupplier({supplier})", ex.Message.ToString());
                    return null;
                }

            }
        }

        public DataTable GetWhiteboardSupplierSummary()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = @"
                                SELECT 
                                WB.completedFlag as Completed,
                                WB.suppShortname as Supplier, 
                                COUNT(WB.jobNo) as NumJobs,
                                SUM(JP.beamLm) as BeamLM,
                                SUM(JP.beamM2) as BeamM2,
                                SUM(JP.slabM2) as SlabM2,
                                SUM(WB.TotalM2) as TotalM2 
                                FROM dbo.Whiteboard WB
                                LEFT JOIN dbo.JobPlanner JP
                                ON WB.jobNo = JP.jobNo
                                GROUP BY WB.completedFlag,WB.suppShortname
                                ORDER BY WB.completedFlag DESC,WB.suppShortname
                                ";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetWhiteboardSupplierSummary() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetWhiteboardSupplierSummary()", ex.Message.ToString());
                    return null;
                }

            }
        }

        public DataTable GetJobPlannerSupplierSummary()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = @"
                                SELECT 
                                completedFlag as Completed,
                                productSupplier as Supplier, 
                                COUNT(jobNo) as NumJobs,
                                SUM(beamLm) as BeamLM,
                                SUM(beamM2) as BeamM2,
                                SUM(slabM2) as SlabM2
                                FROM dbo.JobPlanner
                                GROUP BY completedFlag,productSupplier
                                ORDER BY completedFlag DESC,productSupplier
                                ";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPlannerSupplierSummary() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetJobPlannerSupplierSummary()", ex.Message.ToString());
                    return null;
                }

            }
        }

        public void GetKeyWhiteboardDetails(string jobNo, out DateTime reqDate, out string site, out string suppType, out string product, out string supplier, 
                out int beamLm, out int beamM2, out int slabM2, out int totalM2)
        {
            string error;

            reqDate = DateTime.MinValue;
            site = "";
            suppType = "";
            product = "";
            supplier = "";
            beamLm = 0;
            beamM2 = 0;
            slabM2 = 0;
            totalM2 = 0;

            string qry = $@"
                        select wb.jobNo as jobNo,wb.siteAddress as siteAddress,  
                        jp.beamLm as beamLm,jp.beamM2 as beamM2, 
                        jp.slabM2 as slabM2,wb.totalM2 as totalM2,  
                        wb.suppShortname as supplier, 
                        wb.products as product, wb.supplyType as supplyType, 
                        wb.requiredDate as requiredDate  
                        from dbo.Whiteboard wb  
                        inner join dbo.JobPlanner jp 
                        on wb.jobNo = jp.jobNo 
                        where wb.jobNo = '{jobNo}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reqDate = reader["requiredDate"] == null ? DateTime.MinValue : Convert.ToDateTime(reader["requiredDate"].ToString());
                                site = reader["siteAddress"] == null ? "" : reader["siteAddress"].ToString();
                                suppType = reader["supplyType"] == null ? "" : reader["supplyType"].ToString();
                                product = reader["product"] == null ? "" : reader["product"].ToString();
                                supplier = reader["supplier"] == null ? "" : reader["supplier"].ToString();
                                beamLm = reader["beamLm"] == null ? 0 : Convert.ToInt32(reader["beamLm"].ToString());
                                beamM2 = reader["beamM2"] == null ? 0 : Convert.ToInt32(reader["beamM2"].ToString());
                                slabM2 = reader["slabM2"] == null ? 0 : Convert.ToInt32(reader["slabM2"].ToString());
                                totalM2 = reader["totalM2"] == null ? 0 : Convert.ToInt32(reader["totalM2"].ToString());
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetKeyWhiteboardDetails({jobNo})", ex.Message.ToString());
                    return;
                }

            }
        }

        public void GetKeyWhiteboardQuantities(string jobNo, out string suppType, out string product, out string supplier, out int beamLm, out int beamM2, out int slabM2, out int totalM2)
        {
            string error;

            suppType = "";
            product = "";
            supplier = "";
            beamLm = 0;
            beamM2 = 0;
            slabM2 = 0;
            totalM2 = 0;

            string qry = $@"
                        select wb.jobNo as jobNo,
                        jp.beamLm as beamLm,jp.beamM2 as beamM2, 
                        jp.slabM2 as slabM2,wb.totalM2 as totalM2,  
                        wb.suppShortname as supplier, 
                        wb.products as product, wb.supplyType as supplyType 
                        from dbo.Whiteboard wb  
                        inner join dbo.JobPlanner jp 
                        on wb.jobNo = jp.jobNo 
                        where wb.jobNo = '{jobNo}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                suppType = reader["supplyType"] == null ? "" : reader["supplyType"].ToString();
                                product = reader["product"] == null ? "" : reader["product"].ToString();
                                supplier = reader["supplier"] == null ? "" : reader["supplier"].ToString();
                                beamLm = reader["beamLm"] == null ? 0 : Convert.ToInt32(reader["beamLm"].ToString());
                                beamM2 = reader["beamM2"] == null ? 0 : Convert.ToInt32(reader["beamM2"].ToString());
                                slabM2 = reader["slabM2"] == null ? 0 : Convert.ToInt32(reader["slabM2"].ToString());
                                totalM2 = reader["totalM2"] == null ? 0 : Convert.ToInt32(reader["totalM2"].ToString());
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetKeyWhiteboardDetails({jobNo})", ex.Message.ToString());
                    return;
                }

            }
        }

        public int EmptyCurrentInvoicedWhiteboardJobList()
        {

            string qry = "TRUNCATE TABLE dbo.WhiteboardJobInvoicedToday";

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = qry;
            cmd.Connection = conn;
            conn.Open();
            int numDeleted = cmd.ExecuteNonQuery();
            conn.Close();
            return numDeleted;


        }

        public bool IsCurrentInvoicedWhiteboardJobExists(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.WhiteboardJobInvoicedToday WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsCurrentInvoicedWhiteboardJobExists() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsCurrentInvoicedWhiteboardJobExists({0})", jobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        public string AddWhiteboardJobToCurrentInvoicedList(string jobNo)
        {
            if (IsCurrentInvoicedWhiteboardJobExists(jobNo)) { return "OK"; }
            string insertQry = "INSERT INTO dbo.WhiteboardJobInvoicedToday("
                                            + "jobNo) "
                                            + "VALUES("
                                            + "@jobNo)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("AddWhiteboardJobToCurrentInvoicedList() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("AddWhiteboardJobToCurrentInvoicedList({0},....)", jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string GetLastJobNo(string jobNo)
        {

            String lastJob = jobNo;
            // string qryOld = "SELECT MAX VALUE FOR dbo.ParentJobNoSeq AS nextParentJobNo";
            string qry = String.Format("SELECT jobNo from dbo.Whiteboard WHERE jobNo LIKE '{0}%' ORDER BY jobNo", jobNo);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lastJob = reader["jobNo"].ToString();
                            }
                        }
                    }
                    return lastJob;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetLastJobNo() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetLastJobNo({0})", jobNo), ex.Message.ToString());
                    return jobNo;
                }

            }
        }

        public string GetNextJobSuffix(string jobNo)
        {
            char lastCharacter = jobNo[jobNo.Length - 1];
            if (!Char.IsLetter(lastCharacter)) { return jobNo + ".A"; }

            char nextChar = (char)((int)Convert.ToChar(Convert.ToChar(lastCharacter) + 1));
            return jobNo.Substring(0, jobNo.Length - 1) + nextChar;
        }

        public DataTable GetCompletedWhiteboardJobsWithMissingProducts()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Whiteboard WHERE completedFlag = 'Y' and LEN(products) < 1 ORDER BY requiredDate DESC";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetCompletedWhiteboardJobsWithMissingProducts() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetCompletedWhiteboardJobsWithMissingProducts()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetWhiteboardByJobDT(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = string.Format("SELECT * FROM dbo.Whiteboard WHERE jobNo = '{0}'", jobNo);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetWhiteboardByJobDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetWhiteboardByJobDT({0})", jobNo), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetWBCommentsAuditDT(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = string.Format("SELECT * FROM dbo.WhiteboardDayCommentsAudit WHERE jobNo = '{0}' ORDER BY dateModified DESC", jobNo);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetWBCommentsAuditDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetWBCommentsAuditDT({0})", jobNo), ex.Message.ToString());
                    return null;
                }

            }

        }

        public string CreateWBDayCommentAudit(string jobNo, string comment, DateTime commentDate)
        {

            string insertQry = "INSERT INTO dbo.WhiteboardDayCommentsAudit("
                                            + "jobNo,comment,commentDate,dateModified,modifiedBy) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@comment,"
                                            + "@commentDate,"
                                            + "@dateModified,"
                                            + "@modifiedBy)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("seq", GetNextCommentSeq(jobNo)));
                        command.Parameters.Add(new SqlParameter("comment", comment));
                        command.Parameters.Add(new SqlParameter("commentDate", commentDate));
                        command.Parameters.Add(new SqlParameter("dateModified", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", ConfigurationManager.AppSettings["LoggedInUser"]));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateWBDayCommentAudit() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("CreateWBDayCommentAudit({0},....)", jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public DataTable GetWhiteboardByDateRangeDT(DateTime startDate, DateTime endDate)
        {
            //string qry1 = "SELECT * FROM dbo.Whiteboard WHERE requiredDate between @startDate and @endDate AND completedFlag != 'Y' ORDER BY supplyType,requiredDate";
            string qry1 = @"SELECT * FROM dbo.Whiteboard WHERE requiredDate between @startDate and @endDate 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY sortType,requiredDate,jobNo";



            //string qry = "SELECT * FROM dbo.Whiteboard wb INNER JOIN dbo.JobPlanner jp ON wb.jobNo = jp.jobNo where jp.OnShop = 'Y' and jp.Approved = 'Y' " +
            //                        "and jp.requiredDate between @startDate and @endDate AND jp.completedFlag != 'Y' ORDER BY wb.supplyType,wb.requiredDate,wb.jobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();


                    SqlCommand cmd = new SqlCommand(qry1, conn);
                    //cmd.Parameters.Add(new SqlParameter("wbCompCode", compCode));
                    cmd.Parameters.Add(new SqlParameter("startDate", startDate.Date));
                    cmd.Parameters.Add(new SqlParameter("endDate", endDate.Date));
                    //cmd.Parameters.Add(new SqlParameter("wbSupplyType", supplyType));
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    return dt;



                    //string qry = String.Format("SELECT * FROM dbo.JobPlanner WHERE requiredDate between {0} and {1} ORDER BY jobNo", startDate,endDate);

                    //SqlCommand cmd = new SqlCommand(qry, conn);
                    //DataTable dt = new DataTable();
                    //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    //sda.Fill(dt);

                    //return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetWhiteboardByDateRangeDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetWhiteboardByDateRangeDT({0},{1})", startDate.ToShortDateString(), endDate.ToShortDateString()), ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GeDesignboardByDateRangeDT(DateTime startDate, DateTime endDate)
        {
            string qry1 = @"SELECT * FROM dbo.DesignBoard WHERE designDate between @startDate and @endDate 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY designDate,jobNo";
   
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();


                    SqlCommand cmd = new SqlCommand(qry1, conn);
                    cmd.Parameters.Add(new SqlParameter("startDate", startDate.Date));
                    cmd.Parameters.Add(new SqlParameter("endDate", endDate.Date));
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = $"GeDesignboardByDateRangeDT() Error : {ex.Message.ToString()}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GeDesignboardByDateRangeDT({startDate.ToShortDateString()},{endDate.ToShortDateString()})", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetWhiteboardJobExtensionsWithDatesAndCompleteFlagsDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();

                    string qry = "SELECT * from dbo.ExtendedWhiteboardJobsView ORDER BY jobNo";
                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetWhiteboardJobExtensionsWithDatesAndCompleteFlagsDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    return null;
                }

            }

        }

        public DataTable GetWhiteboardJobProductsByQtyDT(string rptMode)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "";
                    switch (rptMode)
                    {
                        case "ALL":
                            qry = @"
                                    select wb.jobNo as job,
                                    wb.completedFlag as Completed,
                                    wb.requiredDate as RequiredDate,
                                    wb.floorlevel as FloorLevel,
                                    wb.supplyType as jobType,
                                    wb.products as CurrentProduct,
                                    '' as RevisedProduct,
                                    wb.stairsIncl as stairs,
                                    wb.totalM2 as TotalM2,
                                    jp.beamLm as BeamLm,
                                    jp.beamLm as BeamM2,
                                    jp.slabM2 as SlabM2,
                                    wb.suppShortname as Supplier,
                                    wb.custCode as CustomerCode,
                                    REPLACE(REPLACE(wb.siteAddress, CHAR(13), ''), CHAR(10), '') as SiteAddress
                                    from dbo.Whiteboard wb inner join dbo.JobPlanner jp 
                                    on wb.jobNo = jp.jobNo
                                    order by wb.products, wb.completedFlag DESC ,wb.jobNo
                                    ";
                            break;
                        case "COMPLETED":
                            qry = @"
                                    select wb.jobNo as job,
                                    wb.completedFlag as Completed,
                                    wb.requiredDate as RequiredDate,
                                    wb.floorlevel as FloorLevel,
                                    wb.supplyType as jobType,
                                    wb.products as CurrentProduct,
                                    '' as RevisedProduct,
                                    wb.stairsIncl as stairs,
                                    wb.totalM2 as TotalM2,
                                    jp.beamLm as BeamLm,
                                    jp.beamLm as BeamM2,
                                    jp.slabM2 as SlabM2,
                                    wb.suppShortname as Supplier,
                                    wb.custCode as CustomerCode,
                                    REPLACE(REPLACE(wb.siteAddress, CHAR(13), ''), CHAR(10), '') as SiteAddress
                                    from dbo.Whiteboard wb inner join dbo.JobPlanner jp 
                                    on wb.jobNo = jp.jobNo
                                    WHERE (wb.completedFlag = 'Y'AND jp.completedFlag = 'Y') AND LEN(wb.products) < 1
                                    order by wb.products, wb.jobNo
                                    ";
                            break;
                        case "INPROGRESS":
                            qry = @"
                                    select wb.jobNo as job,
                                    wb.completedFlag as Completed,
                                    wb.requiredDate as RequiredDate,
                                    wb.floorlevel as FloorLevel,
                                    wb.supplyType as jobType,
                                    wb.products as CurrentProduct,
                                    '' as RevisedProduct,
                                    wb.stairsIncl as stairs,
                                    wb.totalM2 as TotalM2,
                                    jp.beamLm as BeamLm,
                                    jp.beamLm as BeamM2,
                                    jp.slabM2 as SlabM2,
                                    wb.suppShortname as Supplier,
                                    wb.custCode as CustomerCode,
                                    REPLACE(REPLACE(wb.siteAddress, CHAR(13), ''), CHAR(10), '') as SiteAddress
                                    from dbo.Whiteboard wb inner join dbo.JobPlanner jp 
                                    on wb.jobNo = jp.jobNo
                                    WHERE wb.completedFlag <> 'Y'AND jp.completedFlag <> 'Y' AND LEN(wb.products) < 1
                                    order by wb.products,wb.jobNo
                                    ";
                            break;
                        default:
                            break;
                    }
                    
                    
                    //   string qry = "SELECT * from dbo.ExtendedWhiteboardJobsView ORDER BY jobNo";
                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllWhiteboardJobProductsByQtyDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    return null;
                }

            }

        }

        public DataTable GetWhiteboardCompletedJobsWithMissingProductsFromView()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();

                    string qry = "SELECT * from dbo.CompletedWhiteboardJobsWithMissingProducts";
                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetWhiteboardCompletedJobsWithMissingProductsFromView() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    return null;
                }

            }

        }

        public string CreateWhiteBoard(string jobNo, DateTime requiredDate, string custCode, string siteAddress, string supplyType, string products, int totalM2, int fixingDaysAllowance, decimal salesPrice,
                                        string isInvoiced, string suppShortname, string stairsIncl, string stairsSupplier, string floorlevel, string continuedFlag, string cowSentFlag, string cowReceived,
                                        string wcMonday, string wcTuesday, string wcWednesday, string wcThursday, string wcFriday, string wcSaturday, string wcSunday, string contracts, string ramsFlag,
                                        string ramsSignedReturnedFlag, string ramsCompleteReturnedFlag, string lorry, string craneSize, string craneSupplier, string spreaderMatFlag, string hireContractRcvd,
                                        string fallArrest, string fixingGang, string onHireFlag, string extrasFlag, string concrete, string blocks, string drawingsEmailedFlag, string draughtsman, string salesman,
                                        string lastComment, string sortType)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
            //string custName = GetCustName(custCode);
            string insertQry = "INSERT INTO dbo.WhiteBoard("
                                            + "jobNo,requiredDate,custCode,siteAddress,supplyType,products,totalM2,fixingDaysAllowance,salesPrice,isInvoiced,suppShortname,stairsIncl,stairsSupplier,"
                                            + "floorlevel,continuedFlag,cowSentFlag,cowReceived,wcMonday,wcTuesday,wcWednesday,wcThursday,wcFriday,wcSaturday,wcSunday,contracts,ramsFlag,ramsSignedReturnedFlag,"
                                            + "ramsCompleteReturnedFlag,lorry,craneSize,craneSupplier,spreaderMatFlag,hireContractRcvd,fallArrest,fixingGang,onHireFlag,extrasFlag,concrete,blocks,drawingsEmailedFlag,"
                                            + "draughtsman,salesman,lastComment,modifiedDate,modifiedBy,completedFlag,jobCreatedBy, sortType) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@requiredDate,"
                                            + "@custCode,"
                                            + "@siteAddress,"
                                            + "@supplyType,"
                                            + "@products,"
                                            + "@totalM2,"
                                            + "@fixingDaysAllowance,"
                                            + "@salesPrice,"
                                            + "@isInvoiced,"
                                            + "@suppShortname,"
                                            + "@stairsIncl,"
                                            + "@stairsSupplier,"
                                            + "@floorlevel,"
                                            + "@continuedFlag,"
                                            + "@cowSentFlag,"
                                            + "@cowReceived,"
                                            + "@wcMonday,"
                                            + "@wcTuesday,"
                                            + "@wcWednesday,"
                                            + "@wcThursday,"
                                            + "@wcFriday,"
                                            + "@wcSaturday,"
                                            + "@wcSunday,"
                                            + "@contracts,"
                                            + "@ramsFlag,"
                                            + "@ramsSignedReturnedFlag,"
                                            + "@ramsCompleteReturnedFlag,"
                                            + "@lorry,"
                                            + "@craneSize,"
                                            + "@craneSupplier,"
                                            + "@spreaderMatFlag,"
                                            + "@hireContractRcvd,"
                                            + "@fallArrest,"
                                            + "@fixingGang,"
                                            + "@onHireFlag,"
                                            + "@extrasFlag,"
                                            + "@concrete,"
                                            + "@blocks,"
                                            + "@drawingsEmailedFlag,"
                                            + "@draughtsman,"
                                            + "@salesman,"
                                            + "@lastComment,"
                                            + "@modifiedDate,"
                                            + "@modifiedBy,"
                                            + "@completedFlag,"
                                            + "@jobCreatedBy,"
                                            + "@sortType)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("supplyType", supplyType));
                        command.Parameters.Add(new SqlParameter("products", products));
                        command.Parameters.Add(new SqlParameter("totalM2", totalM2));
                        command.Parameters.Add(new SqlParameter("fixingDaysAllowance", fixingDaysAllowance));
                        command.Parameters.Add(new SqlParameter("salesPrice", salesPrice));
                        command.Parameters.Add(new SqlParameter("isInvoiced", isInvoiced));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("stairsIncl", stairsIncl));
                        command.Parameters.Add(new SqlParameter("stairsSupplier", stairsSupplier));
                        command.Parameters.Add(new SqlParameter("floorlevel", floorlevel));
                        command.Parameters.Add(new SqlParameter("continuedFlag", continuedFlag));
                        command.Parameters.Add(new SqlParameter("cowSentFlag", cowSentFlag));
                        command.Parameters.Add(new SqlParameter("cowReceived", cowReceived));
                        command.Parameters.Add(new SqlParameter("wcMonday", wcMonday));
                        command.Parameters.Add(new SqlParameter("wcTuesday", wcTuesday));
                        command.Parameters.Add(new SqlParameter("wcWednesday", wcWednesday));
                        command.Parameters.Add(new SqlParameter("wcThursday", wcThursday));
                        command.Parameters.Add(new SqlParameter("wcFriday", wcFriday));
                        command.Parameters.Add(new SqlParameter("wcSaturday", wcSaturday));
                        command.Parameters.Add(new SqlParameter("wcSunday", wcSunday));
                        command.Parameters.Add(new SqlParameter("contracts", contracts));
                        command.Parameters.Add(new SqlParameter("ramsFlag", ramsFlag));
                        command.Parameters.Add(new SqlParameter("ramsSignedReturnedFlag", ramsSignedReturnedFlag));
                        command.Parameters.Add(new SqlParameter("ramsCompleteReturnedFlag", ramsCompleteReturnedFlag));
                        command.Parameters.Add(new SqlParameter("lorry", lorry));
                        command.Parameters.Add(new SqlParameter("craneSize", craneSize));
                        command.Parameters.Add(new SqlParameter("craneSupplier", craneSupplier));
                        command.Parameters.Add(new SqlParameter("spreaderMatFlag", spreaderMatFlag));
                        command.Parameters.Add(new SqlParameter("hireContractRcvd", hireContractRcvd));
                        command.Parameters.Add(new SqlParameter("fallArrest", fallArrest));
                        command.Parameters.Add(new SqlParameter("fixingGang", fixingGang));
                        command.Parameters.Add(new SqlParameter("onHireFlag", onHireFlag));
                        command.Parameters.Add(new SqlParameter("extrasFlag", extrasFlag));
                        command.Parameters.Add(new SqlParameter("concrete", concrete));
                        command.Parameters.Add(new SqlParameter("blocks", blocks));
                        command.Parameters.Add(new SqlParameter("drawingsEmailedFlag", drawingsEmailedFlag));
                        command.Parameters.Add(new SqlParameter("draughtsman", draughtsman));
                        command.Parameters.Add(new SqlParameter("salesman", salesman));
                        command.Parameters.Add(new SqlParameter("lastComment", lastComment));
                        command.Parameters.Add(new SqlParameter("completedFlag", "N"));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("jobCreatedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("sortType", sortType));
                        command.ExecuteNonQuery();
                    }
                    string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"CreateWhiteBoard(....{requiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateWhiteBoard() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateWhiteBoard(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string CreateWhiteBoardSpannedJobCopy(string newJobNo, DateTime newRequiredDate, DataTable jobDT)
        {

            if (IsWhiteboardJobExists(newJobNo)) { return "ERR"; }

            DataRow dr = jobDT.Rows[0];
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
            //string custName = GetCustName(custCode);
            string insertQry = "INSERT INTO dbo.WhiteBoard("
                                            + "jobNo,requiredDate,custCode,siteAddress,supplyType,products,totalM2,fixingDaysAllowance,salesPrice,isInvoiced,suppShortname,stairsIncl,stairsSupplier,"
                                            + "floorlevel,continuedFlag,cowSentFlag,cowReceived,wcMonday,wcTuesday,wcWednesday,wcThursday,wcFriday,wcSaturday,wcSunday,contracts,ramsFlag,ramsSignedReturnedFlag,"
                                            + "ramsCompleteReturnedFlag,lorry,craneSize,craneSupplier,spreaderMatFlag,hireContractRcvd,fallArrest,fixingGang,onHireFlag,extrasFlag,concrete,blocks,drawingsEmailedFlag,"
                                            + "draughtsman,salesman,lastComment,modifiedDate,modifiedBy,completedFlag,jobCreatedBy) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@requiredDate,"
                                            + "@custCode,"
                                            + "@siteAddress,"
                                            + "@supplyType,"
                                            + "@products,"
                                            + "@totalM2,"
                                            + "@fixingDaysAllowance,"
                                            + "@salesPrice,"
                                            + "@isInvoiced,"
                                            + "@suppShortname,"
                                            + "@stairsIncl,"
                                            + "@stairsSupplier,"
                                            + "@floorlevel,"
                                            + "@continuedFlag,"
                                            + "@cowSentFlag,"
                                            + "@cowReceived,"
                                            + "@wcMonday,"
                                            + "@wcTuesday,"
                                            + "@wcWednesday,"
                                            + "@wcThursday,"
                                            + "@wcFriday,"
                                            + "@wcSaturday,"
                                            + "@wcSunday,"
                                            + "@contracts,"
                                            + "@ramsFlag,"
                                            + "@ramsSignedReturnedFlag,"
                                            + "@ramsCompleteReturnedFlag,"
                                            + "@lorry,"
                                            + "@craneSize,"
                                            + "@craneSupplier,"
                                            + "@spreaderMatFlag,"
                                            + "@hireContractRcvd,"
                                            + "@fallArrest,"
                                            + "@fixingGang,"
                                            + "@onHireFlag,"
                                            + "@extrasFlag,"
                                            + "@concrete,"
                                            + "@blocks,"
                                            + "@drawingsEmailedFlag,"
                                            + "@draughtsman,"
                                            + "@salesman,"
                                            + "@lastComment,"
                                            + "@modifiedDate,"
                                            + "@modifiedBy,"
                                            + "@completedFlag,"
                                            + "@jobCreatedBy)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", newJobNo));
                        command.Parameters.Add(new SqlParameter("requiredDate", newRequiredDate));
                        command.Parameters.Add(new SqlParameter("custCode", dr["custCode"].ToString()));
                        command.Parameters.Add(new SqlParameter("siteAddress", dr["siteAddress"].ToString()));
                        command.Parameters.Add(new SqlParameter("supplyType", dr["supplyType"].ToString()));
                        command.Parameters.Add(new SqlParameter("products", dr["products"].ToString()));
                        command.Parameters.Add(new SqlParameter("totalM2", Convert.ToInt16(dr["totalM2"].ToString())));
                        command.Parameters.Add(new SqlParameter("fixingDaysAllowance", Convert.ToInt16(dr["fixingDaysAllowance"].ToString())));
                        command.Parameters.Add(new SqlParameter("salesPrice", Convert.ToDecimal(dr["salesPrice"].ToString())));
                        command.Parameters.Add(new SqlParameter("isInvoiced", dr["isInvoiced"].ToString()));
                        command.Parameters.Add(new SqlParameter("suppShortname", dr["suppShortname"].ToString()));
                        command.Parameters.Add(new SqlParameter("stairsIncl", dr["stairsIncl"].ToString()));
                        command.Parameters.Add(new SqlParameter("stairsSupplier", dr["stairsSupplier"].ToString()));
                        command.Parameters.Add(new SqlParameter("floorlevel", dr["floorlevel"].ToString()));
                        command.Parameters.Add(new SqlParameter("continuedFlag", dr["continuedFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("cowSentFlag", dr["cowSentFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("cowReceived", dr["cowReceived"].ToString()));
                        command.Parameters.Add(new SqlParameter("wcMonday", ""));
                        command.Parameters.Add(new SqlParameter("wcTuesday", ""));
                        command.Parameters.Add(new SqlParameter("wcWednesday", ""));
                        command.Parameters.Add(new SqlParameter("wcThursday", ""));
                        command.Parameters.Add(new SqlParameter("wcFriday", ""));
                        command.Parameters.Add(new SqlParameter("wcSaturday", ""));
                        command.Parameters.Add(new SqlParameter("wcSunday", ""));
                        command.Parameters.Add(new SqlParameter("contracts", dr["contracts"].ToString()));
                        command.Parameters.Add(new SqlParameter("ramsFlag", dr["ramsFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("ramsSignedReturnedFlag", dr["ramsSignedReturnedFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("ramsCompleteReturnedFlag", dr["ramsCompleteReturnedFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("lorry", dr["lorry"].ToString()));
                        command.Parameters.Add(new SqlParameter("craneSize", dr["craneSize"].ToString()));
                        command.Parameters.Add(new SqlParameter("craneSupplier", dr["craneSupplier"].ToString()));
                        command.Parameters.Add(new SqlParameter("spreaderMatFlag", dr["spreaderMatFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("hireContractRcvd", dr["hireContractRcvd"].ToString()));
                        command.Parameters.Add(new SqlParameter("fallArrest", dr["fallArrest"].ToString()));
                        command.Parameters.Add(new SqlParameter("fixingGang", dr["fixingGang"].ToString()));
                        command.Parameters.Add(new SqlParameter("onHireFlag", dr["onHireFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("extrasFlag", dr["extrasFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("concrete", dr["concrete"].ToString()));
                        command.Parameters.Add(new SqlParameter("blocks", dr["blocks"].ToString()));
                        command.Parameters.Add(new SqlParameter("drawingsEmailedFlag", dr["drawingsEmailedFlag"].ToString()));
                        command.Parameters.Add(new SqlParameter("draughtsman", dr["draughtsman"].ToString()));
                        command.Parameters.Add(new SqlParameter("salesman", dr["salesman"].ToString()));
                        command.Parameters.Add(new SqlParameter("lastComment", dr["lastComment"].ToString()));
                        command.Parameters.Add(new SqlParameter("completedFlag", "N"));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("jobCreatedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    string err2 = CreateJobDayAudit(newJobNo, newRequiredDate.Date, $"CreateWhiteBoardSpannedJobCopy(....{newRequiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateWhiteBoardSpannedJobCopy() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateWhiteBoardSpannedJobCopy(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }


        public string UpdateWhiteBoard(string jobNo, /*DateTime requiredDate,*/ string custCode, string siteAddress, string supplyType, string products, int totalM2, int fixingDaysAllowance, decimal salesPrice,
                                        string isInvoiced, string suppShortname, string stairsIncl, string stairsSupplier, string floorlevel, string continuedFlag, string cowSentFlag, string cowReceived,
                                        /*string wcMonday, string wcTuesday, string wcWednesday, string wcThursday, string wcFriday, string wcSaturday, string wcSunday,*/ string contracts, string ramsFlag,
                                        string ramsSignedReturnedFlag, string ramsCompleteReturnedFlag, string lorry, string craneSize, string craneSupplier, string spreaderMatFlag, string hireContractRcvd,
                                        string fallArrest, string fixingGang, string onHireFlag, string extrasFlag, string concrete, string blocks, string drawingsEmailedFlag, string draughtsman, string salesman,
                                        string lastComment, string sortType)
        {
            if (jobNo == "40035.1")
            {
                bool isStopHere = true;
            }
            // string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
            // string custName = GetCustName(custCode);
            string insertQry = "UPDATE dbo.WhiteBoard "
                                //   + "SET requiredDate = @requiredDate,"
                                + "SET custCode = @custCode,"
                                // + "custCode = @custCode,"
                                + "siteAddress = @siteAddress,"
                                + "supplyType = @supplyType,"
                                + "products = @products,"
                                + "totalM2 = @totalM2,"
                                + "fixingDaysAllowance = @fixingDaysAllowance,"
                                + "salesPrice = @salesPrice,"
                                + "isInvoiced = @isInvoiced,"
                                + "suppShortname = @suppShortname,"
                                + "stairsIncl = @stairsIncl,"
                                + "stairsSupplier = @stairsSupplier,"
                                + "floorlevel = @floorlevel,"
                                + "continuedFlag = @continuedFlag,"
                                + "cowSentFlag = @cowSentFlag,"
                                + "cowReceived = @cowReceived,"
                                //+ "wcMonday = @wcMonday,"
                                //+ "wcTuesday = @wcTuesday,"
                                //+ "wcWednesday = @wcWednesday,"
                                //+ "wcThursday = @wcThursday,"
                                //+ "wcFriday = @wcFriday,"
                                //+ "wcSaturday = @wcSaturday,"
                                //+ "wcSunday = @wcSunday,"
                                + "contracts = @contracts,"
                                + "ramsFlag = @ramsFlag,"
                                + "ramsSignedReturnedFlag = @ramsSignedReturnedFlag,"
                                + "ramsCompleteReturnedFlag = @ramsCompleteReturnedFlag,"
                                + "lorry = @lorry,"
                                + "craneSize = @craneSize,"
                                + "craneSupplier = @craneSupplier,"
                                + "spreaderMatFlag = @spreaderMatFlag,"
                                + "hireContractRcvd = @hireContractRcvd,"
                                + "fallArrest = @fallArrest,"
                                + "fixingGang = @fixingGang,"
                                + "onHireFlag = @onHireFlag,"
                                + "extrasFlag = @extrasFlag,"
                                + "concrete = @concrete,"
                                + "blocks = @blocks,"
                                + "drawingsEmailedFlag = @drawingsEmailedFlag,"
                                + "draughtsman = @draughtsman,"
                                + "salesman = @salesman,"
                                + "lastComment = @lastComment,"
                                + "completedFlag = @completedFlag,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy,"
                                + "sortType = @sortType "
                                + "WHERE jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        //command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("supplyType", supplyType));
                        command.Parameters.Add(new SqlParameter("products", products));
                        command.Parameters.Add(new SqlParameter("totalM2", totalM2));
                        command.Parameters.Add(new SqlParameter("fixingDaysAllowance", fixingDaysAllowance));
                        command.Parameters.Add(new SqlParameter("salesPrice", salesPrice));
                        command.Parameters.Add(new SqlParameter("isInvoiced", isInvoiced));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("stairsIncl", stairsIncl));
                        command.Parameters.Add(new SqlParameter("stairsSupplier", stairsSupplier));
                        command.Parameters.Add(new SqlParameter("floorlevel", floorlevel));
                        command.Parameters.Add(new SqlParameter("continuedFlag", continuedFlag));
                        command.Parameters.Add(new SqlParameter("cowSentFlag", cowSentFlag));
                        command.Parameters.Add(new SqlParameter("cowReceived", cowReceived));
                        //command.Parameters.Add(new SqlParameter("wcMonday", wcMonday));
                        //command.Parameters.Add(new SqlParameter("wcTuesday", wcTuesday));
                        //command.Parameters.Add(new SqlParameter("wcWednesday", wcWednesday));
                        //command.Parameters.Add(new SqlParameter("wcThursday", wcThursday));
                        //command.Parameters.Add(new SqlParameter("wcFriday", wcFriday));
                        //command.Parameters.Add(new SqlParameter("wcSaturday", wcSaturday));
                        //command.Parameters.Add(new SqlParameter("wcSunday", wcSunday));
                        command.Parameters.Add(new SqlParameter("contracts", contracts));
                        command.Parameters.Add(new SqlParameter("ramsFlag", ramsFlag));
                        command.Parameters.Add(new SqlParameter("ramsSignedReturnedFlag", ramsSignedReturnedFlag));
                        command.Parameters.Add(new SqlParameter("ramsCompleteReturnedFlag", ramsCompleteReturnedFlag));
                        command.Parameters.Add(new SqlParameter("lorry", lorry));
                        command.Parameters.Add(new SqlParameter("craneSize", craneSize));
                        command.Parameters.Add(new SqlParameter("craneSupplier", craneSupplier));
                        command.Parameters.Add(new SqlParameter("spreaderMatFlag", spreaderMatFlag));
                        command.Parameters.Add(new SqlParameter("hireContractRcvd", hireContractRcvd));
                        command.Parameters.Add(new SqlParameter("fallArrest", fallArrest));
                        command.Parameters.Add(new SqlParameter("fixingGang", fixingGang));
                        command.Parameters.Add(new SqlParameter("onHireFlag", onHireFlag));
                        command.Parameters.Add(new SqlParameter("extrasFlag", extrasFlag));
                        command.Parameters.Add(new SqlParameter("concrete", concrete));
                        command.Parameters.Add(new SqlParameter("blocks", blocks));
                        command.Parameters.Add(new SqlParameter("drawingsEmailedFlag", drawingsEmailedFlag));
                        command.Parameters.Add(new SqlParameter("draughtsman", draughtsman));
                        command.Parameters.Add(new SqlParameter("salesman", salesman));
                        command.Parameters.Add(new SqlParameter("lastComment", lastComment));
                        command.Parameters.Add(new SqlParameter("completedFlag", "N"));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("sortType", sortType));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteBoard() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateWhiteBoard(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }
        public string UpdateWhiteBoardLine(string jobNo, /*DateTime requiredDate,*/ string custCode, string siteAddress, string supplyType, string products, int totalM2, int fixingDaysAllowance, decimal salesPrice,
                                        string isInvoiced, string suppShortname, string stairsIncl, string stairsSupplier, string floorlevel, string continuedFlag, string cowSentFlag, string cowReceived,
                                        string wcMonday, string wcTuesday, string wcWednesday, string wcThursday, string wcFriday, string wcSaturday, string wcSunday, string contracts, string ramsFlag,
                                        string ramsSignedReturnedFlag, string ramsCompleteReturnedFlag, string lorry, string craneSize, string craneSupplier, string spreaderMatFlag, string hireContractRcvd,
                                        string fallArrest, string fixingGang, string onHireFlag, string extrasFlag, string concrete, string blocks, string drawingsEmailedFlag, string draughtsman, string salesman,
                                        string lastComment, string sortType)
        {

            // string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
            // string custName = GetCustName(custCode);
            string insertQry = "UPDATE dbo.WhiteBoard "
                                //   + "SET requiredDate = @requiredDate,"
                                + "SET custCode = @custCode,"
                                // + "custCode = @custCode,"
                                + "siteAddress = @siteAddress,"
                                + "supplyType = @supplyType,"
                                + "products = @products,"
                                + "totalM2 = @totalM2,"
                                + "fixingDaysAllowance = @fixingDaysAllowance,"
                                + "salesPrice = @salesPrice,"
                                + "isInvoiced = @isInvoiced,"
                                + "suppShortname = @suppShortname,"
                                + "stairsIncl = @stairsIncl,"
                                + "stairsSupplier = @stairsSupplier,"
                                + "floorlevel = @floorlevel,"
                                + "continuedFlag = @continuedFlag,"
                                + "cowSentFlag = @cowSentFlag,"
                                + "cowReceived = @cowReceived,"
                                + "wcMonday = @wcMonday,"
                                + "wcTuesday = @wcTuesday,"
                                + "wcWednesday = @wcWednesday,"
                                + "wcThursday = @wcThursday,"
                                + "wcFriday = @wcFriday,"
                                + "wcSaturday = @wcSaturday,"
                                + "wcSunday = @wcSunday,"
                                + "contracts = @contracts,"
                                + "ramsFlag = @ramsFlag,"
                                + "ramsSignedReturnedFlag = @ramsSignedReturnedFlag,"
                                + "ramsCompleteReturnedFlag = @ramsCompleteReturnedFlag,"
                                + "lorry = @lorry,"
                                + "craneSize = @craneSize,"
                                + "craneSupplier = @craneSupplier,"
                                + "spreaderMatFlag = @spreaderMatFlag,"
                                + "hireContractRcvd = @hireContractRcvd,"
                                + "fallArrest = @fallArrest,"
                                + "fixingGang = @fixingGang,"
                                + "onHireFlag = @onHireFlag,"
                                + "extrasFlag = @extrasFlag,"
                                + "concrete = @concrete,"
                                + "blocks = @blocks,"
                                + "drawingsEmailedFlag = @drawingsEmailedFlag,"
                                + "draughtsman = @draughtsman,"
                                + "salesman = @salesman,"
                                + "lastComment = @lastComment,"
                                + "completedFlag = @completedFlag,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy,"
                                + "sortType = @sortType "
                                + "WHERE jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        //command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("supplyType", supplyType));
                        command.Parameters.Add(new SqlParameter("products", products));
                        command.Parameters.Add(new SqlParameter("totalM2", totalM2));
                        command.Parameters.Add(new SqlParameter("fixingDaysAllowance", fixingDaysAllowance));
                        command.Parameters.Add(new SqlParameter("salesPrice", salesPrice));
                        command.Parameters.Add(new SqlParameter("isInvoiced", isInvoiced));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("stairsIncl", stairsIncl));
                        command.Parameters.Add(new SqlParameter("stairsSupplier", stairsSupplier));
                        command.Parameters.Add(new SqlParameter("floorlevel", floorlevel));
                        command.Parameters.Add(new SqlParameter("continuedFlag", continuedFlag));
                        command.Parameters.Add(new SqlParameter("cowSentFlag", cowSentFlag));
                        command.Parameters.Add(new SqlParameter("cowReceived", cowReceived));
                        command.Parameters.Add(new SqlParameter("wcMonday", wcMonday));
                        command.Parameters.Add(new SqlParameter("wcTuesday", wcTuesday));
                        command.Parameters.Add(new SqlParameter("wcWednesday", wcWednesday));
                        command.Parameters.Add(new SqlParameter("wcThursday", wcThursday));
                        command.Parameters.Add(new SqlParameter("wcFriday", wcFriday));
                        command.Parameters.Add(new SqlParameter("wcSaturday", wcSaturday));
                        command.Parameters.Add(new SqlParameter("wcSunday", wcSunday));
                        command.Parameters.Add(new SqlParameter("contracts", contracts));
                        command.Parameters.Add(new SqlParameter("ramsFlag", ramsFlag));
                        command.Parameters.Add(new SqlParameter("ramsSignedReturnedFlag", ramsSignedReturnedFlag));
                        command.Parameters.Add(new SqlParameter("ramsCompleteReturnedFlag", ramsCompleteReturnedFlag));
                        command.Parameters.Add(new SqlParameter("lorry", lorry));
                        command.Parameters.Add(new SqlParameter("craneSize", craneSize));
                        command.Parameters.Add(new SqlParameter("craneSupplier", craneSupplier));
                        command.Parameters.Add(new SqlParameter("spreaderMatFlag", spreaderMatFlag));
                        command.Parameters.Add(new SqlParameter("hireContractRcvd", hireContractRcvd));
                        command.Parameters.Add(new SqlParameter("fallArrest", fallArrest));
                        command.Parameters.Add(new SqlParameter("fixingGang", fixingGang));
                        command.Parameters.Add(new SqlParameter("onHireFlag", onHireFlag));
                        command.Parameters.Add(new SqlParameter("extrasFlag", extrasFlag));
                        command.Parameters.Add(new SqlParameter("concrete", concrete));
                        command.Parameters.Add(new SqlParameter("blocks", blocks));
                        command.Parameters.Add(new SqlParameter("drawingsEmailedFlag", drawingsEmailedFlag));
                        command.Parameters.Add(new SqlParameter("draughtsman", draughtsman));
                        command.Parameters.Add(new SqlParameter("salesman", salesman));
                        command.Parameters.Add(new SqlParameter("lastComment", lastComment));
                        command.Parameters.Add(new SqlParameter("completedFlag", "N"));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("sortType", sortType));

                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteBoard() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateWhiteBoard(.....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateWhiteBoardJobDate(string jobNo, DateTime requiredDate)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.WhiteBoard "
                                + "SET requiredDate = @requiredDate,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy "
                                + "WHERE jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"UpdateWhiteBoardJobDate(....{requiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteBoardJobDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateWhiteBoardJobDate({0},{1})", jobNo, requiredDate.ToShortDateString()), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateWhiteBoardJobProduct(string jobNo, string revisedProduct)
        {
           string insertQry = "UPDATE dbo.WhiteBoard "
                                + "SET products = @products "
                                + "WHERE jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("products", revisedProduct));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteBoardJobProduct() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateWhiteBoardJobProduct({0},{1})", jobNo, revisedProduct), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateWhiteBoardJobProductWithSupplierProductType(string jobNo, string suppProductType)
        {
            string wbProduct = GetWhiteboardProductFromSupplierProductType(suppProductType);
            string insertQry = "UPDATE dbo.WhiteBoard "
                                 + "SET products = @products "
                                 + "WHERE jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("products", wbProduct));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteBoardJobProduct() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateWhiteBoardJobProduct({0},{1})", jobNo, suppProductType), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateWhiteBoardSupplierShortName(string jobNo, string shortname, int rgb1, int rgb2, int rgb3)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.WhiteBoard "
                                + "SET suppShortname = @suppShortname,"
                                + "rgb1 = @rgb1,"
                                + "rgb2 = @rgb2,"
                                + "rgb3 = @rgb3,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy "
                                + "WHERE LEFT(jobNo,8) = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo.Substring(0, 8)));
                        command.Parameters.Add(new SqlParameter("suppShortname", shortname));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteBoardSupplierShortName() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateWhiteBoardSupplierShortName({0},{1})", jobNo, shortname), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateWhiteBoardJobQty(string jobNo, int totalM2)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string updateQry = "UPDATE dbo.WhiteBoard "
                                + "SET totalM2 = @totalM2,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy "
                                + "WHERE LEFT(jobNo,8) = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(updateQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo.Substring(0, 8)));
                        command.Parameters.Add(new SqlParameter("totalM2", totalM2));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = $"UpdateWhiteBoardJobQty() Error : {ex.Message}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"UpdateWhiteBoardJobQty({jobNo},{totalM2.ToString()})", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public bool UpdateWhiteBoardSupplier(string jobNo, string suppShortname)
        {
            

            string updateQry = "UPDATE dbo.WhiteBoard "
                                + "SET suppShortname = @suppShortname,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy "
                                + "WHERE LEFT(jobNo,8) = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(updateQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo.Substring(0, 8)));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    string msg = $"UpdateWhiteBoardSupplier() Error : {ex.Message}";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"UpdateWhiteBoardSupplier({jobNo},{suppShortname})", ex.Message.ToString());
                    return false;
                }

            }
        }

        public string UpdateWhiteBoardJobDayComment(string jobNo, string dayFieldName, string comment)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.WhiteBoard "
                                + "SET " + dayFieldName + " = @comment,"
                                + "modifiedDate = @modifiedDate,"
                                + "modifiedBy = @modifiedBy "
                                + "WHERE jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        //  command.Parameters.Add(new SqlParameter("dayFieldName", dayFieldName));
                        command.Parameters.Add(new SqlParameter("comment", comment));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteBoardJobDayComment() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateWhiteBoardJobDayComment({0},{1},{2})", jobNo, dayFieldName, comment), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateWhiteboardCustNameByParentJob(string pJobNo, string custCode)
        {
            //   string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.Whiteboard "
                                    + "SET custCode = @custCode, "
                                    + "modifiedDate = @modifiedDate, "
                                    + "modifiedBy = @modifiedBy "
                                    + " WHERE jobNo LIKE '%' + @jobNo + '%'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", pJobNo));
                        command.Parameters.Add(new SqlParameter("custCode", custCode));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteboardCustNameByParentJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateWhiteboardCustNameByParentJob({0},{1})", pJobNo, custCode), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateWhiteboardViaJobPlanner(string jobNo, string floorLevel, DateTime requiredDate, string siteAddress, int slabM2, int beamM2, string supplyType, string suppShortname,
            string stairsIncl, string lastComment, string wcMonday, string wcTuesday, string wcWednesday, string wcThursday, string wcFriday, string wcSaturday, string wcSunday, decimal phaseInvoiceValue, string sortType)
        {
            // string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.WhiteBoard "
                                   // + "SET requiredDate = @requiredDate, "
                                    + "SET supplyType = @supplyType, "
                                    //+ "supplyType = @supplyType, "
                                    + "siteAddress = @siteAddress, "
                                    + "totalM2 = @totalM2, "
                                    + "suppShortname = @suppShortname, "
                                    + "floorLevel = @floorLevel, "
                                    + "stairsIncl = @stairsIncl, "
                                    + "lastComment = @lastComment, "
                                    + "salesPrice = @salesPrice, "
                                    //+ "wcMonday = @wcMonday, "
                                    //+ "wcTuesday = @wcTuesday, "
                                    //+ "wcWednesday = @wcWednesday, "
                                    //+ "wcThursday = @wcThursday, "
                                    //+ "wcFriday = @wcFriday, "
                                    //+ "wcSaturday = @wcSaturday, "
                                    //+ "wcSunday = @wcSunday, "
                                    + "modifiedDate = @modifiedDate, "
                                    + "modifiedBy = @modifiedBy, "
                                    + "sortType = @sortType "
                                    + " WHERE jobNo = @jobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("floorLevel", floorLevel));
                   //     command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("totalM2", slabM2 + beamM2));
                        command.Parameters.Add(new SqlParameter("supplyType", supplyType));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("stairsIncl", stairsIncl));
                        command.Parameters.Add(new SqlParameter("lastComment", lastComment));
                        command.Parameters.Add(new SqlParameter("salesPrice", phaseInvoiceValue));
                        //command.Parameters.Add(new SqlParameter("wcMonday", wcMonday));
                        //command.Parameters.Add(new SqlParameter("wcTuesday", wcTuesday));
                        //command.Parameters.Add(new SqlParameter("wcWednesday", wcWednesday));
                        //command.Parameters.Add(new SqlParameter("wcThursday", wcThursday));
                        //command.Parameters.Add(new SqlParameter("wcFriday", wcFriday));
                        //command.Parameters.Add(new SqlParameter("wcSaturday", wcSaturday));
                        //command.Parameters.Add(new SqlParameter("wcSunday", wcSunday));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.Parameters.Add(new SqlParameter("sortType", sortType));
                        command.ExecuteNonQuery();
                    }
                  //  string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"UpdateWhiteboardViaJobPlanner(....{requiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateWhiteboard() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateWhiteboardViaJobPlanner(...)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateCustCodeInWhiteboard(string parentJobNo, string custCode)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
            try
            {
                string qry = String.Format("UPDATE dbo.WhiteBoard SET custCode = '{0}' WHERE jobNo LIKE '%{1}%'", custCode, parentJobNo);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numUpdated = cmd.ExecuteNonQuery();
                conn.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                string msg = String.Format("UpdateCustNameInWhiteboard() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("UpdateCustCodeInWhiteboard({0}{1})", parentJobNo, custCode), ex.Message.ToString());
                return msg;
            }
        }

        public string CompleteWhiteboardJob(string jobNo, string completedFlag)
        {
            //   string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];


            //string insertQry = "UPDATE dbo.WhiteBoard "
            //                        + "SET completedFlag = @completedFlag, isInvoiced = @isInvoiced, modifiedDate = @modifiedDate, modifiedBy = @modifiedBy"
            //                        + " WHERE jobNo = @jobNo";
            string phaseJobWithoutSuffix = jobNo.Trim().Substring(0, 8);
            string insertQry = "UPDATE dbo.WhiteBoard "
                                    + "SET completedFlag = @completedFlag, isInvoiced = @isInvoiced, modifiedDate = @modifiedDate, modifiedBy = @modifiedBy"
                                    + " WHERE LEFT(jobNo,8) = @jobNo";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        //command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("jobNo", phaseJobWithoutSuffix));
                        command.Parameters.Add(new SqlParameter("isInvoiced", completedFlag));
                        command.Parameters.Add(new SqlParameter("completedFlag", completedFlag));
                        command.Parameters.Add(new SqlParameter("modifiedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("modifiedBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CompleteWhiteboardJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("CompleteWhiteboardJob({0}{1})", jobNo, completedFlag), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string ClearWhiteboardJobDayComments(string jobNo)
        {
            //   string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];


            string insertQry = "UPDATE dbo.WhiteBoard "
                                    + "SET wcMonday = @wcMonday, wcTuesday = @wcTuesday, wcWednesday = @wcWednesday, wcThursday = @wcThursday, wcFriday = @wcFriday,wcSaturday = @wcSaturday, wcSunday = @wcSunday"
                                    + " WHERE jobNo = @jobNo";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("wcMonday", String.Empty));
                        command.Parameters.Add(new SqlParameter("wcTuesday", String.Empty));
                        command.Parameters.Add(new SqlParameter("wcWednesday", String.Empty));
                        command.Parameters.Add(new SqlParameter("wcThursday", String.Empty));
                        command.Parameters.Add(new SqlParameter("wcFriday", String.Empty));
                        command.Parameters.Add(new SqlParameter("wcSaturday", String.Empty));
                        command.Parameters.Add(new SqlParameter("wcSunday", String.Empty));

                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("ClearWhiteboardJobDayComments() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("ClearWhiteboardJobDayComments({0})", jobNo), ex.Message.ToString());
                    return msg;
                }

            }
        }

        public DataTable WhiteboardDatesDT()
        {
            DataTable dt = new DataTable();
            try
            {
                DateTime firstDate = GetFirstPlannerDate();
                DateTime startDate = GetMonday(firstDate);
                DateTime lastDate = GetLastPlannerDate();

                TimeSpan ts = lastDate - startDate;
                int dateDiff = ts.Days;
                decimal numWeeks = dateDiff / 7m;
                decimal roundedNumWeeks = Decimal.Round(numWeeks, 0);

                DataTable datesDT = new DataTable();


                datesDT.Columns.Add("jobDate", typeof(DateTime));
                datesDT.Columns.Add("wcDate", typeof(DateTime));
                datesDT.Columns.Add("tabNo", typeof(int));

                DateTime currWcDate = DateTime.MinValue;
                int wcCounter = 0;

                for (DateTime i = startDate; i <= lastDate; i = i.AddDays(1))
                {
                    currWcDate = GetMonday(i.Date);

                    if (i.Date == currWcDate)
                    {
                        wcCounter++;
                        datesDT.Rows.Add(i.Date, currWcDate, wcCounter);
                    }
                    else
                    {
                        datesDT.Rows.Add(i.Date, currWcDate, wcCounter);
                    }

                }
                return datesDT;
            }
            catch (Exception ex)
            {
                string msg = String.Format("WhiteboardDatesDT() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", "WhiteboardDatesDT()", ex.Message.ToString());
                return null;
            }

        }

        public DataTable WhiteboardDatesDT(DateTime d1, DateTime d2)
        {
            DataTable dt = new DataTable();
            try
            {
                DateTime firstDate = d1;
                DateTime startDate = GetMonday(d1);
                DateTime lastDate = d2;

                TimeSpan ts = lastDate - startDate;
                int dateDiff = ts.Days;
                decimal numWeeks = dateDiff / 7m;
                decimal roundedNumWeeks = Decimal.Round(numWeeks, 0);

                DataTable datesDT = new DataTable();


                datesDT.Columns.Add("jobDate", typeof(DateTime));
                datesDT.Columns.Add("wcDate", typeof(DateTime));
                datesDT.Columns.Add("tabNo", typeof(int));

                DateTime currWcDate = DateTime.MinValue;
                int wcCounter = 0;

                for (DateTime i = startDate; i <= lastDate; i = i.AddDays(1))
                {
                    currWcDate = GetMonday(i.Date);

                    if (i.Date == currWcDate)
                    {
                        wcCounter++;
                        //if(IsValidWhiteboardJobsOnDate(currWcDate))
                        //{
                        datesDT.Rows.Add(i.Date, currWcDate, wcCounter);
                        //}
                    }
                    else
                    {
                        //if (IsValidWhiteboardJobsOnDate(currWcDate))
                        //{
                        datesDT.Rows.Add(i.Date, currWcDate, wcCounter);
                        //}
                    }

                }
                return datesDT;
            }
            catch (Exception ex)
            {
                string msg = String.Format("WhiteboardDatesDT() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("WhiteboardDatesDT({0},{1})", d1.ToShortDateString(), d2.ToShortDateString()), ex.Message.ToString());
                return null;
            }

        }

        public DataTable GetWBJobIssuesDT(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = $"SELECT * FROM dbo.JobIssue WHERE jobNo = '{jobNo}' ORDER BY jobIssueDate DESC";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = $"GetWBJobIssuesDT() Error : ex.Message.ToString()";
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"GetWBJobIssuesDT({jobNo})", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable WhiteboardDatesDT(string jobNo)
        {
            DataTable dt = new DataTable();
            try
            {
                DateTime jobDate = GetPlannerDateByJobNo(jobNo);
                DateTime firstDate = jobDate;
                DateTime startDate = jobDate;
                DateTime lastDate = jobDate;

                TimeSpan ts = lastDate - startDate;
                int dateDiff = ts.Days;
                decimal numWeeks = dateDiff / 7m;
                decimal roundedNumWeeks = Decimal.Round(numWeeks, 0);

                DataTable datesDT = new DataTable();


                datesDT.Columns.Add("jobDate", typeof(DateTime));
                datesDT.Columns.Add("wcDate", typeof(DateTime));
                datesDT.Columns.Add("tabNo", typeof(int));

                DateTime currWcDate = DateTime.MinValue;
                int wcCounter = 0;

                for (DateTime i = startDate; i <= lastDate; i = i.AddDays(1))
                {
                    currWcDate = GetMonday(i.Date);

                    if (i.Date == currWcDate)
                    {
                        wcCounter++;
                        datesDT.Rows.Add(i.Date, currWcDate, wcCounter);
                    }
                    else
                    {
                        datesDT.Rows.Add(i.Date, currWcDate, wcCounter);
                    }

                }
                return datesDT;
            }
            catch (Exception ex)
            {
                string msg = String.Format("WhiteboardDatesDT() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("WhiteboardDatesDT({0})", jobNo), ex.Message.ToString());
                return null;
            }

        }

        

        public void GetWhiteboardDays(string jobNo, out string mon, out string tue, out string wed, out string thu, out string fri, out string sat, out string sun)
        {
            string error;


            mon = "";
            tue = "";
            wed = "";
            thu = "";
            fri = "";
            sat = "";
            sun = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.Whiteboard WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mon = reader["wcMonday"].ToString();
                                tue = reader["wcTuesday"].ToString();
                                wed = reader["wcWednesday"].ToString();
                                thu = reader["wcThursday"].ToString();
                                fri = reader["wcFriday"].ToString();
                                sat = reader["wcSaturday"].ToString();
                                sun = reader["wcSunday"].ToString();
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetWhiteboardDays({0})", jobNo), ex.Message.ToString());
                    return;
                }

            }
        }

        public bool IsWhiteboardJobExists(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Whiteboard WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsWhiteboardJobExists() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsWhiteboardJobExists({0})", jobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsDesignBoardJobExists(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.Designboard WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsDesignBoardJobExists() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"IsDesignBoardJobExists({jobNo})", ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsValidWhiteboardJob(string jobNo)
        {
            char lastCharacter = jobNo[jobNo.Length - 1];
            string jobNumber = jobNo;
            if (Char.IsLetter(lastCharacter))
            {
                jobNumber = jobNo.Substring(0, 8);
            }
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.JobPlanner WHERE jobNo = '{0}' AND Approved = 'Y' AND OnShop = 'Y'", jobNumber), conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsValidWhiteboardJob() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsValidWhiteboardJob({0})", jobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        public bool IsValidWhiteboardJobsOnDate(DateTime jobDate)
        {

            string dd = jobDate.Day.ToString().Length < 2 ? "0" + jobDate.Day.ToString() : jobDate.Day.ToString();
            string mm = jobDate.Month.ToString().Length < 2 ? "0" + jobDate.Month.ToString() : jobDate.Month.ToString();
            string yyyy = jobDate.Year.ToString();
            string startDateStr = yyyy + "-" + mm + "-" + dd;

            DateTime endDate = jobDate.AddDays(6);

            string dd2 = endDate.Day.ToString().Length < 2 ? "0" + endDate.Day.ToString() : endDate.Day.ToString();
            string mm2 = endDate.Month.ToString().Length < 2 ? "0" + endDate.Month.ToString() : endDate.Month.ToString();
            string yyyy2 = endDate.Year.ToString();
            string endDateStr = yyyy2 + "-" + mm2 + "-" + dd2 + " 23:59:59";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    string qry = String.Format("SELECT count(*) FROM dbo.Whiteboard WHERE requiredDate between '{0}' and '{1}'", startDateStr, endDateStr);
                    using (SqlCommand command = new SqlCommand(qry, conn))
                    {
                        Int32 numjobs = (Int32)command.ExecuteScalar();

                        if (numjobs > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsValidWhiteboardJobsOnDate() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsValidWhiteboardJobsOnDate({0})", jobDate.ToShortDateString()), ex.Message.ToString());
                    return false;
                }

            }
        }

        public DataTable GetFixersDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.WhiteboardFixingGangs ORDER BY wbFixingCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetFixersDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetFixersDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public void GetFixerRGB(string wbFixingGang, out int rgb1, out int rgb2, out int rgb3)
        {
            string error;


            rgb1 = 0;
            rgb2 = 0;
            rgb3 = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT * FROM dbo.WhiteboardFixingGangs WHERE wbFixingCode = '{0}'", wbFixingGang), conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rgb1 = Convert.ToInt16(reader["wbRGB_1"].ToString());
                                rgb2 = Convert.ToInt16(reader["wbRGB_2"].ToString());
                                rgb3 = Convert.ToInt16(reader["wbRGB_3"].ToString());
                            }
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    error = ex.Message.ToString();
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetFixerRGB({0})", wbFixingGang), ex.Message.ToString());
                    return;
                }

            }
        }



        public int DeleteWhiteboardByParentJob(int parentJobNo)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.Whiteboard WHERE jobNo LIKE '%{0}%'", parentJobNo.ToString());

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteWhiteboardByParentJob() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteWhiteboardByParentJob({0})", parentJobNo), ex.Message.ToString());
                return 0;
            }




        }

        public int DeleteWhiteboardByJobNo(string jobNo)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.Whiteboard WHERE jobNo = '{0}'", jobNo);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteWhiteboardByJobNo() Error : {0} and Inner Exception = {1}", ex.Message.ToString(), ex.InnerException.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteWhiteboardByJobNo({0})", jobNo), ex.Message.ToString());
                return 0;
            }




        }

        public int EmptyTheWhiteboard()
        {

            string qry = "TRUNCATE TABLE dbo.Whiteboard";

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = qry;
            cmd.Connection = conn;
            conn.Open();
            int numDeleted = cmd.ExecuteNonQuery();
            conn.Close();
            return numDeleted;


        }

        public string CreateJobPO(string jobNo, DateTime requiredDate, string siteAddress, string products, int totalM2, string suppShortname, string stairsSupplier)
        {
            //  string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "INSERT INTO dbo.JobPurchaseOrder("
                                            + "jobNo, requiredDate, siteAddress,products, totalM2,suppShortname,stairsSupplier, ramSentDate, ramSentBy) "
                                            + "VALUES("
                                            + "@jobNo,"
                                            + "@requiredDate,"
                                            + "@siteAddress,"
                                            + "@products,"
                                            + "@totalM2,"
                                            + "@suppShortname,"
                                            + "@stairsSupplier,"
                                            + "@ramSentDate,"
                                            + "@ramSentBy)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("products", products));
                        command.Parameters.Add(new SqlParameter("totalM2", totalM2));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("stairsSupplier", stairsSupplier));
                        command.Parameters.Add(new SqlParameter("ramSentDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("ramSentBy", loggedInUser));
                        command.ExecuteNonQuery();
                    }
                    //string err2 = CreateJobDayAudit(jobNo, requiredDate.Date, $"CreateJobPO(....{requiredDate.ToShortDateString()}......)");
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateWhiteBoard() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "CreateJobPO(....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public string UpdateJobPO(string jobNo, DateTime requiredDate, string siteAddress, string products, int totalM2, string suppShortname, string stairsSupplier, DateTime ramSentDate,
                                    string ramSentBy, string poNo, string poDetails)
        {
            // string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

            string insertQry = "UPDATE dbo.JobPurchaseOrder "
                                + "SET requiredDate = @requiredDate,"
                                + "siteAddress = @siteAddress,"
                                + "products = @products,"
                                + "totalM2 = @totalM2,"
                                + "suppShortname = @suppShortname,"
                                + "stairsSupplier = @stairsSupplier,"
                                + "ramSentDate = @ramSentDate,"
                                + "ramSentBy = @ramSentBy,"
                                + "poNo = @poNo,"
                                + "poDetails = @poDetails,"
                                + "poRaisedDate = @poRaisedDate,"
                                + "poRaisedBy = @poRaisedBy "
                                + "WHERE jobNo = @jobNo";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("jobNo", jobNo));
                        command.Parameters.Add(new SqlParameter("requiredDate", requiredDate));
                        command.Parameters.Add(new SqlParameter("siteAddress", siteAddress));
                        command.Parameters.Add(new SqlParameter("products", products));
                        command.Parameters.Add(new SqlParameter("totalM2", totalM2));
                        command.Parameters.Add(new SqlParameter("suppShortname", suppShortname));
                        command.Parameters.Add(new SqlParameter("stairsSupplier", stairsSupplier));
                        command.Parameters.Add(new SqlParameter("ramSentDate", ramSentDate));
                        command.Parameters.Add(new SqlParameter("ramSentBy", ramSentBy));
                        command.Parameters.Add(new SqlParameter("poNo", poNo));
                        command.Parameters.Add(new SqlParameter("poDetails", poDetails));
                        command.Parameters.Add(new SqlParameter("poRaisedDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("poRaisedBy", loggedInUser));

                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("UpdateJobPO() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "UpdateJobPO(....)", ex.Message.ToString());
                    return msg;
                }

            }
        }

        public int DeleteJobPO(string jobNo)
        {
            try
            {
                string qry = String.Format("DELETE FROM dbo.JobPurchaseOrder WHERE jobNo = '{0}'", jobNo);

                SqlConnection conn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = qry;
                cmd.Connection = conn;
                conn.Open();
                int numDeleted = cmd.ExecuteNonQuery();
                conn.Close();
                return numDeleted;
            }
            catch (Exception ex)
            {
                string msg = String.Format("DeleteJobPO() Error : {0} and Inner Exception = {1}", ex.Message.ToString(), ex.InnerException.ToString());
                logger.LogLine(msg);
                string audit = CreateErrorAudit("MeltonData.cs", String.Format("DeleteJobPO({0})", jobNo), ex.Message.ToString());
                return 0;
            }




        }

        public bool IsJobPoExists(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(String.Format("SELECT count(*) FROM dbo.JobPurchaseOrder WHERE jobNo = '{0}'", jobNo), conn))
                    {
                        Int32 numSuppliersFound = (Int32)command.ExecuteScalar();

                        if (numSuppliersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsJobPoExists() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("IsJobPoExists({0})", jobNo), ex.Message.ToString());
                    return false;
                }

            }
        }

        #endregion

        #region OTHERS

        public bool IsFeatureToggleEnabled(string feature)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT count(*) FROM dbo.FeatureToggle WHERE feature = '{feature}' AND isEnabled = 'Y'", conn))
                    {
                        Int32 numSuppliersFound = (Int32)command.ExecuteScalar();

                        if (numSuppliersFound > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("IsFeatureToggleEnabled() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", $"IsFeatureToggleEnabled({feature})", ex.Message.ToString());
                    return false;
                }

            }
        }

        public DataTable GetErrorAuditDT()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.ErrorAudit WHERE errDate >= DATEADD(day,-30,GETDATE()) and errdate <= getdate() ORDER BY errDate DESC";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetErrorAuditDT() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetErrorAuditDT()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllColours()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Colours ORDER BY colourCode";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllColours() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllColours()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllProducts()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.Products ORDER BY seq";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllProducts() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllProducts()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetAllJobPurchaseOrders()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = "SELECT * FROM dbo.JobPurchaseOrder ORDER BY requiredDate";

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetAllJobPurchaseOrders() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", "GetAllJobPurchaseOrders()", ex.Message.ToString());
                    return null;
                }

            }

        }

        public DataTable GetJobPurchaseOrder(string jobNo)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    string qry = String.Format("SELECT * FROM dbo.JobPurchaseOrder WHERE jobNo = '{0}'", jobNo);

                    SqlCommand cmd = new SqlCommand(qry, conn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format("GetJobPurchaseOrder() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    string audit = CreateErrorAudit("MeltonData.cs", String.Format("GetJobPurchaseOrder({0})", jobNo), ex.Message.ToString());
                    return null;
                }

            }

        }

        public string CreateErrorAudit(string errClassName, string errMethod, string errMessage)
        {

            string insertQry = "INSERT INTO dbo.ErrorAudit("
                                            + "errClassName,errMethod,errMessage,errDate,errUser) "
                                            + "VALUES("
                                            + "@errClassName,"
                                            + "@errMethod,"
                                            + "@errMessage,"
                                            + "@errDate,"
                                            + "@errUser)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQry, conn))
                    {
                        command.Parameters.Add(new SqlParameter("errClassName", errClassName));
                        command.Parameters.Add(new SqlParameter("errMethod", errMethod));
                        command.Parameters.Add(new SqlParameter("errMessage", errMessage));
                        command.Parameters.Add(new SqlParameter("errDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("errUser", ConfigurationManager.AppSettings["LoggedInUser"]));
                        command.ExecuteNonQuery();
                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    string msg = String.Format("CreateErrorAudit() Error : {0}", ex.Message.ToString());
                    logger.LogLine(msg);
                    return msg;
                }

            }
        }

        public DateTime GetCorrectDate(DateTime date)
        {
            string dd = date.ToString("dd");
            string mm = date.ToString("MM");
            string yyyy = date.ToString("yyyy");


            DateTime newDate = new DateTime(Convert.ToInt16(yyyy), Convert.ToInt16(mm), Convert.ToInt16(dd));


            return newDate;

        }

        public string GetRevisedProductFromQtyAndSupplier(string product,string supplier, int beamLm, int beamM2, int slabM2)
        {
            string revisedProduct = "";
            if(IsStairsSupplier(supplier)) { return "ST Only"; }
            if(beamLm > 0 || beamM2 > 0) { return "BB Only"; }
            if(beamLm < 1 && beamM2 < 1 && slabM2 > 0) { return "SL Only"; }
            if(product == "SLAB") { return "SL Only"; }

            return revisedProduct;

        }

        public string GetWhiteboardProductFromSupplierProductType(string productType)
        {
            /*
             
            BB Only
            SL Only
            SLAB
            ST Only
            */
            string wbProduct = "";

            switch (productType)
            {
                case "BEAM":
                    wbProduct = "BB Only";
                    break;
                case "SLAB":
                    wbProduct = "SL Only";
                    break;
                case "SLAB&STAIRS":
                    wbProduct = "SL Only";
                    break;
                case "STAIRS":
                    wbProduct = "ST Only";
                    break;
                default:
                    break;
            }
            return wbProduct;
        }
         
        #endregion




        #region non DB Methods

        public string GetLoggedInUser()
        {
            return ConfigurationManager.AppSettings["LoggedInUser"];
        }

        public string GetServerName()
        {
            var csb = new SqlConnectionStringBuilder(connStr);
            return csb.DataSource.ToString();
        }

        public string GetDatabaseName()
        {
            var csb = new SqlConnectionStringBuilder(connStr);
            return csb.InitialCatalog.ToString();
        }

        public bool AreTablesTheSameOld(DataTable tbl1, DataTable tbl2)
        {
            if (tbl1.Rows.Count != tbl2.Rows.Count || tbl1.Columns.Count != tbl2.Columns.Count)
                return false;

            for (int i = 0; i < tbl1.Rows.Count; i++)
            {
                for (int c = 0; c < tbl1.Columns.Count; c++)
                {
                    if (!Equals(tbl1.Rows[i][c], tbl2.Rows[i][c]))
                        return false;
                }
            }
            return true;
        }

        public bool AreTablesTheSame(DataTable tbl1, DataTable tbl2)
        {

            int rowsCount1 = tbl1.Rows.Count;
            int rowsCount2 = tbl2.Rows.Count;
            int colcount1 = tbl1.Columns.Count;
            int colcount2 = tbl2.Columns.Count;


            if (rowsCount1 != rowsCount2 || colcount1 != colcount2)
                return false;

            string cell1, cell2 = "";
            for (int i = 0; i < tbl1.Rows.Count; i++)
            {
                for (int c = 0; c < tbl1.Columns.Count; c++)
                {
                    cell1 = tbl1.Rows[i][c].ToString();
                    cell2 = tbl2.Rows[i][c].ToString();
                    if (!Equals(cell1, cell2))
                        return false;
                }
            }
            return true;
        }

        public string GetNextPhaseNumber(int parentJobNo, string maxPhaseNumString)
        {
            int num = Convert.ToInt16(maxPhaseNumString);
            int nextNum = num += 1;
            string nextPhase = nextNum < 10 ? "0" + nextNum.ToString() : nextNum.ToString();
            return parentJobNo.ToString() + "." + nextPhase;
        }

        public bool AreDigitsOnly(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            foreach (char character in text)
            {
                if (character < '0' || character > '9')
                    return false;
            }

            return true;
        }

        public bool IsValidPhaseNo(string entry)
        {
            if (entry.Length != 2) { return false; }

            if (!AreDigitsOnly(entry)) { return false; }

            return true;




        }

        public DateTime GetMonday(DateTime date)
        {

            int delta = DayOfWeek.Monday - date.DayOfWeek;
            if (delta > 0)
                delta -= 7;
            return date.AddDays(delta);
        }

        public string ColourFromSupplyType(string suppType)
        {
            string backColour = "White";

            switch (suppType)
            {
                case "SO":
                    backColour = "LightBlue";
                    break;
                case "SF":
                    backColour = "LightGreen";
                    break;
                default:
                    break;
            }
            return backColour;

        }

        public bool IsCurrentWeek(DateTime checkDate)
        {
            DateTime startDateOfWeek = DateTime.Now.Date; // start with actual date
            while (startDateOfWeek.DayOfWeek != DayOfWeek.Monday) // set first day of week in your country
            { startDateOfWeek = startDateOfWeek.AddDays(-1d); } // after this while loop you get first day of actual week
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6d); // you just find last week day
            return checkDate >= startDateOfWeek && checkDate <= endDateOfWeek;
        }

        public int GetDaysDiffBetweenTwDates(DateTime createdDate)
        {
            TimeSpan diffResult = DateTime.Now.Date - createdDate;
            return diffResult.Days;
        }

        public string GetConnectionString()
        {
            return connStr;
        }

        public bool IsContainLetter(string text)
        {
            bool isLetter = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsLetter(Convert.ToChar(text.Substring(i, 1))))
                {
                    isLetter = true;
                    break;
                }
            }
            return isLetter;
        }

        #endregion


    }
}
