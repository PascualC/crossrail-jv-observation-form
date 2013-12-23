using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossrail.ObservationForm.Domain
{
    public class ObservationType
    {
        public const string GoodPractice = "Good Practice";
        public const string UnsafeCondition = "Unsafe Condition";

        public int ObservationTypeId { get; set; }

        public string Name { get; set; }
    }
}
