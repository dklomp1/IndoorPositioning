using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IndoorPositioning.Models
{
    public class StoreyPlan
    {
        public int ID { get; set; }
        [Required]
        //Foreign Key
        public virtual Storey Storey { get; set; }
    }
}