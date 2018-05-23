using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IndoorPositioning.Models;

namespace IndoorPositioning.DataHandler
{
    public class TrackersHandler
    {
        IndoorPositioningContext db;

        public TrackersHandler(IndoorPositioningContext db)
        {
            this.db = db;
        }
        public Tracker GetTracker(int id)
        {
            Tracker tracker = db.Trackers.FindAsync(id).Result;

            return tracker;
        }

        public void SetStatus(int ID, Space space, string status)
        {
            Tracker tracker = GetTracker(ID);
            tracker.trainingLocation = space;
            tracker.Status = status;
            db.Entry(tracker).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public async Task<bool> setTrainingLocationNull(Space space)
        {
            var trackers = db.Trackers.Where(x => x.trainingLocation.ID == space.ID);

            if (trackers.Count()>0)
            {
                foreach (Tracker tracker in trackers)
                {
                    tracker.trainingLocation = null;
                    db.Entry(tracker).State = EntityState.Modified;
                }
            }
            return true;
        }
        public string GetStatus(int id)
        {
            Tracker tracker = GetTracker(id);
            return tracker.Status;
        }
        //    public void PutTracker(int id, Tracker tracker)
        //    {

        //        db.Entry(tracker).State = EntityState.Modified;

        //    }
        //    public void PostTracker(Tracker tracker)
        //    {

        //        db.Trackers.Add(tracker);
        //        db.SaveChangesAsync();

        //    }
        //    public void DeleteTracker(int id)
        //    {
        //        Tracker tracker = db.Trackers.FindAsync(id).Result;
        //        if (tracker == null)
        //        {

        //        }

        //        db.Trackers.Remove(tracker);
        //        db.SaveChangesAsync();
        //    }

        //    private bool TrackerExists(int id)
        //    {
        //        return db.Trackers.Count(e => e.ID == id) > 0;
        //    }
        //}
    }
}