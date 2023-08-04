using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class OtpTransactions
    {
        [Key]
        public int TrId { get; set; }
        public string MobileOTP { get; set; }
        public string EmailOTP { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime? UsedOn { get; set; }
    }
}