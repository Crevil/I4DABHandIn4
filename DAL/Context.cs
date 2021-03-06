﻿using System;
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
        public Context() : base("GrundfosDormitoryDbIHA")
        {
            
        }

        public DbSet<Appartment> Appartments { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Log> Logs { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Measurement>().HasKey(m => new {m.SensorId, m.Timestamp});

            modelBuilder.Entity<Measurement>()
                .HasRequired(m => m.Appartment);

            modelBuilder.Entity<Measurement>()
                .HasRequired(m => m.Sensor);

            modelBuilder.Entity<Measurement>()
                .Property(d => d.Timestamp)
                .HasColumnType("datetime2")
                .HasPrecision(3);

            modelBuilder.Entity<Appartment>().MapToStoredProcedures();
            modelBuilder.Entity<Sensor>();
            modelBuilder.Entity<Measurement>().MapToStoredProcedures();
        }
    }
}
