using System;
using System.Windows.Forms;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for Logclass
/// </summary>
public class Logclass
{
   protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Logclass));
    static Logclass()
    {
        
        //
        // TODO: Add constructor logic here
        // 
        log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(HttpContext.Current.Request.MapPath("logging.xml")));
    
    }

    public static void WriteDebuglog(string s)
    {
        lock (typeof(Logclass))
        {
            log.Info(s);
        }
    }

}
