using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Utilities
{
    public static class StoredProcedures
    {
        public static string GetUserTypes = "usp_getusertypes";
        public static string GetUserByEmailId = "usp_GetUserByEmailId";
        public static string GetAllInventory = "GetAllInventory";
        public static string GetAllInventory_Count = "GetAllInventory_Count";
        public static string GetStockMovementData = "GetStockMovementData";
        public static string GetStockMovementData_Count = "GetStockMovementData_Count";
        public static string GetAllCustomers = "GetAllCustomers";
        public static string GetAllCustomers_Count = "GetAllCustomers_Count";
    }
}
