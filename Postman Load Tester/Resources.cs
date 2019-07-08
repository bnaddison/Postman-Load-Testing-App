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
            public static ArrayList List = new ArrayList();
        }

        public static class Environments
        {
            public static ArrayList List = new ArrayList();
        }

        public static class UI
        {
            public static string[] Options = {
                "Add Environment", "Add Collection", "Change Number of Virtual Users", "Run Load Test"
            };

            public static int VirtualUsers = 10;
            public static List <Thread> VUList = new List<Thread>();
        }
    }
}
