using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // obtain your token from 
            string token = "<YOUR TOKEN>";     // NOTE: remove from public after replacing

#if DEBUG
            // for testing purposes, "token.txt" have to be in .gitignore
            if (File.Exists("token.txt"))
                token = File.ReadAllText("token.txt").Trim();
#endif

            var api = new SalesForceAPI();
            api.Connect();
        }
    }
}
