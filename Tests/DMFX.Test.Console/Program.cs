using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Test.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DMFX.DarkMindConnect.DMFXClient client = new DarkMindConnect.DMFXClient();
            client.InitSession("http://localhost/", "46E6776E7FA9141FF4CF74BCBEC0DFF8B4898590");
            client.GetTimeSeriesInfo("US", "COT.FinFut.099741.Net");
            client.GetTimeSeries("US", "COT.FinFut.099741.Net", QuotesInterfaces.ETimeFrame.Weekly, DateTime.Now - TimeSpan.FromDays(3650), DateTime.UtcNow);
            for(int i = 0; i <= 30; ++i)
            {
                DateTime dt = DateTime.Parse("12/01/2019") + TimeSpan.FromDays(i);
                decimal value = client.GetDateValue("COT.FinFut.099741.Net", "Dealer_Positions_Net", dt);
                Console.WriteLine(dt.ToShortDateString() + " " + value);
            }
            
        }
    }
}
