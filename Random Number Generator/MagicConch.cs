using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using System.Diagnostics;

namespace Magic_Conch
{
    public static class MagicConch
    {
        public static string ScrapeHTML(string address = "https://playoverwatch.com/en-us/career/pc/DinoNugetDan-1257")
        {
            WebClient webClient = new WebClient();
            try
            {
                string downloadResult = webClient.DownloadString(address);

                return downloadResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return String.Empty;
        }

        public static string[] FindElements(string html, string element = "svg")
        {
            string[] raw = html.Split('<');
            List<string> result = new List<string>();
            int index = 0;
            foreach (string s in raw)
            {
                if(s.Contains(element))
                {
                    result.Add(s);
                    index++;
                }
            }           
            return result.ToArray();
        }

        public static string[] IsolateElements(string[] elements, string atrribute = "data-total")
        {
            List<string> result = new List<string>();
            int index = 0;
            foreach(string s in elements)
            {
                if (s == null)
                    continue;

                if(s.Contains(atrribute))
                {
                    result.Add(s);
                    index++;
                }
            }

            for(int i = 0; i < result.Count; i++)
            {
                string[] temp = result[i].Split(' ');
                foreach(string s in temp)
                {
                    if(s.Contains(atrribute))
                    {
                        result[i] = s;
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("IsoAttribuutes.txt"))
            {
                foreach (string s in result)
                {
                    sw.WriteLine(s);
                }
            }

            return result.ToArray();
        }

        public static int GetAttributeValue(string attribute)
        {
            string[] temp = attribute.Split("'".ToCharArray());
            Int32.TryParse(temp[1], out int result);
            Console.WriteLine(result);
            return result;
        }
    }
}