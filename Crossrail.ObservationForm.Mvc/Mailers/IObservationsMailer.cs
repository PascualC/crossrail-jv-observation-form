using Crossrail.ObservationForm.Domain;
using Mvc.Mailer;

namespace Crossrail.ObservationForm.Mvc.Mailers
{ 
    public interface IObservationsMailer
    {
		MvcMailMessage Details(Observation observation);
	}
}