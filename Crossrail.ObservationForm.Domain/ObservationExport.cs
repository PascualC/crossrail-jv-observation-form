using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossrail.ObservationForm.Domain
{
    public class ObservationExport
    {
        public Observation Observation { get; set; }
        public string AbsoluteUrl { get; set; }
    }
}