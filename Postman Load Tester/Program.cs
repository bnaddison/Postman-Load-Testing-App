using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

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
                    string environmentInput = "";
                    while (environmentInput != "0")
                    {
                        for (int i = 0; i < Resources.Environments.Options.Length; i++)
                        {
                            Console.WriteLine(i + 1 + ": " + Resources.Environments.Options[i]);
                        }
                        Console.WriteLine("0: Exit");
                        environmentInput = Console.ReadLine();
                        if (environmentInput == "1")
                        {
                            Add("Environment", Resources.Environments.List);
                        }
                        if (environmentInput == "2")
                        {
                            Select("Environment", Resources.Environments.List, Resources.Environments.Selected);
                        }
                    }
                }
                if (input == "2")
                {
                    string collectionInput = "";
                    while (collectionInput != "0")
                    {
                        for (int i = 0; i < Resources.Collections.Options.Length; i++)
                        {
                            Console.WriteLine(i + 1 + ": " + Resources.Collections.Options[i]);
                        }
                        Console.WriteLine("0: Exit");
                        collectionInput = Console.ReadLine();
                        if (collectionInput == "1")
                        {
                            Add("Collection", Resources.Collections.List);
                        }
                        if (collectionInput == "2")
                        {
                            Select("Collection", Resources.Collections.List, Resources.Collections.Selected);
                        }
                    }
                }
                if (input == "3")
                {
                    string apiKeyInput = "";
                    while (apiKeyInput != "0")
                    {
                        for (int i = 0; i < Resources.ApiKeys.Options.Length; i++)
                        {
                            Console.WriteLine(i + 1 + ": " + Resources.ApiKeys.Options[i]);
                        }
                        Console.WriteLine("0: Exit");
                        apiKeyInput = Console.ReadLine();
                        if (apiKeyInput == "1")
                        {
                            Add("API Key", Resources.ApiKeys.List);
                        }
                        if (apiKeyInput == "2")
                        {
                            Select("API Key", Resources.ApiKeys.List, Resources.ApiKeys.Selected);
                        }
                    }
                }
                if (input == "4")
                {
                    Console.WriteLine("Please Enter the number of Virtual Users for Load Test (Max 50):");
                    string virtualUsers = Console.ReadLine();
                    Resources.UI.VirtualUsers = int.Parse(virtualUsers);
                }
                if (input == "5")
                {
                    string response = "";
                    Console.WriteLine("API Key: " + Resources.ApiKeys.Selected);
                    Console.WriteLine("Collection: " + Resources.Collections.Selected);
                    Console.WriteLine("Environment: " + Resources.Environments.Selected);
                    Console.WriteLine("Number of Virtual Users: " + Resources.UI.VirtualUsers);
                    while (!IsYesOrNo(response))
                    {
                        Console.WriteLine("Proceed?  (y/n)");
                        response = Console.ReadLine();
                    }

                    if (IsYes(response))
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


        }

        private static void Add(string elementName, List<KeyValuePair<string, string>> list)
        {
            Console.WriteLine("Enter Postman " + elementName + " Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Postman " + elementName + " UUID:");
            string uuid = Console.ReadLine();
            KeyValuePair<string, string> item = new KeyValuePair<string, string>(name, uuid);
            list.Add(item);
        }

        private static void Select(string elementName, IReadOnlyList<KeyValuePair<string, string>> list, string selected)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("No Environments in List.  Please add Environment");
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(i + 1 + ": " + list[i].Key + " ==> " + list[i].Value);
            }

            string selection = Console.ReadLine();
            int select = int.Parse(selection);
            selected = list[select - 1].Value;
        }

        private static void PostmanCollectionRunner()
        {

            string strCmdText;
            strCmdText = "run https://api.getpostman.com/collections/" + Resources.Collections.Selected + "?apikey=" + Resources.ApiKeys.Selected + " --environment https://api.getpostman.com/environments/" + Resources.Environments.Selected + "?apikey=" + Resources.ApiKeys.Selected;
            System.Diagnostics.Process.Start("newman", strCmdText);
        }
        private static void WaitUntilAllThreadsComplete()
        {
            foreach (Thread virtualUser in Resources.UI.VUList)
            {
                virtualUser.Join();
            }
        }

        private static bool IsYes(string value)
        {
            return value == "y" 
                   || value == "Y";
        }
        private static bool IsYesOrNo(string value)
        {
            return value == "y"
                   || value == "Y"
                   || value == "n"
                   || value == "N";
        }
        private static bool IsNo(string value)
        {
            return value == "n"
                   || value == "N";
        }
    }
}
