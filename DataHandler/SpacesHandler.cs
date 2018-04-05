using IndoorPositioning.Models;
using System;
using System.Data.Entity;

namespace IndoorPositioning.DataHandler
{
    public class SpacesHandler
    {


        IndoorPositioningContext db;

        public SpacesHandler(IndoorPositioningContext dbcontext)
        {
            db = dbcontext;
        }
        public Space GetSpace(Guid id)
        {
            var Space = db.Spaces.Find(id);
            return Space;
        }
        public void PostSpace(Space space)
        {
            db.Spaces.Add(space);
            db.SaveChanges();
        }

        public bool PutSpace(Space s)
        {
            if (db.Spaces.Find(s.ID) == null)
            {
                return false;
            }
            db.Entry(s).State = EntityState.Modified;
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
}