using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Siemplify
{
    class Program
    {
        // This data is available on "Manage Connected Apps" -> Edit App page
        // http://prntscr.com/mom7xt
        static string consumerKey = "<YOUR CONSUMER KEY>";     // NOTE: remove from public after replacing
        static string consumerSecret = "<YOUR CONSUMER SECRET>";     
        static string salesforceLogin = "<YOUR SALESFORCE LOGIN>";     
        static string salesforcePassword = "<YOUR SALSEFORCE PASSWORD>";     

        static void Main(string[] args)
        {
            
#if DEBUG
            // for testing purposes, "token.txt" have to be in .gitignore
            if (File.Exists("oauth_data.txt"))
                InitFromFile("oauth_data.txt");                
#endif

            var salesForceManager = new SalesForceManager(consumerKey, consumerSecret, salesforceLogin, salesforcePassword);

            // Enable TLS 1.2 as a main protocol for SalesForce. Otherwise API won't work.
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            salesForceManager.Connect();
        }

        private static void InitFromFile(string fileName)
        {
            try
            {
                var lines = File.ReadAllLines(fileName);
                consumerKey = lines[0].Trim();
                consumerSecret = lines[1].Trim();
                salesforceLogin = lines[2].Trim();
                salesforcePassword = lines[3].Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
