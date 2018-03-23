using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndoorPositioning.Models
{
    public class Storey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        [Required]
        public int Number { get; set; }
        public string Name { get; set; }
        //Foreign Key
        public virtual Building Building { get; set; }
    }
}