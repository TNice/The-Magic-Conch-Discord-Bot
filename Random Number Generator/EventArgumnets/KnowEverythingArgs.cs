using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class KnowEverythingArgs : EventArgs
    {
        public readonly string awsner = "Of Course!";
        public readonly string question;

        public KnowEverythingArgs(string question)
        {
            this.question = question;
        }
    }
}
