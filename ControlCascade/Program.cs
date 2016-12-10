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
                Console.WriteLine("1) Change config file\r\n2) All\r\n3) Only cascade\r\n4) Make stages\r\n5) Exit\r\n");
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
                    {
                        if (Funcs.quest("Clear negatives dir?")) Funcs.clearDir("Bad");
                        int ec = Funcs.execBat(xml.DocumentElement.GetElementsByTagName("negative").Item(0).Attributes.GetNamedItem("file").Value, Funcs.getArgs(xml.DocumentElement.GetElementsByTagName("negative").Item(0)));
                        Console.WriteLine("\r\nExit with code {0}\r\n", ec);
                        if (ec != 0) if (Funcs.quest("Show log?")) Funcs.execBat("notepad.exe", xml.DocumentElement.GetElementsByTagName("negative").Item(0).Attributes.GetNamedItem("file").Value);
                    }

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
                    {
                        int ec = Funcs.execBat(xml.DocumentElement.GetElementsByTagName("positive").Item(0).Attributes.GetNamedItem("file").Value, Funcs.getArgs(xml.DocumentElement.GetElementsByTagName("positive").Item(0)));
                        Console.WriteLine("\r\nExit with code {0}\r\n", ec);
                        if (ec != 0) if (Funcs.quest("Show log?")) Funcs.execBat("notepad.exe", xml.DocumentElement.GetElementsByTagName("positive").Item(0).Attributes.GetNamedItem("file").Value);
                    }

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
                    {
                        if (Funcs.quest("Clear cascades dir?")) Funcs.clearDir("Result");
                        int ec = Funcs.execBat(xml.DocumentElement.GetElementsByTagName("cascade").Item(0).Attributes.GetNamedItem("file").Value, Funcs.getArgs(xml.DocumentElement.GetElementsByTagName("cascade").Item(0)));
                        Console.WriteLine("\r\nExit with code {0}\r\n", ec);
                        if (ec != 0) if (Funcs.quest("Show log?")) Funcs.execBat("notepad.exe", xml.DocumentElement.GetElementsByTagName("cascade").Item(0).Attributes.GetNamedItem("file").Value);
                    }
                }
                else if(ch == ConsoleKey.D3)
                {
                    while (true)
                    {
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
                        {
                            if (Funcs.quest("Clear cascades dir?")) Funcs.clearDir("Result");
                            int ec = Funcs.execBat(xml.DocumentElement.GetElementsByTagName("cascade").Item(0).Attributes.GetNamedItem("file").Value, Funcs.getArgs(xml.DocumentElement.GetElementsByTagName("cascade").Item(0)));
                            Console.WriteLine("\r\nExit with code {0}\r\n", ec);
                            if (ec != 0) if (Funcs.quest("Show log?")) Funcs.execBat("notepad.exe", xml.DocumentElement.GetElementsByTagName("cascade").Item(0).Attributes.GetNamedItem("file").Value);
                        }

                        if (!Funcs.quest("Repeat?")) break;
                    }
                }
                else if(ch == ConsoleKey.D4)
                {
                    while (true)
                    {
                        Console.Write("Enter num stages: ");
                        Console.WriteLine("\r\nExit with code {0}\r\n", Funcs.execBat(xml.DocumentElement.GetElementsByTagName("cascade").Item(0).Attributes.GetNamedItem("file").Value, "-data Result -vec Good.vec -bg Bad.txt -numStages " + Console.ReadLine()));
                        if (!Funcs.quest("Repeat?")) break;
                    }
                }
                else if (ch == ConsoleKey.D5)
                {
                    Console.WriteLine("Exit...");
                    Environment.Exit(0);
                }
                else
                    continue;

                Console.WriteLine("Press any key for continue...");
                Console.ReadKey();
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("\r\nExit...");
            Environment.Exit(0);
        }
    }
}
