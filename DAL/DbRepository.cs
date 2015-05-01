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
        private readonly IRepository<Appartment> _appartmentRepos;
        private readonly IRepository<Sensor> _sensorRepos;
        private readonly IRepository<Measurement> _measureRepos;
        public ICollection<Appartment> Appartments { get; set; }
        public ICollection<Sensor> Sensors { get; set; }

        public DbRepository()
        {
            var context = new Context();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Measurements");
            context.Database.ExecuteSqlCommand("DELETE FROM Sensors");
            context.Database.ExecuteSqlCommand("DELETE FROM Appartments");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Logs");
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Sensors, RESEED, 0)");
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Appartments, RESEED, 0)");

            _appartmentRepos = new Repository<Appartment>(context);
            _sensorRepos = new Repository<Sensor>(context);
            _measureRepos = new Repository<Measurement>(context);
            Appartments = new List<Appartment>();
            Sensors = new List<Sensor>();
        }

        public async Task<ICollection<Measurement>> GetMeasurements(ICollection<Appartment> appartments, string sensorType)
        {
            var returnList = new List<Measurement>();

            foreach (var appartment in appartments)
            {
                var appartment1 = appartment;
                var measurements = await _measureRepos.FindAllDoubleWhere(
                                                            m => m.AppartmentId == appartment1.AppartmentId, 
                                                            m => m.Sensor.Description == sensorType
                );

                returnList.AddRange(measurements.ToList());
            }
            return returnList;
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
        }
    }
}
