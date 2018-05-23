using IndoorPositioning.DataHandler;
using IndoorPositioning.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IndoorPositioning.IPSLogic
{
    public class Parsers
    {
        IndoorPositioningContext db;

        public Parsers(IndoorPositioningContext dbcontext)
        {
            db = dbcontext;
        }
        public object Request { get; private set; }

        public KeyValuePair<int, KeyValuePair<Guid, List<double>>> LocationParser(string resultString)
        {
            BeaconsHandler BH = new BeaconsHandler(db);
            List<double> coordinates = new List<double>();

            TrackerJson Json = JsonConvert.DeserializeObject<TrackerJson>(resultString);
            List<string> data = Json.Data;
            int trackerID = Json.ID;
            List<KeyValuePair<string, string>> subValues = new List<KeyValuePair<string, string>>();
            foreach (string kv in data)
            {
                string[] MacRssi = kv.Split(':');
                KeyValuePair<string, string> kvSub = new KeyValuePair<string, string>(MacRssi[0], MacRssi[1]);
                subValues.Add(kvSub);
            }

            List<KeyValuePair<int, int>> values = getValues(subValues);
            Guid storeyID = BH.GetBeaconStorey(values[0].Key);
            int[] allStoreyBeacons = BH.GetBeaconsFromStorey(storeyID);
            List<KeyValuePair<int, List<double>>> subCoordinates = getCoordinates(values, allStoreyBeacons);
            foreach (KeyValuePair<int, List<double>> subCoordinate in subCoordinates)
            {
                coordinates.Add(Filter.FilterTemplate(subCoordinate.Value));
            }
            KeyValuePair<Guid, List<double>> resultKV = new KeyValuePair<Guid, List<double>>(storeyID, coordinates);
            KeyValuePair<int, KeyValuePair<Guid, List<double>>> result = new KeyValuePair<int, KeyValuePair<Guid, List<double>>>(trackerID, resultKV);
            return result;
        }
        public List<KeyValuePair<int, List<double>>> getCoordinates(List<KeyValuePair<int, int>> values, int[] allStoreyBeacons)
        {
            List<double> subCoordinates = new List<double>();
            List<KeyValuePair<int, List<double>>> result = new List<KeyValuePair<int, List<double>>>();

            for (int i = 0; i < allStoreyBeacons.Length; i++)
            {
                int refBeacon = allStoreyBeacons[i];
                List<double> sub = new List<double>();
                foreach (KeyValuePair<int, int> kv in values)
                {
                    if (kv.Key == refBeacon)
                    {
                        sub.Add(kv.Value);
                    }
                    else
                    {
                        continue;
                    }
                }
                if (sub.Count != 0)
                {
                    KeyValuePair<int, List<double>> kvp = new KeyValuePair<int, List<double>>(refBeacon, sub);
                    result.Add(kvp);
                }
                else
                {
                    sub.Add(0);
                    KeyValuePair<int, List<double>> kvp = new KeyValuePair<int, List<double>>(refBeacon, sub);
                    result.Add(kvp);
                }
                //values.RemoveAll(x => x.Key == refint);
            }
            return result;
        }
        private List<KeyValuePair<int, int>> getValues(List<KeyValuePair<string, string>> subValues)
        {
            BeaconsHandler BH = new BeaconsHandler(db);
            List<KeyValuePair<int, int>> values = new List<KeyValuePair<int, int>>();
            foreach (KeyValuePair<string, string> kv in subValues)
            {
                int value;
                IQueryable<int> ids = BH.GetIDFromMac(kv.Key);
                if (ids.Count() > 0)
                {
                    int ID = ids.First();
                    if (kv.Value[1] == '1')
                    {
                        value = int.Parse(kv.Value);
                    }
                    else
                    {
                        value = int.Parse(kv.Value.Remove(1, 1));
                    }
                    KeyValuePair<int, int> set = new KeyValuePair<int, int>(ID, value);
                    values.Add(set);
                }
            }
            return values;
        }
        private partial class TrackerJson
        {
            public int ID { get; set; }
            public List<string> Data { get; set; }
        }
    }
}