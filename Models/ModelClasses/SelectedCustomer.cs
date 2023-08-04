using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class SelectedCustomer
    {
        public int Cid { get; set; }
        public string FullName { get; set; }
        public string CoustemerCode { get; set; }
        public int statusCode { get; set; }
        public string statusMessage { get; set; }

    }
}
