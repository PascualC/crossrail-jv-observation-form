using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Crossrail.ObservationForm.DataLayer.Models
{
    public class Observation
    {
        public const string Required = "This field is required";

        public int ObservationId { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Type")]
        public int ObservationTypeId { get; set; }

        [Display(Name = "Category")]
        public int? ObservationCategoryId { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Contract Number")]
        public int ContractId { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Date and Time")]
        public DateTime ObservationDate { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Brief Description")]
        public string BriefDescription { get; set; }

        [Display(Name = "Category (if other):")]
        public string OtherCategory { get; set; }

        [Display(Name = "Reported By:")]
        public string ReportedBy { get; set; }

        [Display(Name = "Phone Number:")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Actions Taken")]
        public string ActionsTaken { get; set; }

        [Display(Name = "Contractor/Contractors Involved:")]
        public string ContractorInvolved { get; set; }

        public string FilePath { get; set; }

        [Display(Name = "Email me a copy:")]
        public string Email { get; set; }

        [Display(Name = "Company Reservation Number:")]
        public string CompanyObservationReference { get; set; }

        public bool IsExported { get; set; }

        //Linked Entities

        public virtual Contract Contract { get; set; }

        public virtual ObservationType ObservationType { get; set; }

        public ObservationCategory ObservationCategory { get; set; }
    }
}
