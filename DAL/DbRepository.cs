﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repository;

namespace DAL
{
    public class DbRepository
    {
        private readonly Context _context;
        private IRepository<Appartment> _appartmentRepos;
        private IRepository<Sensor> _sensorRepos;
        private IRepository<Measurement> _measureRepos;
        public ICollection<Appartment> Appartments { get; set; }
        public ICollection<Sensor> Sensors { get; set; }
        public ICollection<Measurement> Measurements { get; set; }

        public DbRepository()
        {
            _context = new Context();
            _appartmentRepos = new Repository<Appartment>(_context);
            _sensorRepos = new Repository<Sensor>(_context);
            _measureRepos = new Repository<Measurement>(_context);
        }

        public Appartment AppartmentWithMeasurements(int id)
        {
            return _appartmentRepos.FindWithInclude(m => m.Measurements.Select(s => s.Sensor), a => a.AppartmentId == id).Result;
        }

        public async Task AddCollectionOfAppartments(ICollection<Appartment> appartments)
        {
            await _appartmentRepos.AddCollection(appartments);
            Appartments = _appartmentRepos.GetAll().Result;
        }

        public async Task AddCollectionOfSensors(ICollection<Sensor> sensors)
        {
            await _sensorRepos.AddCollection(sensors);
            Sensors = _sensorRepos.GetAll().Result;
        }

        public async Task AddCollectionOfMeasurements(ICollection<Measurement> measurements)
        {
            await _measureRepos.AddCollection(measurements);
            Measurements = _measureRepos.GetAll().Result;
        }
    }
}