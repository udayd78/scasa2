using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class MWorkingDaysDisplay
    {
        public int TRId { get; set; }

        public int MonthNumber { get; set; }

        public int YearNumber { get; set; }

        public int NumberOfDays { get; set; }

        public string MonthName { get; set; }
    }
}
