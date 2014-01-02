using System.Web;
using System.Web.Mvc;
using Elmah;

namespace Crossrail.ObservationForm.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ElmahHandledErrorLoggerFilter());
        }
    }

    public class ElmahHandledErrorLoggerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //Log all errors, if it is already handled by the handle error attribute
            //elmah would not log the error.

            ErrorSignal.FromCurrentContext().Raise(context.Exception);
        }
    }
}