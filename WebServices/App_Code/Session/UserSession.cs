using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WebServices.Scalper.DatabaseStuff;
using System.Collections;
using System.Threading;

namespace WebServices.Scalper.Session
{

    /// <summary>
    /// Summary description for UserSession
    /// </summary>
    public class UserSession
    {
        static UserSession _singletonUserSession;
        Hashtable htUsers;

        private UserSession()
        {
            //
            // TODO: Add constructor logic here
            //
            htUsers = new Hashtable();
            Util.Utils.WriteDebugLog("Create Instance Usersession");
        }

        public static UserSession GetInstance()
        {            
            if (_singletonUserSession == null)
            {
                _singletonUserSession = new UserSession();
            }         
            return _singletonUserSession;
        }

        public void AddUserSession(UserRow ur)
        {
            if (htUsers.ContainsKey(ur.LoginId))
            {
                //dupicate user
                Util.Utils.WriteDebugLog("Duplicate user login");
            }
            else
            {
                //start thread for this user
                Util.Utils.WriteDebugLog("Starting user session " + ur.LoginId + " " + ur.EmailId);

                HearBeatThread hbt = new HearBeatThread(ur);

                htUsers.Add(ur.LoginId, hbt);
            }
       }

        public void DeleteUserSession(string loginid)
        {
            if (htUsers.ContainsKey(loginid))
            {
                HearBeatThread hbt = (HearBeatThread)htUsers[loginid];
                hbt.AbortThread();
                hbt = null;
                htUsers.Remove(loginid);
                Util.Utils.WriteDebugLog("User Session deleted : login id - " + loginid); 
               
            }
        }

        public HeartBeat HeartBeat(HeartBeat hb)
        {
            Util.Utils.WriteDebugLog("Users in Session " + htUsers.Count);
            if (htUsers.ContainsKey(hb.LoginID))
            {
                Util.Utils.WriteDebugLog("HeartBeat: Session Found " + hb.LoginID);

                HearBeatThread hbt = (HearBeatThread)htUsers[hb.LoginID];
                return hbt.SetRecvTime();
            }
            else
            {
                Util.Utils.WriteDebugLog("HeartBeat: Session Not Found " + hb.LoginID);

                Users user = new Users();

                Util.Utils.WriteDebugLog("HeartBeat: Add user into Session  " + hb.LoginID);

                int result = user.AddUserIntoSesstion(hb.LoginID);
                
                if (result < 0)
                {
                    Util.Utils.WriteDebugLog("HeartBeat: Add user into session failure " + hb.LoginID);
                    hb.Status = -1;
                    return hb;
                }
                else
                {
                    Util.Utils.WriteDebugLog("HeartBeat: Add user into session success " + hb.LoginID);
            
                    HearBeatThread hbt = (HearBeatThread)htUsers[hb.LoginID];
                    return hbt.SetRecvTime();
                }
            }
        }
  
    }
}
