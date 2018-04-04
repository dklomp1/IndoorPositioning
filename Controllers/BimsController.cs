using IndoorPositioning.DataHandler;
using IndoorPositioning.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace IndoorPositioning.Controllers
{
    public class BimsController : ApiController
    {
        IndoorPositioningContext db = new IndoorPositioningContext();
        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> PostBIM()
        {
            //    string[] buildings = resultString.Split(new string[] { "BuildingStoreys" }, StringSplitOptions.None);
            SpacesHandler SH = new SpacesHandler(db);
            StoreysHandler STH = new StoreysHandler(db);
            BuildingsHandler BH = new BuildingsHandler(db);
            Building building = JsonConvert.DeserializeObject<Building>(Request.Content.ReadAsStringAsync().Result);
            Models.Building b = new Models.Building(building.ID, building.Name);
            BH.PostBuilding(b);
            List<BuildingStorey> storeys = building.BuildingStoreys;
            foreach(BuildingStorey storey in storeys)
            {
                Models.Storey st = new Models.Storey(storey.ID, storey.Name, b);
                STH.PostStorey(st);
                foreach(Space space in storey.Spaces)
                {
                    Models.Space s = new Models.Space(space.ID, space.Name, st);
                    SH.PostSpace(s);
                }
            }
            List<Space> spaces = storeys[0].Spaces;
            return Ok(building);
        }
        private partial class Building
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
            public List<BuildingStorey> BuildingStoreys { get; set; }
        }
        private partial class BuildingStorey
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
            public List<Space> Spaces { get; set; }
        }
        private partial class Space
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
        }
    }
}