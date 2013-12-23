using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace Crossrail.ObservationForm.Business.Exporting
{
    public class ContractCsvMap : CsvClassMap<Domain.Contract>
    {
        public override void CreateMap()
        {
            Map(o => o.Name).Name("Contact Number");
        }
    }
}
