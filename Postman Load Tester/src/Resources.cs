using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Postman_Load_Tester
{
    public class Resources
    {
        
        public static class Collections
        {
            public static List<KeyValuePair<string, string>> List = new List<KeyValuePair<string, string>>();
            public static string Selected = "";
            public static string[] Options = { "Add Collection", "Select Collection" };

            
        }

        public static class Environments
        {
            public static List<KeyValuePair<string, string>> List = new List<KeyValuePair<string, string>>();
            public static string Selected = "";
            public static string[] Options = { "Add Environment", "Select Environment" };
        }

        public static class ApiKeys
        {
            public static List< KeyValuePair<string, string> > List = new List<KeyValuePair<string, string>>();
            public static string Selected = "";
            public static string[] Options = { "Add API Key", "Select API Key" };
        }

        public static class UI
        {
            public static string[] Options = {
                "Environments Management", "Collections Management", "API Keys Management", "Change Number of Virtual Users", "Run Load Test"
            };

            public static int VirtualUsers = 10;
            public static List <Thread> VUList = new List<Thread>();
        }
    }
}
