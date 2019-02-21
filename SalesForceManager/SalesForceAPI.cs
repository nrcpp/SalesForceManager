using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceManager
{
    public class SalesForceAPI
    {
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


        public void Connect()
        {
        }
    }
}
