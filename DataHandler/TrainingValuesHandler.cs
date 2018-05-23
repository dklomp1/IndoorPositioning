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
    public class TrainingValuesHandler : ApiController
    {
        private IndoorPositioningContext db;
        public TrainingValuesHandler(IndoorPositioningContext db)
        {
            this.db = db;
        }
        // GET: api/TrainingValues
        public IQueryable<TrainingValue> GetTrainingValuesFromStorey(Guid StoreyID)
        {
            return db.TrainingValues.Where(x => x.Space.Storey.ID == StoreyID);
        }
        public async Task<bool> DeleteTrainingValues(Space space)
        {
            var trainingValues = db.TrainingValues.Where(x => x.Space.ID == space.ID);
            if (trainingValues.Any()) {
                foreach (TrainingValue tv in trainingValues)
                {
                    db.Entry(tv).State = EntityState.Deleted;
                }
            }
            return true;
        }
        // GET: api/TrainingValues/5
        [ResponseType(typeof(TrainingValue))]
        public async Task<IHttpActionResult> GetTrainingValue(int id)
        {
            TrainingValue trainingValue = await db.TrainingValues.FindAsync(id);
            if (trainingValue == null)
            {
                return NotFound();
            }

            return Ok(trainingValue);
        }

        // PUT: api/TrainingValues/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrainingValue(int id, TrainingValue trainingValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trainingValue.ID)
            {
                return BadRequest();
            }

            db.Entry(trainingValue).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingValueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        
        public void PostTrainingValue(TrainingValue trainingValue)
        {
            db.TrainingValues.Add(trainingValue);
            db.SaveChangesAsync();
        }

        // DELETE: api/TrainingValues/5
        [ResponseType(typeof(TrainingValue))]
        public async Task<IHttpActionResult> DeleteTrainingValue(int id)
        {
            TrainingValue trainingValue = await db.TrainingValues.FindAsync(id);
            if (trainingValue == null)
            {
                return NotFound();
            }

            db.TrainingValues.Remove(trainingValue);
            await db.SaveChangesAsync();

            return Ok(trainingValue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrainingValueExists(int id)
        {
            return db.TrainingValues.Count(e => e.ID == id) > 0;
        }
    }
}