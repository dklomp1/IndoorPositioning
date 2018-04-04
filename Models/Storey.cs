﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndoorPositioning.Models
{
    public class Storey
    {
        public Storey(Guid id, string name, Building building)
        {
            this.ID = id;
            this.Name = name;
            this.Building = building;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Required]
        public int Number { get; set; }
        public string Name { get; set; }
        //Foreign Key
        public virtual Building Building { get; set; }
    }
}