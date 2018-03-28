using Accord.IO;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IndoorPositioning.IPSLogic
{
    public class KnnGenerate
    {
        public static KeyValuePair<KeyValuePair<byte[], byte[]>, string> GenerateTemplate(string trainingSet)
        {
            Dictionary<List<string>, double[][]> processedTrainingSet = processTrainingSet(trainingSet);
            KeyValuePair<Dictionary<int, string>, KNearestNeighbors> labelMapKnn = KnnCreateWithLabelMap(processedTrainingSet);
            KeyValuePair<KeyValuePair<byte[], byte[]>, string> byteArrays = ConvertToByteArray(labelMapKnn);
            //KVP<LabelMap, knnByte>,First label ID>
            return byteArrays;
        }
        public static Dictionary<List<string>, double[][]> processTrainingSet(string trainingSetArray)
        {
            // Split the file trainingSet string
            string[] lines = trainingSetArray.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<List<string>, double[][]> trainingSet = new Dictionary<List<string>, double[][]>();
            List<string> subStringList = new List<string>();
            double[][] subDoubleList = new double[lines.Length][];
            //Console.WriteLine("size trainingset: " + lines.Length);
            //generate the trainingSet as dictionary
            for (int i = 0; i < lines.Length; i++)
            {
                // line format == "roomName;firstRssi;secondRssi;ThirdRssi;" et cetera.
                // every iteration retrieves the roomName and a double[] with the Rssi values
                string[] stringList = lines[i].Split(';');
                List<double> subSubDoubleList = new List<double>();
                subStringList.Add(stringList[0]);
                foreach (string nth in stringList)
                {
                    try
                    {
                        subSubDoubleList.Add(double.Parse(nth) * -1);
                        Console.WriteLine(double.Parse(nth) * -1);
                    }
                    catch { }
                }
                subDoubleList[i] = subSubDoubleList.ToArray();
            }
            trainingSet.Add(subStringList, subDoubleList);
            return trainingSet;
        }
        public static KeyValuePair<Dictionary<int, string>,KNearestNeighbors> KnnCreateWithLabelMap(Dictionary<List<string>, double[][]> trainingSet)
        {
            int labelCounter = -1;
            List<int> classesList = new List<int>();
            Dictionary<int, string> labelMap = new Dictionary<int, string>();
            /* Since the kNN algorithm generates a model with int values instead of string values for the label, 
            it is imperative to generate a map for reference.
            */
            foreach (string label in trainingSet.First().Key.ToArray())
            {
                if (!labelMap.ContainsValue(label))
                {
                    labelCounter++;
                    classesList.Add(labelCounter);
                    labelMap.Add(labelCounter, label);
                    //Console.WriteLine(labelCounter + ": " + label);
                }
                else
                {
                    classesList.Add(labelCounter);
                }
            }

            int[] classes = classesList.ToArray();
            double[][] inputs = trainingSet.First().Value;


            // Now we will create the K-Nearest Neighbors algorithm. 
            // It's possible to swtich around the k: 5 for the possibility of better accuracy
            var knn = new KNearestNeighbors(k: 5);

            // We train the algorithm:
            knn.Learn(inputs, classes);

            // Generate the result.
            KeyValuePair<Dictionary<int, string>, KNearestNeighbors> result = new KeyValuePair<Dictionary<int, string>, KNearestNeighbors>(labelMap,knn);
            return result;
        }
        public static KeyValuePair<KeyValuePair<byte[], byte[]>, string> ConvertToByteArray(KeyValuePair<Dictionary<int, string>, KNearestNeighbors> KV)
        {
            // generate a temporary file to simulate the file construction
            using (var fs = new FileStream(@"C:\\Users\\dklomp1\\Pictures\\location test\\example.txt", FileMode.Create, FileAccess.ReadWrite))
            {
                TextWriter tw = new StreamWriter(fs);
                Dictionary<int, string> labelMapDict = new Dictionary<int, string>(KV.Key);
                foreach (KeyValuePair<int, string> label in labelMapDict)
                {
                    string line = label.Key.ToString() + ";" + label.Value + Environment.NewLine;
                    tw.WriteLine(line);
                }
                tw.Flush();
                fs.Close();
                // read the temporary file and stream the content to a byte[]
                FileStream stream = File.OpenRead(@"C:\\Users\\dklomp1\\Pictures\\location test\\example.txt");
                byte[] LabelMap = new byte[stream.Length];
                stream.Read(LabelMap, 0, LabelMap.Length);
                stream.Close();
                KNearestNeighbors knn = KV.Value;
                knn.Save(Path.Combine(@"C:\\Users\\dklomp1\\Pictures\\location test", "knn.bin"));
                FileStream stream2 = File.OpenRead(@"C:\\Users\\dklomp1\\Pictures\\location test\\knn.bin");
                byte[] knnByte = new byte[stream2.Length];
                stream2.Read(knnByte, 0, knnByte.Length);
                stream2.Close();
                KeyValuePair<byte[], byte[]> sub = new KeyValuePair<byte[], byte[]>(knnByte, LabelMap);
                //KVP<LabelMap, knnByte>,First label ID>
                KeyValuePair<KeyValuePair<byte[], byte[]>,string> result = new KeyValuePair<KeyValuePair<byte[], byte[]>, string>(sub,labelMapDict.First().Value);
                return result;
            }
            
        }
    }
}