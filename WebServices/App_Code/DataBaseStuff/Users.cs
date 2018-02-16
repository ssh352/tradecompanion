using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FirebirdSql.Data.FirebirdClient;
using WebServices.Scalper.Util;
using System.Data;
using System.Security.Cryptography;
using WebServices.Scalper.Session;


namespace WebServices.Scalper.DatabaseStuff
{



    /// <summary>
    /// Summary description for Users
    /// </summary>
    internal class Users
    {

        // Define default min and max password lengths.
        private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "0123456789";


        public Users()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public static string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                        DEFAULT_MAX_PASSWORD_LENGTH);
        }

        public static string Generate(int length)
        {
            return Generate(length, length);
        }

        public static string Generate(int minLength,
                                 int maxLength)
        {
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            char[][] charGroups = new char[][] 
            {
                PASSWORD_CHARS_LCASE.ToCharArray(),
                PASSWORD_CHARS_UCASE.ToCharArray(),
                PASSWORD_CHARS_NUMERIC.ToCharArray(),
            };

            int[] charsLeftInGroup = new int[charGroups.Length];

            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            int[] leftGroupsOrder = new int[charGroups.Length];

            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;


            byte[] randomBytes = new byte[4];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            Random random = new Random(seed);

            char[] password = null;

            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            int nextCharIdx;

            int nextGroupIdx;

            int nextLeftGroupsOrderIdx;

            int lastCharIdx;

            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }

                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    lastLeftGroupsOrderIdx--;
                }
            }


            return new string(password);
        }

        public bool IsEmailIDExist(string emailId)
        {
            string sql = "SELECT COUNT(*) FROM USERS WHERE emailid = @EmailId;";
            FbCommand objCmd = new FbCommand(sql);
            objCmd = new FbCommand(sql);
            Logclass.WriteDebuglog("Checking for the EmailID Existence.....");

            //Create the parameters
            FbParameter paramEmailId;
            paramEmailId = new FbParameter("@EmailId", FbDbType.VarChar, 50);
            paramEmailId.Value = emailId.Trim();
            objCmd.Parameters.Add(paramEmailId);

            Connection dstuff = new Connection();
            dstuff.CreateConnection();
            dstuff.dbcmd = objCmd;
            string count = dstuff.ExecuteSQLScalar();
            if (count == "1")
            {
                //EmailId already exists
                return true;
            }
            dstuff.DestroyConnection();
            Logclass.WriteDebuglog("    EmailID Not Exists:  " + emailId);
            Logclass.WriteDebuglog("    Checking Finished ");
            return false;
        }

        public bool IsLoginIDExist(string loginId)
        {
            string sql = "SELECT COUNT(*) FROM USERS WHERE LoginID = @LoginID;";
            FbCommand objCmd = new FbCommand(sql);

            Logclass.WriteDebuglog("------Checking for existence of loginID---");
            //Create the parameters
            FbParameter paramLoginID;
            paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
            paramLoginID.Value = loginId;
            objCmd.Parameters.Add(paramLoginID);

            Connection dstuff = new Connection();
            dstuff.CreateConnection();
            dstuff.dbcmd = objCmd;
            string count = dstuff.ExecuteSQLScalar();

            if (count == "1")
            {
                //loginId already exists
                return true;
            }
            Logclass.WriteDebuglog("loginid not exists");
            return false;
        }

        public int AddUser(string loginId, string userName, string emailId, string phoneNo, string address,string city,string country)
        {
            string emailText;
            emailText="PERSONAL DETAILS" + Environment.NewLine + Environment.NewLine;
            emailText = emailText + "Loginid: " + loginId + Environment.NewLine;
            emailText = emailText + "Name: " + userName + Environment.NewLine;
            emailText = emailText + "Address: " + address + Environment.NewLine;
            emailText = emailText + "Town/City: " + city + Environment.NewLine;
            emailText = emailText + "Country: " + country + Environment.NewLine +Environment.NewLine;
            emailText = emailText + "CONTACT INFO" + Environment.NewLine + Environment.NewLine;
            emailText = emailText + "Telephone: " + phoneNo + Environment.NewLine;
            emailText = emailText + "Email Address: " + emailId + Environment.NewLine + Environment.NewLine;

            
            //Utils.WriteDebugLog(emailText);
            Logclass.WriteDebuglog("emailtext" + emailText);
            string pwd = Generate(8, 10);
            string sql = "SELECT COUNT(*) FROM USERS WHERE LoginID = @LoginID;";
            FbCommand objCmd = new FbCommand(sql);

            //Create the parameters
            FbParameter paramLoginID;
            paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
            paramLoginID.Value = loginId;
            objCmd.Parameters.Add(paramLoginID);

            Connection dstuff = new Connection();
            if (dstuff.CreateConnection())
            {
                dstuff.dbcmd = objCmd;
                string count = dstuff.ExecuteSQLScalar();

                if (count == "1") return -10;
             
                sql = "SELECT COUNT(*) FROM USERS WHERE emailid = @EmailId;";
                objCmd = new FbCommand(sql);

                //Create the parameters
                FbParameter paramEmailId;
                paramEmailId = new FbParameter("@EmailId", FbDbType.VarChar, 50);
                paramEmailId.Value = emailId;
                objCmd.Parameters.Add(paramEmailId);

                dstuff.dbcmd = objCmd;
                count = dstuff.ExecuteSQLScalar();
                if (count == "1") return -11;

              
                objCmd = new FbCommand(sql);


                sql = "insert into users (id,loginid, username,passwords, emailid,phoneno,actives,loggedin,address,RegistrationDate,Trial,Lastupdated) values (gen_id(gen_users_id, 1), @LoginId,@Username,@Password,@EmailId,@Phoneno,@Actives,@Loggedin,@Address,@RegistrationDate,@Trial,@Lastupdated);";
               
                objCmd = new FbCommand(sql);

                FbParameter paramLogID;
                paramLogID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
                paramLogID.Value = loginId;
                objCmd.Parameters.Add(paramLogID);

                //objCmd.Parameters.Add(paramLoginID);

                FbParameter paramUsername;
                paramUsername = new FbParameter("@Username", FbDbType.VarChar, 20);
                paramUsername.Value = userName;
                objCmd.Parameters.Add(paramUsername);

                //Encrypt the password
                byte[] hashedBytes = Utils.Encrypt(pwd);
                FbParameter paramPwd;
                paramPwd = new FbParameter("@Password", FbDbType.Char, 16);
                paramPwd.Value = hashedBytes;
                objCmd.Parameters.Add(paramPwd);



                FbParameter paramEmaId;
                paramEmaId = new FbParameter("@EmailId", FbDbType.VarChar, 50);
                paramEmaId.Value = emailId;
                objCmd.Parameters.Add(paramEmaId);
                //objCmd.Parameters.Add(paramEmailId);

                FbParameter paramPhoneno;
                paramPhoneno = new FbParameter("@Phoneno", FbDbType.VarChar, 20);
                paramPhoneno.Value = phoneNo;
                objCmd.Parameters.Add(paramPhoneno);

                FbParameter paramActives;
                paramActives = new FbParameter("@Actives", FbDbType.Char, 1);
                paramActives.Value = 'T';
                objCmd.Parameters.Add(paramActives);

                FbParameter paramLoggedIn;
                paramLoggedIn = new FbParameter("@Loggedin", FbDbType.Char, 1);
                paramLoggedIn.Value = 'N';
                objCmd.Parameters.Add(paramLoggedIn);

                FbParameter paramAddress;
                paramAddress = new FbParameter("@Address", FbDbType.VarChar, 50);
                paramAddress.Value = address + " " + city + " " + country;
                objCmd.Parameters.Add(paramAddress);

                FbParameter paramRegistrationDate;
                paramRegistrationDate = new FbParameter("@RegistrationDate", FbDbType.TimeStamp);
                paramRegistrationDate.Value = DateTime.Now;
                objCmd.Parameters.Add(paramRegistrationDate);

                FbParameter paramTrial;
                paramTrial = new FbParameter("@Trial", FbDbType.Char, 1);
                paramTrial.Value = 'Y';
                objCmd.Parameters.Add(paramTrial);


                FbParameter paramLastupdated;
                paramLastupdated = new FbParameter("@Lastupdated", FbDbType.TimeStamp);
                paramLastupdated.Value = DateTime.Now;
                objCmd.Parameters.Add(paramLastupdated);

   
                dstuff.dbcmd = objCmd;
                int result = dstuff.ExecuteSQLCommandNQ();

                if (result == 1)
                {
                        string subject = "AutoShark User Info Request";

                        String Body = "Hi ," + Environment.NewLine +
                        "A request was made to send you your  password for the" + Environment.NewLine +
                        "AutoShark Login. Your details are as follows:" + Environment.NewLine + Environment.NewLine +
                        "Loginid    : " + loginId + Environment.NewLine +
                        "Password    : " + pwd + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                        "Regards, " + Environment.NewLine + "-AutoShark";
                        bool mailed = Utils.SendEmail(emailId, "info@autoshark.co.uk", subject, Body,"");
                        
                        if (mailed)
                        {
                            //send the email to franco about signing up
                            //send the new password by email
                            subject = "AutoShark NewUser Signup";
                            Body = emailText;
                            mailed = Utils.SendEmail("franco@autoshark.co.uk", "info@autoshark.co.uk", subject, Body, "rahul.sax@gmail.com");
                            //Logclass.WriteDebuglog("(AddUser)Email Sent to emailid" + emailId);
                            //Logclass.WriteDebuglog("used SQL" + sql );
                        }

                        if (!mailed)
                        {

                            sql = "SELECT max(ID) from users;";
                            objCmd = new FbCommand(sql);
                            dstuff.dbcmd = objCmd;
                            result = int.Parse(dstuff.ExecuteSQLScalar());
                            Logclass.WriteDebuglog("Fail to send mail to emailid" + emailId);
                            
                            sql = "delete from users where ID =" + result;
                            objCmd = new FbCommand(sql);
                            dstuff.dbcmd = objCmd;
                            dstuff.ExecuteSQLCommandNQ();
                            dstuff.DestroyConnection();
                            return -4;
                        }
                }
                else
                {
                    dstuff.DestroyConnection();
                    return -5;
                    //database error
                }
                dstuff.DestroyConnection();
                //returns the crated id
                //Utils.WriteDebugLog(DateTime.Today.ToShortTimeString() + " Return Result (Insert User) For " + loginId);
                Logclass.WriteDebuglog(" Return Result");
                return result;
            }
            else
            {
                //connection problem
                //Utils.WriteDebugLog(DateTime.Today.ToShortTimeString() + " Connection Error (Insert User) For " + loginId);
                Logclass.WriteDebuglog("(AddUser)Connection error");
                return -2;
            }
        }

        public int InsertUser(UserRow userRow)
        {
            //string strSQL = "insert into users (id,loginid, username,passwords, emailid,phoneno,actives,loggedin) values (gen_id(gen_users_id, 1), @LoginId,@Username,@Password,@EmailId,@Phoneno,@Actives,@Loggedin);";
            string strSQL = "insert into users (id,loginid, username,passwords, emailid,phoneno,actives,loggedin,RegistrationDate,Trial,Lastupdated) values (gen_id(gen_users_id, 1), @LoginId,@Username,@Password,@EmailId,@Phoneno,@Actives,@Loggedin,@RegistrationDate,@Trial,@Lastupdated);";
              
            FbCommand objCmd = new FbCommand(strSQL);
            Logclass.WriteDebuglog(" ---------inserting new user--------");
            //Create parameters
            FbParameter paramLoginId;
            paramLoginId = new FbParameter("@LoginId", FbDbType.VarChar, 50);
            paramLoginId.Value = userRow.LoginId;
            objCmd.Parameters.Add(paramLoginId);

            FbParameter paramUsername;
            paramUsername = new FbParameter("@Username", FbDbType.VarChar, 20);
            paramUsername.Value = userRow.Username;
            objCmd.Parameters.Add(paramUsername);

            //Encrypt the password
            byte[] hashedBytes = Utils.Encrypt(userRow.Password);
            FbParameter paramPwd;
            paramPwd = new FbParameter("@Password", FbDbType.Char, 16);
            paramPwd.Value = hashedBytes;
            objCmd.Parameters.Add(paramPwd);


            FbParameter paramEmailId;
            paramEmailId = new FbParameter("@EmailId", FbDbType.VarChar, 50);
            paramEmailId.Value = userRow.EmailId;
            objCmd.Parameters.Add(paramEmailId);

            FbParameter paramPhoneno;
            paramPhoneno = new FbParameter("@Phoneno", FbDbType.VarChar, 20);
            paramPhoneno.Value = userRow.PhoneNo;
            objCmd.Parameters.Add(paramPhoneno);

            FbParameter paramActives;
            paramActives = new FbParameter("@Actives", FbDbType.Char, 1);
            paramActives.Value = userRow.Active == true ? 'T' : 'F';
            objCmd.Parameters.Add(paramActives);

            FbParameter paramLoggedIn;
            paramLoggedIn = new FbParameter("@Loggedin", FbDbType.Char, 1);
            paramLoggedIn.Value = userRow.LoggedIn == true ? 'Y' : 'N';
            objCmd.Parameters.Add(paramLoggedIn);

            FbParameter paramRegistrationDate;
            paramRegistrationDate = new FbParameter("@RegistrationDate", FbDbType.TimeStamp);
            paramRegistrationDate.Value = DateTime.Now;
            objCmd.Parameters.Add(paramRegistrationDate);

            FbParameter paramTrial;
            paramTrial = new FbParameter("@Trial", FbDbType.Char, 1);
            paramTrial.Value = 'Y';
            objCmd.Parameters.Add(paramTrial);


            FbParameter paramLastupdated;
            paramLastupdated = new FbParameter("@Lastupdated", FbDbType.TimeStamp);
            paramLastupdated.Value = DateTime.Now;
            objCmd.Parameters.Add(paramLastupdated);



            Connection con = new Connection();
            if (con.CreateConnection())
            {
                con.dbcmd = objCmd;
                int result = con.ExecuteSQLCommandNQ();

                if (result == 1)
                {
                    strSQL = "SELECT max(ID) from users;";
                    objCmd = new FbCommand(strSQL);
                    con.dbcmd = objCmd;
                    result = int.Parse(con.ExecuteSQLScalar());
                }
                con.DestroyConnection();
                //returns the crated id
                //Utils.WriteDebugLog(DateTime.Today.ToShortTimeString() + " Return Result (Insert User) For " + userRow.LoginId);
                return result;
            }
            else
            {
                //connection problem
                //Utils.WriteDebugLog(DateTime.Today.ToShortTimeString() + " Connection Error (Insert User) For " + userRow.LoginId);
                Logclass.WriteDebuglog(" (Insertuser) Connection Error ");
                return -2;
            }


        }

        private bool UpdateVersion(string loginid, string version)
        {
            string strSQL = "UPDATE USERS SET VERSION = @Version where LoginID = @LoginID;";

            FbCommand objCmd = new FbCommand(strSQL);

            //Create parameters
            FbParameter paramLoginID;
            paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
            paramLoginID.Value = loginid;
            objCmd.Parameters.Add(paramLoginID);

            FbParameter paramVersion;
            paramVersion = new FbParameter("@Version", FbDbType.VarChar, 50);
            paramVersion.Value = version;
            objCmd.Parameters.Add(paramVersion);

            Connection dstuff = new Connection();
            if (dstuff.CreateConnection())
            {
                dstuff.dbcmd = objCmd;
                dstuff.ExecuteSQLCommandNQ();
                dstuff.DestroyConnection();

                return true;
            }
            else
            {
                dstuff.DestroyConnection();
                Logclass.WriteDebuglog("(UpdateVersion)Connection Error for loginid" + loginid);
                return false;
            }
        }
   
        public bool Loggedin(string loginid, bool status)
        {
            string strSQL = "UPDATE USERS SET Loggedin = @Loggedin where LoginID = @LoginID;";


            //if (!status)
            //{
            //    UserSession.GetInstance().DeleteUserSession(loginid);
            //}

            FbCommand objCmd = new FbCommand(strSQL);

            //Create parameters
            FbParameter paramLoginID;
            paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
            paramLoginID.Value = loginid;
            objCmd.Parameters.Add(paramLoginID);

            FbParameter paramLoggedIn;
            paramLoggedIn = new FbParameter("@Loggedin", FbDbType.Char, 1);
            paramLoggedIn.Value = status == true ? 'Y' : 'N';
            objCmd.Parameters.Add(paramLoggedIn);

            Connection dstuff = new Connection();
            if (dstuff.CreateConnection())
            {
                dstuff.dbcmd = objCmd;
                dstuff.ExecuteSQLCommandNQ();
                dstuff.DestroyConnection();
                
                return true;
            }
            else
            {
                dstuff.DestroyConnection();
                Logclass.WriteDebuglog("(Loggedin)Connection Error for loginid" + loginid);
                return false;
            }
        }

        public int ValidatePassword(string loginid, string password,string version)
        {
            Connection dsstuff = new Connection();
            if (dsstuff.CreateConnection())
            {
                //string sql = "select * from USERS where NAME = '" + username + "'";
                //string strSQL = "SELECT COUNT(*) FROM USERS WHERE LoginID = @LoginID AND Passwords = @Password AND ACTIVES = 'T';";
                //string strSQL = "SELECT * FROM USERS WHERE LoginID = '" + loginid + "' AND Passwords = '" + password + "' AND ACTIVES = 'T';";
                string strSQL = "SELECT * FROM USERS WHERE LoginID = @LoginID AND Passwords = @Password AND ACTIVES = 'T';";

                FbCommand objCmd = new FbCommand(strSQL);

                //Create the parameters
                FbParameter paramLoginID;
                paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
                paramLoginID.Value = loginid;
                objCmd.Parameters.Add(paramLoginID);

                //Hash the password
                byte[] hashedDataBytes = Utils.Encrypt(password);
                //Execute the parameterized query
                FbParameter paramPwd;
                paramPwd = new FbParameter("@Password", FbDbType.Char, 16);
                paramPwd.Value = hashedDataBytes;
                objCmd.Parameters.Add(paramPwd);


                //dstuff.dbcmd = objCmd;
                //string count = dstuff.ExecuteSQLScalar();
                DataSet ds = dsstuff.ExecuteSQLAdapter(objCmd);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    //change the status to Loggedin
                    Loggedin(loginid, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    UserRow ur = new UserRow();
                    ur.LoginId = (string)dr["LOGINID"];
                    ur.EmailId = (string)dr["EMAILID"];
                    ur.Version = version;
                    //UserSession.GetInstance().AddUserSession(ur);
                    //new WebServices.Scalper.WebServicesScalper().GetUserSession().AddUserSession(ur);

                    UpdateVersion(loginid, version);

                    Logclass.WriteDebuglog("checked for the password successfully for SQL " + strSQL);

                    dsstuff.DestroyConnection();
                    //success
                    return 1;
                }
                else
                {
                    dsstuff.DestroyConnection();

                    //failure 
                    Logclass.WriteDebuglog("checking sor passwrod failed for SQL " + strSQL);
                    return -1;
                }

            }
            else
            {
                //error in creating connection
                //Utils.WriteDebugLog(DateTime.Today.ToShortTimeString() + " Connection Error (ValidatePassword) For " + loginid);
                Logclass.WriteDebuglog("(ValidatePassword) Connection Error for loginid" + loginid);
                return -2;
            }
        }

        public String GetEmailID(string loginid)
        {
            Connection dsstuff = new Connection();
            if (dsstuff.CreateConnection())
            {
                string strSQL = "SELECT * FROM USERS WHERE LoginID = @LoginID AND ACTIVES = 'T';";
                FbCommand objCmd = new FbCommand(strSQL);

                //Create the parameters
                FbParameter paramLoginID;
                paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
                paramLoginID.Value = loginid;
                objCmd.Parameters.Add(paramLoginID);
                
                DataSet ds = dsstuff.ExecuteSQLAdapter(objCmd);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    String email = (string)dr["EMAILID"];
                    return email;
                }
                else
                {
                    dsstuff.DestroyConnection();
                    return "";
                }
            }
            else
            {
                Logclass.WriteDebuglog("Connection Error : email id will be returned as blank");
                return "";
            }
        }

        public int ValidatePassword(string loginid, string password)
        {
            Connection dsstuff = new Connection();
            if (dsstuff.CreateConnection())
            {
                //string sql = "select * from USERS where NAME = '" + username + "'";
                //string strSQL = "SELECT COUNT(*) FROM USERS WHERE LoginID = @LoginID AND Passwords = @Password AND ACTIVES = 'T';";
                //string strSQL = "SELECT * FROM USERS WHERE LoginID = '" + loginid + "' AND Passwords = '" + password + "' AND ACTIVES = 'T';";
                string strSQL = "SELECT * FROM USERS WHERE LoginID = @LoginID AND Passwords = @Password AND ACTIVES = 'T';";
               
                FbCommand objCmd = new FbCommand(strSQL);

                //Create the parameters
                FbParameter paramLoginID;
                paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
                paramLoginID.Value = loginid;
                objCmd.Parameters.Add(paramLoginID);

                //Hash the password
                byte[] hashedDataBytes = Utils.Encrypt(password);
                //Execute the parameterized query
                FbParameter paramPwd;
                paramPwd = new FbParameter("@Password", FbDbType.Char, 16);
                paramPwd.Value = hashedDataBytes;
                objCmd.Parameters.Add(paramPwd);
                
                
                //dstuff.dbcmd = objCmd;
                //string count = dstuff.ExecuteSQLScalar();
                DataSet ds = dsstuff.ExecuteSQLAdapter(objCmd);
                
                if (ds.Tables[0].Rows.Count == 1)
                {
                    //change the status to Loggedin
                    Loggedin(loginid, true);

                    Logclass.WriteDebuglog(loginid + ":----logged in successfully ");

                    dsstuff.DestroyConnection();
                    //success
                    return 1;
                }
                else
                {
                    dsstuff.DestroyConnection();

                    //failure 
                    Logclass.WriteDebuglog(loginid + ":-----Login Fails ");
                    Logclass.WriteDebuglog(loginid + ": User name or Password Wrong");
                    return -1;
                }
               
           }
            else
            {
                //error in creating connection
                //Utils.WriteDebugLog(DateTime.Today.ToShortTimeString() + " Connection Error (ValidatePassword) For " + loginid);
                Logclass.WriteDebuglog(loginid + ":   Connection Error while password check");
                return -2;
            }


        }

        public bool ModifyPassword(string loginid, string oldpassword, string newpassword)
        {
            Connection dstuff = new Connection();


            string strSQL = "UPDATE USERS SET Passwords = @NewPassword where LoginID = @LoginID and Passwords = @OldPassword and ACTIVES = 'T';";

            FbCommand objCmd = new FbCommand(strSQL);

            Logclass.WriteDebuglog(loginid + ":------Modifing Password----------");
            //Create parameters
            FbParameter paramLoginID;
            paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
            paramLoginID.Value = loginid;
            objCmd.Parameters.Add(paramLoginID);

            //Encrypt the password
            byte[] hashedBytesOldPwd = Utils.Encrypt(oldpassword);

            FbParameter paramOldPwd;
            paramOldPwd = new FbParameter("@OldPassword", FbDbType.Char, 16);
            paramOldPwd.Value = hashedBytesOldPwd;
            objCmd.Parameters.Add(paramOldPwd);

            //Encrypt the password
            byte[] hashedBytesNewPwd = Utils.Encrypt(newpassword);

            FbParameter paramNewPwd;
            paramNewPwd = new FbParameter("@NewPassword", FbDbType.Char, 16);
            paramNewPwd.Value = hashedBytesNewPwd;
            objCmd.Parameters.Add(paramNewPwd);



            if (dstuff.CreateConnection())
            {
                dstuff.dbcmd = objCmd;
                int result = dstuff.ExecuteSQLCommandNQ();
                Logclass.WriteDebuglog("(ModifyPassword)Password is modified for SQL " + strSQL);
                dstuff.DestroyConnection();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    Logclass.WriteDebuglog(loginid + ":    Fail to change password");
                    Logclass.WriteDebuglog(loginid + ":    either loginID or oldPasswd is wrong");
                    Logclass.WriteDebuglog(dstuff.dbcmd.CommandText);
                    return false;
                }

            }
            else
            {
                dstuff.DestroyConnection();
                Logclass.WriteDebuglog(loginid + ":    connection Error while Modifying Password");
                return false;
            }


        }

        public int ForgotPassword(string loginid, string emailid)
        {
            Connection dstuff = new Connection();
            if (dstuff.CreateConnection())
            {
                //string sql = "select * from USERS where NAME = '" + username + "'";
                string strSQL = "SELECT COUNT(*) FROM USERS WHERE LoginID = @LoginID AND EmailID = @EmailID AND ACTIVES = 'T';";
                FbCommand objCmd = new FbCommand(strSQL);

                //Create the parameters
                FbParameter paramLoginID;
                paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
                paramLoginID.Value = loginid;
                objCmd.Parameters.Add(paramLoginID);

                //Execute the parameterized query
                FbParameter paramEmailID;
                paramEmailID = new FbParameter("@EmailID", FbDbType.VarChar, 50);
                paramEmailID.Value = emailid;
                objCmd.Parameters.Add(paramEmailID);

                dstuff.dbcmd = objCmd;
                string count = dstuff.ExecuteSQLScalar();


                if (count == "1")
                {
                    //success
                    strSQL = "UPDATE USERS SET Passwords = @NewPassword where LoginID = @LoginID;";

                    objCmd = new FbCommand(strSQL);

                    objCmd.Parameters.Add(paramLoginID);

                    //Encrypt the password
                    //string newpassword = new Random().Next(10000000, 99999999).ToString();
                    string newpassword = Generate(8, 10);
                    byte[] hashedBytesNewPwd = Utils.Encrypt(newpassword);

                    FbParameter paramNewPwd;
                    paramNewPwd = new FbParameter("@NewPassword", FbDbType.Char, 16);
                    paramNewPwd.Value = hashedBytesNewPwd;
                    objCmd.Parameters.Add(paramNewPwd);

                    dstuff.dbcmd = objCmd;
                    dstuff.ExecuteSQLCommandNQ();




                    //send the new password by email
                    string subject = "[BGC Trade Companion] User Info Request";

                    String Body = "Hi ," + Environment.NewLine +
                    "A request was made to send you your  password for the" + Environment.NewLine +
                    "BGC Trade Companion Login. Your details are as follows:" + Environment.NewLine + Environment.NewLine +
                    "Loginid    : " + loginid + Environment.NewLine +
                    "Password    : " + newpassword + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                    "Regards, " + Environment.NewLine + "-BGC";
                    bool sent = Utils.SendEmail(emailid, "UserInfo@scalper.co.uk", subject, Body,"");
                    if (sent == true)
                    {
                        Logclass.WriteDebuglog(loginid + ":    New Password Sent to User-  " + loginid);
                        return 1;
                    }
                    else
                        return -3;
                }
                else
                {
                    dstuff.DestroyConnection();
                    Logclass.WriteDebuglog(loginid + ":    Failure, Either Loginid or Emailid is wrong");
                    Logclass.WriteDebuglog(loginid + " :emailID -" + emailid);
                    //failure 
                    return -1;
                }

            }
            else
            {
                //error in creating connection
                //Logclass.WriteDebuglog("(ForgotPassword) connection error");
                return -2;
            }
        }

        public DataSet GetUsersDS()
        {
            Connection dstuff = new Connection();

            if (dstuff.CreateConnection())
            {

                string sql = "select * from users;";
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                dstuff.DestroyConnection();
                //set the primary key
                DataColumn[] myColArray = new DataColumn[1];

                myColArray[0] = ds.Tables[0].Columns["ID"];
                ds.Tables[0].PrimaryKey = myColArray;
                return ds;
            }
            else
            {
                return null;
            }
        }

        public int EditUser(UserRow userRow)
        {
            string strSQL = "update users set loginid = @LoginID , username = @Username, emailid = @EmailId,phoneno = @Phoneno,actives = @Actives where id = @ID;";

            FbCommand objCmd = new FbCommand(strSQL);
            Logclass.WriteDebuglog(userRow.LoginId + ":---------Editing User Deatails----------");
            //Create parameters
            FbParameter paramLoginId;
            paramLoginId = new FbParameter("@LoginId", FbDbType.VarChar, 50);
            paramLoginId.Value = userRow.LoginId;
            objCmd.Parameters.Add(paramLoginId);

            FbParameter paramUsername;
            paramUsername = new FbParameter("@Username", FbDbType.VarChar, 20);
            paramUsername.Value = userRow.Username;
            objCmd.Parameters.Add(paramUsername);

            //Encrypt the password
            FbParameter paramID;
            paramID = new FbParameter("@ID", FbDbType.Char, 20);
            paramID.Value = userRow.Id;
            objCmd.Parameters.Add(paramID);


            FbParameter paramEmailId;
            paramEmailId = new FbParameter("@EmailId", FbDbType.VarChar, 50);
            paramEmailId.Value = userRow.EmailId;
            objCmd.Parameters.Add(paramEmailId);

            FbParameter paramPhoneno;
            paramPhoneno = new FbParameter("@Phoneno", FbDbType.VarChar, 20);
            paramPhoneno.Value = userRow.PhoneNo;
            objCmd.Parameters.Add(paramPhoneno);

            FbParameter paramActives;
            paramActives = new FbParameter("@Actives", FbDbType.Char, 1);
            paramActives.Value = userRow.Active == true ? 'T' : 'F';
            objCmd.Parameters.Add(paramActives);



            Connection con = new Connection();
            if (con.CreateConnection())
            {

                con.dbcmd = objCmd;
                int result = con.ExecuteSQLCommandNQ();
                Logclass.WriteDebuglog(userRow.LoginId + ":    Details is Edited for User: " + userRow.Username);
                con.DestroyConnection();
                return result;
            }
            else
            {
                //connection problem
                Logclass.WriteDebuglog(userRow.LoginId + ":    connection Problem while editing user deatils");
                return -2;
            }


        }
  
        public int CheckDependency(string tradecompanionId)
        {
            try
            {
                Connection dstuff = new Connection();
                if (dstuff.CreateConnection())
                {
                    //string sql = "select * from USERS where NAME = '" + username + "'";
                    string strSQL = "SELECT COUNT(*) FROM ORDERS WHERE TRADECOMPANIONID = @TRADECOMPANIONID;";
                    FbCommand objCmd = new FbCommand(strSQL);

                    //Create the parameters
                    FbParameter paramLoginID;
                    paramLoginID = new FbParameter("@TRADECOMPANIONID", FbDbType.VarChar, 20);
                    paramLoginID.Value = tradecompanionId;
                    objCmd.Parameters.Add(paramLoginID);

                    dstuff.dbcmd = objCmd;
                    int count =  int.Parse(dstuff.ExecuteSQLScalar());
                    dstuff.DestroyConnection();
                    if (count > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        
                        //no dependency 
                        return 2;
                    }
                }
                else
                {
                    //connection error
                    Logclass.WriteDebuglog(tradecompanionId + ":    connection error in checkdependency");
                    return -2;
                }
            }
            catch(Exception ex)
            {
                //some exception
                Logclass.WriteDebuglog("    error: " + ex.Message + ex.StackTrace);
                return -3;
            }
        }
   
        public int DeleteUser(int userId)
        {
            string strSQL = "delete from  users where  ID = @ID;";

            FbCommand objCmd = new FbCommand(strSQL);
            Logclass.WriteDebuglog(userId + ":---------Deleteing the user---------");

            //Create parameters
            FbParameter paramID;
            paramID = new FbParameter("@ID", FbDbType.Integer);
            paramID.Value = userId;
            objCmd.Parameters.Add(paramID);


            Connection con = new Connection();
            if (con.CreateConnection())
            {

                con.dbcmd = objCmd;
                int result = con.ExecuteSQLCommandNQ();
                con.DestroyConnection();
                return result;
            }
            else
            {
                //connection problem
                Logclass.WriteDebuglog(userId + ":    Connection error while Deleting user");
                return -2;
            }
        }
        public DataSet GetUsersDSFromQuery(String sql)
        {
            Connection dstuff = new Connection();

            if (dstuff.CreateConnection())
            {

                //string sql = "select * from users;";
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                dstuff.DestroyConnection();
                //set the primary key
                DataColumn[] myColArray = new DataColumn[1];

                myColArray[0] = ds.Tables[0].Columns["ID"];
                ds.Tables[0].PrimaryKey = myColArray;
                return ds;
            }
            else
            {
                return null;
            }
        }


        public int AddUserIntoSesstion(string loginid)
        {
            Connection dsstuff = new Connection();
            if (dsstuff.CreateConnection())
            {
                string strSQL = "SELECT * FROM USERS WHERE LoginID = @LoginID ;";

                FbCommand objCmd = new FbCommand(strSQL);

                //Create the parameters
                FbParameter paramLoginID;
                paramLoginID = new FbParameter("@LoginID", FbDbType.VarChar, 50);
                paramLoginID.Value = loginid;
                objCmd.Parameters.Add(paramLoginID);

                DataSet ds = dsstuff.ExecuteSQLAdapter(objCmd);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    //change the status to Loggedin
                    Loggedin(loginid, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    UserRow ur = new UserRow();
                    ur.LoginId = (string)dr["LOGINID"];
                    ur.EmailId = (string)dr["EMAILID"];
                    //UserSession.GetInstance().AddUserSession(ur);

                    dsstuff.DestroyConnection();
                    //success
                    return 1;
                }
                else
                {
                    dsstuff.DestroyConnection();

                    //failure 
                    Logclass.WriteDebuglog("Adding User Session Failed");
                    return -1;
                }

            }
            else
            {
                Logclass.WriteDebuglog("(AddUserIntoSesstion) Connection Error for loginid" + loginid);
                return -2;
            }

        }
    }
}
