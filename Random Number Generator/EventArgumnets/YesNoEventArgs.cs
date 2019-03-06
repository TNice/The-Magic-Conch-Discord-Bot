using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class YesNoEventArgs : EventArgs
    {
        public readonly string question;
        public readonly string awnser;

        public YesNoEventArgs(string question, string awnser)
        {
            this.question = question;
            this.awnser = awnser;
        }
    }
}
