using SCASA.Models.ModelClasses;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IInventoryService
    {
        void DeleteProductImage(string imagename, int invid);
        InventoryImages GetProductImageById(int id);
        List<StockMovmentInvoice> GetStockMovInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10);
        List<StandByInvoice> GetStandByInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10);
        List<StandByInvoice> GetPreviousStandByInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10);
        int GetStockMovInvoices_Count(DateTime? fromDate, DateTime? toDate);
        int GetCurrentStandByInvoices_Count(DateTime? fromDate, DateTime? toDate);
        int GetPreviousStandByInvoices_Count(DateTime? fromDate, DateTime? toDate);
        StockMovementForPrint GetStockStockMovementByInvoice(int id);
        StockMovementForPrint GetStandByInvoice(int id);
        public List<InventoryModelDrop> GetInventoryModelDrop();

        public StockMovmentBasicInfo GetStockMmtBasicInfo(int id);

        List<InventoryModelDrop> GetInventoryModelDrop(int categoryId, int subCategoryId);
        ProcessResponse MoveStock(StockMovementMasterModel request);
        ProcessResponse MoveStandByStock(StockMovementMasterModel request);

        ProcessResponse UpdateInventory(InventoryMaster request);
        void UpdateProductImage(InventoryImages request);
        InventoryMaster GetInentoryById(int id);
        List<InventoryAllDisplayModel> GetInventoryAll(string type = "", int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search="");

        int GetInventoryAll_Count(string type = "", int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search="");
        InventoryImages GetInventoryImageById(int id);
        ProcessResponse UpdateInventoryImages(InventoryImages request);
        List<InventoryImages> GetInventoryImages(int id);
        InventoryDocuments GetInvDocById(int id);
        ProcessResponse UploadInventoryDocument(InventoryDocuments request);
        List<StockMovementDisplayModel> GetStockMovement(int
           pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "");

        int GetStockMovement_Count(int catId = 0, int subCateId = 0, string search = "");
        List<InventoryImages> InventoryOtherImgsUploaded(int id);
        List<InventoryDocuments> InventoryDocsUploaded(int id);
        void UpdateStockMovementInvoiceEBill(int smId, string ebill);
        void UpdateStockMovementInvoiceDispatch(int smId, string dispthr);
        void UpdateStandByInvoiceEBill(int smId, string ebill);
        void UpdateStandByInvoiceDispatch(int smId, string dispthr);
        SalesProductDetailModel GetProductForSales(int id);
        void GetBackFromStandBack(int id);
        List<IncentoryCatWiseModel> GetCategoryWiswReports();
        bool IsCart(int cartId, int pId);
        List<InventoryCategoryPrintModel> GetProdsCatWiseToPrint(int catId, int SubCatId, int LocationType);
    }
}
