using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IndoorPositioning.Models
{
    public class TrackerLocation
    {
        [Obsolete("Only needed for serialization and materialization", true)]
        public TrackerLocation() { }
        public TrackerLocation(DateTime TimeStamp,byte[] Options, Tracker Tracker)
        {
            this.TimeStamp = TimeStamp;
            this.Options = Options;
            this.Tracker = Tracker;
        }
        public int ID { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public byte[] Options { get; set; }
        public virtual Tracker Tracker { get; set; }
    }
}