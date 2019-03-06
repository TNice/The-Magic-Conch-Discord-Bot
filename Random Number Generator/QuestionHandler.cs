using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Conch
{
    class QuestionHandler
    {
        public delegate void QuestionAwnseredEventHandeler(object sender, QuestionAwnseredEventArgs eventArgs);
        public event QuestionAwnseredEventHandeler QuestionAwnsered;

        public delegate void YesNoQuestionEventHandeler(object sender, YesNoEventArgs eventArgs);
        public event YesNoQuestionEventHandeler YesNoQuestion;

        public delegate void ChoiceQuestionEventHandeler(object sender, ChoiceEventArgs eventArgs);
        public event ChoiceQuestionEventHandeler ChoiceAwnsered;

        public delegate void KnowEveryThingQuestionHandeler(object sender, KnowEverythingArgs eventArgs);
        public event KnowEveryThingQuestionHandeler KnowEverythingAsked;

        public delegate void FavoriteQuestionEventHandeler(object sender, QuestionAwnseredEventArgs eventArgs);
        public event FavoriteQuestionEventHandeler FavoriteAwnsered;

        public delegate void FrustationEventHandeler(object sender, FrustrationEventArgs eventArgs);
        public event FrustationEventHandeler Frustrated;

        public delegate void RemeberEventHandler(object sender, QuestionAwnseredEventArgs eventArgs);
        public event RemeberEventHandler RememberSomething;

        public delegate void InsultEventHandler(object sender, InsultEventArgs eventArgs);
        public event InsultEventHandler Insulted;

        public delegate void UnknownQuestionEventHandeler(object sender, UnknownEventArgs eventArgs);
        public event UnknownQuestionEventHandeler UnknownQuestion;

        public delegate void EncodingEventHandler(object sender, EncodingEventArgs args);
        public event EncodingEventHandler EncodedMessage;

        private List<string> question;

        private string lastQuestion = String.Empty;
        private int timesQuestionAsked = 0;
        public readonly int frustationThreshold = 200000000;

        private string[] yesNoType = {
            "should", "could", "is", "was", "have", "are", "will", "can", "am"};
        private string[] choiceType = {
            "should", "is", "was"};
        private string[] knowEverytingType = {
            "you", "know", "everything"};

        private string[] aboutType = {
            "tell me about you", "tell me about the magic conch", "inform me", "enlighten me"
        };
        private string aboutAwnser = $"'''I am the magical conch. I am all knowing and all powerfull. I am nothing less than a deity and you are to treat me as such." +
            $" \nMy favorite form of worship is the statemet \"All Hail The Magic Conch\" '''";
        private string[] insultType = {
            "fuck", "cunt", "nigg", "asshole", "idiot", "stupid", "dumb", "ass", "retard", "bitch", "dingus", "wrong", "dunce", "slut", "whore", "pussy"};

        private string[] yesNoAwnsers = {
            "yes", "no", "nigga please"};
        private int[] yesNoWeights = { 450, 900, 1000 };
        private string[] awnserModifiers = {
            "hell ", "fuck ", ""};
        private int[] modifiersWeights = { 200, 400, 1000 };

        public List<string> randomShit = new List<string>();
        public List<string> knownColors = new List<string>();


        //accessor to know what list to use apon request
        public Dictionary<string, List<string>> shitIKnow = new Dictionary<string, List<string>>();
        public Dictionary<string, string> favoriteShit = new Dictionary<string, string>();

        public QuestionHandler()
        {
            LoadShit();
        }

        public QuestionHandler(string question)
        {
            LoadShit();
            Question = question;
        }

        public QuestionHandler(string question, int frustationThreshold)
        {
            LoadShit();
            Question = question;
            this.frustationThreshold = frustationThreshold;
        }

        public string Question
        {
            set
            {
                if (lastQuestion.Contains(value))
                {
                    timesQuestionAsked++;

                }
                else
                {
                    lastQuestion = value;
                    timesQuestionAsked = 1;
                }
                question = value.ToLower().Split(' ').ToList();
            }
        }

        private void LoadShit()
        {
            Deserialize(ref knownColors, "colors.bin");
            Deserialize(ref randomShit, "randomShit.bin");

            shitIKnow.Add("color", knownColors);
            favoriteShit.Add("color", PickFavoriteShit("color"));

            shitIKnow.Add("fact", randomShit);
            favoriteShit.Add("fact", PickFavoriteShit("fact"));
        }

        private void HandleQuesiton()
        {
            if (question == null)
            {
                UnknownQuestion?.Invoke(this, new UnknownEventArgs("NONE", "No Question Given"));
            }

            string localQuestion = string.Empty;
            string awnser = string.Empty;
            for (int i = 0; i < question.Count; i++)
            {
                localQuestion += $"{question[i]} ";
            }

            if (timesQuestionAsked >= frustationThreshold && frustationThreshold != 0)
            {
                Frustrated?.Invoke(this, new FrustrationEventArgs(localQuestion));
                return;
            }

            if (IsKnowEverythinQuestion())
            {
                KnowEverythingAsked?.Invoke(this, new KnowEverythingArgs(localQuestion));
            }
            else if (IsAbout())
            {
                awnser = aboutAwnser;
            }
            else if (IsRemember())
            {
                awnser = Remember();
                RememberSomething?.Invoke(this, new QuestionAwnseredEventArgs(localQuestion, awnser));
            }
            else if (IsYesNo())
            {
                awnser = AwnserYesNo();
                YesNoQuestion?.Invoke(this, new YesNoEventArgs(localQuestion, awnser));
            }
            else if (IsChoice())
            {
                string[] choices = GetChoices();
                awnser = AwnserChoice(choices);
                ChoiceAwnsered?.Invoke(this, new ChoiceEventArgs(localQuestion, awnser, choices));
            }
            else if (IsFavorite())
            {
                awnser = FavoriteQuestion();
                FavoriteAwnsered?.Invoke(this, new QuestionAwnseredEventArgs(localQuestion, awnser));
            }
            else if (IsEncode())
            {
                localQuestion = localQuestion.Remove(0, 7);
                EncodedMessage?.Invoke(this, new EncodingEventArgs(localQuestion));
                return;
            }
            else if (IsInsult())
            {
                Insulted?.Invoke(this, new InsultEventArgs());
            }       
            else
            {
                UnknownQuestion?.Invoke(this, new UnknownEventArgs(localQuestion));
            }

            QuestionAwnsered?.Invoke(this, new QuestionAwnseredEventArgs(localQuestion, awnser));

        }

        public void HandleQuesiton(string question)
        {
            Question = question;
            HandleQuesiton();
        }

        public bool HasQuestion()
        {
            if (question == null)
                return false;
            else
                return true;
        }

        private bool IsAbout()
        {
            for (int i = 0; i < aboutType.Length; i++)
            {
                if (lastQuestion.Contains(aboutType[i]))
                    return true;
            }

            return false;
        }

        private bool IsInsult()
        {
            for (int i = 0; i < insultType.Length; i++)
            {
                if (lastQuestion.Contains(insultType[i]))
                    return true;
            }

            return false;
        }

        private bool IsKnowEverythinQuestion()
        {
            for (int i = 0; i < knowEverytingType.Length; i++)
            {
                if (!question.Contains(knowEverytingType[i]))
                    return false;
            }

            return true;
        }

        private bool IsYesNo()
        {
            for (int i = 0; i < yesNoType.Length; i++)
            {
                if (question[0].Contains(yesNoType[i]))
                {
                    for (int j = 0; j < choiceType.Length; j++)
                    {
                        if ((yesNoType[i] == choiceType[j]) && question.Contains("or"))
                            return false;
                    }

                    return true;
                }
            }
            return false;
        }

        private string AwnserYesNo()
        {
            StringBuilder awnser = new StringBuilder();

            string response = WeightedChoice(yesNoAwnsers, yesNoWeights);
            awnser.Append(response);

            if (response != yesNoAwnsers[2])
            {
                string modifier = WeightedChoice(awnserModifiers, modifiersWeights);

                awnser.Insert(0, modifier);

                if (modifier == awnserModifiers[1] && response == yesNoAwnsers[1])
                {
                    response.Replace($"{modifier}no", $"{modifier}nah");
                }

            }

            return awnser.ToString();
        }

        private bool IsChoice()
        {
            if (question.Contains("or"))
                return true;

            return false;
        }

        private bool IsEncode()
        {
            if (question[0] == "encode" || question[0] == "encrypt")
                return true;
            else
                return false;
        }

        private string[] GetChoices()
        {
            question.RemoveRange(0, 2);

            List<string> options = new List<string>();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < question.Count; i++)
            {
                if (question[i] == "or")
                {
                    continue;
                }
                else if (question[i].Contains(','))
                {
                    question[i] = question[i].Remove(question[i].Length - 1);

                    sb.Append(question[i]);
                    options.Add(sb.ToString());
          
                    sb.Clear();
                }
                else
                {
                    if (question[i] == "")
                        continue;

                    if(question[i].Contains("?"))
                        question[i] = question[i].Remove(question[i].Length - 1);

                    sb.Append(question[i] + " ");
                }
            }

            options.Add(sb.ToString());

            return options.ToArray();
        }

        private string AwnserChoice(string[] choices)
        {
            if (IsAwnserYes())
            {
                return "yes";
            }

            List<int> list = new List<int>();
            for (int i = 0; i < choices.Length; i++)
            {
                list.Add(0);
            }

            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 100000; i++)
            {
                int tempI = r.Next() % choices.Length;
                list[tempI]++;
            }

            int winner = 0;
            int wIndex = 1000;
            for (int i = 0; i < choices.Length; i++)
            {
                if (list[i] > winner)
                {
                    winner = list[i];
                    wIndex = i;
                }

            }

            if (wIndex == (choices.Length)) wIndex--;

            return choices[wIndex];
        }

        private string WeightedChoice(string[] options, int[] weights)
        {

            Random r = new Random();

            int choice = r.Next(0, 1000);
            int wIndex = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                if (choice <= weights[i])
                {
                    wIndex = i;
                    break;
                }
            }

            return $"{options[wIndex]}";

        }

        private bool IsFavorite()
        {
            if (question.Contains("your") && question.Contains("favorite"))
            {
                return true;
            }

            return false;
        }

        private string FavoriteQuestion()
        {
            int index = question.IndexOf("favorite");
            string objectWanted = question[++index];

            if (!favoriteShit.ContainsKey(objectWanted))
            {
                string eventMessage = "I don't have a favorite " + objectWanted + ". I only have a favorite";
                foreach (string s in shitIKnow.Keys)
                {
                    eventMessage += $" {s}";
                }

                UnknownQuestion?.Invoke(this, new UnknownEventArgs(lastQuestion, eventMessage));
                return string.Empty;
            }

            return favoriteShit[objectWanted];
        }

        private string PickFavoriteShit(string shit)
        {
            try
            {
                return AwnserChoice(shitIKnow[shit].ToArray());
            }
            catch
            {
                return "No";
            }
        }

        private bool IsRemember()
        {
            if (question.Contains("you") && question.Contains("remember") && question.Contains("that"))
            {
                return true;
            }

            return false;
        }

        private string Remember()
        {
            if (lastQuestion.Contains(" is a color"))
            {
                string color = question[question.IndexOf("that") + 1];

                if (knownColors.Contains(color))
                {
                    return "Thank you I already knew that!";
                }
                else
                {
                    knownColors.Add(color);
                    return "I will remember that!";
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                for (int i = question.IndexOf("that") + 1; i < question.Count; i++)
                {
                    string word = question[i];
                    if (word == "you" || word == "u")
                        word = "me";

                    if (i == (question.Count - 1))
                        sb.Append(word);
                    else
                        sb.Append(word + " ");
                }

                string shit = sb.ToString();

                if (randomShit.Contains(shit))
                {
                    return "Thank you I already knew that!";
                }
                else
                {
                    randomShit.Add(shit);
                    return "I will remember that!";
                }
            }
        }

        public void Save()
        {
            SerializeObject(knownColors, "colors.bin");
            SerializeObject(randomShit, "randomShit.bin");
        }

        private bool IsAwnserYes()
        {
            Random r = new Random();

            int num = r.Next(1, 1000);

            if (num < 700 && num > 550)
            {
                return true;
            }

            return false;
        }

        private void SerializeObject(List<string> list, string fileName)
        {
            try
            {
                using (Stream stream = File.OpenWrite(fileName))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, list);
                }
            }
            catch (IOException)
            {
            }
        }
        private void Deserialize(ref List<string> list, string fileName)
        {
            try
            {
                using (Stream stream = File.OpenRead(fileName))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    var newlist = (List<string>)bin.Deserialize(stream);
                    list = newlist;
                }
            }
            catch (IOException)
            {
            }
        }
    }

    //If saving and loading doesnt work use these.
    public static class Serializer
    {
        public static void SerializeObject(this List<string> list, string fileName)
        {
            try
            {
                using (Stream stream = File.OpenWrite(fileName))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, list);
                }
            }
            catch (IOException)
            {
            }
        }
        public static void Deserialize(ref List<string> list, string fileName)
        {
            try
            {
                using (Stream stream = File.OpenRead(fileName))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    var newlist = (List<string>)bin.Deserialize(stream);
                    list = newlist;
                }
            }
            catch (IOException)
            {
            }
        }
    }
}