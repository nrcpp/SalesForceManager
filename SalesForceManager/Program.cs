using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new SalesForceAPI();
            api.Connect();
        }
    }
}
