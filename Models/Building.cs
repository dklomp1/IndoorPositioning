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
        public Building(Guid id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        //Foreign Key
        public virtual Address Address { get; set; }

    }
}