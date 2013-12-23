using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossrail.ObservationForm.Domain
{
    public class ObservationCategory
    {
        public const string Other = "Other";

        public int ObservationCategoryId { get; set; }

        public string Name { get; set; }
    }
}
