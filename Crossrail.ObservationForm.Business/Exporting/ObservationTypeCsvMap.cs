using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace Crossrail.ObservationForm.Business.Exporting
{
    public class ObservationTypeCsvMap : CsvClassMap<Domain.ObservationType>
    {
        public override void CreateMap()
        {
            Map(o => o.Name).Name("Type of observation");
        }
    }

}
