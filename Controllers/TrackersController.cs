
using IndoorPositioning.Models;
using IndoorPositioning.DataHandler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;


namespace IndoorPositioning.Controllers
{
    public class TrackersController : ApiController
    {
        BeaconsHandler BC = new BeaconsHandler();
        //KnnController KC;
        //GET: api/Trackers/ClassifyingBeacons/1
        [ResponseType(typeof(Beacon))]
        public async Task<IHttpActionResult> GetBeacons(int id)
        {
            return Ok(BeaconsFromStorey(id));
        }
        public int[] BeaconsFromStorey(int id)
        {
            Guid StoreyID = BC.GetBeaconStorey(id);
            int[] Beacons = BC.GetBeaconsFromStorey(StoreyID);
            return Beacons;
        }

        public void SaveLocation([FromUri] List<KeyValuePair<Guid, int>> values)
        {
            //List<double> coordinates = new List<double>();
            //Guid storeyID = BC.GetBeaconStorey(values[0].Key);
            //int length = GetBeaconsFromStorey(values[0].Key).Length;

            //List<List<double>> subCoordinates = getCoordinates(values, length);
            //foreach (List<double> subCoordinate in subCoordinates)
            //{
            //    coordinates.Add(Filter.FilterTemplate(subCoordinate));
            //}


            //KNearestNeighbors Knn = KC.GetKnn(StoreyID);
            
        }
        public List<List<double>> getCoordinates(List<KeyValuePair<Guid, int>> values, int length)
        {
            List<List<double>> subCoordinates = new List<List<double>>();
            for (int i = 0; i <= length; i++)
            {
                int count = 0;
                Guid refint = values[count].Key;
                foreach (KeyValuePair<Guid, int> kv in values)
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