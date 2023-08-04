using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class FinanceGroups
    {
        [Key]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Group Name Required")]
        public string GroupName { get; set; }

        public string GroupCode { get; set; }
        public bool? IsDeleted { get; set; }
        public string AcountType { get; set; }
    }
}
