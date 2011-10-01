using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using WebServices.Scalper.DatabaseStuff;
using WebServices.Scalper.Util;
using WebServices.Scalper.Session;


namespace WebServices.Scalper
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebServicesScalper : System.Web.Services.WebService
    {

        public WebServicesScalper()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        /*
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public Int32 FahrenheitToCelsius(Int32 Fahrenheit)
        {
            Int32 celsius;
            celsius = ((((Fahrenheit) - 32) / 9) * 5);
            return celsius;
        }
       
        [WebMethod]
        public Int32 CelsiusToFahrenheit(Int32 Celsius)
        {
            Int32 fahrenheit;
            fahrenheit = ((((Celsius) * 9) / 5) + 32);
            return fahrenheit;

        }
        */

        [WebMethod]
        public int CreateUser(UserRow userRow)
        {
            Users user = new Users();
            return user.InsertUser(userRow);
        }

        [WebMethod]
        public bool ModifyPassword(string loginid, string oldpassword, string newpassword)
        {
            Users user = new Users();
            return user.ModifyPassword(loginid, oldpassword, newpassword);
        }

        [WebMethod]
        public int ValidatePassword(string loginid, string password)
        {
            Users user = new Users();
            return user.ValidatePassword(loginid, password);
        }

        [WebMethod]
        public int ForgotPassword(string loginid, string email)
        {
            Users user = new Users();
            return user.ForgotPassword(loginid, email);
        }

        [WebMethod]
        public bool Loggedin(string loginid, bool status)
        {
            Users user = new Users();
            return user.Loggedin(loginid, status);
        }

        [WebMethod]
        public DataSet GetUsersDS()
        {
            Users user = new Users();
            return user.GetUsersDS();
        }

        [WebMethod]
        public int EditUsers(UserRow userRow)
        {
            Users user = new Users();
            return user.EditUser(userRow);
        }  

        [WebMethod]
        public int AddOrder(OrderRow orderRow)
        {
            Orders orders = new Orders();
            return orders.InsertOrder(orderRow);            
        }

        [WebMethod]
        public DateTime AddOrderReturnDate(OrderRow orderRow)
        {
            Orders orders = new Orders();
            int result = orders.InsertOrder(orderRow);
            if (result > 0)
                return orderRow.DateIDCustomer;
            else
                return orderRow.TimeStamp;
        }

        [WebMethod]
        public bool AddOrders(DataSet dsUser, string loginID, string currenExID)
        {
            Orders orders = new Orders();
            return orders.InsertOrder(dsUser, loginID, currenExID);
        }

        [WebMethod]
        public bool AddOrderRows(OrderRow[] orderRows)
        {
            Orders orders = new Orders();
            return orders.InsertOrder(orderRows);
        }

        [WebMethod]
        public DataSet GetOrdersDS()
        {
            Orders order = new Orders();
            return order.GetOrdersDS();
        }

        [WebMethod]
        public int AddUser(string loginId, string userName, string emailId, string phoneNo, string address, string city,string country)
        {
            Users user = new Users();
            return user.AddUser(loginId, userName, emailId, phoneNo, address,city,country);

        }

        [WebMethod]
        public DataSet GetOrdersFromQuery(String sql)
        {
            Orders order = new Orders();
            return order.GetOrdersFromQuery(sql);
        }
        [WebMethod]
        public bool IsEmailIDExist(string emailId)
        {
            Users user = new Users();
            return user.IsEmailIDExist(emailId);   
        }

        [WebMethod]
        public bool IsLoginIDExist(string loginId)
        {
            Users user = new Users();
            return user.IsLoginIDExist(loginId);
        }

        [WebMethod]
        public int DeleteUser(int userId)
        {
            Users user = new Users();
            return user.DeleteUser(userId);
        }

        [WebMethod]
        public int CheckDependency(string tradecompanionId)
        {
            Users user = new Users();
            return user.CheckDependency(tradecompanionId);
        }

        [WebMethod]
        public DataSet GetPLTradeDS()
        {
            PLTrade pltrade = new PLTrade();
            return pltrade.GetPLTradeDS();
        }
        [WebMethod]
        public DataSet GetPLTradeDSFromQuery(string sql)
        {
            PLTrade plTrade = new PLTrade();
            return plTrade.GetPLTradeDSFromQuery(sql);
        }

        //[WebMethod(EnableSession = true)]
        //public HeartBeat HeartBeat(HeartBeat hb)
        //{
        //    //    UserSession us;
        //    //    if (Session["UserSession"] == null)
        //    //      Session["UserSession"] = UserSession.GetInstance();

        //    //    us = (UserSession)Session["UserSession"];
        //    //    return us.HeartBeat(hb); //UserSession.GetInstance().HeartBeat(hb);
        //    //}
        //    return GetUserSession().HeartBeat(hb);
        //}


        //[WebMethod(EnableSession = true)]
        //public UserSession GetUserSession()
        //{
        //    UserSession us;
        //    if (Context.Session["UserSession"] == null)
        //        Context.Session["UserSession"] = UserSession.GetInstance();

        //    us = (UserSession)Context.Session["UserSession"];
        //    return us;
        //}

        [WebMethod]
        public int ValidatePasswordVersion(string loginid, string password, string version)
        {
            Users user = new Users();
            return user.ValidatePassword(loginid,password,version);
            
        }
        [WebMethod]
        public DataSet GetPIPSFromPLTradeQuery(string sql, string startDate, string endDate)
        {
            PLTrade plTrade = new PLTrade();
            return plTrade.GetPIPSFromPLTradeQuery(sql, startDate, endDate);
        }
        [WebMethod]
        public DataSet GetGraphData(string sql, string startDate, string endDate)
        {
            PLTrade plTrade = new PLTrade();
            return plTrade.GetGraphData(sql, startDate, endDate);
        }
        [WebMethod]
        public DataSet GetUsersDSFromQuery(String sql)
        {
            Users user = new Users();
            return user.GetUsersDSFromQuery(sql);
        }

        [WebMethod]
        public String GetEmailID(string loginid)
        {
            Users user = new Users();
            return user.GetEmailID(loginid);

        }
    }
}
