using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;


namespace GUI.Model
{
    public class GDL
    {
        static private DbRepository repository = new DbRepository();

        static public void LoadOriginal()
        {
            string originalUrl = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt";

            Tuple<ICollection<Appartment>, ICollection<Sensor>> t = JSONDeserialisator.DeserialiseOriginalFile(StringDownloader.DownloadStringFromURL(originalUrl));

            repository.AddCollectionOfAppartments(t.Item1);
            repository.AddCollectionOfSensors(t.Item2);

            // CALL VIEW UPDATE 
        }

        static public ICollection<Measurement> LoadJson(int nr)
        {
            string begin = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/";
            string end = ".json";
            return JSONDeserialisator.DeserialiseMeasurement(StringDownloader.DownloadStringFromURL(begin + nr.ToString() + end));
        }


         public ICollection<Measurement> GetMeasurements(ICollection<Appartment> appartments, Sensor sensor )
         {
             return null;

             // WHAT IS THIS CRAP??
         }
    }
}
