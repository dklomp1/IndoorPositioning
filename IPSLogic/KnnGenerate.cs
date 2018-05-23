using Accord.IO;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;
using IndoorPositioning.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;

namespace IndoorPositioning.IPSLogic
{
    public class KnnGenerate
    {
        public static KeyValuePair<KeyValuePair<byte[], byte[]>, string> GenerateTemplateTrainingFile(string trainingSet)
        {
            Dictionary<List<string>, double[][]> processedTrainingSet = processTrainingSet(trainingSet);
            KeyValuePair<Dictionary<int, string>, KNearestNeighbors> labelMapKnn = KnnCreateWithLabelMap(processedTrainingSet);
            KeyValuePair<KeyValuePair<byte[], byte[]>, string> byteArrays = ConvertToByteArray(labelMapKnn);
            //KVP<LabelMap, knnByte>,First label ID>
            return byteArrays;
        }
        public static KeyValuePair<KeyValuePair<byte[], byte[]>, string> GenerateTemplateTrainingValues(IQueryable<TrainingValue> trainingValues)
        {
            Dictionary<List<string>, double[][]> processedTrainingSet = processTrainingValues(trainingValues);
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
        public static Dictionary<List<string>, double[][]> processTrainingValues(IQueryable<TrainingValue> trainingValues)
        {

            Dictionary<List<string>, double[][]> trainingSet = new Dictionary<List<string>, double[][]>();
            List<string> subStringList = new List<string>();
            double[][] subDoubleList = new double[trainingValues.Count()][];
            int i = 0;
            //generate the trainingSet as dictionary
            foreach (TrainingValue tv in trainingValues)
            {
                // tv format == "Guid Space_ID, stValues" et cetera.
                // every iteration retrieves the roomName and a double[] with the Rssi values
                
                List<double> subSubDoubleList = new List<double>();
                subStringList.Add(tv.Space.ID.ToString());
                string[] values = tv.Values.Split(',');
                foreach (string value in values)
                {
                    try
                    {
                        subSubDoubleList.Add(double.Parse(value) * -1);
                        Console.WriteLine(double.Parse(value) * -1);
                    }
                    catch { }
                }
                subDoubleList[i] = subSubDoubleList.ToArray();
                i++;
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
            Dictionary<int, string> labelMapDict = new Dictionary<int, string>(KV.Key);
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, labelMapDict);
            mStream.Close();
            //This gives you the byte array.
            byte[] LabelMap = mStream.ToArray();

            //kNN to byte[]
            MemoryStream stream = new MemoryStream();
            KV.Value.Save(stream);
            
            KeyValuePair<byte[], byte[]> sub = new KeyValuePair<byte[], byte[]>(stream.ToArray(), LabelMap);
            //KVP<LabelMap, knnByte>,First label ID>
            KeyValuePair<KeyValuePair<byte[], byte[]>,string> result = new KeyValuePair<KeyValuePair<byte[], byte[]>, string>(sub,labelMapDict.First().Value);
            return result;
            }
            
        }
    }
