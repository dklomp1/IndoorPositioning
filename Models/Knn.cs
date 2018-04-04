using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IndoorPositioning.Models
{
    public class Knn
    {
        [Obsolete("Only needed for serialization and materialization", true)]
        public Knn() { }
        public Knn(Storey Storey, byte[] TrainingSet, byte[] LabelMap, byte[] kNN)
        {
            this.Storey = Storey;
            this.TrainingSet = TrainingSet;
            this.LabelMap = LabelMap;
            this.kNN = kNN;
        }
        public int ID { get; set; }
        public virtual Storey Storey { get; set; }
        public byte[] TrainingSet { get; set; }
        public byte[] LabelMap { get; set; }
        public byte[] kNN { get; set; }
    }
}