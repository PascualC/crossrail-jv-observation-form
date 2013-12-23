using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossrail.ObservationForm.DataLayer.Models
{
    public class ObservationType
    {
        public int ObservationTypeId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Observation> Observations { get; set; } 
    }
}