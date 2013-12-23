using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossrail.ObservationForm.DataLayer.Models
{
    public class Contract
    {
        public int ContractId { get; set; }

        public string Code { get; set; }

        public string Location { get; set; }

        public virtual ICollection<Observation> Observations { get; set; } 
    }
}
