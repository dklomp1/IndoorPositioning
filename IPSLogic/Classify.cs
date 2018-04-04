using Accord.IO;
using Accord.MachineLearning;
using IndoorPositioning.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace IndoorPositioning.IPSLogic
{
    public class Classify
    {
        public static byte[] ClassifyTemplate(List<double> coordinates, Knn knn)
        {
            //This gives you the byte array.
            Dictionary<int, string> LabelMap = ReadLabelMap(knn.LabelMap);
            var loaded_knn = Serializer.Load<KNearestNeighbors>(knn.kNN); //KnnGenerate.KnnCreateWithLabelMap(KnnGenerate.processTrainingSet(trainingString));
            Dictionary<string,double> optionsDict = getOptions(coordinates.ToArray(),loaded_knn, LabelMap);

            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, optionsDict);
            mStream.Close();

            //This gives you the byte array.
            byte[] options = mStream.ToArray();
            return options;
        }
        public static string getRoomname(int roomInt, Dictionary<int, string> labelMap )
        {
            return labelMap[roomInt];
        }
        //public static string GetRoom(KNearestNeighbors knn, double[] coordinates)
        //{
        //    // After the algorithm has been created, we can classify a new instance:
        //    Console.WriteLine("Room: " + getRoomname(knn.Decide(coordinates)));
        //    return getRoomname(knn.Decide(coordinates));
        //}
        public static Dictionary<string,double> getOptions(double[] coordinates, KNearestNeighbors Knn, Dictionary<int, string> labelMap)
        {
            Dictionary<string, int> optionDict = new Dictionary<string, int>();
            int[] list = labelMap.Keys.ToArray();
            int length = Knn.GetNearestNeighbors(coordinates, out list).Length;
            foreach (double[] g in Knn.GetNearestNeighbors(coordinates, out list))
            {
                string roomname = getRoomname(Knn.Decide(g), labelMap);
                if (optionDict.Keys.Contains(roomname))
                {
                    optionDict[roomname] += 1;
                }
                else
                {
                    optionDict.Add(roomname, 1);
                }
            }
            Dictionary<string, double> options = new Dictionary<string, double>();
            foreach(KeyValuePair<string,int> option in optionDict)
            {
                double factor = (double)1 / (double)length;
                double value = option.Value * factor;
                options.Add(option.Key, value);
            }
            return options;
        }
        public static Dictionary<int, string> ReadLabelMap(byte[] LabelMap)
        {
            Dictionary<int, string> lm = new Dictionary<int, string>();
            Stream stream = new MemoryStream(LabelMap);
            BinaryFormatter binfor = new BinaryFormatter();
            lm = (Dictionary<int, string>)binfor.Deserialize(stream);
            return lm;
        }
    }
}