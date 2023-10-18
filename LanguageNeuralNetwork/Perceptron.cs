namespace LanguageNeuralNetwork;

public class Perceptron
{
    private double _alpha;
    private string _type;
    private List<double> _weights = new List<double>();
    private double _bias;
    
    private bool _isReady = false;
    private int _epochs = 0;
    private List<double> _BestWeightsVec = new List<double>();
    private double _BestBias;
    private int _BestAccuracy = 0;

    public Perceptron(double alpha, string type, double bias, List<double> weights)
    {
        _weights = weights;
        _alpha = alpha;
        _type = type;
        _bias = bias;
    }

    public double CalculateNet(List<double> Vector)
    {
        double net = 0;
        for (int i = 0; i < _weights.Count; i++)
        {
            net += _weights.ElementAt(i) * Vector.ElementAt(i);
        }
        return net;
    }

    public int MakeDecision(double net)
    {
        return net >= _bias ? 1 : 0;;
    }
    
    public void Learn(List<double> InputVector, int ExpectedOutput)
    {
        int ActualOutput = MakeDecision(CalculateNet(InputVector));
        if (ActualOutput == ExpectedOutput)
        {
            return;
        }

        List<double> output = new List<double>();
        List<double> InputVectorUpdate = new List<double>();

        foreach (var value in InputVector)
        {
            InputVectorUpdate.Add(value * _alpha * (ExpectedOutput - ActualOutput));
        }

        double NewBias = (-1 * _alpha * (ExpectedOutput - ActualOutput)) + _bias;

        for (int i = 0; i < _weights.Count; i++)
        {
            output.Add(_weights.ElementAt(i) + InputVectorUpdate.ElementAt(i));
        }

        _weights = output;
        _bias = NewBias;
    }

    public void SaveBestPreset(int Accuracy)
    {
        _BestWeightsVec = _weights;
        _BestBias = _bias;
        _BestAccuracy = Accuracy;
    }

    public void ReturnToBestPreset()
    {
        _weights = _BestWeightsVec;
        _bias = _BestBias;
    }
    
    public List<double> WeightsVec
    {
        get => _weights;
        set => _weights = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double Alpha
    {
        get => _alpha;
        set => _alpha = value;
    }
    
    public int Epochs
    {
        get => _epochs;
        set => _epochs = value;
    }

    public bool Ready
    {
        get => _isReady;
        set => _isReady = value;
    }

    public string Type
    {
        get => _type;
        set => _type = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double Bias
    {
        get => _bias;
        set => _bias = value;
    }

    public int BestAccuracy
    {
        get => _BestAccuracy;
    }

    public override string ToString()
    {
        return "[Weigths: " + WeightsToString() + "\nReady: " + Ready + "; Bias: " + Bias + "; Epochs: " + _epochs + "]";
    }
    
    public string WeightsToString()
    {
        string tmp = "[";
        foreach (var VARIABLE in _weights)
        {
            tmp += VARIABLE + ", ";
        }
        tmp = tmp.Remove(tmp.Length-2, 2);
        tmp += "]";

        return tmp;
    }
}