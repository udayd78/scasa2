using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class AddressMaster
    {
        [Key]
        public int Addid { get; set; }
        public string HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string Location { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string AddressType { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsShipping { get; set; }
        public string PostalCode { get; set; }
    }
}
