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
            int length = BH.GetBeaconsFromStorey(storeyID).Length;

            List<List<double>> subCoordinates = getCoordinates(values, length);
            foreach (List<double> subCoordinate in subCoordinates)
            {
                coordinates.Add(Filter.FilterTemplate(subCoordinate));
            }
            Knn Knn = KH.GetKnn(storeyID);
            Classify.ClassifyTemplate(coordinates, Knn);
            return Ok(Classify.ClassifyTemplate(coordinates, Knn));
        }
        
        public List<List<double>> getCoordinates(List<KeyValuePair<int, int>> values, int length)
        {
            List<List<double>> subCoordinates = new List<List<double>>();
            for (int i = 0; i <= length; i++)
            {
                int count = 0;
                int refint = values[count].Key;
                foreach (KeyValuePair<int, int> kv in values)
                {
                    if (kv.Key == refint)
                    {
                        subCoordinates[i].Add(kv.Value);
                    }
                    else
                    {
                        break;
                    }
                }
                values.RemoveAll(x => x.Key == refint);
            }
            return subCoordinates;
        }
    }
}