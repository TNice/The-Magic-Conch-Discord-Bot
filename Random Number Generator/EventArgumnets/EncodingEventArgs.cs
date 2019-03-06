using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class EncodingEventArgs
    {

        public string message;
        public string key;

        public EncodingEventArgs(string input)
        {
            string temp = Encode(input);

            message = temp;
        }

        string Encode(string input)
        {
            StringBuilder sb = new StringBuilder();
            char[] c = input.ToCharArray();

            return CharArrayToString(EncodeString(c));
        }

        static string CharArrayToString(char[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in array)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        static char[] EncodeString(char[] message)
        {
            return new char[0];
        }

    }
}
