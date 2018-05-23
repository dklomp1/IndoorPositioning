using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IndoorPositioning.Models
{
    public class TrainingValue
    {
        public TrainingValue() { }
        public TrainingValue(Space Space,string Values)
        {
            this.Space = Space;
            this.Values = Values;
        }
        [Required]
        public int ID { get; set; }
        public string Values { get; set; }
        public virtual Space Space { get; set; }
    }
}