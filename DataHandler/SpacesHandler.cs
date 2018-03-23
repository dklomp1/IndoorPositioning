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
    //    public class SpacesController
    //    {
    //    private IndoorPositioningContext db = new IndoorPositioningContext();

    //    // GET: api/Spaces
    //    public IQueryable<Space> GetSpaces()
    //    {
    //        return db.Spaces;
    //    }

    //    // GET: api/Spaces/5
    //    [ResponseType(typeof(Space))]
    //    public async Task<IHttpActionResult> GetSpace(int id)
    //    {
    //        Space space = await db.Spaces.FindAsync(id);
    //        if (space == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(space);
    //    }

    //    // PUT: api/Spaces/5
    //    [ResponseType(typeof(void))]
    //    public async Task<IHttpActionResult> PutSpace(int id, Space space)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        //if (id != space.ID)
    //        //{
    //        //    return BadRequest();
    //        //}

    //        db.Entry(space).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            //if (!SpaceExists(id))
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

    //    // POST: api/Spaces
    //    [ResponseType(typeof(Space))]
    //    public async Task<IHttpActionResult> PostSpace(Space space)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        db.Spaces.Add(space);
    //        await db.SaveChangesAsync();

    //        return CreatedAtRoute("DefaultApi", new { id = space.ID }, space);
    //    }

    //    // DELETE: api/Spaces/5
    //    [ResponseType(typeof(Space))]
    //    public async Task<IHttpActionResult> DeleteSpace(int id)
    //    {
    //        Space space = await db.Spaces.FindAsync(id);
    //        if (space == null)
    //        {
    //            return NotFound();
    //        }

    //        db.Spaces.Remove(space);
    //        await db.SaveChangesAsync();

    //        return Ok(space);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    //private bool SpaceExists(int id)
    //    //{
    //    //    return db.Spaces.Count(e => e.ID == id) > 0;
    //    //}
    //}
}