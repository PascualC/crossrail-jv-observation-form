using System.Web.Mvc;
using Crossrail.ObservationForm.Domain;
using Mvc.Mailer;

namespace Crossrail.ObservationForm.Mvc.Mailers
{ 
    public sealed class ObservationsMailer : MailerBase, IObservationsMailer 	
	{
		public ObservationsMailer()
		{
			MasterName = "_Layout";
		}
		
		public MvcMailMessage Details(Observation observation)
		{
            ViewData = new ViewDataDictionary(observation);

            string subject = string.Format("Crossrail Observation form - {0}", 
                observation.ObservationDate.ToShortDateString());

			return Populate(x =>
			{
                x.Subject = subject;
				x.ViewName = "Details";
				x.To.Add(observation.Email);
			});
		}
 	}
}