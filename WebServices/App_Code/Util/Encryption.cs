// This sample demonstrates using a key based on the cryptographic service provider (CSP) version
// of the Data Encryption Standard (DES)algorithm to encrypt a string to a byte array, and then 
// to decrypt the byte array back to a string.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Configuration;
using System.Web.Services;





namespace WebServices.Scalper.Util
{
    class Utils
    {
        // Encrypt the string.
        public static byte[] Encrypt(string PlainText)
        {
            //Encrypt the password
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(PlainText));
            return hashedBytes;
        }

        public static void WriteDebugLog(string s)
        {
            //new System.Web.UI.Page().Server.MapPath
            //new System.Web.Services.Protocols.SoapServerMessage().s
            String fnPath = @"D:\Scalper\WebServices\App_Code\Util\DBLOG"; //new System.Web.UI.Page().Server.MapPath(@"\DBLOG");
            if (Directory.Exists(fnPath) == false)
                Directory.CreateDirectory(fnPath);
            try
            {
                String fn = fnPath + @"\DB.log";
         
                FileStream Fs = new FileStream(fn, FileMode.Append, FileAccess.Write);
                StreamWriter tw = new StreamWriter(Fs);

                tw.WriteLine(DateTime.Now.ToString() + ":" + s);
                tw.Close();
            }
            catch (Exception ex)
            { }
        }
        public static bool SendEmail(string emailto, string emailfrom, string subject, string emailtext,string carboncopy)
        {
            //WriteDebugLog("Sending Email to " + emailto);
            Logclass.WriteDebuglog("(SendEmail)Sending Email to " + emailto);
            SmtpClient client;
            MailAddress fromAddr, ToAddr;
            MailMessage mesg;

            //String mailToAddr = "sales@scalper.co.uk";
            String mailToAddr = emailto;
            string smtpUserID = ConfigurationSettings.AppSettings["SMTPUserID"];
            string smtpPasswd = ConfigurationSettings.AppSettings["SMTPPassword"];
            string smtpServer = ConfigurationSettings.AppSettings["SMTPServer"];

            //smtpServer = "88.208.220.198";
            //smtpUserID = "logs@tradercompanion.co.uk";
            //smtpPasswd = "shusiloo";

            //System.Configuration.ConfigurationManager.AppSettings["SMTPUserID"]
            try
            {
                client = new SmtpClient(smtpServer);
                client.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["SMTPPort"]);
                fromAddr = new MailAddress(smtpUserID, "Franco Dimuccio", System.Text.Encoding.UTF8);
                ToAddr = new MailAddress(mailToAddr, emailto, System.Text.Encoding.UTF8);

                client.Credentials = new System.Net.NetworkCredential(smtpUserID, smtpPasswd);
                client.EnableSsl = true;

                mesg = new MailMessage(fromAddr, ToAddr);
                mesg.Subject = subject;//"[BGC Trade Companion] User Info Request";
                mesg.SubjectEncoding = System.Text.Encoding.UTF8;

                mesg.Body = emailtext;
                mesg.BodyEncoding = System.Text.Encoding.UTF8;

                if (carboncopy != "")
                {
                    MailAddress copy = new MailAddress(carboncopy);
                    mesg.CC.Add(copy);
                }

                client.Send(mesg);
                mesg.Dispose();
                //WriteDebugLog("Email sent to" + emailto);
                Logclass.WriteDebuglog("(SendEmail)Email sent to" + emailto);
                return true;
            }
            catch (Exception ex)
            {
                //WriteDebugLog("Exception Sending Email" + ex.Message);
                Logclass.WriteDebuglog("(SendEmail)Exception Sending Email" + ex.Message);
                return false;
            }
           
        }
    }

}

