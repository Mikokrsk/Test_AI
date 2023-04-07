using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test_AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test_AI.Tests
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void FeedForwardTest()
        {
            var outputs = new double[] { 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 };
            var inputs = new double[,]
            {
                // Результат - Пациент болен - 1
                //             Пациент Здоров - 0

                // Неправильная температура T
                // Хороший возраст A
                // Курит S
                // Правильно питается F
                // T  A  S  F
                 { 0, 0, 0, 0 },
                 { 0, 0, 0, 1 },
                 { 0, 0, 1, 0 },
                 { 0, 0, 1, 1 },
                 { 0, 1, 0, 0 },
                 { 0, 1, 0, 1 },
                 { 0, 1, 1, 0 },
                 { 0, 1, 1, 1 },
                 { 1, 0, 0, 0 },
                 { 1, 0, 0, 1 },
                 { 1, 0, 1, 0 },
                 { 1, 0, 1, 1 },
                 { 1, 1, 0, 0 },
                 { 1, 1, 0, 1 },
                 { 1, 1, 1, 0 },
                 { 1, 1, 1, 1 }

            };

            var topology = new Topology(4, 1, 0.1, 2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs, inputs, 100000);

            var results = new List<double>();
            for (int i = 0; i < outputs.Length; i++)
            {
                var row = NeuralNetwork.GetRow(inputs, i);
                var res = neuralNetwork.Predict(row).Output;
                results.Add(res);
            }
            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 3);
                var actual = Math.Round(results[i], 3);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void DatasetTest()
        {
            var outputs = new List<double>();
            var inputs = new List<double[]>();
            using (var sr = new StreamReader("heart_cleveland_upload.csv"))
            {
                var header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    var values = row.Split(',').Select(v => Convert.ToDouble(v.Replace(".", ","))).ToList();
                    var output = values.Last();
                    var input = values.Take(values.Count - 1).ToArray();
                    outputs.Add(output);
                    inputs.Add(input);

                }
            }

            var inputSignal = new double[inputs.Count, inputs[0].Length];
            for (int i = 0; i < inputSignal.GetLength(0); i++)
            {
                for (int j = 0; j < inputSignal.GetLength(1); j++)
                {
                    inputSignal[i, j] = inputs[i][j];
                }
            }
            var topology = new Topology(outputs.Count, 1, 0.1, outputs.Count / 2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs.ToArray(), inputSignal, 1000);

            var results = new List<double>();
            for (int i = 0; i < outputs.Count; i++)
            {

                var res = neuralNetwork.Predict(inputs[i]).Output;
                results.Add(res);
            }
            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 3);
                var actual = Math.Round(results[i], 3);
                Assert.AreEqual(expected, actual);
            }

        }


        [TestMethod()]
        public void RecognizeImages()
        {
            var size = 100;
            var parasitizedPath = @"D:\Test_AI_C#\cell_images\Parasitized\";
            var unparasitizedPath = @"D:\Test_AI_C#\cell_images\Uninfected\";

            var converter = new PictureConverter();

            var testParasitizedImageInput = converter.Convert(@"D:\Test_AI_C#\Test_AI\Test_AITests\Images\Parasitized.png");
            var testUnparasitizedImageInput = converter.Convert(@"D:\Test_AI_C#\Test_AI\Test_AITests\Images\Uninfected.png");
            var topology = new Topology(testParasitizedImageInput.Count, 1, 0.1, testParasitizedImageInput.Count / 2);
            
            var neuralNetwork = new NeuralNetwork(topology);

            double[,] parasitizedInputs = GetData(parasitizedPath, converter, testParasitizedImageInput, size);
            neuralNetwork.Learn(new double[] { 1 }, parasitizedInputs, 1);

            double[,] unparasitizedInputs = GetData(unparasitizedPath, converter, testUnparasitizedImageInput, size);
            neuralNetwork.Learn(new double[] { 0 }, unparasitizedInputs, 1);

            var par = neuralNetwork.Predict(testParasitizedImageInput.Select(t => (double)t).ToArray());
            var unpar = neuralNetwork.Predict(testUnparasitizedImageInput.Select(t => (double)t).ToArray());

            Assert.AreEqual(1, Math.Round(par.Output, 2));
            Assert.AreEqual(0, Math.Round(unpar.Output, 2));
        }

        private static double[,] GetData(string parasitizedPath, PictureConverter converter, List<int> testImageInput, int size)
        {
            var images = Directory.GetFiles(parasitizedPath);
            var result = new double[size, testImageInput.Count];
            for (int i = 0; i < size; i++)
            {
                var image = converter.Convert(images[i]);
                for (int j = 0; j < image.Count; j++)
                {
                    result[i, j] = image[j];
                }
            }

            return result;
        }
    }
}