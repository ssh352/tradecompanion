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
using System.Collections;
namespace WebServices.Scalper.DatabaseStuff
{
    /// <summary>
    /// Summary description for PLTrade
    /// </summary>
    public class PLTrade
    {
        public PLTrade()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public Boolean insertPL(OrderRow orderRow)
        {
            PLTradeRow plTradeRow = new PLTradeRow();
            Logclass.WriteDebuglog(orderRow.TradeCompanionID + ":Inserting into PLTrade from orderRow");
            Logclass.WriteDebuglog("where symbol = " + orderRow.Symbol + "orderid = " + orderRow.OrderID);
            plTradeRow.OrderID = orderRow.OrderID;
            plTradeRow.Amount = orderRow.Quantity;
            plTradeRow.Actions = orderRow.Side;
            plTradeRow.DateID = orderRow.DateIDCustomer;
            plTradeRow.ExecOrderId = orderRow.ExecOrderId;
            plTradeRow.Price = orderRow.Price;
            plTradeRow.SenderID = orderRow.SenderId;
            plTradeRow.TradingServerDateTime = orderRow.TimeStamp;
            plTradeRow.TradeCompanionID = orderRow.TradeCompanionID;
            plTradeRow.Symbol = orderRow.Symbol;
            plTradeRow.Status = orderRow.Status;
            plTradeRow.MarketPrice = orderRow.MarketPrice;

            int n = insertPL(plTradeRow);
            if (n <= 0)
            {
                Logclass.WriteDebuglog(orderRow.TradeCompanionID + ":   -------Fail To insert into PLTrade DB");
                Logclass.WriteDebuglog("OrderID -" + orderRow.OrderID);
                Logclass.WriteDebuglog("Currenex ID -" + orderRow.SenderId);
                Logclass.WriteDebuglog("symbol  -" + orderRow.Symbol);
                Logclass.WriteDebuglog("ExecOrderID-" + orderRow.ExecOrderId);
                Logclass.WriteDebuglog(orderRow.TradeCompanionID + ":------------------------------------");
                return false;
            }
            return true;
        }
        public int insertPL(PLTradeRow plTradeRow) 
        {
            double remaining;
            decimal pips = 0;
            double remainingamount = 0;
            int result = 0;
            double netAmount = Convert.ToDouble(plTradeRow.Amount) * Convert.ToDouble(plTradeRow.Price);
            string sql;
            Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + ": ------Inserting into PLTrade from PltradeRow---------");
            Connection con = new Connection();
            try
            {
                Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");
                DataSet ds = GetPLTradeDS();
                Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");

                if (plTradeRow.Actions == 1) //buy
                {
                    string filter = "Actions = 2 AND Remaining > 0 AND Symbol = '" + plTradeRow.Symbol + "' AND SenderID = '" + plTradeRow.SenderID + "' AND TCID = '" + plTradeRow.TradeCompanionID + "'";
                    DataRow[] drs = ds.Tables[0].Select(filter, "DateID");
                    remainingamount = plTradeRow.Amount;

                    foreach (DataRow dr in drs)
                    {
                        double temp = Convert.ToDouble(dr["remaining"]);
                        pips = Convert.ToDecimal(dr["pips"]);
                        if (remainingamount > temp)
                        {
                            remainingamount = remainingamount - temp;
                            decimal diffprice;
                            diffprice = Convert.ToDecimal(dr["price"]) - Convert.ToDecimal(plTradeRow.Price);
                            diffprice = diffprice * (Decimal)temp;
                            pips = pips + diffprice;

                            remaining = 0;
                        }
                        else
                        {
                            remaining = temp - remainingamount;
                            decimal diffprice;
                            diffprice = Convert.ToDecimal(dr["price"]) - Convert.ToDecimal(plTradeRow.Price);
                            diffprice = diffprice * (Decimal)remainingamount;
                            pips = pips + diffprice;

                            remainingamount = 0;
                            Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");
                            result = ModifyChanges(remaining, pips, (int)dr["RowID"]);
                            Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");
                            break;
                        }
                        Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");
                        result = ModifyChanges(remaining, pips, (int)dr["RowID"]);
                        Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");

                    }
                    pips = 0;

                }
                else if (plTradeRow.Actions == 2)//Sell
                {
                    string filter = "Actions = 1 AND Remaining > 0 AND Symbol = '" + plTradeRow.Symbol + "' AND SenderID = '" + plTradeRow.SenderID + "' AND TCID = '" + plTradeRow.TradeCompanionID + "'";
                    DataRow[] drs = ds.Tables[0].Select(filter, "DateID");
                    remainingamount = plTradeRow.Amount;
                    foreach (DataRow dr in drs)
                    {
                        double temp = Convert.ToDouble(dr["remaining"]);
                        if (remainingamount > temp)
                        {
                            remainingamount = remainingamount - temp;
                            decimal diffprice;
                            diffprice = Convert.ToDecimal(plTradeRow.Price) - Convert.ToDecimal(dr["price"]);
                            diffprice = diffprice * (Decimal)temp;
                            pips = pips + diffprice;

                            remaining = 0;
                        }
                        else
                        {
                            remaining = temp - remainingamount;
                            decimal diffprice;
                            diffprice = Convert.ToDecimal(plTradeRow.Price) - Convert.ToDecimal(dr["price"]);
                            diffprice = diffprice * (Decimal)remainingamount;
                            pips = pips + diffprice;

                            remainingamount = 0;
                            Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");
                            result = ModifyRemaining((int)dr["RowID"], remaining);
                            Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");
                            break;
                        }
                        Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + "-----------");
                        result = ModifyRemaining((int)dr["RowID"], remaining);
                        Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + " ----------");
                    }
                }
                sql = " Insert into PLTrade (RowID,OrderID,Status,Symbol,Actions,Amount,Price,ExecOrderId,DateID,Remaining,Pips,SenderID,TCID,ServerDateTime,TradingServerDateTime,NetAmount,USDMarketPrice) values(gen_ID(gen_PLTrade_id, 1),@OrderID,@Status,@Symbol,@Actions,@Amount,@Price,@ExecOrderId,@DateID,@Remaining,@Pips,@SenderID,@TCID,@ServerDateTime,@TradingServerDateTime,@NetAmount,@MarketPrice) ";
                FbCommand objcmd = new FbCommand(sql);

                FbParameter paramOrderID;
                paramOrderID = new FbParameter("@orderID", FbDbType.VarChar, 50);
                paramOrderID.Value = plTradeRow.OrderID;
                objcmd.Parameters.Add(paramOrderID);

                FbParameter paramStatus;
                paramStatus = new FbParameter("@Status", FbDbType.VarChar, 50);
                paramStatus.Value = plTradeRow.Status;
                objcmd.Parameters.Add(paramStatus);


                FbParameter paramSymbol;
                paramSymbol = new FbParameter("@Symbol", FbDbType.VarChar, 20);
                paramSymbol.Value = plTradeRow.Symbol;
                objcmd.Parameters.Add(paramSymbol);

                FbParameter paramActions;
                paramActions = new FbParameter("@Actions", FbDbType.Integer);
                paramActions.Value = plTradeRow.Actions;
                objcmd.Parameters.Add(paramActions);

                FbParameter paramAmount;
                paramAmount = new FbParameter("@Amount", FbDbType.Integer);
                paramAmount.Value = plTradeRow.Amount;
                objcmd.Parameters.Add(paramAmount);


                FbParameter paramPrice;
                paramPrice = new FbParameter("@Price", FbDbType.VarChar, 50);
                paramPrice.Value = plTradeRow.Price;
                objcmd.Parameters.Add(paramPrice);

                FbParameter paramDateID;
                paramDateID = new FbParameter("@DateID", FbDbType.Float);
                paramDateID.Value = plTradeRow.DateID.ToOADate();
                objcmd.Parameters.Add(paramDateID);

                FbParameter paramRemaining;
                paramRemaining = new FbParameter("@Remaining", FbDbType.Integer);
                paramRemaining.Value = remainingamount;
                objcmd.Parameters.Add(paramRemaining);

                FbParameter paramPips;
                paramPips = new FbParameter("@Pips", FbDbType.Double);
                paramPips.Value = pips;
                objcmd.Parameters.Add(paramPips);

                FbParameter paramExecOrderId;
                paramExecOrderId = new FbParameter("@ExecOrderId", FbDbType.VarChar, 50);
                paramExecOrderId.Value = plTradeRow.ExecOrderId;
                objcmd.Parameters.Add(paramExecOrderId);

                FbParameter paramSenderId;
                paramSenderId = new FbParameter("@SenderID", FbDbType.VarChar, 50);
                paramSenderId.Value = plTradeRow.SenderID;
                objcmd.Parameters.Add(paramSenderId);

                FbParameter paramServerDateTime;
                paramServerDateTime = new FbParameter("@ServerDateTime", FbDbType.Float);
                paramServerDateTime.Value = DateTime.Now.ToOADate();
                objcmd.Parameters.Add(paramServerDateTime);

                FbParameter paramTradingSerDT;
                paramTradingSerDT = new FbParameter("@TradingServerDateTime", FbDbType.Float);
                paramTradingSerDT.Value = plTradeRow.TradingServerDateTime.ToOADate();
                objcmd.Parameters.Add(paramTradingSerDT);

                FbParameter paramTCID;
                paramTCID = new FbParameter("@TCID", FbDbType.VarChar, 50);
                paramTCID.Value = plTradeRow.TradeCompanionID;
                objcmd.Parameters.Add(paramTCID);

                FbParameter paramNetAmount;
                paramNetAmount = new FbParameter("@NetAmount", FbDbType.Float);
                paramNetAmount.Value = netAmount;
                objcmd.Parameters.Add(paramNetAmount);

                FbParameter paramMarketPrice;
                paramMarketPrice = new FbParameter("@MarketPrice", FbDbType.Double);
                paramMarketPrice.Value = plTradeRow.MarketPrice;
                objcmd.Parameters.Add(paramMarketPrice);

                if (con.CreateConnection())
                {
                    con.dbcmd = objcmd;
                    result = con.ExecuteSQLCommandNQ();
                }
                else
                    Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + ":    Connection Error in inseting Pltrade by PlTradeRow ");
            }
            catch (Exception ex)
            {
                Logclass.WriteDebuglog(plTradeRow.TradeCompanionID + ":    Error: " + ex.Message + ex.StackTrace );
                throw ex;
            }
            finally
            {
                con.DestroyConnection();
            }
            return result;
        }
        public DataSet GetPLTradeDS()
        {
            Connection dstuff = new Connection();
            Logclass.WriteDebuglog("Retriving dataset from PlTrade------");
            if (dstuff.CreateConnection())
            {

                string sql = "select * from PLTrade;";
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                dstuff.DestroyConnection();
                //set the primary key
                DataColumn[] myColArray = new DataColumn[1];

                myColArray[0] = ds.Tables[0].Columns["RowID"];
                ds.Tables[0].PrimaryKey = myColArray;
                ds.Tables[0].TableName = "PLTrade";
                return ds;
            }
            else
            {
                Logclass.WriteDebuglog("Connection Error while Get PLTrade data from DB" + dstuff.dbcmd.CommandText);
                return null;
            }
        }
        public DataSet GetPLTradeDSFromQuery(string sql)
        {
            Connection dstuff = new Connection();
            Logclass.WriteDebuglog("Retriving dataset by Query from Pltrade ------");
            if (dstuff.CreateConnection())
            {
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                dstuff.DestroyConnection();
                DataColumn[] myColArray = new DataColumn[1];

                myColArray[0] = ds.Tables[0].Columns["RowID"];
                ds.Tables[0].PrimaryKey = myColArray;
                ds.Tables[0].TableName = "PLTrade";
                return ds;
            }
            else
            {
                Logclass.WriteDebuglog("Connection Error while Get PLTrade data from DB using query" + dstuff.dbcmd.CommandText);
                Logclass.WriteDebuglog("Query: " + sql);
                return null;
            }
        }

        public int ModifyChanges(double remaining, Decimal pips, int id)
        {
            try
            {
                string sql;
                sql = "UPDATE PLTrade SET Remaining = @remaining, Pips = @pips where RowID = @RowID";
                FbCommand objCmd = new FbCommand(sql);

                Logclass.WriteDebuglog("Updating Reamining & Pips----- ");
                FbParameter paramId;
                paramId = new FbParameter("@RowID", FbDbType.Integer);
                paramId.Value = id;
                objCmd.Parameters.Add(paramId);

                FbParameter paramRemaining;
                paramRemaining = new FbParameter("@remaining", FbDbType.Double);
                paramRemaining.Value = remaining;
                objCmd.Parameters.Add(paramRemaining);

                FbParameter paramPips;
                paramPips = new FbParameter("@pips", FbDbType.Double);
                paramPips.Value = pips;
                objCmd.Parameters.Add(paramPips);

                Connection con = new Connection();
                if (con.CreateConnection())
                {
                    con.dbcmd = objCmd;
                    int result = con.ExecuteSQLCommandNQ();
                    con.DestroyConnection();
                    return result;
                }
                else
                {
                    Logclass.WriteDebuglog("Connection Error While Updating Remaining & Pips");
                    return -1;
                }
            }
            catch(Exception ex)
            {
                Logclass.WriteDebuglog("modifyChanges "+ ex.Message + ex.StackTrace);
                return -1;
            }
        }
        public int ModifyRemaining(int id, double remaining)
        {
            try
            {
                string sql;
                sql = " UPDATE PLTrade SET Remaining = @remaining where RowID = @RowID";
                FbCommand objCmd = new FbCommand(sql);
                Logclass.WriteDebuglog("Updating Reamining-----------");
                FbParameter paramId;
                paramId = new FbParameter("@RowID", FbDbType.Integer);
                paramId.Value = id;
                objCmd.Parameters.Add(paramId);

                FbParameter paramRemaining;
                paramRemaining = new FbParameter("@remaining", FbDbType.Double);
                paramRemaining.Value = remaining;
                objCmd.Parameters.Add(paramRemaining);

                Connection con = new Connection();
                if (con.CreateConnection())
                {
                    con.dbcmd = objCmd;
                    int result = con.ExecuteSQLCommandNQ();
                    con.DestroyConnection();
                    return result;
                }
                else
                {
                    Logclass.WriteDebuglog("Connection Error While Updating Remaining");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Logclass.WriteDebuglog(" modifyRemaing "+ ex.Message + ex.StackTrace);
                return -1;
            }
        }

        public DataSet GetPIPSFromPLTradeQuery(string sql, string startDate,string endDate)
        {
            Connection dstuff = new Connection();
            if (startDate != null && endDate != null)
            {
                DateTime sd = Convert.ToDateTime(startDate);
                double sdt = sd.ToOADate();
                DateTime ed = Convert.ToDateTime(endDate);
                double edt = ed.ToOADate();
                sql = sql + "'" + sdt + "' and SERVERDATETIME<='" + edt + "'";
            }
            //Logclass.WriteDebuglog(sql);
            if (dstuff.CreateConnection())
            {
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                dstuff.DestroyConnection();
                DataColumn[] myColArray = new DataColumn[1];
                myColArray[0] = ds.Tables[0].Columns["RowID"];
                ds.Tables[0].PrimaryKey = myColArray;
                ds.Tables[0].TableName = "PLTrade";
                return ds;
            }
            else
            {
                return null;
            }
        }
        public DataSet GetGraphData(string sql, string startDate, string endDate)
        {
            // creating connection to database
            Connection dstuff = new Connection();
            // completing the sql query after converting date in desire format
            if (startDate != null && endDate != null)
            {
                DateTime sd = Convert.ToDateTime(startDate);
                double sdt = sd.ToOADate();
                DateTime ed = Convert.ToDateTime(endDate);
                double edt = ed.ToOADate();
                sql = sql + "'" + sdt + "'";// and SERVERDATETIME<='" + edt + "'";
            }
            Logclass.WriteDebuglog(sql);
            // variable declaration for output proccessing
            Int32 IntVal;
            String DblVal;
            Int32 IntDate=0,i=0,flag=0;
            Double IntPips=0, tIntPips=0;
            Double MPrice;
            if (dstuff.CreateConnection())
            {
                // execute the sql
                DataSet ds = dstuff.ExecuteSQLAdapter(sql);
                // database connection closed
                dstuff.DestroyConnection();
                //seting data table parameter
                DataColumn[] myColArray = new DataColumn[1];
                myColArray[0] = ds.Tables[0].Columns["RowID"];
                ds.Tables[0].PrimaryKey = myColArray;
                ds.Tables[0].TableName = "PLTrade";
                //creating return Database
                DataSet rDs = new DataSet();
                //creating data table
                DataTable dt = new DataTable();
                // creating column
                dt.Columns.Add(new DataColumn("PIPS", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("DATEID",System.Type.GetType("System.Double")));
                //add table to dataset
                rDs.Tables.Add(dt);
                                                          
                int row = ds.Tables[0].Rows.Count;
                char[] ch = {'.'};
                DblVal = (Convert.ToString(ds.Tables[0].Rows[0]["DATEID"])).Split(ch)[0];
                IntVal = Convert.ToInt32(DblVal);
                IntDate = IntVal;
                //These are to hold the total amount traded for the day
                Double totalAmountBuy = 0;
                Double totalAmountSell = 0;
                Double remainingAmount = 0;
                for (i = 0; i < row; i++)
                {
                    MPrice = 0;
                    DblVal = (Convert.ToString(ds.Tables[0].Rows[i]["DATEID"])).Split(ch)[0];
                    IntVal = Convert.ToInt32(DblVal);
                    IntPips = Convert.ToInt32(ds.Tables[0].Rows[i]["PIPS"]);
                    
                    Logclass.WriteDebuglog(i.ToString() + ":Val" + IntVal.ToString());
                    Logclass.WriteDebuglog(i.ToString() + ":Date" + IntDate.ToString());
                    Logclass.WriteDebuglog(i.ToString()+":Pips"+IntPips.ToString());
                    if (ds.Tables[0].Rows[i]["USDMARKETPRICE"] != "")
                    {
                        if (ds.Tables[0].Rows[i]["USDMARKETPRICE"]!=System.DBNull.Value)
                        {
                            MPrice = Convert.ToDouble(ds.Tables[0].Rows[i]["USDMARKETPRICE"]);
                            Logclass.WriteDebuglog(i.ToString() + ":MPrice" + MPrice.ToString());
                        }
                    }
                    // calculating the pips in realized USD
                    if (MPrice != 0)
                    {
                        if (IntPips != 0)
                        {
                            IntPips = IntPips / MPrice;
                        }
                        if (IntVal == IntDate)
                        {
                            tIntPips = tIntPips + IntPips;
                            //This to get the total sum of buying and selling trade amount for a day
                            if (Convert.ToString(ds.Tables[0].Rows[i]["ACTIONS"]).Equals("1"))
                            {
                                totalAmountBuy = totalAmountBuy + Convert.ToDouble(ds.Tables[0].Rows[i]["AMOUNT"]);
                            }
                            else
                            {
                                totalAmountSell = totalAmountSell + Convert.ToDouble(ds.Tables[0].Rows[i]["AMOUNT"]);
                            }
                            remainingAmount = Convert.ToDouble(ds.Tables[0].Rows[i]["REMAINING"]);
                            Logclass.WriteDebuglog(i.ToString() + ":Sum " + tIntPips.ToString());
                            flag = 0;
                            if (i == row - 1)
                            {
                                flag = 1;
                            }
                        }
                        else
                        {
                            if (tIntPips != 0)
                            {
                                DataRow dr = rDs.Tables[0].NewRow();
                                Double amount = 0;
                                Logclass.WriteDebuglog("================");
                                Logclass.WriteDebuglog(IntDate.ToString());
                                Logclass.WriteDebuglog(tIntPips.ToString());
                                Logclass.WriteDebuglog("================");
                                //adding value to data row
                                //This to get the %age of profit that user made for the day.....
                                //We are getting %age for the day, We take the total trade for the day and profit or loss 

                                dr["DATEID"] = IntDate;
                                //This condtion is to get the remaining amount for the day.
                                if (totalAmountBuy >= totalAmountSell)
                                {
                                    amount = totalAmountBuy - remainingAmount;
                                }
                                else
                                {
                                    amount = totalAmountSell - remainingAmount;
                                }
                                //In case amount is zero then there is no profit or loss then set the pips to zero 
                                //else calculate the %age,here we are using 1000 not 100......
                                if (amount == 0)
                                {
                                    dr["PIPS"] = 0;
                                }
                                else
                                {
                                    dr["PIPS"] = ((tIntPips * 100) / amount);
                                }
                                rDs.Tables[0].Rows.Add(dr);
                            }
                            totalAmountSell = 0;
                            totalAmountBuy = 0;
                            remainingAmount = 0;
                            IntDate = IntVal;
                            tIntPips = IntPips;
                            flag = 1;
                            if (i == row - 1)
                            {
                                tIntPips = tIntPips + IntPips;
                            }
                        }
                        //if (i == row - 2 && ds.Tables[0].Rows[i + 1]["USDMARKETPRICE"] != System.DBNull.Value)
                        //{
                        //    if (Convert.ToDouble(ds.Tables[0].Rows[i]["USDMARKETPRICE"]) == 0)
                        //        flag = 1;
                        //}
                    }
                    else
                    {
                        //This to get the total sum of buying and selling trade amount for a day
                        if (Convert.ToString(ds.Tables[0].Rows[i]["ACTIONS"]).Equals("1"))
                        {
                            totalAmountBuy = totalAmountBuy + Convert.ToDouble(ds.Tables[0].Rows[i]["AMOUNT"]);
                        }
                        else
                        {
                            totalAmountSell = totalAmountSell + Convert.ToDouble(ds.Tables[0].Rows[i]["AMOUNT"]);
                        }
                        remainingAmount = Convert.ToDouble(ds.Tables[0].Rows[i]["REMAINING"]);
                    }

                }
                
                if (flag==1)
                {
                    Logclass.WriteDebuglog("================");
                    Logclass.WriteDebuglog(IntDate.ToString());
                    Logclass.WriteDebuglog(tIntPips.ToString());
                    Logclass.WriteDebuglog("================");    
                    DataRow dr = rDs.Tables[0].NewRow();
                    Double amount = 0;
                    dr["DATEID"] = IntDate;
                    if (totalAmountBuy >= totalAmountSell)
                    {
                        amount = totalAmountBuy - remainingAmount;
                    }
                    else
                    {
                         amount = totalAmountSell - remainingAmount;
                    }
                    if (amount == 0)
                    {
                        dr["PIPS"] = 0;
                    }
                    else
                    {
                        dr["PIPS"] = ((tIntPips * 100) / amount) ;
                    }
                    rDs.Tables[0].Rows.Add(dr);

                }
                Logclass.WriteDebuglog("Row Count:" + rDs.Tables[0].Rows.Count.ToString());
                if (rDs.Tables[0].Rows.Count==0 )
                {
                    Logclass.WriteDebuglog("Null Return");
                    return null;
                }
                else
                {
                    return rDs;
                }
                
            }
            else
            {
                return null;
            }
        }
    }
}
