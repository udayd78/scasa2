using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class UserMasterDisplay
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string PWord { get; set; }
        public int? UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? CityId { get; set; }
        public string CityId_Name { get; set; }
        public int? StateId { get; set; }
        public string StateId_Name { get; set; }
        public int? CountryId { get; set; }
        public string CountryId_Name { get; set; }
        public string ZipCode { get; set; }
        public string ReferredbyName { get; set; }
        public string ReferredByMobile { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string ProfileImage { get; set; }
        public decimal MaxDiscoutPercentage { get; set; }
        public string ReleivedRemarks { get; set; }
        public DateTime ReleivedOn { get; set; }
        public string EmployeeCode { get; set; }
        public string CurrentStatus { get; set; }

        public string PANNumber { get; set; }
        public string PFNumber { get; set; }
        public string UANNumber { get; set; }
        public string ESICNumber { get; set; }
        public string BioMetricId { get; set; }
        
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public int PRMId { get; set; }
        public int? StaffId { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HRA { get; set; }
        public decimal? DearnessAllowance { get; set; }
        public decimal? FoodAllowance { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? MedicalAllowances { get; set; }
        public decimal? OtherAllowances { get; set; }
        public decimal? ProvidentFund { get; set; }
        public decimal? ProfessionalTax { get; set; }
        public decimal? ESIFund { get; set; }
        public decimal? TDS { get; set; }
        public string SalarySlip { get;  set; }
        public string PrevExpLetter { get;  set; }
        public string ResumeDoc { get;  set; }
        public string AadharBack { get;  set; }
        public string AadharFront { get;  set; }
        public string PanCardUploaded { get;  set; }
        public string OfferLetterLink { get; set; }
        public string WhatsappNumber { get; set; }
        public string OfficialEmail { get; set; }
    }
}
