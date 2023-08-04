using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class CreateSaleOrderModel
    {
        public int CRFQId { get; set; }
        public decimal? DelivaryCharges { get; set; }
        public decimal RoundOff { get; set; }
        public int[] SelWareHouseQty { get; set; }
        public int[] SelShowRoomQty { get; set; }
        public int[] CRFQDId { get; set; }
    }
}
