using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class UserMasterModel
    {

        public int UserId { get; set; }
        [Required(ErrorMessage ="User Name rquired")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email Id required")]
        public string EmailId { get; set; }
        [Required(ErrorMessage ="Mobile Number required")]
        public string MobileNumber { get; set; }
       
        public string PWord { get; set; }
        public int? UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        [Required(ErrorMessage = "Address 1 Required")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int? CountryId { get; set; }
        [Required(ErrorMessage = "Postal code Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public string ZipCode { get; set; }
        public string ReferredbyName { get; set; }
        public string ReferredByMobile { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string ProfileImage { get; set; }

        public IFormFile ProfileImageUploaded { get; set; }
        public decimal? MaxDiscoutPercentage { get; set; }
        public string ReleivedRemarks { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReleivedOn { get; set; }
        [Required(ErrorMessage = "Employee code Required")]
        public string EmployeeCode { get; set; }
        public string CurrentStatus { get; set; }

        public string PANNumber { get; set; }
        public string AadharNumber { get; set; }
        public string PFNumber { get; set; }
        public string UANNumber { get; set; }
        public string ESICNumber { get; set; }
        public string BioMetricId { get; set; }
        public string Gender { get; set; }
        public string PreviousOrgName { get; set; }
        public string PreviousOrgAddress { get; set; }
        public decimal? PreviousOrgExperience { get; set; }
        public decimal? TotalPreviousExpeience { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public string AccountHolderName { get; set; }
        public string BranchAddress { get; set; }
        public string UPINumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SubMitedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }

        public DateTime? ReviewedOn { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public int ReportingManager { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfJoin { get; set; }
        public int? ShiftId { get; set; }
        public string FormFillUrl { get; set; }

        public List<UserTypeDrop> userTypeDrops { get; set; }
        public List<CityDrop> cityDrops{ get; set; }
        public List<StateDrop> stateDrops{ get; set; }
        public List<CountryDrop> countryDrops{ get; set; }
        public List<StaffDrop> StaffDrops { get; set; }
        public List<ShiftDrop> ShiftDrops { get; set; }
        public int PRMId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? BasicSalary { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? HRA { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? DearnessAllowance { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? FoodAllowance { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? Conveyance { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? MedicalAllowances { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? OtherAllowances { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? ProvidentFund { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? ProfessionalTax { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? ESIFund { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid number")]
        public decimal? TDS { get; set; }

        public IFormFile PanCardUploaded { get; set; }
        public string PanCard { get; set; }
        public IFormFile AadharCardUploaded { get; set; }
        public string AdharCard { get; set; }
        public IFormFile ResumeUploaded { get; set; }
        public string ResumeDoc { get; set; }
        public IFormFile SalarySlipUploaded { get; set; }
        public string SalarySlip { get; set; }
        public IFormFile PreviousRelievingLetterUploaded { get; set; }
        public string PreviousRelievingLetter { get; set; }
        public IFormFile AadharBackUploaded { get; set; }
        public string AadharBack { get; set; }
        public string OfferLetterLink { get; set; }
        public string WhatsappNumber { get; set; }
        public string OfficialEmail { get; set; }
        public string DisplayName { get; set; }
    }
}
