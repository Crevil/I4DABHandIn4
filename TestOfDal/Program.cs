using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace TestOfDal
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Context())
            {
                var test = new Location(1,1,1);
                db.Locations.Add(test);
                db.SaveChanges();
            }
        }
    }
}
