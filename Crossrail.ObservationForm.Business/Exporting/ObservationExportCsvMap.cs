using System;
using System.Collections.Generic;
using System.Linq;
using Crossrail.ObservationForm.Domain;
using CsvHelper.Configuration;

namespace Crossrail.ObservationForm.Business.Exporting
{
    public class ObservationExportCsvMap : CsvClassMap<ObservationExport>
    {
        public override void CreateMap()
        {
            References<ObservationCsvMap>(o => o.Observation);
            Map(o => o.AbsoluteUrl).Name("Image");
        }
    }
}
