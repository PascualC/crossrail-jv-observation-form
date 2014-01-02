using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Crossrail.ObservationForm.Business.Exporting;
using Crossrail.ObservationForm.Domain;
using CsvHelper;
using AutoMapper;

namespace Crossrail.ObservationForm.Business
{
    public class ObservationExportService
    {
        private const string MimeTypeCsv = "text/csv";

        private readonly DataLayer.ObservationDbContext _context;
        private readonly DataLayer.Repository<DataLayer.Models.Observation> _observationRepository;

        public string MaillAddressTo 
        {
            get { return ConfigurationManager.AppSettings["ObservationExportMailAddressTo"]; }
        }

        public ObservationExportService(DataLayer.ObservationDbContext context)
        {
            _context = context;
            _observationRepository = new DataLayer.Repository<DataLayer.Models.Observation>(_context);
        }

        /// <summary>
        /// Will export anything that hasn't yet been exported. This can be constituted by <see cref="Observation.IsExported"/>
        /// that tracks if the observation has been exported. We can then export at
        /// any time of the day and only export new ones in this case.
        /// </summary>

        public void Export()
        {
            var observations = GetAllForExport().ToList();

            if (!observations.Any())
            {
                //Don't send any emails etc, nothing to send.
                return;
            }

            IEnumerable<ObservationExport> observationsExportData = observations
                .Select(observation => new ObservationExport
                {
                    //Map to domain object so we have a bit more information

                    Observation = Mapper.Map<Observation>(observation),

                    //Inject custom properties here for export that can't be retrieved
                    //by the main data query.

                    AbsoluteUrl = ObservationService.GetAbsoluteUrl(observation.FilePath)
                });

            using (var memoryStream = new MemoryStream())
            using (var textWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(textWriter))
            {
                csvWriter.Configuration.RegisterClassMap<ObservationExportCsvMap>();

                //Use WriteRecords rather than a single WriteRecord as it doesn't seem to add 
                //header rows for some reason? Presumably because you are only writing a single
                //item at a time?

                csvWriter.WriteRecords((IEnumerable)observationsExportData);

                //Flush the buffer and seek back to the start for output.

                textWriter.Flush();
                memoryStream.Position = 0;

                SendEmail(memoryStream);
            }

            //Mark the records as exported now that we have "exported" the list.

            foreach (var observation in observations)
            {
                observation.IsExported = true;
            }

            _context.SaveChanges();
        }

        /// <remarks>
        /// We need to be working with the data layer models initially so that we can
        /// easily update and submit changes to these objects, rather than losing it
        /// through the automapper projection.
        /// </remarks>

        private IQueryable<DataLayer.Models.Observation> GetAllForExport()
        {
            return _observationRepository.GetAll().Where(o => !o.IsExported);
        }

        private string GetExportFilename()
        {
            //Date format: 20-12-2013-1046. Seems fairly readable. Date Formats
            //suffer from characters that don't particularly suit emails.

            return string.Format("Export-{0:dd'-'MM'-'yyyy'-'HHmm}.csv", DateTime.Now);
        }

        private void SendEmail(Stream attachmentContentStream)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                MailMessage mailMessage = new MailMessage();

                mailMessage.To.Add(MaillAddressTo);
                mailMessage.Attachments.Add(new Attachment(attachmentContentStream, GetExportFilename(), MimeTypeCsv));

                mailMessage.IsBodyHtml = false;
                mailMessage.Subject = "Crossrail Observation form Version 2.0 - Export";
                
                smtpClient.Send(mailMessage);
            }
        }
    }
}