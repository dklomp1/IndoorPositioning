using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IndoorPositioning.Models
{
    public class TrackerLocation
    {
        public int ID { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public virtual List<Space> Space { get; set; }
        public virtual Tracker Tracker { get; set; }
    }
}