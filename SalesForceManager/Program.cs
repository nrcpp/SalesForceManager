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
            // for testing purposes,  have to be in .gitignore
            if (File.Exists("oauth_data.txt"))
                InitFromFile("oauth_data.txt");                
#endif

            var forceManager = new SalesForceManager(consumerKey, consumerSecret, salesforceLogin, salesforcePassword);

            // Enable TLS 1.2 as a main protocol for SalesForce. 
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            forceManager.Connect();

            //
            // Fetch Account objects with ALL fields
            var accounts = forceManager.GetObjectsByName("Account");

            //Console.WriteLine("First 10 Accounts:");
            foreach (var a in accounts)            
                Console.WriteLine($"Id: {a.Id} - Name: {a.Name} - Description: {a.Description}");


            Console.WriteLine("Press enter to read Contacts");  Console.ReadLine();

            //
            // Fetch Contact objects with ALL fields obtained before
            string contactFields = forceManager.GetObjectFields("Contact");
            var contacts = forceManager.GetObjectsByName("Contact", contactFields);

            foreach (var c in contacts)        
            {
                Console.Write($"Id: {c.Id} - Name: {c.Name} - Title: {c.Title} - Phone: {c.Phone} - Email: {c.Email}");

                try
                {
                    if (c.CustomField1__c != null || c.CustomField2__c != null)
                        Console.Write($" - CustomField1: {c.CustomField1__c} - CustomField2 - {c.CustomField2__c}");
                }
                catch (Exception)
                {
                    Console.WriteLine("Custom fields not found");
                }

                Console.WriteLine();
            }



            //-------------------------------------------------
            Console.WriteLine("Press enter to read Opportunity and History"); Console.ReadLine();

            // Get opportunities object with history
            var opps = forceManager.GetOpportunitiesWithHistory();

            // output opportunity and history
            Console.WriteLine("\r\nOPPORTUNITIES:");
            foreach (var op in opps)
            {
                Console.WriteLine($"Name: {op.Name} - Stage: {op.Stage} - CreatedDate: {op.CreatedDate}\r\n\tStages History:");
                foreach (var h in op.StagesHistory)
                {
                    Console.WriteLine($"\t\tOpportunityName: {h.OpportunityName} - Stage: {h.ToStage} - CreatedDate: {h.CreatedDate}");
                }
            }

            Console.ReadLine();
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
