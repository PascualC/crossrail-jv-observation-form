using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossrail.ObservationForm.Domain
{
    public class Contract
    {
        public int ContractId { get; set; }

        public string Code { get; set; }

        public string Location { get; set; }

        public string Name { get { return string.Format("{0} ({1})", Code, Location); } }
    }
}