using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class QuestionAwnseredEventArgs : EventArgs
    {
        public readonly string question;
        public readonly string awnser;

        public QuestionAwnseredEventArgs(string question, string awnser)
        {
            this.question = question;
            this.awnser = awnser;
        }

    }
}
