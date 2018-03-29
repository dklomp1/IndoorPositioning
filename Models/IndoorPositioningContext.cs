using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IndoorPositioning.Models
{
    public class IndoorPositioningContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public IndoorPositioningContext() : base("name=IndoorPositioningContext")
        {
            //Database.Delete("IndoorPositioningContext");
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IndoorPositioningContext, Migrations.Configuration>());
            //Configuration.LazyLoadingEnabled = false;
        }

        public System.Data.Entity.DbSet<IndoorPositioning.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<IndoorPositioning.Models.Building> Buildings { get; set; }

        public System.Data.Entity.DbSet<IndoorPositioning.Models.Tracker> Trackers { get; set; }

        public System.Data.Entity.DbSet<IndoorPositioning.Models.Storey> Storeys { get; set; }

        public System.Data.Entity.DbSet<IndoorPositioning.Models.Beacon> Beacons { get; set; }

        public System.Data.Entity.DbSet<IndoorPositioning.Models.Space> Spaces { get; set; }
        public System.Data.Entity.DbSet<IndoorPositioning.Models.Knn> Knn { get; set; }
        public System.Data.Entity.DbSet<IndoorPositioning.Models.StoreyPlan> StoreyPlans { get; set; }

        public System.Data.Entity.DbSet<IndoorPositioning.Models.TrackerLocation> TrackerLocations { get; set; }
    }
}
