using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL
{
    public class Context : DbContext
    {
        public Context() : base("GrundfosDormitoryDb")
        {
            
        }

        public DbSet<Appartment> Appartments { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Log> Logs { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Measurement>().HasKey(m => new {m.AppartmentId, m.SensorId});

            modelBuilder.Entity<Measurement>()
                .HasRequired(m => m.Appartment);

            modelBuilder.Entity<Measurement>()
                .HasRequired(m => m.Sensor);

            modelBuilder.Entity<Appartment>().MapToStoredProcedures();
            modelBuilder.Entity<Sensor>().MapToStoredProcedures();
            modelBuilder.Entity<Measurement>().MapToStoredProcedures();
        }
    }
}
