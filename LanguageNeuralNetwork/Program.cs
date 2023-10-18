using System.Text.RegularExpressions;
using LanguageNeuralNetwork;

internal class Program
{
    private static List<NormalizedText> TrainList = new();
    private static List<NormalizedText> TestList = new();
    private static List<Perceptron> Perceptrons = new();

    public static void Main(string[] args)
    {
        double alpha = 0.5;
        
        //Transforming raw directory path into normalized vectors of proportions
        TransformRawToNormalized(Directory.GetDirectories("LanguagesForTrain"), true);
        TransformRawToNormalized(Directory.GetDirectories("LanguagesForTest"), false);

        //Creating n perceptrons, where n is number of directories(languages)
        foreach (var language in Directory.GetDirectories("LanguagesForTrain"))
        {
            List<double> weights = new List<double>();
            Random random = new Random();

            for (int i = 0; i < TrainList.ElementAt(0).Proportions.Count(); i++)
            {
                weights.Add(random.Next(0, 5));
            }

            Perceptron perceptron = new Perceptron(alpha, language.Remove(0, 18), random.Next(0, 3), weights);
            perceptron.Type = language.Remove(0, 18);
            Perceptrons.Add(perceptron);
        }

        //Printing info about perceptrons before training
        Console.Out.WriteLine("Before training: ");
        foreach (var perceptron in Perceptrons)
        {
            Console.Out.WriteLine(perceptron);
        }
        
        //Learning each perceptron
        foreach (var perceptron in Perceptrons)
        {
            if (perceptron.Ready)
            {
                continue;
            }
    
            TrainPerceptron(perceptron, TrainList);
        }

        //For each perceptron returning its best weights and bias
        foreach (var perceptron in Perceptrons)
        {
            perceptron.ReturnToBestPreset();
        }

        //Printing info about perceptrons after training
        Console.Out.WriteLine("\nAfter training: ");
        foreach (var perceptron in Perceptrons)
        {
            Console.Out.WriteLine(perceptron);
        }


        //Printing HEADER for table of results
        Console.Out.WriteLine("\n\tTest-set results:");
        Console.Out.Write("Text\\Language\t");
        foreach (var perceptron in Perceptrons)
        {
            Console.Out.Write(perceptron.Type + "\t");
        }
        Console.Out.WriteLine();
        
        //Final counting correct answers, making decisions and printing results
        int rightAnswers = 0;
        int incorrectAnswers = 0;
        int testnum = 0;
        foreach (var text in TestList)
        {
            testnum++;
            Console.Out.Write(text.Language + "\t\t");
            List<int> answers = new List<int>();
            foreach (var perceptron in Perceptrons)
            {
                int ans = perceptron.MakeDecision(perceptron.CalculateNet(text.Proportions));
                answers.Add(ans); }

            //If there are more than 1 perceptron with answer "1"
            if (answers.Sum() != 1)
            {
                double maxNet = 0;
                string activatedType = "";
                
                //Finding language of the perceptron with higest Net and making decision about answer
                foreach (var perceptron in Perceptrons)
                {
                    if (maxNet < perceptron.CalculateNet(text.Proportions))
                    {
                        maxNet = perceptron.CalculateNet(text.Proportions);
                        activatedType = perceptron.Type;
                    }
                }
                if (activatedType == text.Language)
                {
                    rightAnswers++;
                }
                if (activatedType != text.Language)
                {
                    incorrectAnswers++;
                }
                
                //Printing one line in table of results
                foreach (var perceptron in Perceptrons)
                {
                    if (perceptron.Type == activatedType)
                    {
                        Console.Out.Write(1 + "\t");
                    }
                    else
                    {
                        Console.Out.Write(0 + "\t");
                    }
                }
            }
            else
            {
                rightAnswers++;
                foreach (var perceptron in Perceptrons)
                {
                    Console.Out.Write(perceptron.MakeDecision(perceptron.CalculateNet(text.Proportions)) + "\t");
                }
            }
            
            Console.Out.WriteLine();
        }
        
        //Printing results of accuracy
        Console.Out.WriteLine();
        double finalAccuracy = (double)rightAnswers / ( double)TestList.Count * 100.0;
        Console.Out.WriteLine("Incorrect answers: " + incorrectAnswers + "/" + testnum + "\nAccuracy is: " + Math.Round(finalAccuracy, 2) + "%");
        Console.Out.WriteLine();

        
        //User text input section
        while (true)
        {
            Console.Out.WriteLine("Write here your text using one of the supported languages:");
            string input = Console.ReadLine();
            NormalizedText text = new NormalizedText();
            text.SetUpProportions(input);
            double maxNet = 0;
            string activatedType = "";
            foreach (var perceptron in Perceptrons)
            {
                if (maxNet < perceptron.CalculateNet(text.Proportions))
                {
                    maxNet = perceptron.CalculateNet(text.Proportions);
                    activatedType = perceptron.Type;
                }
            }
            Console.Out.WriteLine("Neural network decision is: " + activatedType);
            Console.Out.WriteLine();
        }
    }
    
    //=========================================HelpMethods=========================================
    
    public static void TransformRawToNormalized(string[] dir, bool isTrain)
    {
        foreach (var language in dir)
        {
            string[] files = Directory.GetFiles(language);
            foreach (var file in files)
            {
                NormalizedText text = new NormalizedText();
                string pattern = @"(?<=\/)[^\/]+(?=\/[^\/]+$)";
                Match match = Regex.Match(file, pattern);
                text.SetUpProportions(file, match.Groups[0].Value);
                if (isTrain)
                {
                    TrainList.Add(text);
                }
                else
                {
                    TestList.Add(text);
                }
                
            }
        }
    }
    
    private static void TrainPerceptron(Perceptron perceptron, List<NormalizedText> trainList)
    {
        for (int i = 0; i < 1000; i++)
        {
            List<NormalizedText> incorrect = new List<NormalizedText>();
            int accuracy = CalculateAccuracy(perceptron, trainList, incorrect);
        
            if (incorrect.Count == 0)
            {
                perceptron.SaveBestPreset(accuracy);
                perceptron.Ready = true;
                perceptron.Epochs = i;
                return;
            }
        
            if (accuracy > perceptron.BestAccuracy)
            {
                perceptron.SaveBestPreset(accuracy);
            }

            //Learning from all incorrect elements
            for (int j = 0; j < incorrect.Count; j++)
            {
                NormalizedText incorrectText = incorrect.ElementAt(j);
                int dec = perceptron.MakeDecision(perceptron.CalculateNet(incorrectText.Proportions));
                perceptron.Learn(incorrectText.Proportions, incorrectText.Language == perceptron.Type ? 1 : 0);
            }
        }
    }

    private static int CalculateAccuracy(Perceptron perceptron, List<NormalizedText> trainList, List<NormalizedText> incorrect)
    {
        int accuracy = 0;
    
        foreach (var text in trainList)
        {
            int dec = perceptron.MakeDecision(perceptron.CalculateNet(text.Proportions));
        
            if ((dec == 1 && text.Language == perceptron.Type) || (dec == 0 && text.Language != perceptron.Type))
            {
                accuracy++;
            }
            else
            {
                incorrect.Add(text);
            }
        }
    
        return accuracy;
    }
}