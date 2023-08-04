using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class UserTypeResultModel
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
