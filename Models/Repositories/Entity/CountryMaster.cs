using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class CountryMaster
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CurrenceString { get; set; }
        public string PhoneCode { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
