using IndoorPositioning.DataHandler;
using IndoorPositioning.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
            AddressesHandler AH = new AddressesHandler(db);
            Building building = JsonConvert.DeserializeObject<Building>(Request.Content.ReadAsStringAsync().Result);
            Models.Building b = new Models.Building(building.ID, building.Name);
            BH.PostBuilding(b);
            double? longitude = building.Address.Longitude;
            double? latitude = building.Address.Latitude;
            Models.Address address = new Models.Address(building.Address.Street, building.Address.Number, building.Address.Town, building.Address.PostalCode, building.Address.Region, building.Address.Country, longitude, latitude, b);
            AH.PostAddress(address);
            List<BuildingStorey> storeys = building.BuildingStoreys;
            foreach (BuildingStorey storey in storeys)
            {
                Models.Storey st = new Models.Storey(storey.ID, storey.Name, b);
                STH.PostStorey(st);
                foreach (Space space in storey.Spaces)
                {
                    Models.Space s = new Models.Space(space.ID, space.Name, st);
                    SH.PostSpace(s);
                }
            }
            List<Space> spaces = storeys[0].Spaces;
            return Ok(building);
        }

        [HttpPut]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> PutBuilding()
        {
            BuildingsHandler BH = new BuildingsHandler(db);
            Models.Building Building = JsonConvert.DeserializeObject<Models.Building>(Request.Content.ReadAsStringAsync().Result);

            Models.Building b = new Models.Building(Building.ID, Building.Name);
            if (!BH.PutBuilding(b))
            {
                return Content(HttpStatusCode.Conflict, Building.ID.ToString());
            }
            return Ok();
        }
        [HttpPut]
        public async Task<IHttpActionResult> PutStorey()
        {
            StoreysHandler SH = new StoreysHandler(db);
            List<Models.Storey> StoreyList = JsonConvert.DeserializeObject<List<Models.Storey>>(Request.Content.ReadAsStringAsync().Result);
            foreach (Models.Storey Storey in StoreyList)
            {
                Models.Storey s = new Models.Storey(Storey.ID, Storey.Name);
                if (!SH.PutStorey(s))
                {
                    return Content(HttpStatusCode.Conflict, Storey.ID.ToString());
                }

            }
            return Ok();
        }
        [HttpPut]
        public async Task<IHttpActionResult> PutSpace()
        {
            SpacesHandler SH = new SpacesHandler(db);
            List<Models.Space> SpaceList = JsonConvert.DeserializeObject<List<Models.Space>>(Request.Content.ReadAsStringAsync().Result);
            foreach (Models.Space Space in SpaceList)
            {
                Models.Space s = new Models.Space(Space.ID, Space.Name);
                if (!SH.PutSpace(s))
                {
                    return Content(HttpStatusCode.Conflict, Space.ID.ToString());
                }

            }
            return Ok();
        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBuilding(string ID)
        {
            bool succes = Guid.TryParse(ID, out Guid id);
            BuildingsHandler BH = new BuildingsHandler(db);
            if (!BH.DeleteBuilding(id).Result)
            {
                return Content(HttpStatusCode.Conflict, id.ToString());
            }
            return Ok();
        }
        private partial class Address{
            public string Street { get; set; }
            public string Number { get; set; }
            public string Town { get; set; }
            public string PostalCode { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public double? Longitude { get; set; }
            public double? Latitude { get; set; }
        }
        private partial class Building
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
            public List<BuildingStorey> BuildingStoreys { get; set; }
            public Address Address { get; set; }
        }
        private partial class BuildingStorey
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
            public Building Building { get; set; }

            public List<Space> Spaces { get; set; }
        }
        private partial class Space
        {
            public Guid ID { get; set; }
            public string Name { get; set; }

            public Storey Storey { get; set; }
        }
    }
}