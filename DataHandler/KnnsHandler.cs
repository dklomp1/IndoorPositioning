using System;
using System.Data;
using System.Linq;
using IndoorPositioning.Models;
using Accord.MachineLearning;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

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
            var id = from b in db.Knn where b.Storey.ID == StoreyID select b.ID;
            var knn = db.Knn.Find(id.First());
            Knn result = new Knn(knn.Storey,knn.TrainingSet,knn.LabelMap,knn.kNN);
            return result;
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
        public async Task<bool> setStoreyNull(Storey storey)
        {
            var knns = db.Knn.Where(x => x.Storey.ID == storey.ID);
            if (knns.Any())
            {
                foreach (Knn knn in knns)
                {
                    knn.Storey = null;
                    db.Entry(knn).State = EntityState.Modified;
                }
            }
            return true;
        }
    }
}