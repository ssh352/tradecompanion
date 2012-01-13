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
using System.Diagnostics;
using WebServices.Scalper.Util;

namespace WebServices.Scalper.DatabaseStuff
{
    /// <summary>
    /// Summary description for DatabaseStuff
    /// </summary>
    public class Connection
    {
        string connectionString = "User=SYSDBA;Password=masterkey;Database=TradeCompanion;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Packet Size=8192;ServerType=0;";
        FbConnection dbcon;
        public IDbCommand dbcmd;
        string sql;
        IDataReader reader;
        DataSet ds;


        public Connection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool CreateConnection()
        {
            try
            {
                dbcon = new FbConnection(connectionString);
                dbcon.Open();
                return true;
            }
            catch (Exception ex)
            {
                //error in creating connection
                //Debug.WriteLine(ex.Message);
                Logclass.WriteDebuglog("(CreateConnection)Error " + ex.Message + ex.StackTrace );
                return false;
            }


        }

        public IDataReader ExecuteSQLReader(string sql)
        {
            try
            {
                dbcmd = dbcon.CreateCommand();
                this.sql = sql;
                dbcmd.CommandText = this.sql;
                reader = dbcmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                //error in executing sql
                //Debug.WriteLine(ex.Message);
                Logclass.WriteDebuglog("(ExecuteSQLReader) SQL Failed " + sql);
                Logclass.WriteDebuglog("(ExecuteSQLReader)Error " + ex.Message + ex.StackTrace);
                return null;
            }
        }
        public IDataReader ExecuteSQLCommand()
        {
            try
            {
                reader = dbcmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                //error in executing sql
                //Debug.WriteLine(ex.Message);
                Logclass.WriteDebuglog("(ExecuteSQLCommand) Commad failed " + dbcmd.CommandText);
                Logclass.WriteDebuglog("(ExecuteSQLCommand) Error " + ex.Message + ex.StackTrace );
                return null;
            }
        }

        public int ExecuteSQLCommandNQ()
        {
            try
            {
                dbcmd.Connection = dbcon;
                int rowseffected = dbcmd.ExecuteNonQuery();
                if (rowseffected >= 1)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                //error in executing sql
                try
                {
                    //Debug.WriteLine(ex.Message);
                    if (ex.Message.Substring(0, 45) == "violation of PRIMARY or UNIQUE KEY constraint")
                        return -3;
                    else
                    {
                        //unkwown reason
                        Logclass.WriteDebuglog("(ExecuteSQLCommandNQ) SQL Failed " + dbcmd.CommandText);
                        Logclass.WriteDebuglog("(ExecuteSQLCommandNQ)Error " + ex.Message + ex.StackTrace);
                        return -5;
                    }
                }
                catch
                {
                    //unkwown reason
                    Logclass.WriteDebuglog("(ExecuteSQLCommandNQ) SQL Failed " + dbcmd.CommandText);
                    Logclass.WriteDebuglog("(ExecuteSQLCommandNQ)Error " + ex.Message + ex.StackTrace);
                    return -6;
                }
            }
        }

        public String ExecuteSQLScalar()
        {
            try
            {
                dbcmd.Connection = dbcon;
                return dbcmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                //error in executing sql
                //Debug.WriteLine(ex.Message);
                Logclass.WriteDebuglog("(ExecuteSQLScalar) SQL Failed " + dbcmd.CommandText);
                Logclass.WriteDebuglog("(ExecuteSQLScalar) Error " + ex.Message + ex.StackTrace);
                return null;
            }
        }

        public DataSet ExecuteSQLAdapter(string sql)
        {
            try
            {
                this.sql = sql;
                ds = new DataSet();
                FbDataAdapter fda = new FbDataAdapter(sql, dbcon);
                fda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //error in executing sql
                //Debug.WriteLine(ex.Message);
                //Utils.WriteDebugLog(DateTime.Today.ToShortTimeString() + " (ExecuteSQLAdapter) " + ex.Message);
                //Utils.WriteDebugLog("(ExecuteSQLAdapter) SQL" + dbcmd.CommandText);
                Logclass.WriteDebuglog("(ExecuteSQLAdapter) SQL Failed" + dbcmd.CommandText);
                Logclass.WriteDebuglog("(ExecuteSQLAdapter) Error" + ex.Message);
                return null;
            }
        }

        public DataSet ExecuteSQLAdapter(FbCommand fbCommand)
        {
            try
            {
                //this.sql = sql;
                ds = new DataSet();
                fbCommand.Connection = dbcon;
                FbDataAdapter fda = new FbDataAdapter(fbCommand);
                fda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //error in executing sql
                //Debug.WriteLine(ex.Message);
                Logclass.WriteDebuglog("(ExecuteSQLAdapter) SQL Failed " + dbcmd.CommandText);
                Logclass.WriteDebuglog("(ExecuteSQLAdapter) Error " + ex.Message + ex.StackTrace);
                return null;
            }
        }

        public void DestroyConnection()
        {
            // clean up
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
            if (dbcmd != null)
            {
                dbcmd.Dispose();
                dbcmd = null;
            }

            dbcon.Close();
            dbcon = null;
        }


    }

}

