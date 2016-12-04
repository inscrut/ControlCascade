using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ControlCascade
{
    class Program
    {
        static void Main(string[] args)
        {
            //Funcs.check();

            Console.CancelKeyPress += Console_CancelKeyPress;

            XmlDocument xml = new XmlDocument();
            while (true)
            {
                xml.Load("config.xml");
                Console.Clear();
                Console.WriteLine("Press Ctrl+C for exit.");
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
                if (Funcs.quest("Start exec?"))
                {
                    var conf = XDocument.Parse(File.ReadAllText("config.xml"));
                    
                    Funcs.execBat(conf.Root.Element("negative").Attribute("file").Value, Funcs.getArgs(conf.Root.Element("negative")));
                    Funcs.execBat(conf.Root.Element("positive").Attribute("file").Value, Funcs.getArgs(conf.Root.Element("positive")));
                    Funcs.execBat(conf.Root.Element("cascade").Attribute("file").Value, Funcs.getArgs(conf.Root.Element("cascade")));

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
