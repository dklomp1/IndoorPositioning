using System;
using System.Data;
using System.Linq;
using IndoorPositioning.Models;
using Accord.MachineLearning;

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
    }
}