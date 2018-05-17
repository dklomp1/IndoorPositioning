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
using System.Linq;

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
            List<Option> options = new List<Option>();
            foreach(KeyValuePair<string,double> kv in Options)
            {
                Space space = SH.GetSpace(Guid.Parse(kv.Key));
                Guid spaceID = space.ID;
                Guid storeyID = space.Storey.ID;
                Guid buildingID = space.Storey.Building.ID;
                Option option = new Option(spaceID, storeyID, buildingID, kv.Value);
                options.Add(option);
            }
            KeyValuePair<DateTime, List<Option>> result = new KeyValuePair<DateTime, List<Option>>(loc.TimeStamp, options);
            return Json(result);
        }
        public async Task<IHttpActionResult> GetOrientation(int ID)
        {
            TrackerOrientationsHandler TOH = new TrackerOrientationsHandler(db);
            TrackerOrientation TO = TOH.GetTrackerOrientation(ID);
            KeyValuePair<DateTime, int> result = new KeyValuePair<DateTime, int>(TO.TimeStamp, TO.Orientation);
            return Ok(result);
        }
        public async Task<IHttpActionResult> PutOrientation()
        {
            Heading newHeading = JsonConvert.DeserializeObject < Heading >(Request.Content.ReadAsStringAsync().Result);
            TrackerOrientationsHandler TOH = new TrackerOrientationsHandler(db);
            TrackersHandler TH = new TrackersHandler(db);
            Tracker tracker = TH.GetTracker(newHeading.ID);
            TrackerOrientation trackerOrientation = new TrackerOrientation(DateTime.Now, tracker, newHeading.Orientation);
            TOH.PutOrientation(trackerOrientation);
            return Ok();
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
            DateTime time = DateTime.Now;
            TrackersHandler TH = new TrackersHandler(db);
            TrackerLocationsHandler TLH = new TrackerLocationsHandler(db);
            SpacesHandler SH = new SpacesHandler(db);
            KnnsHandler KH = new KnnsHandler(db);
            string resultString = Request.Content.ReadAsStringAsync().Result.Trim();
            KeyValuePair<int, KeyValuePair<Guid, List<double>>> coordinates = LocationParser(resultString);
            Knn Knn = KH.GetKnn(coordinates.Value.Key);
            byte[] options = Classify.ClassifyTemplate(coordinates.Value.Value, Knn);
            Tracker tracker = TH.GetTracker(coordinates.Key);
            TrackerLocation loc = new TrackerLocation(time,options,tracker);
            TLH.PostTrackerLocation(loc);
            return Ok(Classify.ClassifyTemplate(coordinates.Value.Value, Knn));
        }

        private KeyValuePair<int,KeyValuePair<Guid, List<double>>> LocationParser(string resultString)
        {
            BeaconsHandler BH = new BeaconsHandler(db);
            List<double> coordinates = new List<double>();
            TrackerJson Json = JsonConvert.DeserializeObject<TrackerJson>(Request.Content.ReadAsStringAsync().Result);
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
                coordinates.Add(IPSLogic.Filter.FilterTemplate(subCoordinate.Value));
            }
            KeyValuePair<Guid, List<double>> resultKV = new KeyValuePair<Guid, List<double>>(storeyID,coordinates);
            KeyValuePair<int, KeyValuePair<Guid, List<double>>> result = new KeyValuePair<int, KeyValuePair<Guid, List<double>>>(trackerID,resultKV);
            return result;
        }
        //hier verder "mac"(beaconid),"-087"(rssiint) naar int int
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
                    if(kv.Value[1] == '1')
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
        public List<KeyValuePair<int,List<double>>> getCoordinates(List<KeyValuePair<int, int>> values, int[] allStoreyBeacons)
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
        private partial class Heading
        {
            public Heading(int ID, int Orientation)
            {
                this.ID = ID;
                this.Orientation = Orientation;
            }
            public int ID { get; set; }
            public int Orientation { get; set; }
        }
    }
}