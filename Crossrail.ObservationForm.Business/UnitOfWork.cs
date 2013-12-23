using System;
using System.Collections.Generic;
using System.Linq;
using Crossrail.ObservationForm.DataLayer;

namespace Crossrail.ObservationForm.Business
{
    /// <summary>
    /// Manages all of the services and creates an internal db
    /// context for use by the services.
    /// </summary>

    public class UnitOfWork : IDisposable
    {
        private readonly ObservationDbContext _context;
        
        public ContractService ContractService { get; set; }

        public ObservationService ObservationService { get; set; }

        public ObservationExportService ObservationExportService { get; set; }

        public UnitOfWork()
        {
            _context = new ObservationDbContext();

            ContractService = new ContractService(_context);
            ObservationService = new ObservationService(_context);
            ObservationExportService = new ObservationExportService(_context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
