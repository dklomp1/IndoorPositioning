using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using IndoorPositioning.Models;

namespace IndoorPositioning.DataHandler
{
    public class StoreysHandler
    {
        IndoorPositioningContext db;

        public StoreysHandler(IndoorPositioningContext db)
        {
            this.db = db;
        }
        // GET: api/Storeys
        public IQueryable<Storey> GetStoreys()
        {
            return db.Storeys;
        }

        public Storey GetStorey(Guid id)
        {
            Storey storey = db.Storeys.Find(id);
            return storey;
        }

        public void PostStorey(Storey storey)
        {
            db.Storeys.Add(storey);
            db.SaveChanges();
        }

        
        public bool PutStorey(Storey storey)
        {
            if (db.Storeys.Find(storey.ID) == null)
            {
                return false;
            }
            db.Entry(storey).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                 throw;
            }
            return true;
        }

        //    // POST: api/Storeys
        //    [ResponseType(typeof(Storey))]
        //    public async Task<IHttpActionResult> PostStorey(Storey storey)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        db.Storeys.Add(storey);

        //        try
        //        {
        //            await db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateException)
        //        {
        //            //if (StoreyExists(storey.ID))
        //            //{
        //            //    return Conflict();
        //            //}
        //            //else
        //            //{
        //            //    throw;
        //            //}
        //        }

        //        return CreatedAtRoute("DefaultApi", new { id = storey.ID }, storey);
        //    }

        //    // DELETE: api/Storeys/5
        //    [ResponseType(typeof(Storey))]
        //    public async Task<IHttpActionResult> DeleteStorey(string id)
        //    {
        //        Storey storey = await db.Storeys.FindAsync(id);
        //        if (storey == null)
        //        {
        //            return NotFound();
        //        }

        //        db.Storeys.Remove(storey);
        //        await db.SaveChangesAsync();

        //        return Ok(storey);
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }

        private bool StoreyExists(Guid id)
        {
            return db.Storeys.Count(e => e.ID == id) > 0;
        }
    }
}
