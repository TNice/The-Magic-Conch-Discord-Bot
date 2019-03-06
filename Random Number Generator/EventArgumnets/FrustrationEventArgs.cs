using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class FrustrationEventArgs : EventArgs
    {
        public readonly string question;
        public readonly string awnser;
        private string[] possibleAwnsers = { "Fuck You!", "You Just Asked That Question!", "Fuck You Think I Am? Some Sort Of Magic Conch?", "Hop Off Me Dick!",
            "Hop On Me Dick!", "Ill Awnser For Some Sucky Sucky", "Fuck If I Care!", "That awnser will cost you.", "Fuck Off!"};

        public FrustrationEventArgs(string question)
        {
            this.question = question;
            awnser = PickAwnser();
        }

        private string PickAwnser()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < possibleAwnsers.Length; i++)
            {
                list.Add(0);
            }

            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 100000; i++)
            {
                int tempI = r.Next() % possibleAwnsers.Length;
                list[tempI]++;
            }

            int winner = 0;
            int wIndex = 1000;
            for (int i = 0; i < possibleAwnsers.Length; i++)
            {
                if (list[i] > winner)
                {
                    winner = list[i];
                    wIndex = i;
                }

            }

            if (wIndex == (possibleAwnsers.Length)) wIndex--;

            return possibleAwnsers[wIndex];
        }
    }
}
