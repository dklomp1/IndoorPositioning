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
            var storey = db.Storeys.Find(id);
            return storey;
        }

        public void PostStorey(Storey storey)
        {
            db.Storeys.Add(storey);
            db.SaveChanges();
        }

        
        public bool PutStorey(Storey s)
        {
            if (db.Storeys.Find(s.ID) == null)
            {
                return false;
            }
            Storey storey = db.Storeys.Find(s.ID);
            storey.Name = s.Name;
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

        public bool DeleteStorey(Storey storey)
        {
            foreach (Space space in db.Spaces.Where(x => x.Storey.ID == storey.ID))
            {
                SpacesHandler SH = new SpacesHandler(db);
                SH.DeleteSpace(space);
            }
            db.Storeys.Remove(storey);
            try
            {
                db.SaveChangesAsync();
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
