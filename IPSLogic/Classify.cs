using Accord.IO;
using Accord.MachineLearning;
using IndoorPositioning.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IndoorPositioning.IPSLogic
{
    public class Classify
    {
        public static List<string> ClassifyTemplate(List<double> coordinates, Knn knn)
        {
            var loaded_knn = Serializer.Load<KNearestNeighbors>(knn.kNN);
            List<string> options = getOptions(coordinates.ToArray(),loaded_knn, ReadLabelMap(knn.LabelMap));
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
        public static List<string> getOptions(double[] coordinates, KNearestNeighbors Knn, Dictionary<int, string> labelMap)
        {
            List<string> options = new List<string>();
            int[] list = labelMap.Keys.ToArray();
            foreach (double[] g in Knn.GetNearestNeighbors(coordinates, out list))
            {
                options.Add(getRoomname(Knn.Decide(g), labelMap));
            }
            return options;
        }
        public static Dictionary<int, string> ReadLabelMap(byte[] labelMap)
        {
            string[] lines = labelMap.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<int, string> map = new Dictionary<int, string>();
            foreach (string line in lines)
            {
                string[] stringList = line.Split(';');
                if (!map.ContainsKey(int.Parse(stringList[0])))
                {
                    map.Add(int.Parse(stringList[0]), stringList[1]);
                }
            }
            return map;
        }
    }
}