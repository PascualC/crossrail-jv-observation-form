using System;
using System.Collections.Generic;
using System.Linq;
using Crossrail.ObservationForm.Business;

namespace Crossrail.ObservationForm.ExportScheduler
{
    class Program
    {
        static void Main()
        {
            MapperConfiguration.RegisterMappings();

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.ObservationExportService.Export();
            }
        }
    }
}
