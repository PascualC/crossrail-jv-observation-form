using System.Collections.Generic;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Crossrail.ObservationForm.DataLayer.Models;

namespace Crossrail.ObservationForm.DataLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ObservationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Crossrail.ObservationForm.DataLayer.ObservationDbContext";
        }

        protected override void Seed(ObservationDbContext context)
        {
            //Contracts

            List<Contract> contracts = new List<Contract>
            {
                new Contract  { Code = "C405", Location = "Paddington" },
                new Contract  { Code = "C411", Location = "Bond Street" },
                new Contract  { Code = "C360", Location = "Mile End" },
            };

            contracts.ForEach(c => context.Contracts.AddOrUpdate(i => i.Code, c));
            context.SaveChanges();

            //Type of Observation

            List<ObservationType> observationTypes = new List<ObservationType>
            {
                new ObservationType { Name = "Good Practice" },
                new ObservationType { Name = "Unsafe Condition" },
            };

            observationTypes.ForEach(o => context.ObservationTypes.AddOrUpdate(i => i.Name, o));
            context.SaveChanges();

            //Observation Categories
            // - Order index to preserve order (non-alphabetical)
            // - Reference for validation

            List<ObservationCategory> observationCategories = new List<ObservationCategory>
            {
                new ObservationCategory { Name = "Excavation/confined space", Reference = "ExcavationConfinedSpace", OrderIndex = 0 },
                new ObservationCategory { Name = "PPE", Reference = "PPE", OrderIndex = 1 },
                new ObservationCategory { Name = "Slips/trips", Reference = "SlipsTrips", OrderIndex = 2 },
                new ObservationCategory { Name = "Health/well-being", Reference = "HealthWellbeing", OrderIndex = 3 },
                new ObservationCategory { Name = "Heat/sparks/flame", Reference = "HeatSparksFlame", OrderIndex = 4 },
                new ObservationCategory { Name = "Environmental issues (noise/dust/lighting/hazardous substances)", Reference = "Environmental", OrderIndex = 5 },
                new ObservationCategory { Name = "Plant/equipment (including lifting)", Reference = "PlantEquipment", OrderIndex = 6 },
                new ObservationCategory { Name = "Risk of person falling", Reference = "RiskOfPersonFalling", OrderIndex = 7 },
                new ObservationCategory { Name = "Access/egress", Reference = "AccessEgress", OrderIndex = 8 },
                new ObservationCategory { Name = "Electrical hazard", Reference = "ElectricalHazard", OrderIndex = 9 },
                new ObservationCategory { Name = "Access equipment",  Reference = "AccessEquipment", OrderIndex = 10 },
                new ObservationCategory { Name = "Manual handling", Reference = "ManualHandling", OrderIndex = 11 },

                //This could be a nullable category ID but it does make validation and displaying
                //this category a pain, since we would have to either add on an extra radio in the
                //UI which wouldn't have a value, and then ensure client validation still works
                //correctly. The data requirements should mean that adding this in is as a extra
                //category is fine.

                new ObservationCategory { Name = "Other category not mentioned e.g paperwork", Reference = "Other", OrderIndex = 12 },
            };

            observationCategories.ForEach(oc => context.ObservationCategories.AddOrUpdate(i => i.Name, oc));
            context.SaveChanges();
        }
    }
}
