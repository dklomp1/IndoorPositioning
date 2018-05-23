using IndoorPositioning.DataHandler;
using IndoorPositioning.IPSLogic;
using IndoorPositioning.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;


namespace IndoorPositioning.Controllers
{
    public class TrainingController : ApiController
    {
        IndoorPositioningContext db = new IndoorPositioningContext();
        [HttpPost]
        [ResponseType(typeof(Knn))]
        public async Task<IHttpActionResult> PostTrainingValue()
        {
            TrackersHandler TH = new TrackersHandler(db);
            string PostString = Request.Content.ReadAsStringAsync().Result;
            Parsers parser = new Parsers(db);
            // trackerid,[space,list]
            KeyValuePair<int, KeyValuePair<Guid, List<double>>> allBeaconValues= parser.LocationParser(PostString);
            Tracker tracker = TH.GetTracker(allBeaconValues.Key);
            Space space = tracker.trainingLocation;
            TrainingValue TV = new TrainingValue(space, string.Join(",", allBeaconValues.Value.Value.ToArray()));
            TrainingValuesHandler TVH = new TrainingValuesHandler(db);
            TVH.PostTrainingValue(TV);
            return Ok();
        }
        //public async Task<IHttpActionResult> GetStoreyTraining(Guid Storey)
        //{
        //    TrainingValuesHandler TVH = new TrainingValuesHandler(db);
        //    StoreysHandler SH = new StoreysHandler(db);
        //    List<TrainingValue> trainingSet = TVH.GetTrainingValuesFromStorey();
        //}
    }
}