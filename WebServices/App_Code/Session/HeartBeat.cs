using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebServices.Scalper.Session
{

    /// <summary>
    /// Summary description for HeartBeat
    /// </summary>
    public class HeartBeat
    {
        string loginID;
        int status; //0 OK 1 Exit TC



        public HeartBeat()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public string LoginID
        {
            get { return loginID; }
            set { loginID = value; }
        }


        public int Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
