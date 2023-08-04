using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class LoginTracking
    {
        [Key]
        public int TrakingId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? Logintime { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
