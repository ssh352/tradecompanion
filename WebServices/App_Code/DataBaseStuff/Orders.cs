using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FirebirdSql.Data.FirebirdClient;

namespace WebServices.Scalper.DatabaseStuff
{
    /// <summary>
    /// Summary description for Orders
    /// </summary>
    internal class Orders
    {
        public Orders()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool InsertOrder(DataSet dsUser, string loginID, string currenExID)
        {
            Logclass.WriteDebuglog(loginID + ": -----------Inserting into orders using dataset-------");
            try
            {
                foreach (DataRow dr in dsUser.Tables[0].Rows)
                {
                    OrderRow orderRow = new OrderRow();
                    orderRow.OrderID = (string)dr["OrderID"];
                    orderRow.Exchange = (string)dr["Exchange"];
                    orderRow.Status = (string)dr["Status"];
                    orderRow.Symbol = (string)dr["Symbol"];
                    orderRow.MonthYear = (string)dr["MonthYear"];
                    orderRow.Side = (int)dr["Side"];
                    orderRow.Quantity = (int)dr["Quantity"];
                    //orderRow.Price =  Convert.ToDecimal(Convert.ToDouble(dr["Price"].ToString()));
                    orderRow.Price = Convert.ToString(dr["Price"]);
                    orderRow.TradeCurrency = (string)dr["TradeCurrency"];
                    orderRow.ExecOrderId = (string)dr["ExecOrderId"];
                    //20061017-12:52:28 to 2006 10 17 12:52:28
                    orderRow.TimeStamp = DateTime.Parse(((string)dr["TimeStamp"]).Insert(4, " ").Insert(7, " ").Replace("-", " "));
                    orderRow.TradeCompanionID = loginID;
                    orderRow.CurrenExID = currenExID;
                    orderRow.DateIDCustomer = DateTime.FromOADate(Convert.ToDouble(dr["DateID"].ToString()));
                    orderRow.SenderId = (string)dr["SenderID"];
                    int result = InsertOrder(orderRow);
                    if (result <= 0)
                    {
                        Logclass.WriteDebuglog(loginID + ": Fail to insert(InsertOrder) orderID " + orderRow.OrderID);
                        Logclass.WriteDebuglog("CurrenexID-     " + orderRow.SenderId);
                        Logclass.WriteDebuglog("Exchange-    " + orderRow.Exchange);
                        Logclass.WriteDebuglog("Symbol-      " + orderRow.Symbol);
                        Logclass.WriteDebuglog("Status-     " + orderRow.Status);
                        Logclass.WriteDebuglog("TradeCurrnecy- " + orderRow.TradeCurrency);
                        Logclass.WriteDebuglog("ExecOrderId- " + orderRow.ExecOrderId);
                        Logclass.WriteDebuglog(loginID + ":--------------------------------------------------------------------");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Logclass.WriteDebuglog("    error" + ex.Message + ex.StackTrace );
                return false;
            }
        }
        public bool InsertOrder(OrderRow[] orderRows)
        {
            Logclass.WriteDebuglog("    ----inserting into orders from array--------");
            foreach (OrderRow or in orderRows)
            {
                int result = InsertOrder(or);
                if (result <= 0)
                    return false;
            }
            return true;
        }

        public int InsertOrder(OrderRow orderRow)
        {
            if (OrderRowNotExistenceTest(orderRow.OrderID,orderRow.Status))//"Select * from Orders where OrderID = '" + orderRow.OrderID + "' and Status = '" + orderRow.Status + "' "))
            {
                string strSQL = "insert into orders (ID,OrderID, Exchange,Status, Symbol,MonthYear,Side,Quantity,Price,TimeStamps,TradeCurrency,ExecOrderId,DateID,TradeCompanionID,CurrenExID,DATEIDCUSTOMER,SENDERID) values (gen_id(gen_orders_id, 1), @OrderID,@Exchange,@Status,@Symbol,@MonthYear,@Side,@Quantity,@Price,@TimeStamp,@TradeCurrency,@ExecOrderID,@DateID,@TradeCompanionID,@CurrenExID,@DateIDCustomer,@senderID);";
                Logclass.WriteDebuglog(orderRow.TradeCompanionID + ":Inserting into orders orderid = " + orderRow.OrderID);

                FbCommand objCmd = new FbCommand(strSQL);

                //Create parameters
                FbParameter paramOrderID;
                paramOrderID = new FbParameter("@OrderID", FbDbType.VarChar, 100);
                paramOrderID.Value = orderRow.OrderID;
                objCmd.Parameters.Add(paramOrderID);

                FbParameter paramExchange;
                paramExchange = new FbParameter("@Exchange", FbDbType.VarChar, 20);
                paramExchange.Value = orderRow.Exchange;
                objCmd.Parameters.Add(paramExchange);

                FbParameter paramStatus;
                paramStatus = new FbParameter("@Status", FbDbType.VarChar, 50);
                paramStatus.Value = orderRow.Status;
                objCmd.Parameters.Add(paramStatus);


                FbParameter paramSymbol;
                paramSymbol = new FbParameter("@Symbol", FbDbType.VarChar, 20);
                paramSymbol.Value = orderRow.Symbol;
                objCmd.Parameters.Add(paramSymbol);

                FbParameter paramMonthYear;
                paramMonthYear = new FbParameter("@MonthYear", FbDbType.VarChar, 20);
                paramMonthYear.Value = orderRow.MonthYear;
                objCmd.Parameters.Add(paramMonthYear);

                FbParameter paramSide;
                paramSide = new FbParameter("@Side", FbDbType.Integer);
                paramSide.Value = orderRow.Side;
                objCmd.Parameters.Add(paramSide);

                FbParameter paramQuantity;
                paramQuantity = new FbParameter("@Quantity", FbDbType.Integer);
                paramQuantity.Value = orderRow.Quantity;
                objCmd.Parameters.Add(paramQuantity);


                FbParameter paramPrice;
                paramPrice = new FbParameter("@Price", FbDbType.VarChar, 50);
                paramPrice.Value = orderRow.Price;
                objCmd.Parameters.Add(paramPrice);

                FbParameter paramTimeStamp;
                paramTimeStamp = new FbParameter("@TimeStamp", FbDbType.TimeStamp);
                paramTimeStamp.Value = orderRow.TimeStamp;
                objCmd.Parameters.Add(paramTimeStamp);

                FbParameter paramTradeCurrency;
                paramTradeCurrency = new FbParameter("@TradeCurrency", FbDbType.VarChar, 20);
                paramTradeCurrency.Value = orderRow.TradeCurrency;
                objCmd.Parameters.Add(paramTradeCurrency);

                FbParameter paramExexOrderID;
                paramExexOrderID = new FbParameter("@ExecOrderID", FbDbType.VarChar, 50);
                paramExexOrderID.Value = orderRow.ExecOrderId;
                objCmd.Parameters.Add(paramExexOrderID);

                FbParameter paramDateID;
                paramDateID = new FbParameter("@DateID", FbDbType.TimeStamp);
                paramDateID.Value = DateTime.Now;
                objCmd.Parameters.Add(paramDateID);

                FbParameter paramTradeCompanionID;
                paramTradeCompanionID = new FbParameter("@TradeCompanionID", FbDbType.VarChar, 20);
                paramTradeCompanionID.Value = orderRow.TradeCompanionID;
                objCmd.Parameters.Add(paramTradeCompanionID);

                FbParameter paramCurrenExID;
                paramCurrenExID = new FbParameter("@CurrenExID", FbDbType.VarChar, 50);
                paramCurrenExID.Value = orderRow.CurrenExID;
                objCmd.Parameters.Add(paramCurrenExID);

                FbParameter paramDateIDCustomer;
                paramDateIDCustomer = new FbParameter("@DateIDCustomer", FbDbType.TimeStamp);
                paramDateIDCustomer.Value = orderRow.DateIDCustomer;
                objCmd.Parameters.Add(paramDateIDCustomer);

                FbParameter paramSenderID;
                paramSenderID = new FbParameter("@senderID", FbDbType.VarChar, 50);
                paramSenderID.Value = orderRow.SenderId;
                objCmd.Parameters.Add(paramSenderID);

                if (orderRow.Status == "Filled" | orderRow.Status == "Partially Filled")
                {
                    PLTrade plTrade = new PLTrade();
                    Boolean n = plTrade.insertPL(orderRow);//Pltrade data inserting
                }

                Connection con = new Connection();
                if (con.CreateConnection())
                {

                    con.dbcmd = objCmd;
                    int result = con.ExecuteSQLCommandNQ();
                    con.DestroyConnection();
                    //returns the crated id
                    return result;
                }
                else
                { //connection problem
                    Logclass.WriteDebuglog(orderRow.TradeCompanionID + ":    connection Error while inserting order " + con.dbcmd.CommandText);
                    return -2;
                }
            }
            return 1;
        }

        public DataSet GetOrdersFromQuery(String sqlQuery)
        {
            Connection dstuff = new Connection();
            Logclass.WriteDebuglog("Retriving dataset from orders using query-------");
            if (dstuff.CreateConnection())
            {

                string sql = sqlQuery;
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                dstuff.DestroyConnection();
                //set the primary key
                DataColumn[] myColArray = new DataColumn[1];

                myColArray[0] = ds.Tables[0].Columns["ID"];
                ds.Tables[0].PrimaryKey = myColArray;
                ds.Tables[0].TableName = "Orders";
                return ds;
            }
            else
            {
                Logclass.WriteDebuglog("Connection Error while get order from DB using Query" + dstuff.dbcmd.CommandText);
                Logclass.WriteDebuglog("Query-" + sqlQuery);
                return null;
            }

        }
        public DataSet GetOrdersDS()
        {
            Connection dstuff = new Connection();
            Logclass.WriteDebuglog("Retriving dataset from orders-------");
            if (dstuff.CreateConnection())
            {

                string sql = "select * from orders;";
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                dstuff.DestroyConnection();
                //set the primary key
                DataColumn[] myColArray = new DataColumn[1];

                myColArray[0] = ds.Tables[0].Columns["ID"];
                ds.Tables[0].PrimaryKey = myColArray;
                ds.Tables[0].TableName = "Orders";
                return ds;
            }
            else
            {
                Logclass.WriteDebuglog("Connection Error while geting data from Order Table" + dstuff.dbcmd.CommandText);
                return null;
            }
        }
        private bool OrderRowNotExistenceTest(string orderid,string status)
        {
            if (status != "Partially Filled")
            {
                string str = "Select * from Orders where OrderID = '" + orderid + "' and Status = '" + status + "' ";
                DataSet dsTest = new DataSet();
                dsTest = GetOrdersFromQuery(str);
                if (dsTest.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;
            }
            return true;
        }
    }
}
