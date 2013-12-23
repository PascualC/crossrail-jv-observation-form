using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crossrail.ObservationForm.Business;

namespace Crossrail.ObservationForm.Mvc.Controllers
{
    /// <summary>
    /// Base controller with unit of work for easy access to 
    /// service layer within controllers.
    /// </summary>

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BaseController : Controller
    {
        private UnitOfWork _unitOfWork;

        /// <summary>
        /// Use for the entire lifecycle of the controller. Disposed in <see cref="Dispose"/>
        /// </summary>

        protected UnitOfWork UnitOfWork 
        { 
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); } 
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
