using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class InsultEventArgs : EventArgs
    {
        public readonly string awnser;
        private string[] responses = { "Everyone has to deal with some shit in their lives. But you deserve it.", "You're a load that should have been swallowed.",
            "Do you use your personality as a contraceptive?", "Somewhere out there, there is a tree producing oxygen just so you can breath. Go and apologise to it.",
            "The moment you were being born, did someone passing by say 'Hey look, there's a cunt coming out of that cunt's cunt'.",
            "I'd call you a cunt, but you lack both the depth and the warmth.", "I'm not mad, I'm just....disappointed", "This is why people talk about you when you're not around.",
            "You're the worst mistake your mother never swallowed.", "You're a failed abortion whose birth certificate is an apology from the condom factory",
            "If I had a gun with two bullets and I was in room with you Hitler and Ted Bundy, I'd shoot you twice in the head.", "You must not have any friends. Which is a surprise as you have enough extra chromosomes to make one.",
            "You're like Rapunzel, except that instead of letting your hair down, you just let down everyone you know.", "I envy people who have never met you.", "You suck dick for beer money and you don't even drink beer"};

        public InsultEventArgs()
        {
            awnser = PickAwnser();
        }

        private string PickAwnser()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < responses.Length; i++)
            {
                list.Add(0);
            }

            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 100000; i++)
            {
                int tempI = r.Next() % responses.Length;
                list[tempI]++;
            }

            int winner = 0;
            int wIndex = 1000;
            for (int i = 0; i < responses.Length; i++)
            {
                if (list[i] > winner)
                {
                    winner = list[i];
                    wIndex = i;
                }

            }

            if (wIndex == (responses.Length)) wIndex--;

            return responses[wIndex];
        }
    }
}
