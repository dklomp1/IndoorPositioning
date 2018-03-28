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
    public class KnnsHandler
    {
        IndoorPositioningContext db;

        public KnnsHandler(IndoorPositioningContext db)
        {
            this.db = db;
        }
        public Knn GetKnn(Guid StoreyID)
        {
            return db.Knn.Find(StoreyID);
        }

        public void PostKnn(Knn knn)
        {
            db.Knn.Add(knn);
            db.SaveChangesAsync();
        }
       
        private bool KnnExists(int id)
        {
            return db.Knn.Count(e => e.ID == id) > 0;
        }
    }
}