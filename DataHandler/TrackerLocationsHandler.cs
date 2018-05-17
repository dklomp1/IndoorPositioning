using System.Data;
using System.Linq;
using IndoorPositioning.Models;

namespace IndoorPositioning.DataHandler
{
    public class TrackerLocationsHandler
    {
        IndoorPositioningContext db;

        public TrackerLocationsHandler(IndoorPositioningContext db)
        {
            this.db = db;
        }
        public void PostTrackerLocation(TrackerLocation loc)
        {
            db.TrackerLocations.Add(loc);
            db.SaveChangesAsync();
        }
        public TrackerLocation GetTrackerLocation(int id)
        {
            var trackerLoc = from b in db.TrackerLocations where b.Tracker.ID == id select b ;
            var trackerLocation = trackerLoc.OrderByDescending(x => x.TimeStamp).First();
            TrackerLocation result = new TrackerLocation(trackerLocation.TimeStamp, trackerLocation.Options, trackerLocation.Tracker);
            return result;
        }
        private bool TrackerLocationExists(int id)
        {
            return db.TrackerLocations.Count(e => e.ID == id) > 0;
        }
    }
}