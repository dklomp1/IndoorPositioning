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
    //public class TrackerLocationsController
    //{
    //    private IndoorPositioningContext db = new IndoorPositioningContext();

    //    // GET: api/TrackerLocations
    //    public IQueryable<TrackerLocation> GetTrackerLocations()
    //    {
    //        return db.TrackerLocations;
    //    }

    //    // GET: api/TrackerLocations/5
    //    [ResponseType(typeof(TrackerLocation))]
    //    public async Task<IHttpActionResult> GetTrackerLocation(int id)
    //    {
    //        TrackerLocation trackerLocation = await db.TrackerLocations.FindAsync(id);
    //        if (trackerLocation == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(trackerLocation);
    //    }

    //    // PUT: api/TrackerLocations/5
    //    [ResponseType(typeof(void))]
    //    public async Task<IHttpActionResult> PutTrackerLocation(int id, TrackerLocation trackerLocation)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        if (id != trackerLocation.ID)
    //        {
    //            return BadRequest();
    //        }

    //        db.Entry(trackerLocation).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!TrackerLocationExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return StatusCode(HttpStatusCode.NoContent);
    //    }

    //    // POST: api/TrackerLocations
    //    [ResponseType(typeof(TrackerLocation))]
    //    public async Task<IHttpActionResult> PostTrackerLocation(TrackerLocation trackerLocation)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        db.TrackerLocations.Add(trackerLocation);
    //        await db.SaveChangesAsync();

    //        return CreatedAtRoute("DefaultApi", new { id = trackerLocation.ID }, trackerLocation);
    //    }

    //    // DELETE: api/TrackerLocations/5
    //    [ResponseType(typeof(TrackerLocation))]
    //    public async Task<IHttpActionResult> DeleteTrackerLocation(int id)
    //    {
    //        TrackerLocation trackerLocation = await db.TrackerLocations.FindAsync(id);
    //        if (trackerLocation == null)
    //        {
    //            return NotFound();
    //        }

    //        db.TrackerLocations.Remove(trackerLocation);
    //        await db.SaveChangesAsync();

    //        return Ok(trackerLocation);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    private bool TrackerLocationExists(int id)
    //    {
    //        return db.TrackerLocations.Count(e => e.ID == id) > 0;
    //    }
    //}
}