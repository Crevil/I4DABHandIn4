using System;
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

        public void AddCollectionOfAppartments(ICollection<Appartment> appartments)
        {
            _appartmentRepos.AddCollection(appartments).Wait();
            Appartments = appartments;
        }

        public void AddCollectionOfSensors(ICollection<Sensor> sensors)
        {
            _sensorRepos.AddCollection(sensors).Wait();
            Sensors = sensors;
        }

        public void AddCollectionOfMeasurements(ICollection<Measurement> measurements)
        {
            _measureRepos.AddCollection(measurements).Wait();
            Measurements = measurements;
        }


    }
}
