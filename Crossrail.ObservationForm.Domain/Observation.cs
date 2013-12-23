using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Crossrail.ObservationForm.Domain
{
    public class Observation
    {
        public const string Required = "This field is required";

        public int ObservationId { get; set; }

        [Required(ErrorMessage = Required)]
        [Range(1, int.MaxValue, ErrorMessage = Required)]
        [Display(Name = "Type of observation")]

        public int ObservationTypeId { get; set; }

        [Required(ErrorMessage = Required)]
        [Range(1, int.MaxValue, ErrorMessage = Required)]
        [Display(Name = "Select category of observation")]

        public int? ObservationCategoryId { get; set; }

        /// <summary>
        /// Category Name is nullable and not linked via an entity
        /// reference due to the nullable-ness. We would have to
        /// give the linked entity a nullable-ID which does not seem
        /// correct. 
        /// </summary>

        [Display(Name = "Category Name")]
        public string ObservationCategoryName { get; set; }

        [Required(ErrorMessage = Required)]
        [Range(1, int.MaxValue, ErrorMessage = Required)]
        [Display(Name = "Contract Number")]

        public int ContractId { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Location of observation")]
        public string Location { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Date of observation")]
        public DateTime ObservationDate { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Time of observation"), DataType(DataType.Time)]
        public DateTime ObservationTime { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Brief Description of your observation")]
        public string BriefDescription { get; set; }

        //if ObservationCategoryId == 11, OtherCategory is required

        [Required(ErrorMessage = Required)]
        [Display(Name = "Area to explain other category"), DataType(DataType.EmailAddress)]

        public string OtherCategory { get; set; }

        [Display(Name = "Reported by full name")]
        public string ReportedBy { get; set; }

        /// <summary>
        /// Phone number is a string but we only allow numbers and spaces to restrict
        /// to phone numbers (roughly). Wouldn't normally add such validation but
        /// it was a customer requirement.
        /// </summary>

        [Display(Name = "Phone Number")]
        [RegularExpression("^[\\d ]+$", ErrorMessage = "Only numbers and spaces are allowed")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Actions taken following your observation")]
        public string ActionsTaken { get; set; }

        [Display(Name = "If known contractor or contractors involved")]
        public string ContractorInvolved { get; set; }

        [Display(Name = "Add a photo")]
        public string FilePath { get; set; }

        public string VirtualPath { get; set; }

        [Required(ErrorMessage = Required)]
        [Display(Name = "Email me a copy of this observation card")]
        public bool EmailYesOrNo { get; set; }

        //if checkbox checked, Email is required
        [Required(ErrorMessage = Required)]
        [Display(Name = "Your email")]
        [EmailAddress(ErrorMessage = "This field is not a valid e-mail address.")]

        public string Email { get; set; }

        [Display(Name = "Add your company reservation number")]
        public string CompanyObservationReference { get; set; }

        public bool IsExported { get; set; }

        //Linked Entities

        public ObservationType ObservationType { get; set; }

        public Contract Contract { get; set; }
    }
}
