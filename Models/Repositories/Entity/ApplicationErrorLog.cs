using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class ApplicationErrorLog
    {
        [Key]
        public int ID { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }
        public string InnerException { get; set; }
        public string Source { get; set; }
        public DateTime? ExceptionDateTime { get; set; }
        public string ModuleName { get; set; }
    }
}
