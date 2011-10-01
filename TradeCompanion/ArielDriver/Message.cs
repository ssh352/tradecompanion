using System;
using System.Collections.Generic;
using System.Text;

namespace ArielDriver
{
    class Message
    {
        private static readonly String dateFormat = "yyyyMMdd";
        private static readonly String timeFormat = "yyyyMMdd-HH:mm:ss.fff";
        private static readonly string timeFormatMillis = "yyyyMMdd-HH:mm:ss.fff";

        public static System.String buildDateString(ref System.DateTime date, bool ignoreTime, bool useMillis)
        {
            return Message.buildDateString(ref date, ignoreTime, useMillis, null);
        }

        private static System.String buildDateString(ref System.DateTime date, bool ignoreTime, bool useMillis, System.TimeZone timeZone)
        {
            String df;
            string tempDateTime = string.Empty;
            if (ignoreTime)
            {
                df = Message.dateFormat;
            }
            else
            {
                if (useMillis)
                {

                    df = Message.timeFormat;
                    tempDateTime = date.ToString(df);
                }
                else
                {
                    df = Message.timeFormat;
                    tempDateTime = date.ToString(df);
                }
            }

            if (timeZone != null)
            {

            }
            return tempDateTime;
        }
    }
}
