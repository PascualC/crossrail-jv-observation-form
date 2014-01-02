using System;
using System.Collections.Generic;
using System.Linq;
using Crossrail.ObservationForm.Business;
using Elmah;

namespace Crossrail.ObservationForm.ExportScheduler
{
    class Program
    {
        static void Main()
        {
            try
            {
                MapperConfiguration.RegisterMappings();

                using (var unitOfWork = new UnitOfWork())
                {
                    unitOfWork.ObservationExportService.Export();
                }
            }
            catch (Exception ex)
            {
                //Don't catch failed logging errors, ensure that logging is fixed.

                ErrorLog.GetDefault(null).Log(new Error(ex));
                Console.WriteLine("An error has occurred. See elmah for more details");
            }
        }
    }
}