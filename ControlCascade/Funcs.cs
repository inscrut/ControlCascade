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
        internal static int execBat(string bat, string arg)
        {
            Process exe = new Process();

            exe.StartInfo.FileName = bat;
            exe.StartInfo.Arguments = arg;
            exe.StartInfo.UseShellExecute = false;
            exe.StartInfo.RedirectStandardOutput = true;
            exe.Start();

            using (var stream = exe.StandardOutput.BaseStream)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine(sr.ReadLine());
                    }
                    sr.Close();
                }
                stream.Close();
            }
            exe.WaitForExit();

            return exe.ExitCode;
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
            XmlNode subElement = doc.CreateElement("sub");
            XmlAttribute attr = doc.CreateAttribute("file");
            attr.Value = "make_neg.bat";

            element.Attributes.Append(attr);
            doc.DocumentElement.AppendChild(element);

            element.AppendChild(node(ref doc, ref subElement, ref attr, "weight", "-w", "100"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "heigth", "-h", "100"));

            subElement = doc.CreateElement("args");
            subElement.InnerText = "-mode 1 -outfile Bad.txt -outdir Bad";
            element.AppendChild(subElement);
            #endregion

            #region positive
            element = doc.CreateElement("positive");
            attr = doc.CreateAttribute("file");
            attr.Value = "make_pos.bat";
            element.Attributes.Append(attr);
            doc.DocumentElement.AppendChild(element);

            element.AppendChild(node(ref doc, ref subElement, ref attr, "img", "-img", "Good/target.png"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "weight", "-w", "24"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "heigth", "-h", "24"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "num", "-num", "2000"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "maxxangle", "-maxxangle", "0.1"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "maxyangle", "-maxyangle", "0.1"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "maxzangle", "-maxzangle", "0.1"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "bgcolor", "-bgcolor", "0"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "bgthresh", "-bgthresh", "0"));

            subElement = doc.CreateElement("args");
            subElement.InnerText = "-bg Bad.txt -vec Good.vec";
            element.AppendChild(subElement);
            #endregion

            #region cascade
            element = doc.CreateElement("cascade");
            attr = doc.CreateAttribute("file");
            attr.Value = "make_cas.bat";
            element.Attributes.Append(attr);
            doc.DocumentElement.AppendChild(element);

            element.AppendChild(node(ref doc, ref subElement, ref attr, "weight", "-w", "24"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "heigth", "-h", "24"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "pos", "-numPos", "500"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "neg", "-numNeg", "500"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "stages", "-numStages", "27"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "hitrate", "-minHitRate", "0.999"));
            element.AppendChild(node(ref doc, ref subElement, ref attr, "alarmrate", "-maxFalseAlarmRate", "0.5"));

            subElement = doc.CreateElement("args");
            subElement.InnerText = "-data Result -vec Good.vec -bg Bad.txt -numThreads 4 -precalcValBufSize 1024 -precalcIdxBufSize 1024 -mode ALL -stageType BOOST -featureType HAAR";
            element.AppendChild(subElement);
            #endregion

            doc.Save("config.xml");
            doc = null;
        }

        private static XmlNode node(ref XmlDocument _doc, ref XmlNode _subElement, ref XmlAttribute _attr, string _nodename, string _cmd, string _value)
        {
            _subElement = _doc.CreateElement(_nodename);
            _attr = _doc.CreateAttribute("cmd");
            _subElement.InnerText = _value;
            _attr.Value = _cmd;
            _subElement.Attributes.Append(_attr);

            return _subElement;
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

        internal static string getArgs(XmlNode el)
        {
            string res = "";
            foreach (XmlNode item in el.ChildNodes)
            {
                try
                {
                    res += item.Attributes.GetNamedItem("cmd").Value + " ";
                }
                catch { }
                res += item.InnerText + " ";
            }

            return res.Substring(0, res.Length - 1);
        }
    }
}
