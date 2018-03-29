using IndoorPositioning.Models;
using IndoorPositioning.DataHandler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IndoorPositioning.IPSLogic;

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
        public int[] BeaconsFromStorey(int id)
        {
            BeaconsHandler BC = new BeaconsHandler(db);
            Guid StoreyID = BC.GetBeaconStorey(id);
            int[] Beacons = BC.GetBeaconsFromStorey(StoreyID);
            return Beacons;
        }
        [HttpPost]
        [ResponseType(typeof(List<string>))]
        public async Task<IHttpActionResult> PostLocation()
        {
            BeaconsHandler BH = new BeaconsHandler(db);
            KnnsHandler KH = new KnnsHandler(db);
            List<double> coordinates = new List<double>();
            string resultstring = Request.Content.ReadAsStringAsync().Result.Trim();

            string[] resultSub = resultstring.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            List<string> resultList = new List<string>();
            for (int i = 1; i < resultSub.Length - 1; i++)
            {
                resultList.Add(resultSub[i].Trim().Trim('"'));
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
            foreach (KeyValuePair<int,List<double>> subCoordinate in subCoordinates)
            {
                coordinates.Add(Filter.FilterTemplate(subCoordinate.Value));
            }
            Knn Knn = KH.GetKnn(storeyID);
            Classify.ClassifyTemplate(coordinates, Knn);
            return Ok(Classify.ClassifyTemplate(coordinates, Knn));
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
    }
}