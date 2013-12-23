using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace Crossrail.ObservationForm.Business.Exporting
{
    public class ObservationCsvMap : CsvClassMap<Domain.Observation>
    {
        public override void CreateMap()
        {
            References<ContractCsvMap>(o => o.Contract);
            References<ObservationTypeCsvMap>(o => o.ObservationType);

            Map(o => o.Location).Name("Location");
            Map(o => o.ObservationDate).Name("Date of observation");
            Map(o => o.BriefDescription).Name("Brief description");
            Map(o => o.ObservationCategoryName).Name("Observation category");
            Map(o => o.OtherCategory).Name("Other category");
            Map(o => o.ReportedBy).Name("Reported by");
            Map(o => o.PhoneNumber).Name("Phone Number");
            Map(o => o.ActionsTaken).Name("Actions Taken");
            Map(o => o.ContractorInvolved).Name("Contractor(s) Involved");

            Map(o => o.EmailYesOrNo).Name("Email (Y/N)");
            Map(o => o.Email).Name("Email address");
            Map(o => o.CompanyObservationReference).Name("Company Reservation Number");
        }
    }
}
