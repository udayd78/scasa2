using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class CustomerDisplayModel
    {
        public int Cid { get; set; }

        [Required(ErrorMessage = "User Name required")]
        public string FullName { get; set; }
        public string CoustemerCode { get; set; }
        [Required(ErrorMessage = "Mobile Number required")]
        public string MobileNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email Id required")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "WhatsApp Number required")]
        public string WhatsAppNumber { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public string RefferedBy { get; set; }
        public string OrganigationName { get; set; }
        public string OrginizationAddress { get; set; }
        public string OrganizationNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string OrganizationEmailId { get; set; }
        public bool? IsCustomer { get; set; }
        public CustomerAddressModel cAddress { get; set; }
        public List<CityDrop> cityDrops { get; set; }
        public List<StateDrop> stateDrops { get; set; }
        public List<CountryDrop> countryDrops { get; set; }
        public int CountryId { get;   set; }
        public int StateId { get;   set; }
        public string GSTNumber { get; set; }

        public int Addid { get; set; }
        public string HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string Location { get; set; }
        public int? CityId { get; set; }
        
        public string AddressType { get; set; }
        public bool? IsShipping { get; set; }
        public int? TotalOrders { get; set; }
        public int? TotalQuotations { get; set; }
        public int? ReadyToDispatch { get; set; }
        public int? DespatchedOrders { get; set; }

        public int? FinanceHeadId { get; set; }

    }
    public class CustomerAddressModel
    {
        public  int Cid { get; set; }
        public int Addid { get; set; }
        //[Required(ErrorMessage = "House Number required")]
        public string HouseNumber { get; set; }
        //[Required(ErrorMessage = "Street required")]
        public string StreetName { get; set; }
        //[Required(ErrorMessage = "Location  required")]
        public string Location { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string AddressType { get; set; }
        public bool? IsShipping { get; set; }
        public bool? IsDeleted { get;   set; }
        //[Required(ErrorMessage = "Postal Code required")]
        public string PostalCode { get; set; }
        public List<CityDrop> cityDrops { get; set; }
        public List<StateDrop> stateDrops { get; set; }
        public List<CountryDrop> countryDrops { get; set; }


    }
}