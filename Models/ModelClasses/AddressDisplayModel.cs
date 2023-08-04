using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class AddressDisplayModel
    {
        public int Addid { get; set; }
        public string HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string Location { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string AddressType { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsShipping { get; set; }
        public string PostalCode { get; set; }
    }
}
