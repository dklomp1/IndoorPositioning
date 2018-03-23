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
    //public class StoreysController
    //{
    //    private IndoorPositioningContext db = new IndoorPositioningContext();

    //    // GET: api/Storeys
    //    public IQueryable<Storey> GetStoreys()
    //    {
    //        return db.Storeys;
    //    }

    //    // GET: api/Storeys/5
    //    [ResponseType(typeof(Storey))]
    //    public async Task<IHttpActionResult> GetStorey(int id)
    //    {
    //        Storey storey = await db.Storeys.FindAsync(id);
    //        if (storey == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(storey);
    //    }

    //    // PUT: api/Storeys/5
    //    [ResponseType(typeof(void))]
    //    public async Task<IHttpActionResult> PutStorey(int id, Storey storey)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        //if (id != storey.ID)
    //        //{
    //        //    return BadRequest();
    //        //}

    //        db.Entry(storey).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            //if (!StoreyExists(id))
    //            //{
    //            //    return NotFound();
    //            //}
    //            //else
    //            //{
    //            //    throw;
    //            //}
    //        }

    //        return StatusCode(HttpStatusCode.NoContent);
    //    }

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

    //    //private bool StoreyExists(int id)
    //    //{
    //    //    return db.Storeys.Count(e => e.ID == id) > 0;
    //    //}
    //}
}