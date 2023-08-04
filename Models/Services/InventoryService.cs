using SCASA.Models.ModelClasses;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepo iRepo;
        public InventoryService(IInventoryRepo repo)
        {
            iRepo = repo;
        }

        public ProcessResponse UpdateInventory(InventoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {   
                    response = iRepo.UpdateInventory(request);

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed " + ex.Message;
            }

            return response;
        }

        public InventoryMaster GetInentoryById(int id)
        {
            return iRepo.GetInentoryById(id);
        }

        public List<InventoryAllDisplayModel> GetInventoryAll(string type = "", int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0,string search="")
        {
            return iRepo.GetInventoryAll(type, pageNumber, pageSize,catId,subCateId,search);
        }

        public int GetInventoryAll_Count(string type = "", int pageNumber = 1, int pageSize = 10,int catId =0, int subCateId=0,string search="")
        {
            return iRepo.GetInventoryAll_Count (type, pageNumber, pageSize,catId,subCateId,search);
        }
        public InventoryImages GetInventoryImageById(int id)
        {
            return iRepo.GetInventoryImageById(id);
        }
        public ProcessResponse UpdateInventoryImages(InventoryImages request)
        {
            return iRepo.UpdateInventoryImages(request);
        }
        public List<InventoryImages> GetInventoryImages(int id)
        {
            return iRepo.GetInventoryImages(id);
        }
        public InventoryDocuments GetInvDocById(int id)
        {
            return iRepo.GetInvDocById(id);
        }
        public ProcessResponse UploadInventoryDocument(InventoryDocuments request)
        {
            return iRepo.UploadInventoryDocument(request);
        }

        public ProcessResponse MoveStock(StockMovementMasterModel request)
        {
            return iRepo.MoveStock(request);
        }
        public ProcessResponse MoveStandByStock(StockMovementMasterModel request)
        {
            return iRepo.MoveStandByStock(request);
        }

        public List<InventoryModelDrop> GetInventoryModelDrop(int categoryId, int subCategoryId)
        {
            return iRepo.GetInventoryModelDrop(categoryId, subCategoryId);
        }
        public List<InventoryModelDrop> GetInventoryModelDrop()
        {
            return iRepo.GetInventoryModelDrop();
        }

        public List<StockMovementDisplayModel> GetStockMovement(int
           pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "")
        {
            return iRepo.GetStockMovement(pageNumber, pageSize, catId, subCateId, search);
        }

        public int GetStockMovement_Count(int catId = 0, int subCateId = 0, string search = "")
        {

            return iRepo.GetStockMovement_Count(catId, subCateId, search);
        }

        public List<InventoryImages> InventoryOtherImgsUploaded(int id)
        {
            return iRepo.InventoryOtherImgsUploaded(id);
        }

        public List<InventoryDocuments> InventoryDocsUploaded(int id)
        {
            return iRepo.InventoryDocsUploaded(id);
        }

        public StockMovmentBasicInfo GetStockMmtBasicInfo(int id)
        {
            return iRepo.GetStockMmtBasicInfo(id);
        }

        public List<StockMovmentInvoice> GetStockMovInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10)
        {
            return iRepo.GetStockMovInvoices(fromDate, toDate, pageNumber, pageSize);
        }
        public List<StandByInvoice> GetStandByInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10)
        {
            return iRepo.GetStandByInvoices(fromDate, toDate, pageNumber, pageSize);
        }
        public List<StandByInvoice> GetPreviousStandByInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10)
        {
            return iRepo.GetPreviousStandByInvoices(fromDate, toDate, pageNumber, pageSize);
        }

        public int GetStockMovInvoices_Count(DateTime? fromDate, DateTime? toDate)
        {
            return iRepo.GetStockMovInvoices_Count(fromDate, toDate);
        }
        public int GetCurrentStandByInvoices_Count(DateTime? fromDate, DateTime? toDate)
        {
            return iRepo.GetCurrentStandByInvoices_Count(fromDate, toDate);
        }
        public int GetPreviousStandByInvoices_Count(DateTime? fromDate, DateTime? toDate)
        {
            return iRepo.GetPreviousStandByInvoices_Count(fromDate, toDate);
        }
        public StockMovementForPrint GetStockStockMovementByInvoice(int id)
        {
            return iRepo.GetStockStockMovementByInvoice(id);
        }
        public StockMovementForPrint GetStandByInvoice(int id)
        {
            return iRepo.GetStandByInvoice(id);
        }
        public InventoryImages GetProductImageById(int id)
        {
            return iRepo.GetProductImageById(id);
        }

        public void UpdateProductImage(InventoryImages request)
        {
            iRepo.UpdateProductImage(request);
        }

        public void DeleteProductImage(string imagename, int invid)
        {
            iRepo.DeleteProductImage(imagename, invid);
        }
        public void UpdateStockMovementInvoiceEBill(int smId, string ebill)
        {
            iRepo.UpdateStockMovementInvoiceEBill(smId, ebill);
        }
        public void UpdateStockMovementInvoiceDispatch(int smId, string dispthr)
        {
            iRepo.UpdateStockMovementInvoiceDispatch(smId, dispthr);
        }
        public void UpdateStandByInvoiceEBill(int smId, string ebill)
        {
            iRepo.UpdateStandByInvoiceEBill(smId, ebill);
        }
        public void UpdateStandByInvoiceDispatch(int smId, string dispthr)
        {
            iRepo.UpdateStandByInvoiceDispatch(smId, dispthr);
        }
        public SalesProductDetailModel GetProductForSales(int id)
        {
            return iRepo.GetProductForSales(id);
        }
        public void GetBackFromStandBack(int id)
        {
            iRepo.GetBackFromStandBack(id);
        }
        public List<IncentoryCatWiseModel> GetCategoryWiswReports()
        {
            return iRepo.GetCategoryWiswReports();
        }
        public bool IsCart(int cartId, int pId)
        {
            return iRepo.IsCart(cartId, pId);
        }
        public List<InventoryCategoryPrintModel> GetProdsCatWiseToPrint(int catId, int SubCatId, int LocationType)
        {
            return iRepo.GetProdsCatWiseToPrint(catId, SubCatId, LocationType);
        }
    }
}
