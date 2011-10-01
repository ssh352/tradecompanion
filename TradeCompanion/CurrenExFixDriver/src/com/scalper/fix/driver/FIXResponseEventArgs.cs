using System;
using System.Collections.Generic;
using System.Text;

using com.scalper.fix;

namespace com.scalper.fix.driver
{
    public class FIXResponseEventArgs : EventArgs
    {
        public FIXResponseEventArgs(FIXNotice n)
        {
            this._fixNotice = n;
        }

        public String Text
        {
            get { return ((this._fixNotice != null) ? this._fixNotice.NoticeText : ""); } 
        }

        public FIXNotice _fixNotice;
    }
}
