using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class UserProfileModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "User Name rquired")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email Id required")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Mobile Number required")]
        public string MobileNumber { get; set; }
        public string UserTypeName { get; set; }
        [Required(ErrorMessage = "Address 1 Required")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? CityId { get; set; }
        public string CityId_Name { get; set; }
        public int? StateId { get; set; }
        public string StateId_Name { get; set; }
        public int? CountryId { get; set; }
        public string CountryId_Name { get; set; }
        [Required(ErrorMessage = "Postal code Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public string ZipCode { get; set; }        
        public string ProfileImage { get; set; }
        public DateTime? Logintime { get; set; }

        public List<CityDrop> cityDrops { get; set; }
        public List<StateDrop> stateDrops { get; set; }
        public List<CountryDrop> countryDrops { get; set; }
    }
}
