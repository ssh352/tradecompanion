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

namespace WebServices.Scalper
{

    /// <summary>
    /// Summary description for TraderDetails
    /// </summary>
    public class TraderDetails
    {

        DataSet ds;
        Orders orders;
        
        public TraderDetails()
        {
            //
            // TODO: Add constructor logic here
            //
            orders = new Orders();
        }

        public DataSet GetOrders(string tradername)
        {
            string sql;
            sql = "select * from orders where TradeCompanionID = '" + tradername + "'";
            ds = orders.GetOrdersFromQuery(sql);
            return ds;
        }
        //private DataRow GetRow(DateTime startDate, DateTime nextDate)
        //{
        //    decimal sumBUY;
        //    decimal sumSELL;
        //    string filter;
        //    int count;
        //    filter = "side=1 and DateID >= " + startDate.ToOADate().ToString() + " and DateID <= " + nextDate.ToOADate().ToString();
        //    try
        //    {
        //        sumBUY = ((decimal)(ds.Tables[0].Compute("Sum(Amount)", filter)));
        //    }
        //    catch (Exception ex)
        //    {
        //        sumBUY = 0;
        //    }
        //    DataView dv = ds.Tables[0].DefaultView;
        //    dv.RowFilter = filter;
        //    count = dv.Count;
        //    filter = "side=2 and DateID >= " + startDate.ToOADate().ToString() + " and DateID <= " + nextDate.ToOADate().ToString();
        //    try
        //    {
        //        sumSELL = ((decimal)(ds.Tables[0].Compute("Sum(Amount)", filter)));
        //    }
        //    catch (Exception ex)
        //    {
        //        sumSELL = 0;
        //    }
        //    dv = ds.Tables[0].DefaultView;
        //    dv.RowFilter = filter;
        //    count = count + dv.Count;
        //    DataRow dr = dsTradeSummary.Tables[0].NewRow();
        //    drp["Period"] = startDate.ToString() + " - " + nextDate.ToString();
        //    dr["NetProfit"] = (sumSELL - sumBUY).ToString();
        //    if (sumBUY != 0)
        //    {
        //        dr["ProfitFactor"] = decimal.Round((sumSELL / sumBUY), 2).ToString();
        //    }
        //    dr["Trades"] = count.ToString();
        //    return dr;
        //}

        //private DataSet GetTraderSummary(string type)
        //{
        //    try
        //    {
        //        DateTime startDate;
        //        DateTime endDate;
        //        DateTime nextDate;
        //        if ((ds.Tables(0).Rows.Count > 0))
        //        {
        //            startDate = DateTime.FromOADate(ds.Tables(0).Rows(0)("DateID").ToString());
        //            endDate = DateTime.FromOADate(ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("DateID").ToString());
        //            Calendar cal;
        //            cal = CultureInfo.InvariantCulture.Calendar;
        //            dsTradeSummary.Tables(0).Rows.Clear();
        //            if (cmbReportType.SelectedIndex == 0)
        //            {
        //                while ((startDate.Year <= endDate.Year))
        //                {
        //                    nextDate = "31 DECEMBER " + startDate.Year.ToString() + " 23:59:59";
        //                    dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate));
        //                    startDate = nextDate.AddSeconds(1);
        //                }
        //            }
        //            else if (cmbReportType.SelectedIndex == 2)
        //            {
        //                while ((startDate <= endDate))
        //                {
        //                    string st;
        //                    st = cal.GetDaysInMonth(startDate.Year, startDate.Month).ToString() + " " + MonthName(startDate.Month) + " " + startDate.Year.ToString() + " 23:59:59";
        //                    nextDate = st;
        //                    dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate));
        //                    startDate = nextDate.AddSeconds(1);
        //                }
        //            }
        //            else if (cmbReportType.SelectedIndex == 3)
        //            {
        //                while ((startDate <= endDate))
        //                {
        //                    int addDays;
        //                    addDays = 7 - cal.GetDayOfWeek(startDate.Date);
        //                    nextDate = startDate.Date.Date.AddDays(addDays) + " 23:59:59";
        //                    dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate));
        //                    startDate = nextDate.AddSeconds(1);
        //                }
        //            }
        //            else if (cmbReportType.SelectedIndex == 1)
        //            {
        //                while ((startDate <= endDate))
        //                {
        //                    string st;
        //                    st = startDate.Date.Date + " 23:59:59";
        //                    nextDate = st;
        //                    dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate));
        //                    startDate = nextDate.AddSeconds(1);
        //                }
        //            }
        //            ShowPeriodicalReturns();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.WriteDebugLog(" .... Show Periodical Returns ERROR " + ex.Message);
        //    }
        //}


    }
}
