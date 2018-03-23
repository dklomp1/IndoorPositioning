using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndoorPositioning.IPSLogic
{
    public class Filter
    {
        //template method
        public static double FilterTemplate (List<double> values)
        {
            double mean = Mean(values);
            double std = StandardDeviation(values, mean);
            List<double> filteredValues = correctedList(values, mean, std);
            mean = Mean(filteredValues);
            return Math.Round(mean);
        }
        // calculate the mean of the list with rssi values
        public static double Mean(List<double> values)
        {
            int n = values.Count;
            double sum = values.Sum();
            double result = sum / n;
            return result;
        }
        // calculate the standard deviation of the list
        public static double StandardDeviation(List<double> values, double mean)
        {

            List<double> devList = new List<double>();
            foreach (int i in values)
            {
                double absoluteVal = Math.Sqrt(i * i);
                double absoluteMean = Math.Sqrt(mean * mean);
                double absoluteRes = Math.Sqrt((absoluteMean - absoluteVal) * (absoluteMean - absoluteVal));
                devList.Add(absoluteRes);
            }
            double result = devList.Sum() / devList.Count;
            return result;
        }
        // filter the values that deviate to much from the mean.
        public static List<double> correctedList(List<double> values, double mean, double std)
        {
            List<double> result = new List<double>();

            foreach (double i in values)
            {
                //Console.WriteLine("std: " + std + ", mean: " + mean + ", value: " + i);
                if ((i > mean - (std * 1.5) && i < mean + (std * 1.5)))
                {
                    result.Add(i);
                }
                else
                {
                    //Console.WriteLine("filter");
                }
            }
            return result;

        }
    }
}