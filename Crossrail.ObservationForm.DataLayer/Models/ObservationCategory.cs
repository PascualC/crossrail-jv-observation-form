using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Crossrail.ObservationForm.DataLayer.Models
{
    public class ObservationCategory
    {
        public int ObservationCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Reference { get; set; }

        [Required]
        public int OrderIndex { get; set; }
        
        public virtual ICollection<Observation> Observations { get; set; } 
    }
}