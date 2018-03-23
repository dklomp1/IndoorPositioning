
using IndoorPositioning.DataHandler;
using IndoorPositioning.IPSLogic;
using IndoorPositioning.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace IndoorPositioning.Controllers
{
    public class KnnsController : ApiController
    {
        BeaconsHandler BC;
        KnnsHandler KC;
        [HttpPost]
        [ResponseType(typeof(Knn))]
        public async Task<IHttpActionResult> PostKnnTraining()
        {
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents[0];
            string filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            //Bytes from the binary reader stored in BlobValue array
            byte[] TrainingSet = file.ReadAsByteArrayAsync().Result; //reader.ReadBytes((int)file.Length);

            string trainingString = System.Text.Encoding.UTF8.GetString(TrainingSet);
            KeyValuePair<KeyValuePair<byte[], byte[]>, string> LabelMapKnn = KnnGenerate.GenerateTemplate(trainingString);
            //Generate the files from the generate template method within the template method. save those 3 to db
            //Storey storey = BC.GetBeaconStorey(BeaconID);
            //Knn knn = new Knn();
            return Ok(filename);
        }
    }
}