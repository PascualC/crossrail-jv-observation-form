using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Crossrail.ObservationForm.DataLayer.Migrations;
using Crossrail.ObservationForm.DataLayer.Models;

namespace Crossrail.ObservationForm.DataLayer
{
    public class ObservationDbContext : DbContext
    {
        //Tables

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<ObservationType> ObservationTypes { get; set; }
        public DbSet<ObservationCategory> ObservationCategories { get; set; }

        static ObservationDbContext()
        {
            //Use Code First Migrations.
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ObservationDbContext, Configuration>());
        }

        public ObservationDbContext()
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            Configuration.UseDatabaseNullSemantics = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}