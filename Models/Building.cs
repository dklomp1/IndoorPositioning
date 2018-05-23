using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndoorPositioning.Models
{
    public class Building
    {
        public Building() { }
        public Building(Guid ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid ID { get; set; }
        public string Name { get; set; }

    }
}