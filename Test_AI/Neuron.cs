using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_AI
{
    //Клас нейрона
    public class Neuron
    {
        //Ваги
        public List<double> Weights { get; }
        //вхідні сигнали
        public List<double> Inputs { get; }
        //тип нейрона
        public NeuronType NeuronType { get; }
        //вихід
        public double Output { get; private set; }
        //дельта
        public double Delta { get; private set; }
        //inputCount - к-сть вхідних нейронів
        public Neuron(int inputCount,NeuronType type = NeuronType.Normal)
        {
            NeuronType = type;
            Weights = new List<double>();
            Inputs = new List<double>();
            InitWeightsRandomValue(inputCount);
        }

        private void InitWeightsRandomValue(int inputCount)
        {
            var rnd = new Random();
            for (int i = 0; i < inputCount; i++)
            { 
                if(NeuronType == NeuronType.Input)
                {
                    Weights.Add(1);
                }
                else
                {
                    Weights.Add(rnd.NextDouble());
                }

                Inputs.Add(0);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            for (int i = 0;i<inputs.Count;i++)
            {
                Inputs[i] = inputs[i];
            }
            var sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }
            if (NeuronType != NeuronType.Input)
            {
                Output = Sigmoid(sum);
                
            }
            else
            {
                Output = sum;
            }
            return Output;
        }
        private double Sigmoid (double x)
        {
            var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
            return result; 
        }
        private double SigmoidDx(double x)
        {
            var sigmoid = Sigmoid(x);
            var result = sigmoid / (1 - sigmoid);
            return result;
        }
    
        public void Learn(double error , double learnigRate)
        {
            if(NeuronType == NeuronType.Input) 
            {
                return;
            }

             Delta = error * SigmoidDx(Output);

            for (int i = 0; i < Weights.Count; i++)
            {

                var weight = Weights[i];
                var input = Inputs[i];

                var newWeight = weight - input * Delta * learnigRate;
                Weights[i] = newWeight;
            }
          
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}
