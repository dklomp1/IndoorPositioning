using Accord.MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndoorPositioning.IPSLogic
{
    public class Classify
    {
        public List<string> ClassifyTemplate(List<double> list, KNearestNeighbors knn)
        {
            List<string> list1 = new List<string>();
            return list1;
        }
        public static string getRoomname(int roomInt)
        {
            Dictionary<int, string> labelMap = new Dictionary<int, string>();
            return labelMap[roomInt];

        }
        public static string GetRoom(KNearestNeighbors knn, double[] coordinates)
        {
            // After the algorithm has been created, we can classify a new instance:
            Console.WriteLine("Room: " + getRoomname(knn.Decide(coordinates)));
            return getRoomname(knn.Decide(coordinates));
        }
        public static List<string> getOptions(double[] coordinates, KNearestNeighbors knn)
        {
            List<string> options = new List<string>();
            Dictionary<int, string> labelMap =  new Dictionary<int, string>(); //Fingerprinting.ReadLabelMap();
            int[] list = labelMap.Keys.ToArray();
            foreach (double[] g in knn.GetNearestNeighbors(coordinates, out list))
            {
                options.Add(getRoomname(knn.Decide(g)));
            }
            return options;
        }
    }
}