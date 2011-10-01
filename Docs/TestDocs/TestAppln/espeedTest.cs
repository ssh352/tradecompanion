using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
using System.Text;

namespace espeedTest
{
    public class espeedTest
    {
        public static void Main()
        {
            espeedTest es = new espeedTest();
            es.OnTimer();
        }
        bool checknum(int t)
        {
            if (t < 10)
                return true;
            else
                return false;
        }
        public void OnTimer()
        {
            System.IO.StreamWriter SW;
            System.IO.StreamWriter SW1;
            Random ran = new Random();
            for (int i = 1; i < 10000; i++)
            {

                string[] symbol ={ "EURUSD", "EURCHF", "EURJPY", "EURGBP", "USDCHF", "USDCAD", "AUDUSD", "GBPUSD", "USDJPY" };
                Int32 HH = DateTime.Now.Hour;
                int MM = DateTime.Now.Minute;
                int SS = DateTime.Now.Second;
                int Ss =ran.Next(0, 8);
                string Dates;
                if(checknum(HH))
                    Dates="0"+HH.ToString();
                else
                    Dates = HH.ToString();
                if (checknum(MM))
                    Dates = Dates + "0" + MM.ToString();
                else
                    Dates = Dates + MM.ToString();
                if (checknum(SS))
                    Dates = Dates + "0" + SS.ToString();
                else
                    Dates = Dates + SS.ToString();
            
                string Act;
                string filename="C:\\Program Files\\BGC\\EXPORT\\" +symbol[Ss]+ Dates +".req";
                if (i%2 == 0)
                    Act = "Sell";
                else
                    Act = "Buy";
                SW = System.IO.File.CreateText(filename);
                SW.WriteLine("1070703 " + Dates + " FOREX " + symbol[Ss] + " " + Act + " 2 1 Scalper2");
                SW.Close();
                SW1 = System.IO.File.AppendText("d:\\List.txt");
                SW1.WriteLine(Dates + " " + symbol[Ss] + " " + Act);
                SW1.Close();
                Console.WriteLine(Dates + " " + symbol[Ss] + " " + i);
                Thread.Sleep(30000);
            }
        }
    }
}
