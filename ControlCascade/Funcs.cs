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
    internal static class Funcs
    {
        internal static void execBat(string bat, string arg)
        {
            Process exe = new Process();
            exe.StartInfo.FileName = bat;
            exe.StartInfo.Arguments = arg;
            exe.StartInfo.UseShellExecute = false;
            exe.StartInfo.RedirectStandardOutput = true;
            exe.Start();

            Console.WriteLine(exe.StandardOutput.ReadToEnd());

            exe.WaitForExit();
        }

        internal static void check() {
            if (!File.Exists("config.xml")) { Console.WriteLine("ERR: config.xml not found!\r\nMaking default and exit..."); makeDefXml(); Environment.Exit(1); }
            
            var conf = XDocument.Parse(File.ReadAllText("config.xml"));
            if (!File.Exists(conf.Root.Element("negative").Attribute("file").Value)) { Console.WriteLine("ERR: file {0} not found!", conf.Root.Element("negative").Attribute("file").Value); Environment.Exit(1); }
            else if (!File.Exists(conf.Root.Element("positive").Attribute("file").Value)) { Console.WriteLine("ERR: file {0} not found!", conf.Root.Element("positive").Attribute("file").Value); Environment.Exit(1); }
            else if (!File.Exists(conf.Root.Element("cascade").Attribute("file").Value)) { Console.WriteLine("ERR: file {0} not found!", conf.Root.Element("cascade").Attribute("file").Value); Environment.Exit(1); }
            else
                Console.WriteLine("Check success!");
        }

        internal static void makeDefXml()
        {
            using (XmlTextWriter textWritter = new XmlTextWriter("config.xml", Encoding.UTF8))
            {
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("config");
                textWritter.WriteEndElement();
                textWritter.Close();
            }
            XmlDocument doc = new XmlDocument();
            doc.Load("config.xml");

            #region negative
            XmlNode element = doc.CreateElement("negative");
            doc.DocumentElement.AppendChild(element);

            XmlAttribute attr = doc.CreateAttribute("file");
            attr.Value = "make_neg.bat";

            element.Attributes.Append(attr);

            XmlNode subElement = doc.CreateElement("weight");
            subElement.InnerText = "128";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("heigth");
            subElement.InnerText = "128";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("args");
            subElement.InnerText = "-mode 1 -outfile Bad.txt -outdir Bad";
            element.AppendChild(subElement);
            #endregion

            #region positive
            element = doc.CreateElement("positive");
            doc.DocumentElement.AppendChild(element);

            attr = doc.CreateAttribute("file");
            attr.Value = "make_pos.bat";

            element.Attributes.Append(attr);

            subElement = doc.CreateElement("weight");
            subElement.InnerText = "24";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("heigth");
            subElement.InnerText = "24";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("num");
            subElement.InnerText = "2000";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("maxxangle");
            subElement.InnerText = "0.3";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("maxyangle");
            subElement.InnerText = "0.0";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("maxzangle");
            subElement.InnerText = "0.0";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("bgcolor");
            subElement.InnerText = "0";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("bgthresh");
            subElement.InnerText = "0";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("args");
            subElement.InnerText = "-img Good/cur1.png -bg Bad.txt -vec Good.vec";
            element.AppendChild(subElement);
            #endregion

            #region cascade
            element = doc.CreateElement("cascade");
            doc.DocumentElement.AppendChild(element);

            attr = doc.CreateAttribute("file");
            attr.Value = "make_cas.bat";

            element.Attributes.Append(attr);

            subElement = doc.CreateElement("weight");
            subElement.InnerText = "24";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("heigth");
            subElement.InnerText = "24";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("pos");
            subElement.InnerText = "1000";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("neg");
            subElement.InnerText = "500";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("stages");
            subElement.InnerText = "7";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("hitrate");
            subElement.InnerText = "0.95";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("alarmrate");
            subElement.InnerText = "0.5";
            element.AppendChild(subElement);

            subElement = doc.CreateElement("args");
            subElement.InnerText = "-data Result -vec Good.vec -bg Bad.txt -numThreads 4 -precalcValBufSize 1024 -precalcIdxBufSize 1024 -mode ALL -stageType BOOST -featureType HAAR";
            element.AppendChild(subElement);
            #endregion

            doc.Save("config.xml");
            doc = null;
        }

        internal static bool quest(string q)
        {
            ConsoleKey buf;

            for (;;)
            {
                Console.Write(q+ " (y/n): ");
                buf = Console.ReadKey(false).Key;
                if (buf == ConsoleKey.Y) { Console.WriteLine(); return true; }
                else if (buf == ConsoleKey.N) { Console.WriteLine(); return false; }
                else Console.WriteLine();
            }
        }
    }
}
