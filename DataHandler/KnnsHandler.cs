using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using IndoorPositioning.Models;

namespace IndoorPositioning.DataHandler
{
    public class KnnsHandler : Controller
    {
        private IndoorPositioningContext db = new IndoorPositioningContext();
        
        public IQueryable<Knn> GetKnn()
        {
            return db.Knn;
        }

        public byte[] PostTrainingSet(HttpPostedFileBase collection)
        {
            Stream inputStream = Request.Files[0].InputStream;

            //The reader reads the binary data from the file stream
            BinaryReader reader = new BinaryReader(inputStream);

            //Bytes from the binary reader stored in BlobValue array
            byte[] BlobValue = reader.ReadBytes((int)inputStream.Length);
            return BlobValue;
        }
       
        private bool KnnExists(int id)
        {
            return db.Knn.Count(e => e.ID == id) > 0;
        }
    }
}