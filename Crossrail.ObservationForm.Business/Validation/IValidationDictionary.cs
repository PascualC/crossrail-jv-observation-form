using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossrail.ObservationForm.Business.Validation
{
    public interface IValidationDictionary
    {
        void AddError(string key, string errorMessage);
        void Remove(string key);
        bool IsValid { get; }
    }
}