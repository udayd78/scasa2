using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class InventoryDocuments
    {
        [Key]
        public int DocumentId { get; set; }
        public DateTime? UploadedOn { get; set; }
        public string DocumentName { get; set; }
        public string DocumentURL { get; set; }
        public string UploadedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public int? InventoryId { get; set; }
        public string Remarks { get; set; }
    }
}
