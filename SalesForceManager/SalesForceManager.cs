using RestSharp;
using SalesforceSharp;
using SalesforceSharp.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Siemplify
{
    public class SalesForceManager : SalesforceSharp.SalesforceClient
    {
        string _consumerKey, _consumerSecret;
        private string _userName, _password;
        private string _callbackUrl = "https://login.salesforce.com/services/oauth2/success";

        public SalesForceManager(string consumerKey, string consumerSecret, string userName, string password)
        {            
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _userName = userName;
            _password = password;
        }


        public static void Log(string msg, [CallerMemberName] string caller = null) =>
            Console.WriteLine($"[{caller}]: {msg}");
        
        public static bool Log(IRestResponse response, [CallerMemberName] string caller = null)
        {            
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Log($"OK: {response.Content}", caller);
                    return true;
                }
                else
                    throw new Exception($"ERROR: {response.Content}");
            }
            catch (Exception ex)
            {
                Log(ex.Message, caller);
                return false;
            }
        }



        /// <summary>
        /// Authenticates on SalesForce via OAuth 2.0
        /// See also: https://stackoverflow.com/questions/12794302/salesforce-authentication-failing 
        /// </summary>
        public void Connect()
        {
            try
            {                
                var authFlow = new UsernamePasswordAuthenticationFlow(_consumerKey, _consumerSecret, _userName, _password);
                base.Authenticate(authFlow);

                Log("OK");
            }
            catch (SalesforceException ex)
            {
                Log($"Authentication failed: {ex.Error}");
            }            
        }


        public IList<dynamic> GetObjectsByName(string objectName, string fields = "")
        {
            if (string.IsNullOrWhiteSpace(fields))
                fields = GetObjectFields(objectName);

            var records = base.Query<dynamic>($"SELECT {fields} FROM {objectName}");        // "SELECT id, name, description FROM Account"

            Log($"{records.Count} records fetched from [{objectName}]");
            return records;
        }


        public string GetObjectFields(string objectName)
        {
            var sobjDetails = base.GetSObjectDetail(objectName);
            if (sobjDetails == null)
            {
                Log("Error");
                return "";
            }

            var fieldNames = sobjDetails.Fields.ConvertAll<string>(f => f.Name);
            string result = string.Join(", ", fieldNames);

            return result;
        }

        public List<Opportunity> GetOpportunitiesWithHistory()
        {
            //string historyFields = GetObjectFields("OpportunityHistory");   
            DateTime? SafeParse(string str)
            {
                try
                {
                    return DateTime.Parse(str, CultureInfo.InvariantCulture);
                }
                catch
                {
                    return null;
                }
            }

            //string oppFields = GetObjectFields("Opportunity");

            const string fieldsToObtain = "Id, Name, StageName, CreatedDate, OwnerId, CreatedById";
            var opportunities = GetObjectsByName("Opportunity", fieldsToObtain);            
            var oppHistory = Query<dynamic>("SELECT Id, (SELECT OpportunityId, CreatedDate, StageName FROM OpportunityHistories) FROM Opportunity");


            List<Opportunity> result = new List<Opportunity>();

            foreach (var opp in opportunities)
            {
                var resultOppObj = new Opportunity()
                {
                    Id = opp.Id,
                    Name = opp.Name,
                    Stage = opp.StageName,
                    BillingCountry = null,              // TODO: NO data
                    CreatedDate = SafeParse((string)opp.CreatedDate),
                    Owner = opp.OwnerId,
                    FYCV = 0,                           // TODO: NO data?
                    CreatedBy = opp.CreatedById,
                };

                OpportunityStageHistory history = new OpportunityStageHistory();

                // TODO: combine oppHistory and opportunities

                result.Add(resultOppObj);
            }

            return result;
        }
    }
}
