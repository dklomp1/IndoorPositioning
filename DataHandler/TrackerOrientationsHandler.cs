using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using IndoorPositioning.Models;
namespace IndoorPositioning.DataHandler
{
    public class TrackerOrientationsHandler
    {
        private IndoorPositioningContext db;
        public TrackerOrientationsHandler(IndoorPositioningContext db)
        {
            this.db = db;
        }
        public TrackerOrientation GetTrackerOrientation(int id)
        {
            var trackerLoc = from b in db.TrackerOrientations where b.Tracker.ID == id select b;
            var trackerOrientation = trackerLoc.OrderByDescending(x => x.TimeStamp).First();
            TrackerOrientation result = new TrackerOrientation(trackerOrientation.ID,trackerOrientation.TimeStamp, trackerOrientation.Tracker, trackerOrientation.Orientation);
            return result;
        }
        
        public void PutOrientation(TrackerOrientation tO)
        {
            var trackerOrientations = db.TrackerOrientations.Where(x => x.Tracker.ID == tO.Tracker.ID);
            if (trackerOrientations.Count() == 0)
            {
                db.TrackerOrientations.Add(tO);
            }
            else
            {
                var trackerOrientation = trackerOrientations.First();
                trackerOrientation.Orientation = tO.Orientation;
                trackerOrientation.TimeStamp = tO.TimeStamp;
                db.Entry(trackerOrientation).State = EntityState.Modified;
            }
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                throw;
            }
            }
        }
        
    }