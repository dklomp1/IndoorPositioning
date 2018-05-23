using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndoorPositioning.Models
{
    public class Address
    {
        public Address() { }
        public Address(string Street, string Number, String Town, string PostalCode, string Region, string Country, double? Longitude, double? Latitude, Building Building) {
            this.Street = Street;
            this.Number = Number;
            this.Town = Town;
            this.PostalCode = PostalCode;
            this.Region = Region;
            this.Country = Country;
            this.Longitude = Longitude;
            this.Latitude = Latitude;
            this.Building = Building;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Required]
        public string Street { get; set; }
        
        public string Number { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public virtual Building Building{get; set;}

    }
}