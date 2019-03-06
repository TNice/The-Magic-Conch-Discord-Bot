using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class ChoiceEventArgs : EventArgs
    {
        public readonly string question;
        public readonly string awnser;
        public readonly string[] choices;

        public ChoiceEventArgs(string question, string awnser, string[] choices)
        {
            this.question = question;
            this.awnser = awnser;
            this.choices = choices;
        }
    }
}
