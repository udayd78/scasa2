using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class CreateDCModel
    {
        public int SOMId { get; set; }
        public string DCRemarks { get; set; }
        public int[] SelWareHouseQty { get; set; }
        public int[] SelShowRoomQty { get; set; }
        public int[] SODId { get; set; }
        
    }
}
