using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndoorPositioning.Models
{
    public class Space
    {
        public Space(Guid id, string name, Storey storey)
        {
            this.ID = id;
            this.Name = name;
            this.Storey = storey;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        //Foreign Key        
        public virtual Storey Storey { get; set; }

    }
}