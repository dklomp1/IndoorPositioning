
using Accord.IO;
using Accord.MachineLearning;
using IndoorPositioning.DataHandler;
using IndoorPositioning.IPSLogic;
using IndoorPositioning.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace IndoorPositioning.Controllers
{

    public class KnnsController : ApiController
    {
        IndoorPositioningContext db = new IndoorPositioningContext();
        [HttpPost]
        [ResponseType(typeof(Knn))]
        public async Task<IHttpActionResult> PostKnnTrainingFile()
        {
            StoreysHandler SH = new StoreysHandler(db);
            SpacesHandler SpH = new SpacesHandler(db);
            KnnsHandler KH = new KnnsHandler(db);
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents[0];
            string filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            //Bytes from the binary reader stored in BlobValue array
            byte[] TrainingSet = file.ReadAsByteArrayAsync().Result; //reader.ReadBytes((int)file.Length);

            string trainingString = System.Text.Encoding.UTF8.GetString(TrainingSet);

            //KVP<LabelMap, knnByte>,First label ID>
            KeyValuePair<KeyValuePair<byte[], byte[]>, string> LabelMapKnn = KnnGenerate.GenerateTemplateTrainingFile(trainingString);
            //Generate the files from the generate template method within the template method. save those 3 to db
            //Storey storey = BC.GetBeaconStorey(BeaconID);
            //Knn knn = new Knn();

            //Once trainingSets generated, Space Guid will be saved in the LabelMapKnn string.
            Guid storeyId = SpH.GetSpaceStorey(Guid.Parse(LabelMapKnn.Value)).ID;
            Storey storey = SH.GetStorey(storeyId);
            byte[] LabelMap = LabelMapKnn.Key.Value;
            byte[] Knn = LabelMapKnn.Key.Key;
            Knn knn = new Knn(storey, TrainingSet, LabelMap, Knn);
            KH.PostKnn(knn);
            return Ok(storey);
        }
        public async Task<IHttpActionResult> PostStoreyKnn()
        {
            Guid storeyID = JsonConvert.DeserializeObject<Guid>(Request.Content.ReadAsStringAsync().Result);
            StoreysHandler SH = new StoreysHandler(db);
            SpacesHandler SpH = new SpacesHandler(db);
            TrackersHandler TH = new TrackersHandler(db);
            TrainingValuesHandler TVH = new TrainingValuesHandler(db);
            KnnsHandler KH = new KnnsHandler(db);
            IQueryable<TrainingValue> values = TVH.GetTrainingValuesFromStorey(storeyID);
            KeyValuePair<KeyValuePair<byte[], byte[]>, string> LabelMapKnn = KnnGenerate.GenerateTemplateTrainingValues(values);
            Storey storey = SH.GetStorey(storeyID);
            byte[] LabelMap = LabelMapKnn.Key.Value;
            byte[] Knn = LabelMapKnn.Key.Key;
            Knn knn = new Knn(storey, LabelMap, Knn);
            KH.PostKnn(knn);
            return Ok();
        }
        //[HttpGet]
        //[ResponseType(typeof(Knn))]
        ////public async Task<IHttpActionResult> GetClassification()
        ////{
        ////}

    }
}