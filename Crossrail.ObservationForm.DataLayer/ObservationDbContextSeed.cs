using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Crossrail.ObservationForm.DataLayer.Models;

namespace Crossrail.ObservationForm.DataLayer
{
    /// <summary>
    /// An EF code first seed to add default contracts, observation types
    /// and categories that we need to get the application running.
    /// </summary>

    public class ObservationDbContextSeed : DropCreateDatabaseIfModelChanges<ObservationDbContext>
    {
        protected override void Seed(ObservationDbContext context)
        {
            //Contracts

            List<Contract> contracts = new List<Contract>
            {
                new Contract  { Code = "C405", Location = "Paddington" },
                new Contract  { Code = "C411", Location = "Bond Street" },
                new Contract  { Code = "C360", Location = "Mile End" },
            };

            contracts.ForEach(c => context.Contracts.Add(c));
            context.SaveChanges();

            //Type of Observation

            List<ObservationType> observationTypes = new List<ObservationType>
            {
                new ObservationType { Name = "Good Practice" },
                new ObservationType { Name = "Unsafe Condition" },
            };

            observationTypes.ForEach(o => context.ObservationTypes.Add(o));
            context.SaveChanges();

            //Observation Categories

            List<ObservationCategory> observationCategories = new List<ObservationCategory>
            {
                new ObservationCategory { Name = "Excavation/confined space" },
                new ObservationCategory { Name = "PPE" },
                new ObservationCategory { Name = "Slips/Trips" },
                new ObservationCategory { Name = "Health/well-being" },
                new ObservationCategory { Name = "Heat/sparks/flame" },
                new ObservationCategory { Name = "Environmental issues (noise/dust/lighting/hazardous substances)" },
                new ObservationCategory { Name = "Plant/equipment (including lifting)" },
                new ObservationCategory { Name = "Risk of person falling" },
                new ObservationCategory { Name = "Access/egress" },
                new ObservationCategory { Name = "Electrical hazard" },
                new ObservationCategory { Name = "Access equipment" },
                new ObservationCategory { Name = "Manual handling" },

                //This could be a nullable category ID but it does make validation and displaying
                //this category a pain, since we would have to either add on an extra radio in the
                //UI which wouldn't have a value, and then ensure client validation still works
                //correctly. The data requirements should mean that adding this in is as a extra
                //category is fine.

                new ObservationCategory { Name = "Other category not mentioned e.g paperwork" },
            };

            observationCategories.ForEach(o => context.ObservationCategories.Add(o));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
