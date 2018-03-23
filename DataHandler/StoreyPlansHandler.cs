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
    //public class StoreyPlansController
    //{
    //    private IndoorPositioningContext db = new IndoorPositioningContext();

    //    // GET: api/StoreyPlans
    //    public IQueryable<StoreyPlan> GetStoreyPlans()
    //    {
    //        return db.StoreyPlans;
    //    }

    //    // GET: api/StoreyPlans/5
    //    [ResponseType(typeof(StoreyPlan))]
    //    public async Task<IHttpActionResult> GetStoreyPlan(int id)
    //    {
    //        StoreyPlan storeyPlan = await db.StoreyPlans.FindAsync(id);
    //        if (storeyPlan == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(storeyPlan);
    //    }

    //    // PUT: api/StoreyPlans/5
    //    [ResponseType(typeof(void))]
    //    public async Task<IHttpActionResult> PutStoreyPlan(int id, StoreyPlan storeyPlan)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        if (id != storeyPlan.ID)
    //        {
    //            return BadRequest();
    //        }

    //        db.Entry(storeyPlan).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!StoreyPlanExists(id))
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

    //    // POST: api/StoreyPlans
    //    [ResponseType(typeof(StoreyPlan))]
    //    public async Task<IHttpActionResult> PostStoreyPlan(StoreyPlan storeyPlan)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        db.StoreyPlans.Add(storeyPlan);
    //        await db.SaveChangesAsync();

    //        return CreatedAtRoute("DefaultApi", new { id = storeyPlan.ID }, storeyPlan);
    //    }

    //    // DELETE: api/StoreyPlans/5
    //    [ResponseType(typeof(StoreyPlan))]
    //    public async Task<IHttpActionResult> DeleteStoreyPlan(int id)
    //    {
    //        StoreyPlan storeyPlan = await db.StoreyPlans.FindAsync(id);
    //        if (storeyPlan == null)
    //        {
    //            return NotFound();
    //        }

    //        db.StoreyPlans.Remove(storeyPlan);
    //        await db.SaveChangesAsync();

    //        return Ok(storeyPlan);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    private bool StoreyPlanExists(int id)
    //    {
    //        return db.StoreyPlans.Count(e => e.ID == id) > 0;
    //    }
    //}
}