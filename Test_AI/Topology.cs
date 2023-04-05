using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_AI
{
    public class Topology
    {
        //к-сть вхідних даних в нейро мережу
        public int InputCount { get; }
        //к-сть виходів
        public int OutputCount { get; }
        //к-сть нейронів в схованих шарах
        public List<int> HiddenLayers { get; }
        
        public Topology(int inputCount,int outputCount,params int[] layers)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
        }
    }
}
