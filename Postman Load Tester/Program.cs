using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Postman_Load_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";

            while (input != "0")
            {
                for (int i = 0; i < Resources.UI.Options.Length; i++)
                {
                    Console.WriteLine(i+1 + ": " + Resources.UI.Options[i]);
                }
                Console.WriteLine("0: Exit");

                input = Console.ReadLine();

                if (input == "1")
                {
                    Console.WriteLine("Enter Postman Environment UUID:");
                    string environment = Console.ReadLine();
                    Resources.Environments.List.Add(environment);
                }
                if (input == "2")
                {
                    Console.WriteLine("Enter Postman Collection UUID:");
                    string collection = Console.ReadLine();
                    Resources.Collections.List.Add(collection);
                }
                if (input == "3")
                {
                    Console.WriteLine("Please Enter the number of Virtual Users for Load Test (Max 50):");
                    string virtualUsers = Console.ReadLine();
                    Resources.UI.VirtualUsers = int.Parse(virtualUsers);
                }
                if (input == "4")
                {
                    Resources.UI.VUList.Clear();
                    for (int i = 0; i < Resources.UI.VirtualUsers; i++)
                    {
                        Thread virtualUser = new Thread(new ThreadStart(PostmanCollectionRunner));
                        Resources.UI.VUList.Add(virtualUser);
                    }
                    for (int i = 0; i < Resources.UI.VirtualUsers; i++)
                    {
                        Resources.UI.VUList[i].Start();
                    }

                    WaitUntilAllThreadsComplete();

                }
            }


        }

        private static void PostmanCollectionRunner()
        {

            string strCmdText;
            strCmdText = "run https://api.getpostman.com/collections/7767269-ad2575a1-f754-4b6e-acd5-b70b03257267?apikey=3ca104ad4d80499f96a027dd717c001e --environment https://api.getpostman.com/environments/7767269-e3577ea5-02e3-47a1-b96d-b6d2138dd3ca?apikey=3ca104ad4d80499f96a027dd717c001e";
            System.Diagnostics.Process.Start("newman", strCmdText);
        }
        private static void WaitUntilAllThreadsComplete()
        {
            foreach (Thread virtualUser in Resources.UI.VUList)
            {
                virtualUser.Join();
            }
        }
    }
}
