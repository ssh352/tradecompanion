using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EspeedDriver
{
    public class Util
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Util));
        static Util()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Application.StartupPath + "\\\\logging.xml"));
        }

        public static void WriteDebugLogInfo(string s)
        {
            lock (typeof(Util))
            {
                log.Info(s);
            }
        }

        public static void WriteDebugLogDebug(string s)
        {
            lock (typeof(Util))
            {
                log.Debug(s);
            }
        }

        public static void WriteDebugLogWarn(string s)
        {
            lock (typeof(Util))
            {
                log.Warn(s);
            }
        }

        public static void WriteDebugLogError(string s)
        {
            lock (typeof(Util))
            {
                log.Error(s);
            }
        }
    }
}
