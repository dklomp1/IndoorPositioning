using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IndoorPositioning.Models
{
    public class Tracker
    {
        public Tracker() { }
        [Required]
        public int ID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public virtual Space trainingLocation { get; set; }
    }
}