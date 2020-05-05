using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelDB;
using Newtonsoft.Json;

namespace LevelDBParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = args[0];
            string outputDir = args[1];

            Console.WriteLine("INFO: Parsing Database");

            var options = new Options { CreateIfMissing = false }; //have to add false to make it work
            var db = new DB(options, path);
            var kvp = from kv in db as IEnumerable<KeyValuePair<byte[], byte[]>> select kv;
            List<KeyValuePair<byte[], byte[]>> kvpList = kvp.ToList();
            var dict = new Dictionary<string, string>();

            string longString = "";
            foreach (KeyValuePair<byte[], byte[]> pair in kvpList)
            {
                dict.Add(System.Text.Encoding.UTF8.GetString(pair.Key), System.Text.Encoding.UTF8.GetString(pair.Value));
            }
            
            longString += "<html>";
            longString += "<style>\r\ntable, th, td {\r\n  border: 1px solid black;\r\n}\r\n</style>";
            longString += "<table>";
            longString += "<th>Key</th><th>Value</th>";

            foreach (var pair in dict)
            {

                longString += "<tr>";
                longString += "<td>" + pair.Key + "</td><td>" + pair.Value + "</td>";
                longString += "</tr>";
                //WriteToFile(outputDir, longString);
            }
            longString += "</html>";
            longString += "</table>";

            WriteToFile(outputDir, longString);
            Console.WriteLine("INFO: Process Completed. Press an key to exit.");
            Console.ReadKey();
        }

        public static void WriteToFile(string fileName, string textToWrite)
        {
            File.AppendAllText(fileName + "\\outputHtml.html",
                textToWrite + Environment.NewLine);
        }
    }
}
