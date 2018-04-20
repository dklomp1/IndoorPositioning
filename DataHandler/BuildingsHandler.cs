﻿using System;
using System.Data.Entity;
using System.Linq;
using IndoorPositioning.Models;

namespace IndoorPositioning.DataHandler
{
    public class BuildingsHandler
    {
        IndoorPositioningContext db;

        public BuildingsHandler(IndoorPositioningContext dbcontext)
        {
            db = dbcontext;
        }
        // GET: api/Buildings
        public IQueryable<Building> GetBuildings()
        {
            return db.Buildings;
        }
        public Building GetBuilding(int id)
        {
            Building building = db.Buildings.Find(id);
            

            return building;
        }
        public void PostBuilding(Building building)
        {
            db.Buildings.Add(building);
            db.SaveChanges();
        }

        public bool PutBuilding(Building b)
        {
            Building building = db.Buildings.Find(b.ID);
            if (building == null)
            {
                return false;
            }
            
            building.Name = b.Name;
            db.Entry(building).State = EntityState.Modified;
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
        public bool DeleteBuilding(Guid b)
        {
            Building building = db.Buildings.Find(b);
            if (building == null)
            {
                return false;
            }
            foreach (Storey storey in db.Storeys.Where(x => x.Building.ID == building.ID))
            {
                StoreysHandler SH = new StoreysHandler(db);
                SH.DeleteStorey(storey);
            }
            db.Buildings.Remove(building);
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

        //// PUT: api/Buildings/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutBuilding(int id, Building building)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //if (id != building.ID)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    db.Entry(building).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        //if (!BuildingExists(id))
        //        //{
        //        //    return NotFound();
        //        //}
        //        //else
        //        //{
        //        //    throw;
        //        //}
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Buildings
        //[ResponseType(typeof(Building))]
        //public async Task<IHttpActionResult> PostBuilding(Building building)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Buildings.Add(building);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = building.ID }, building);
        //}

        //// DELETE: api/Buildings/5
        //[ResponseType(typeof(Building))]
        //public async Task<IHttpActionResult> DeleteBuilding(int id)
        //{
        //    Building building = await db.Buildings.FindAsync(id);
        //    if (building == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Buildings.Remove(building);
        //    await db.SaveChangesAsync();

        //    return Ok(building);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool BuildingExists(int id)
        //{
        //    return db.Buildings.Count(e => e.ID == id) > 0;
        //}
    }
    }