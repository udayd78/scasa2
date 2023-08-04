using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class GenericRequest : PaginationRequest
    {
        public int Id { get; set; }
    }
    public class PaginationRequest
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;

        public string search { get; set; } = "";
    }
}
