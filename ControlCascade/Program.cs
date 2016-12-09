using System;
using System.Xml;

namespace ControlCascade
{
    class Program
    {
        static void Main(string[] args)
        {
            Funcs.check();

            Console.CancelKeyPress += Console_CancelKeyPress;

            XmlDocument xml = new XmlDocument();
            while (true)
            {
                xml.Load("config.xml");
                Console.Clear();
                Console.WriteLine("Press Ctrl+C for exit.\r\n");

                Console.WriteLine("Choose work mode:");
                Console.WriteLine("1) Change config file\r\n2) Training");
                Console.Write("Set: ");

                ConsoleKey ch = Console.ReadKey(false).Key;

                Console.WriteLine("\r\n");

                if(ch == ConsoleKey.D1)
                {
                    foreach (XmlElement conf in xml.DocumentElement)
                    {
                        if (Funcs.quest("Change params for " + conf.Name + " script?"))
                        {
                            foreach (XmlElement item in conf)
                            {
                                if (item.Name == "args") continue;
                                Console.Write("{0} ({1}): ", item.Name, item.InnerText);
                                string line = Console.ReadLine();
                                if (line != "") item.InnerText = line;
                            }
                        }
                    }

                    if (Funcs.quest("Save config?")) xml.Save("config.xml");
                }
                else if(ch == ConsoleKey.D2)
                {
                    if (Funcs.quest("Change params for \"negative\" script?"))
                    {
                        foreach (XmlNode item in xml.DocumentElement.GetElementsByTagName("negative")[0])
                        {
                            if (item.Name == "args") continue;
                            Console.Write("{0} ({1}): ", item.Name, item.InnerText);
                            string line = Console.ReadLine();
                            if (line != "") item.InnerText = line;
                        }
                    }
                    if (Funcs.quest("Exec the \"negative\" programm?"))
                        Console.WriteLine("\r\nExit with code {0}\r\n", Funcs.execBat(xml.DocumentElement.GetElementsByTagName("negative").Item(0).Attributes.GetNamedItem("file").Value, Funcs.getArgs(xml.DocumentElement.GetElementsByTagName("negative").Item(0))));

                    if (Funcs.quest("Change params for \"positive\" script?"))
                    {
                        foreach (XmlNode item in xml.DocumentElement.GetElementsByTagName("positive")[0])
                        {
                            if (item.Name == "args") continue;
                            Console.Write("{0} ({1}): ", item.Name, item.InnerText);
                            string line = Console.ReadLine();
                            if (line != "") item.InnerText = line;
                        }
                    }
                    if (Funcs.quest("Exec the \"positive\" programm?"))
                        Console.WriteLine("\r\nExit with code {0}\r\n", Funcs.execBat(xml.DocumentElement.GetElementsByTagName("positive").Item(0).Attributes.GetNamedItem("file").Value, Funcs.getArgs(xml.DocumentElement.GetElementsByTagName("positive").Item(0))));

                    if (Funcs.quest("Change params for \"cascade\" script?"))
                    {
                        foreach (XmlNode item in xml.DocumentElement.GetElementsByTagName("cascade")[0])
                        {
                            if (item.Name == "args") continue;
                            Console.Write("{0} ({1}): ", item.Name, item.InnerText);
                            string line = Console.ReadLine();
                            if (line != "") item.InnerText = line;
                        }
                    }
                    if (Funcs.quest("Exec the \"cascade\" programm?"))
                        Console.WriteLine("\r\nExit with code {0}\r\n", Funcs.execBat(xml.DocumentElement.GetElementsByTagName("cascade").Item(0).Attributes.GetNamedItem("file").Value, Funcs.getArgs(xml.DocumentElement.GetElementsByTagName("cascade").Item(0))));

                }
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("\r\nExit...");
            Environment.Exit(0);
        }
    }
}
