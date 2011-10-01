using System;
using System.Collections.Generic;
using System.Text;

using Icap.com.scalper.fix;

namespace Icap.com.scalper.fix.driver
{
    public class FIXTResponseEventArgs : EventArgs
    {
        public FIXTResponseEventArgs(FIXTNotice n)
        {
            this._fixNotice = n;
        }

        public String Text
        {
            get { return ((this._fixNotice != null) ? this._fixNotice.NoticeText : ""); } 
        }

        public FIXTNotice _fixNotice;
    }
}
