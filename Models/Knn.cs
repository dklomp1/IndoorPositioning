using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IndoorPositioning.Models
{
    public class Knn
    {
        public int ID { get; set; }
        public virtual Storey Storey { get; set; }
        public byte[] TrainingSet { get; set; }
        public byte[] LabelMap { get; set; }
        public byte[] kNN { get; set; }
    }
}