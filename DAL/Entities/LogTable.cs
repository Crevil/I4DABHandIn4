using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        public string Operation { get; set; }
        public string LogEntryInserted { get; set; }
        public string LogEntryDeleted { get; set; }
    }
}
