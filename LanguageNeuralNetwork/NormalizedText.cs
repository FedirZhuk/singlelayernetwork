namespace LanguageNeuralNetwork;

public class NormalizedText
{
    private string _language;
    private List<double> _proportions = new List<double>();

        public string Language
        {
            get => _language;
            set => _language = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<double> Proportions
        {
            get => _proportions;
            set => _proportions = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void SetUpProportions(string path, string language)
        {
            _language = language;
            List<char> lettersList = new List<char>();
            string text = File.ReadAllText(path);
            foreach (var symbol in text)
            {
                char letter = Char.ToUpper(symbol);
                if (letter >= 65 && letter <= 90)
                {
                    lettersList.Add(letter);
                }
            }

            AddToProportionList(lettersList);
            
            //Printing 
            // string tmp = "[";
            // foreach (var VARIABLE in _proportions)
            // {
            //     tmp += VARIABLE + ", ";
            // }
            // tmp = tmp.Remove(tmp.Length-2, 2);
            // tmp += "]";
            // Console.Out.WriteLine(tmp);
        }

        public void SetUpProportions(string input)
        {
            _language = "None";
            List<char> lettersList = new List<char>();
            foreach (var symbol in input)
            {
                char letter = Char.ToUpper(symbol);
                if (letter >= 65 && letter <= 90)
                {
                    lettersList.Add(letter);
                }
            }

            AddToProportionList(lettersList);
        }

        public void AddToProportionList(List<char> lettersList)
        {
            for (int i = 60; i < 91; i++)
            {
                _proportions.Add((double)lettersList.Count(c => c == (char)i) / (double)lettersList.Count);
            }
        }
}