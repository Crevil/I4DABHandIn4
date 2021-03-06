﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace GUI.Model
{
    public class GDL
    {
        private readonly DbRepository _repository;

        public event EventHandler StaticDataLoaded;

        public GDL(DbRepository repository)
        {
            _repository = repository;
        }

        public void LoadStaticData()
        {
            const string originalUrl = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt";

            var t = JSONDeserialisator.DeserialiseOriginalFile(StringDownloader.DownloadStringFromURL(originalUrl));
            
            _repository.AddCollectionOfAppartments(t.Item1).Wait();
            _repository.AddCollectionOfSensors(t.Item2).Wait();

            if (StaticDataLoaded != null)
                StaticDataLoaded(this, new EventArgs());
        }

        static public ICollection<Measurement> LoadJson(int nr)
        {
            const string begin = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/";
            const string end = ".json";
            return JSONDeserialisator.DeserialiseMeasurement(StringDownloader.DownloadStringFromURL(begin + nr.ToString() + end));
        }

        /// <summary>
        /// Returns a list of measurements associated with appartments and sensor type
        /// </summary>
        /// <param name="appartments">Collection of appartments to search from</param>
        /// <param name="sensorType">Sensor type to search from</param>
        /// <returns></returns>
        public async Task<ICollection<Measurement>> GetMeasurements(ICollection<Appartment> appartments, string sensorType )
        {
            var r = await _repository.GetMeasurements(appartments, sensorType);
            return r;
        }

        /// <summary>
        /// Gets appartments from database
        /// </summary>
        /// <returns>Collection of appartments</returns>
        public ICollection<Appartment> GetAppartments()
        {
            return _repository.Appartments;
        }

        /// <summary>
        /// Gets sensors from database
        /// </summary>
        /// <returns>Collection of sensors</returns>
        public ICollection<Sensor> GetSensors()
        {
            return _repository.Sensors;
        }
    }
}
