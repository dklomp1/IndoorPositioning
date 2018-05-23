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
    public class AddressesHandler
    {
        IndoorPositioningContext db;
        public AddressesHandler(IndoorPositioningContext dbcontext)
        {
            db = dbcontext;
        }
        public void PostAddress(Address address)
        {
            db.Addresses.Add(address);
            db.SaveChanges();
        }
        //    // GET: api/Addresses
        //    public IQueryable<Address> GetAddresses()
        //    {
        //        return db.Addresses;
        //    }

        //    // GET: api/Addresses/5
        //    [ResponseType(typeof(Address))]
        //    public async Task<IHttpActionResult> GetAddress(int id)
        //    {
        //        Address address = await db.Addresses.FindAsync(id);
        //        if (address == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(address);
        //    }

        //    // PUT: api/Addresses/5
        //    [ResponseType(typeof(void))]
        //    public async Task<IHttpActionResult> PutAddress(int id, Address address)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        //if (id != address.ID)
        //        //{
        //        //    return BadRequest();
        //        //}

        //        db.Entry(address).State = EntityState.Modified;

        //        try
        //        {
        //            await db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            //if (!AddressExists(id))
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

        //    // POST: api/Addresses
        //    [ResponseType(typeof(Address))]
        //    public async Task<IHttpActionResult> PostAddress(Address address)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        db.Addresses.Add(address);
        //        await db.SaveChangesAsync();

        //        return CreatedAtRoute("DefaultApi", new { id = address.ID }, address);
        //    }
        
        public async Task<bool> DeleteAddress(Guid id)
        {
            var address = db.Addresses.Where(x => x.Building.ID == id);
            db.Entry(address.First()).State = EntityState.Deleted;
            return true;
        }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }
    }
}