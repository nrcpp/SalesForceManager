using RestSharp;
using SalesforceSharp;
using SalesforceSharp.Security;
using System;
using System.Collections.Generic;
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
    }
}
