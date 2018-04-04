using IndoorPositioning.Models;
using IndoorPositioning.DataHandler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IndoorPositioning.IPSLogic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.Text;

namespace IndoorPositioning.Controllers
{
    public class TrackersController : ApiController
    {
        IndoorPositioningContext db = new IndoorPositioningContext();
        
        //KnnController KC;
        //GET: api/Trackers/ClassifyingBeacons/1
        [ResponseType(typeof(Beacon))]
        public async Task<IHttpActionResult> GetBeacons(int id)
        {
            return Ok(BeaconsFromStorey(id));
        }
        [ResponseType(typeof(List<string>))]
        public async Task<IHttpActionResult> GetLocation(int id)
        {
            SpacesHandler SH = new SpacesHandler(db);
            TrackerLocationsHandler TLH = new TrackerLocationsHandler(db);
            StoreysHandler StH = new StoreysHandler(db);
            TrackerLocation loc = TLH.GetTrackerLocation(id);
            Dictionary<string, double> Options = new Dictionary<string,double>();
            Stream stream = new MemoryStream(loc.Options);
            BinaryFormatter binfor = new BinaryFormatter();
            Options = (Dictionary<string,double>)binfor.Deserialize(stream);
            List<Option> result = new List<Option>();
            foreach(KeyValuePair<string,double> kv in Options)
            {
                Space space = SH.GetSpace(Guid.Parse(kv.Key));
                Guid spaceID = space.ID;
                Guid storeyID = space.Storey.ID;
                Guid buildingID = space.Storey.Building.ID;
                Option option = new Option(spaceID, storeyID, buildingID, kv.Value);
                result.Add(option);
            }
            return Json(result);
        }
        public int[] BeaconsFromStorey(int id)
        {
            BeaconsHandler BC = new BeaconsHandler(db);
            Guid StoreyID = BC.GetBeaconStorey(id);
            int[] Beacons = BC.GetBeaconsFromStorey(StoreyID);
            return Beacons;
        }
        [System.Web.Http.HttpPost]
        [ResponseType(typeof(List<string>))]
        public async Task<IHttpActionResult> PostLocation()
        {
            TrackersHandler TH = new TrackersHandler(db);
            TrackerLocationsHandler TLH = new TrackerLocationsHandler(db);
            SpacesHandler SH = new SpacesHandler(db);
            KnnsHandler KH = new KnnsHandler(db);
            string resultString = Request.Content.ReadAsStringAsync().Result.Trim();
            KeyValuePair<int, KeyValuePair<Guid, List<double>>> coordinates = LocationParser(resultString);
            Knn Knn = KH.GetKnn(coordinates.Value.Key);
            byte[] options = Classify.ClassifyTemplate(coordinates.Value.Value, Knn);
            Tracker tracker = TH.GetTracker(coordinates.Key);
            TrackerLocation loc = new TrackerLocation(DateTime.Now,options,tracker);
            TLH.PostTrackerLocation(loc);
            return Ok(Classify.ClassifyTemplate(coordinates.Value.Value, Knn));
        }

        private KeyValuePair<int,KeyValuePair<Guid, List<double>>> LocationParser(string resultString)
        {
            BeaconsHandler BH = new BeaconsHandler(db);
            List<double> coordinates = new List<double>();
            string[] resultSub = resultString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int trackerID = new int();
            List<string> resultList = new List<string>();
            for (int i = 1; i < resultSub.Length - 1; i++)
            {
                if (i == 1)
                {
                    trackerID = int.Parse(resultSub[i].Trim().Trim('"').Split(':')[1].Trim(',').Trim('"'));
                }
                else
                {
                    resultList.Add(resultSub[i].Trim().Trim('"'));
                }
            }
            List<KeyValuePair<int, int>> values = new List<KeyValuePair<int, int>>();
            foreach (string r in resultList)
            {
                KeyValuePair<int, int> sub = new KeyValuePair<int, int>(int.Parse(r.Split(':')[0].Remove(1, 1)), int.Parse(r.Split(':')[1].Remove(1, 1).Trim(',').Trim('"').Trim('\\')));
                values.Add(sub);
            }
            Guid storeyID = BH.GetBeaconStorey(values[0].Key);
            int[] allStoreys = BH.GetBeaconsFromStorey(storeyID);

            List<KeyValuePair<int, List<double>>> subCoordinates = getCoordinates(values, allStoreys);
            foreach (KeyValuePair<int, List<double>> subCoordinate in subCoordinates)
            {
                coordinates.Add(IPSLogic.Filter.FilterTemplate(subCoordinate.Value));
            }
            KeyValuePair<Guid, List<double>> resultKV = new KeyValuePair<Guid, List<double>>(storeyID,coordinates);
            KeyValuePair<int, KeyValuePair<Guid, List<double>>> result = new KeyValuePair<int, KeyValuePair<Guid, List<double>>>(trackerID,resultKV);
            return result;
        }

        public List<KeyValuePair<int,List<double>>> getCoordinates(List<KeyValuePair<int, int>> values, int[] allStoreys)
        {
            List<double> subCoordinates = new List<double>();
            List<KeyValuePair<int, List<double>>> result = new List<KeyValuePair<int, List<double>>>();

            for (int i = 0; i < allStoreys.Length; i++)
            {
                int refBeacon = allStoreys[i];
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
                } else
                {
                    sub.Add(0);
                    KeyValuePair<int, List<double>> kvp = new KeyValuePair<int, List<double>>(refBeacon, sub);
                    result.Add(kvp);
                }
                //values.RemoveAll(x => x.Key == refint);
            }
            return result;
        }
        private partial class Option
        {
            
            public Option( Guid spaceID, Guid storeyID, Guid buildingID, double probability)
            {
                this.spaceID = spaceID;
                this.storeyID = storeyID;
                this.buildingID = buildingID;
                this.probability = probability;
            }
            
            public Guid spaceID { get; set; }
            public Guid storeyID { get; set; }
            public Guid buildingID { get; set; }
            public double probability { get; set; }
        }
    }
}