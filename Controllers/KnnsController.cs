using IndoorPositioning.DataHandler;
using IndoorPositioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace IndoorPositioning.Controllers
{
    public class KnnsController : ApiController
    {
        KnnsHandler KC;
        [HttpPost]
        [ResponseType(typeof(Knn))]
        public async Task<IHttpActionResult> PostKnnTraining()
        {
            HttpPostedFileBase file = Request.Content.ReadAsByteArrayAsync();
            byte[] file = Request.Content.ReadAsByteArrayAsync();
            return Ok(file);
        }
    }
}