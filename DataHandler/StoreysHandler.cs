using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<bool> DeleteStorey(Storey storey)
        {
            var spaces = db.Spaces.Where(x => x.Storey.ID == storey.ID);
            foreach (Space space in spaces)
            {
                SpacesHandler SH = new SpacesHandler(db);
                await SH.DeleteSpace(space);
            }
            BeaconsHandler BH = new BeaconsHandler(db);
            await BH.setStoreyNull(storey);
            KnnsHandler KH = new KnnsHandler(db);
            await KH.setStoreyNull(storey);
            db.Entry(storey).State = EntityState.Deleted;
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
