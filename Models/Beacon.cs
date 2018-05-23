using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IndoorPositioning.Models
{
    public class Beacon
    {
        public Beacon() { }
        [Required]
        public int ID { get; set; }
        public string MAC { get; set; }
        public string Type { get; set; }
        //Foreign Key
        public virtual Storey Storey { get; set; }
    }
}