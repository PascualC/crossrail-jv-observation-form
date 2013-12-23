using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Crossrail.ObservationForm.Business
{
    public static class MapperConfiguration
    {
        public static void RegisterMappings()
        {
            //Set the virtual path in the mapping. More complicated properties should not be 
            //resolved here and alternative methods should be taken.

            Mapper.CreateMap<DataLayer.Models.Observation, Domain.Observation>()
                .ForMember(m => m.VirtualPath, m => m.Ignore())
                .ForMember(m => m.ObservationTime, m => m.MapFrom(o => o.ObservationDate))
                .ForMember(m => m.EmailYesOrNo, m => m.Ignore())
                .ForMember(m => m.ObservationCategoryName, m => m.MapFrom(o => o.ObservationCategory.Name));

            Mapper.CreateMap<Domain.Observation, DataLayer.Models.Observation>()
                .ForMember(m => m.ObservationType, m => m.Ignore())
                .ForMember(m => m.ObservationCategory, m => m.Ignore())
                .ForMember(m => m.Contract, m => m.Ignore())
                //File path is maintained by the service layer, so mapping this
                //will clear any existing file path from the data access object.
                .ForMember(m => m.FilePath, m => m.Ignore());

            Mapper.CreateMap<DataLayer.Models.ObservationCategory, Domain.ObservationCategory>();
            Mapper.CreateMap<DataLayer.Models.ObservationType, Domain.ObservationType>();
            Mapper.CreateMap<DataLayer.Models.Contract, Domain.Contract>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
