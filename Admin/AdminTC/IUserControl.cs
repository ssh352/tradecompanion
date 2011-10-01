using System;
using System.Collections.Generic;
using System.Text;

namespace AdminTC
{
    interface IUserControl
    {
        void ShowDetails(int id);
        void RefreshDataset();
    }
}
