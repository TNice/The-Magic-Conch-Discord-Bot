using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class UnknownEventArgs : EventArgs
    {
        public readonly string message;
        public readonly string question;

        public UnknownEventArgs(string question, string message = "I don't know the awnser")
        {
            this.message = message;
            this.question = question;
        }
    }
}
