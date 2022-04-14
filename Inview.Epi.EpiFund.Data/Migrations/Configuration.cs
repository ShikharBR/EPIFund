namespace Inview.Epi.EpiFund.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Inview.Epi.EpiFund.Domain.Entity;
    using Inview.Epi.EpiFund.Domain.Enum;

    internal sealed class Configuration : DbMigrationsConfiguration<Inview.Epi.EpiFund.Data.EPIRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true ;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Inview.Epi.EpiFund.Data.EPIRepository context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.DeferredMaintenanceCosts.AddOrUpdate(x => x.MaintenanceDetail,
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.ExteriorRenovations,
                InputType = "Buildings"

            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.CoveredParkingInstall,
                InputType = "Units"
            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.CoveredParkingStructure,
                InputType = "Units"
            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.Lighting,
                InputType = "Units"

            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.ParkingLot,
                InputType = null

            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.SuiteBuildOut,
                InputType = "Total Sq.Ft"

            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.LeaseUpCommissions,
                InputType = "Base Estimate"

            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.Other2,
                InputType = "Units"
            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.PavementRepair,
                InputType = "Units"
            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.ExteriorLighting,
                InputType = "Units"
            },
            new DeferredMaintenanceCost()
            {
                Cost = 0,
                MaintenanceDetail = MaintenanceDetails.ParkOwnedRepairs,
                InputType = "Units"
            }

        );
        }

       
    }
}
