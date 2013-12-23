using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crossrail.ObservationForm.Business.Validation;

namespace Crossrail.ObservationForm.Mvc.Models
{
    /// <summary>
    /// An implementation of <see cref="IValidationDictionary"/> for
    /// passing model state into the service layer to add more complex
    /// validation messages.
    /// </summary>

    public class ModelStateWrapper : IValidationDictionary
    {
        private readonly ModelStateDictionary _modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        #region IValidationDictionary Members

        public void AddError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }

        public void Remove(string key)
        {
            _modelState.Remove(key);
        }

        public bool IsValid
        {
            get { return _modelState.IsValid; }
        }

        #endregion
    }
}