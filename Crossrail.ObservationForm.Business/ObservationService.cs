using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossrail.ObservationForm.Business.Utilities;
using Crossrail.ObservationForm.Business.Validation;
using Crossrail.ObservationForm.DataLayer;

namespace Crossrail.ObservationForm.Business
{
    /// <summary>
    /// Observation service for all observation related methods. Provides a business logic
    /// based wrapper around the repositories for more coherent usage in the web / api tiers.
    /// </summary>
    /// <remarks>
    /// AutoMapper is used throughout this service to maintain a separate set of objects for the business
    /// layer vs. EF. This allows us greater flexibility in the objects returned from the business layer 
    /// rather than being tied to the EF schema directly.
    /// 
    /// <see cref="MapperConfiguration"/> and <see cref="Crossrail.ObservationForm.Domain"/> for the mappings
    /// themselves and the domain objects they are mapped to.
    /// </remarks>

    public class ObservationService
    {
        private readonly ObservationDbContext _context;
        private readonly Repository<DataLayer.Models.Observation> _observationRepository;
        private readonly Repository<DataLayer.Models.ObservationType> _observationTypeRepository;
        private readonly Repository<DataLayer.Models.ObservationCategory> _observationCategoryRepository;

        /// <summary>
        /// Virtual root within the web site for observation image uploads.
        /// </summary>

        private const string ObservationVirtualDirectory = "~/Uploads/Observations/";

        /// <summary>
        /// Site root for observation uploads, e.g. http://costain.staging.com/Observations
        /// </summary>

        public static string SiteRootUrl
        {
            get { return ConfigurationManager.AppSettings["SiteRootUrl"]; }
        }

        public ObservationService(ObservationDbContext context)
        {
            _context = context;
            _observationRepository = new Repository<DataLayer.Models.Observation>(context);
            _observationTypeRepository = new Repository<DataLayer.Models.ObservationType>(context);
            _observationCategoryRepository = new Repository<DataLayer.Models.ObservationCategory>(context);
        }

        public IQueryable<Domain.Observation> GetAll()
        {
            IQueryable<DataLayer.Models.Observation> observations = _observationRepository.GetAll();
            return observations.Project().To<Domain.Observation>();
        }

        public List<Domain.ObservationType> GetAllTypes()
        {
            return _observationTypeRepository.GetAll().Project().To<Domain.ObservationType>().ToList();
        }

        public List<Domain.ObservationCategory> GetAllCategories()
        {
            return _observationCategoryRepository.GetAll()
                .OrderBy(c => c.OrderIndex)
                .Project().To<Domain.ObservationCategory>()
                .ToList();
        }

        public Domain.Observation GetById(int id)
        {
            var observation = _observationRepository.GetById(id);
            return Mapper.Map<Domain.Observation>(observation);
        }

        public Domain.Observation Add(
            IValidationDictionary validationDictionary,
            Domain.Observation observation, 
            HttpPostedFileBase postedFile)
        {
            if (!Validate(validationDictionary, observation))
            {
                return null;
            }

            SetObservationValues(observation);

            var data = _observationRepository.Create();
            _observationRepository.Add(data);

            Mapper.Map(observation, data);
            
            //Reset the observation ID so that consumers cannot set this.
            data.ObservationId = 0;

            SaveFile(data, postedFile);
            _context.SaveChanges();

            //Map back to the domain so we can use the full
            //result in the web site for sending email, etc.

            return Mapper.Map<Domain.Observation>(data);
        }

        public Domain.Observation Edit(
            IValidationDictionary validationDictionary,
            Domain.Observation observation,
            HttpPostedFileBase postedFile)
        {
            if (!Validate(validationDictionary, observation))
            {
                return null;
            }

            SetObservationValues(observation);

            var data = _observationRepository.GetById(observation.ObservationId);
            data = Mapper.Map(observation, data);

            SaveFile(data, postedFile);
            _context.SaveChanges();

            //Map back to the domain so we can use the full
            //result in the web site for sending email, etc.

            return Mapper.Map<Domain.Observation>(data);
        }

        private static void SetObservationValues(Domain.Observation observation)
        {
            //Currently just map the time from the observation onto to the single
            //date property within the data layer. 

            observation.ObservationDate = observation.ObservationDate
                .AddHours(observation.ObservationTime.Hour)
                .AddMinutes(observation.ObservationTime.Minute);
        }

        public void Remove(int observationId)
        {
            _observationRepository.Remove(observationId);
        }

        /// <summary>
        /// Performs field logic that cannot be represented with attributes for an observation. 
        /// </summary>
        /// <param name="validationDictionary">A dictionary that will contain the resulting errors</param>
        /// <param name="observation">The observation to validate</param>
        /// <returns><c>true</c> for success, <c>false</c> for failure</returns>
        /// <remarks>
        /// This function should be used in conjunction with the built-in data annotations to validate
        /// simple conditions such as required.
        /// </remarks>

        public bool Validate(IValidationDictionary validationDictionary, Domain.Observation observation)
        {
            var type = _observationTypeRepository.GetById(observation.ObservationTypeId);

            //Email

            if (observation.EmailYesOrNo)
            {
                if (string.IsNullOrWhiteSpace(observation.Email))
                {
                    //Email validation is handled by the [EmailAddress] attribute on the domain
                    validationDictionary.AddError("Email", Domain.Observation.Required);
                }
            }
            else
            {
                validationDictionary.Remove("Email");
                observation.Email = null;
            }

            //Ensure type has a value first, may be a 0 / null type.

            if (type == null)
            {
                validationDictionary.AddError("ObservationTypeId", Domain.Observation.Required);
            }
            else
            {
                //Remove the built-in validation for observation category and other category
                //since we will override the rules to be more specific based on conditional
                //logic here.

                validationDictionary.Remove("ObservationCategoryId");
                validationDictionary.Remove("OtherCategory");

                if (type.Name == Domain.ObservationType.UnsafeCondition)
                {
                    //A category is required if the observation type is "Unsafe Condition"

                    if (!observation.ObservationCategoryId.HasValue)
                    {
                        validationDictionary.AddError("ObservationCategoryId", Domain.Observation.Required);
                    }
                    else 
                    {
                        var category = _observationCategoryRepository.GetById(observation.ObservationCategoryId.Value);

                        //If other specified, we need a reason (OtherCategory)

                        if (category.Reference == Domain.ObservationCategory.Other &&
                            string.IsNullOrWhiteSpace(observation.OtherCategory))
                        {
                            validationDictionary.AddError("OtherCategory", Domain.Observation.Required);
                        }
                        else
                        {
                            observation.OtherCategory = null;
                        }
                    }
                }
                else
                {
                    //Remove category Id and other category if "Good Practice" is specified.

                    observation.ObservationCategoryId = null;
                    observation.OtherCategory = null;
                }
            }

            //Return whether the validation dictionary is valid. This is so that
            //we can extend the result given initially, for required etc.

            return validationDictionary.IsValid;
        }

        /// <summary>
        /// Retrieve a virtual path for an observation.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>

        public static string GetVirtualPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            return VirtualPathUtility.Combine(ObservationVirtualDirectory, filePath);
        }

        /// <summary>
        /// Gets a fully qualified URL for the observation image.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>

        public static string GetAbsoluteUrl(string filePath)
        {
            string virtualPath = GetVirtualPath(filePath);

            if (!string.IsNullOrEmpty(virtualPath))
            {
                string absolutePath = VirtualPathUtility.ToAbsolute(virtualPath, "/");
                return UrlUtilities.Combine(SiteRootUrl, absolutePath);
            }

            return null;
        }

        /// <summary>
        /// Saves files in a MMMM/yyyy/{Guid} structure for observation images, to avoid
        /// a single directory being filled over time.
        /// </summary>
        /// <param name="observation">The observation to save the file path against</param>
        /// <param name="postedFile">A posted file to save</param>
        /// <returns>
        /// the relative path of the file for storage in the database. We do not want 
        /// to store the full virtual path so that the files can easily
        /// be migrated elsewhere without having to update the database.
        /// </returns>

        private void SaveFile(DataLayer.Models.Observation observation, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                HttpServerUtility server = HttpContext.Current.Server;

                //Use the relative path provided if we already have one, so that we can overwrite
                //the existing upload.

                if (string.IsNullOrWhiteSpace(observation.FilePath))
                {
                    // - {Guid}.jpg
                    string filename = Guid.NewGuid() + Path.GetExtension(postedFile.FileName);

                    // - C:/.../Uploads/
                    var uploadFolder = new DirectoryInfo(server.MapPath(ObservationVirtualDirectory));

                    // - "2012/Feb"
                    string directoryRelativePath = CreateUploadDirectory(uploadFolder.FullName);

                    // - "2012/Feb/{Guid}.jpg"
                    observation.FilePath = UrlUtilities.Combine(directoryRelativePath, filename);
                }

                // - "~/Uploads/2012/Feb/{GUID}.xxx"
                string virtualFilePath = VirtualPathUtility.Combine(ObservationVirtualDirectory, observation.FilePath);

                // - Save the file
                postedFile.SaveAs(server.MapPath(virtualFilePath));
            }
        }

        /// <summary>
        /// Create a sub-directory from the path passed in.
        /// </summary>
        /// <param name="path">The full server path of the root directory</param>
        /// <returns>A relative path of the folder created for web use, e.g. "2013/December"</returns>

        private string CreateUploadDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DateTime now = DateTime.Now;

            var yearFolder = directoryInfo.CreateSubdirectory(now.Year.ToString());
            var monthFolder = yearFolder.CreateSubdirectory(now.ToString("MMMM"));

            return yearFolder.Name + "/" + monthFolder.Name;
        }
    }
}