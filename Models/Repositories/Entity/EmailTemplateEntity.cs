using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class EmailTemplateEntity
    {
        [Key]
        public int EMailTemplateID { get; set; }
        public string ModuleName { get; set; }
        public string EmailContent { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Subject { get; set; }
    }
}
