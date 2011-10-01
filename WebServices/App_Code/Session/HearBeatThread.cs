using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using WebServices.Scalper.DatabaseStuff;
using WebServices.Scalper.Util;

namespace WebServices.Scalper.Session
{
    /// <summary>
    /// Summary description for HearBeatThread
    /// </summary>
    public class HearBeatThread  
    {
        string loginid;
        DateTime recvTime;
        string email;
        string version;
        HeartBeat hb;
        Thread hbThread;
         
        public HearBeatThread(UserRow ur)
        {
            this.loginid = ur.LoginId;
            this.email = ur.EmailId;
            this.version = ur.Version;

            recvTime = DateTime.Now;
            hb = new HeartBeat();
            hb.LoginID = this.loginid;
            hb.Status = 0;

            hbThread = new Thread(Run);
            hbThread.Start();
        }

        public void Run()
        {
            Utils.WriteDebugLog("Start HeartBeat Thread for " + loginid);
            //break the thread after 3 minutes if no heart beat is received
            while ((Math.Abs(((TimeSpan)recvTime.Subtract(DateTime.Now)).Minutes)) < 3)
            {
                Thread.Sleep(30 * 1000);
                Utils.WriteDebugLog("HeartBeat Thread Success");
            }
            //send the email
            Utils.WriteDebugLog("[BGC Trade Companion] Breakdown ");
            string subject = "[BGC Trade Companion] Breakdown";

            String Body = "Hi ," + Environment.NewLine +
            "There seems to be some problem with Trade Companion logged in as " + loginid + Environment.NewLine +
            "Please make sure all the things are working fine." + Environment.NewLine + Environment.NewLine +
            "Reastart the Trade Companion if it has gone down." + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "Regards, " + Environment.NewLine + "-BGC";
            bool mailed = Utils.SendEmail(email, "UserInfo@tradercompanion.co.uk", subject, Body, "");
            
            if (mailed)
            {
                Utils.WriteDebugLog("Email sent to Login: " + loginid + " Email: " + email);
            }
            else
            {
                Utils.WriteDebugLog("Problem in Email sent to Login: " + loginid + " Email: " + email);
            }
            //Delete the user
            UserSession.GetInstance().DeleteUserSession(loginid);

        }

        public void AbortThread()
        {
            hbThread.Abort();
        }
        
        public HeartBeat SetRecvTime()
        {
            Util.Utils.WriteDebugLog("HeartBeat Received " + loginid + " " + DateTime.Now.Minute.ToString());                           
            recvTime = DateTime.Now;
            hb.Status = 0;
            return hb;
        }
    }
}
