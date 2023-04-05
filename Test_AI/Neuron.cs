﻿using System;
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
        //тип нейрона
        public NeuronType NeuronType { get; }
        //вихід
        public double Output { get; private set; }
        //inputCount - к-сть вхідних нейронів
        public Neuron(int inputCount,NeuronType type = NeuronType.Normal)
        {
            NeuronType = type;
            Weights = new List<double>();

            for (int i = 0; i < inputCount; i++)
            {
                Weights.Add(1);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            var sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }
            Output = Sigmoid(sum);
            return Output;
        }
        private double Sigmoid (double x)
        {
            var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
            return result; 
        }

        public void SetWeigths(params double[] weights)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                Weights[i] = weights[i];
            }
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}
