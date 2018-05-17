using System;
using System.ComponentModel.DataAnnotations;

namespace IndoorPositioning.Models
{
    public class TrackerOrientation
    {
        public TrackerOrientation() { }
        public TrackerOrientation(int ID, DateTime TimeStamp, Tracker Tracker, int Orientation)
        {
            this.ID = ID;
            this.TimeStamp = TimeStamp;
            this.Orientation = Orientation;
            this.Tracker = Tracker;
        }
        public TrackerOrientation(DateTime TimeStamp, Tracker Tracker, int Orientation)
        {
            this.TimeStamp = TimeStamp;
            this.Orientation = Orientation;
            this.Tracker = Tracker;
        }
        public int ID { get; set; }
        [Required]
        public virtual Tracker Tracker { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Orientation { get; set; }
    }
}