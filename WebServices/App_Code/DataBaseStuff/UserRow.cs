using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebServices.Scalper.DatabaseStuff
{
    /// <summary>
    /// Summary description for Users
    /// </summary>
    public class UserRow
    {
        int id;
        string loginID;
        string username;
        string password;
        string emailid;
        string phoneno;
        bool active;
        bool loggedin;
        string version;

        public UserRow()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string LoginId
        {
            get
            {
                return loginID;
            }
            set
            {
                loginID = value;
            }
        }
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        public string EmailId
        {
            get
            {
                return emailid;
            }
            set
            {
                emailid = value;
            }
        }
        public string PhoneNo
        {
            get
            {
                return phoneno;
            }
            set
            {
                phoneno = value;
            }
        }
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public bool LoggedIn
        {
            get
            {
                return loggedin;
            }
            set
            {
                loggedin = value;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }
    }
}
