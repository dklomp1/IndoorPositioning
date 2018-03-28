using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using IndoorPositioning.Models;

namespace IndoorPositioning.DataHandler
{
    public class BeaconsHandler
    {

        IndoorPositioningContext db;

        public BeaconsHandler(IndoorPositioningContext db)
        {
            this.db = db;
        }
        public int[] GetBeaconsFromStorey(Guid id)
        {
            var beacons = from b in db.Beacons where b.Storey.ID == id select b.ID;
            
            return beacons.ToList().ToArray();
        }
        public Guid GetBeaconStorey(int id)
        {
            Guid StoreyID = db.Beacons.Find(id).Storey.ID;
            return StoreyID;
        }

        // GET: api/Beacons
        //    public IQueryable<Beacon> GetBeacons()
        //    {
        //        return db.Beacons;
        //    }

        //    // GET: api/Beacons/5
        //    [ResponseType(typeof(Beacon))]
        //    public async Task<IHttpActionResult> GetBeacon(int id)
        //    {
        //        Beacon beacon = await db.Beacons.FindAsync(id);
        //        if (beacon == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(beacon);
        //    }

        //    // PUT: api/Beacons/5
        //    [ResponseType(typeof(void))]
        //    public async Task<IHttpActionResult> PutBeacon(int id, Beacon beacon)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        if (id != beacon.ID)
        //        {
        //            return BadRequest();
        //        }

        //        db.Entry(beacon).State = EntityState.Modified;

        //        try
        //        {
        //            await db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BeaconExists(id))
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

        //    // POST: api/Beacons
        //    [ResponseType(typeof(Beacon))]
        //    public async Task<IHttpActionResult> PostBeacon(Beacon beacon)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        db.Beacons.Add(beacon);
        //        await db.SaveChangesAsync();

        //        return CreatedAtRoute("DefaultApi", new { id = beacon.ID }, beacon);
        //    }

        //    // DELETE: api/Beacons/5
        //    [ResponseType(typeof(Beacon))]
        //    public async Task<IHttpActionResult> DeleteBeacon(int id)
        //    {
        //        Beacon beacon = await db.Beacons.FindAsync(id);
        //        if (beacon == null)
        //        {
        //            return NotFound();
        //        }

        //        db.Beacons.Remove(beacon);
        //        await db.SaveChangesAsync();

        //        return Ok(beacon);
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }

        //    private bool BeaconExists(int id)
        //    {
        //        return db.Beacons.Count(e => e.ID == id) > 0;
        //    }
        //}

        //internal interface IBeaconRepository
        //{
    }
}