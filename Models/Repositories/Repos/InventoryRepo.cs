using SCASA.Models.ModelClasses;
using SCASA.Models.Utilities;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using SCASA.Models.Utilities;

namespace SCASA.Models.Repositories.Repos
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly MyDbContext context;

        public InventoryRepo(MyDbContext _db)
        {
            context = _db;
        }

        public ProcessResponse UpdateInventory(InventoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.InventoryId > 0)
                {
                    context.Entry(request).CurrentValues.SetValues(request);
                    context.SaveChanges();
                }
                else
                {
                    context.inventoryMasters.Add(request);
                    context.SaveChanges();
                    response.currentId = request.InventoryId;
                }

                response.statusCode = 1;
                response.statusMessage = "Success ";

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
            return context.inventoryMasters.Where(a => a.InventoryId == id).FirstOrDefault();
        }
        public InventoryImages GetProductImageById(int id)
        {
            return context.InventoryImages.Where(a => a.ImageId == id).FirstOrDefault();
        }
         
        public void DeleteProductImage(string imagename, int invid)
        {
            var pi = context.inventoryMasters.Where(a => a.InventoryId == invid).FirstOrDefault();
            // checking in primary image;
            if(pi != null)
            {
                List<string> iList = pi.PrimaryImage.Split(",").ToList();
                if(iList.Count>0)
                {
                    for(int i =0; i < iList.Count; i ++)
                    {
                        if (iList[i].Equals(imagename))
                        {
                            iList.RemoveAt(i);
                        }
                    }
                    string fImage = string.Join(",", iList);
                    pi.PrimaryImage = fImage;
                    context.Entry(pi).CurrentValues.SetValues(pi);
                    context.SaveChanges();
                }
            }
            // checking in product images
             InventoryImages Mylist = context.InventoryImages.Where(a => a.InventoryId == invid && a.ImageURL == imagename).FirstOrDefault();
            if(Mylist != null)
            {
                Mylist.IsDeleted = true;
                context.Entry(Mylist).CurrentValues.SetValues(Mylist);
                context.SaveChanges();
            } 
        }
        public List<InventoryImages> InventoryOtherImgsUploaded(int id)
        {
            return context.InventoryImages.Where(a => a.IsDeleted == false && a.InventoryId == id).ToList();
        } 
        public List<InventoryDocuments> InventoryDocsUploaded(int id)
        {
            return context.inventoryDocuments.Where(a => a.IsDeleted == false && a.InventoryId == id).ToList();
        }
        public List<InventoryAllDisplayModel> GetInventoryAll(string type = "", int 
            pageNumber = 1, int pageSize = 10, int catId =0, int subCateId=0, string search="")
        {
            List<InventoryDisplayModel> response = new List<InventoryDisplayModel>();
            List<InventoryAllDisplayModel> final = new List<InventoryAllDisplayModel>();
            SqlParameter[] sParams =
           {
                new SqlParameter("pageNumber",pageNumber),
                new SqlParameter("pageSize",pageSize),
                new SqlParameter("search", search ?? ""),
                new SqlParameter("categoryId", catId),
                new SqlParameter("subcategory", subCateId ),
                new SqlParameter("src", type)
            };
            string sp = StoredProcedures.GetAllInventory+ " @pageNumber, @pageSize, @categoryId, @subcategory, @search,@src";
            response = context.Set<InventoryDisplayModel>().FromSqlRaw(sp, sParams).ToList();
            foreach(InventoryDisplayModel i in response)
            {
                InventoryAllDisplayModel a = new InventoryAllDisplayModel();
                CloneObjects.CopyPropertiesTo(i, a);
                a.ReservedQty = 0;
                List<ReservedQtyMaster> rq = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.ProductId == i.InventoryId && 
                                        (a.CurrentStatus != "Despatched" && a.CurrentStatus != "Returned back")).ToList();
                a.Reserved = new List<ReservedNameClass>();
                if (rq != null)
                {
                    foreach (ReservedQtyMaster j in rq)
                    {
                        //a.ReservedQty += (int)j.Quantity;
                        ReservedNameClass x = new ReservedNameClass();
                        x.qty = (int)j.Quantity;
                        x.name = j.SalesExename;
                        a.Reserved.Add(x);
                    }
                }
                final.Add(a);
            }
            return final;
        }

        public int GetInventoryAll_Count(string type = "", int pageNumber = 1, 
            int pageSize = 10, int catId = 0, int subCateId = 0, string search="")
        {

            int total = 0;
            SqlParameter[] sParams =
        {
               
                new SqlParameter("search", search ?? ""),
                new SqlParameter("categoryId", catId),
                new SqlParameter("subcategory", subCateId ),
                new SqlParameter("src", type)
            };


            string sp = StoredProcedures.GetAllInventory_Count + " @categoryId, @subcategory, @search, @src";
            total = context.Set<RecordsCountFromSql>().FromSqlRaw(sp, sParams)
                .AsEnumerable().Select(r => r.cnt).FirstOrDefault();
            return total;
        }
        public InventoryImages GetInventoryImageById(int id)
        {
            return context.InventoryImages.Where(a => a.IsDeleted == false && a.ImageId == id).FirstOrDefault();
        }
        public ProcessResponse UpdateInventoryImages(InventoryImages request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.ImageId > 0)
                {
                    context.Entry(request).CurrentValues.SetValues(request);
                    context.SaveChanges();
                }
                else
                {
                    context.InventoryImages.Add(request);
                    context.SaveChanges();
                }

                response.statusCode = 1;
                response.statusMessage = "Success ";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed " + ex.Message;
            }

            return response;
        }
        public InventoryDocuments GetInvDocById(int id)
        {
            return context.inventoryDocuments.Where(a => a.IsDeleted == false && a.DocumentId == id).FirstOrDefault();
        }
        public ProcessResponse UploadInventoryDocument(InventoryDocuments request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.DocumentId> 0)
                {
                    context.Entry(request).CurrentValues.SetValues(request);
                    context.SaveChanges();
                }
                else
                {
                    context.inventoryDocuments.Add(request);
                    context.SaveChanges();
                }

                response.statusCode = 1;
                response.statusMessage = "Success ";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed " + ex.Message;
            }

            return response;
        }
        public List<InventoryImages> GetInventoryImages(int id)
        {
            return context.InventoryImages.Where(a => a.IsDeleted == false && a.InventoryId == id).ToList();
        }

        public List<InventoryModelDrop> GetInventoryModelDrop(int categoryId, int subCategoryId)
        {
            List<InventoryModelDrop> myList = new List<InventoryModelDrop>();
            try
            {
                myList = context.inventoryMasters.Where(a => a.IsDeleted == false &&
                a.CategoryId == categoryId && a.SubCategoryId == subCategoryId)
                    .Select(b => new InventoryModelDrop
                    {
                        InventoryId = b.InventoryId,
                        ModelNumber = b.ModelNumber + ", Warehouse qty : " + b.WharehouseQty + ", Showroom qty : " + b.ShowroomQty
                    }).ToList();
            }
            catch(Exception ex)
            {

            }

            return myList;

        }

        
        public ProcessResponse MoveStock(StockMovementMasterModel request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                StockMovementMaster smm = new StockMovementMaster();
                CloneObjects.CopyPropertiesTo(request, smm);
                int cid = 1;
                var v = context.stockMovmentInvoices.Where(a => a.IsDeleted == false).OrderByDescending(b => b.SeqId).FirstOrDefault();
                if(v != null)
                {
                    cid = (int) v.SeqId + 1;
                }
                
                smm.MovedOn = DateTime.Now;
                smm.Remarks = request.Remarks;
                int stdt = 0;
                int endt = 0;
                if (DateTime.Now.Month <= 3)
                {
                    stdt = DateTime.Now.Year - 1;
                    endt = DateTime.Now.Year;
                }
                else
                {
                    stdt = DateTime.Now.Year;
                    endt = DateTime.Now.Year + 1;
                }
                smm.StockTransferNumber = "EH/" + stdt + "-" + endt + "/" + cid.ToString();
                context.stockMovementMasters.Add(smm);
                context.SaveChanges();
                decimal? totalValue = 0;
                if (smm.SMRId > 0)
                {
                    // save each item
                    if (request.Qty.Count() > 0)
                    {
                        for (int j = 0; j < request.Qty.Count(); j++)
                        {
                            int? currentInvid = request.InventoryId[j];
                            var inv = context.inventoryMasters.Where(a => a.InventoryId == currentInvid).FirstOrDefault();
                            StockMovementRegister smr = new StockMovementRegister();
                            smr.InventoryId = currentInvid;
                            smr.IsDeleted = false;
                            smr.ItemPrice = inv.ActualPrice;
                            smr.MovedOn = DateTime.Now;
                            smr.Qty = request.Qty[j];
                            smr.SMRId = smm.SMRId;
                            smr.TotalPrice = smr.ItemPrice * smr.Qty;
                            smr.MovedFrom = request.MovedFrom;
                            smr.MovedTo = request.MovedTo;
                            smr.MovedBy = request.MovedBy;
                            smr.Notes = request.Remarks;
                            context.stockMovementRegisters.Add(smr);
                            context.SaveChanges();
                            totalValue += smr.TotalPrice;
                            //update inventory
                            var i = context.inventoryMasters.Where(a => a.InventoryId == currentInvid).FirstOrDefault();
                            if (request.MovedTo == "Showroom")
                            {
                                i.ShowroomQty = i.ShowroomQty != null ? (i.ShowroomQty + (int)smr.Qty) : smr.Qty;
                                i.WharehouseQty = i.WharehouseQty - (int)smr.Qty;
                            }
                            else
                            {
                                i.WharehouseQty = i.WharehouseQty != null ? (i.WharehouseQty + smr.Qty) : smr.Qty;
                                i.ShowroomQty = (i.ShowroomQty - smr.Qty);
                            }
                            context.Entry(i).CurrentValues.SetValues(i);
                            context.SaveChanges();
                        }


                        //update invoice
                        CompanyMaster cm = new CompanyMaster();
                        cm = context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
                        StockMovmentInvoice si = new StockMovmentInvoice();
                        si.CurrentStatus = "Active";
                        if (request.MovedFrom == "Warehouse")
                        {
                            si.ToAddress = cm.ShowroomAddress;
                            si.FromAddress = cm.WarehouseAddress;
                            si.FromEmail = cm.WarehouseEmail;
                            si.FromMobile = cm.WarehousePhoneNumbers;
                            si.ToAddress = cm.ShowroomAddress;
                            si.ToEmail = cm.ShowroomEmail;
                            si.ToMobile = cm.ShowroomPhoneNumbers;
                        }
                        else
                        {
                            si.FromAddress = cm.ShowroomAddress;
                            si.FromEmail = cm.ShowroomEmail;
                            si.FromMobile = cm.ShowroomPhoneNumbers;
                            si.ToAddress = cm.WarehouseAddress;
                            si.ToEmail = cm.WarehouseEmail;
                            si.ToMobile = cm.WarehousePhoneNumbers;
                        }
                        si.IsDeleted = false;
                        si.Notes = request.Remarks;
                        si.SeqId = cid;
                        si.STDate = DateTime.Now;
                        si.MovedFrom = request.MovedFrom;
                        si.MovedTo = request.MovedTo;
                        si.StockMovementMasterId = smm.SMRId;
                        si.StockTransferNumber = smm.StockTransferNumber;
                        si.DespatchDocumnetNo = smm.StockTransferNumber;
                        si.SMRId = smm.SMRId;
                        si.DespatchThrough = smm.StockTransferNumber;
                        si.EWayBillNo = smm.StockTransferNumber;
                        si.TotalValue = totalValue;
                        context.stockMovmentInvoices.Add(si);
                        context.SaveChanges();
                    }
                }
                response.statusCode = 1;
                response.statusMessage = "success";
            }catch(Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = ex.ToString();
            }

            return response;
        }
        public ProcessResponse MoveStandByStock(StockMovementMasterModel request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                CustomerMaster cust = context.customerMasters.Where(a => a.IsDeleted == false && a.Cid == request.customerID).FirstOrDefault();
                AddressMaster am = context.addressMasters.Where(a => a.IsDeleted == false && a.CustomerId == request.customerID).FirstOrDefault();
                if (am != null)
                {
                    StandByMaster sbm = new StandByMaster();
                    sbm.CustomerId = request.customerID;
                    sbm.MovedFrom = request.MovedFrom;
                    sbm.MovedBy = request.MovedBy;
                    sbm.MovedStaffId = request.StaffId;
                    sbm.AddressId = am.Addid;
                    int cid = 1;
                    var v = context.standByInvoices.Where(a => a.IsDeleted == false).OrderByDescending(b => b.SeqId).FirstOrDefault();
                    if (v != null)
                    {
                        cid = (int)v.SeqId + 1;
                    }

                    sbm.MovedOn = DateTime.Now;
                    sbm.Remarks = request.Remarks;
                    int stdt = 0;
                    int endt = 0;
                    if (DateTime.Now.Month <= 3)
                    {
                        stdt = DateTime.Now.Year - 1;
                        endt = DateTime.Now.Year;
                    }
                    else
                    {
                        stdt = DateTime.Now.Year;
                        endt = DateTime.Now.Year + 1;
                    }
                    sbm.StockTransferNumber = "EH/" + stdt + "-" + endt + "/" + cid.ToString();
                    sbm.currentStatus = "At Customer";
                    sbm.Remarks = request.Remarks;
                    sbm.IsDeleted = false;
                    context.standByMasters.Add(sbm);
                    context.SaveChanges();
                    decimal? totalValue = 0;
                    if (sbm.TrId > 0)
                    {
                        // save each item
                        if (request.Qty.Count() > 0)
                        {
                            for (int j = 0; j < request.Qty.Count(); j++)
                            {
                                int? currentInvid = request.InventoryId[j];
                                var inv = context.inventoryMasters.Where(a => a.InventoryId == currentInvid).FirstOrDefault();
                                StandByDetails sbd = new StandByDetails();
                                sbd.MasterId = sbm.TrId;
                                sbd.InventoryId = currentInvid;
                                sbd.Quantity = request.Qty[j];
                                sbd.Notes = request.Remarks;
                                sbd.ItemPrice = inv.MRPPrice;
                                sbd.TotalPrice = sbd.Quantity * sbd.ItemPrice;
                                sbd.IsDeleted = false;
                                context.standByDetails.Add(sbd);
                                context.SaveChanges();
                                totalValue += sbd.TotalPrice;
                                //update inventory
                                var i = context.inventoryMasters.Where(a => a.InventoryId == currentInvid).FirstOrDefault();
                                if (request.MovedFrom == "Showroom")
                                {
                                    i.ShowroomQty -= request.Qty[j];
                                }
                                else
                                {
                                    i.WharehouseQty -= request.Qty[j];
                                }
                                i.Qty -= request.Qty[j];
                                context.Entry(inv).CurrentValues.SetValues(i);
                                context.SaveChanges();
                                ReservedQtyMaster rqm = new ReservedQtyMaster();
                                rqm.ProductId = currentInvid;
                                rqm.Quantity = request.Qty[j];
                                rqm.IsDeleted = false;
                                rqm.CurrentStatus = "StandBy";
                                rqm.DCmId = 0;
                                rqm.SOMId = 0;
                                if (request.MovedFrom == "Showroom")
                                {
                                    rqm.SQty = request.Qty[j];
                                    rqm.WQty = 0;
                                }
                                else
                                {
                                    rqm.WQty = request.Qty[j];
                                    rqm.SQty = 0;
                                }
                                rqm.StandById = sbm.TrId;
                                rqm.SalesExename = "Stand By("+cust.FullName+")";
                                context.reservedQtyMasters.Add(rqm);
                                context.SaveChanges();
                            }

                            //update invoice
                            CompanyMaster cm = new CompanyMaster();
                            cm = context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
                            StandByInvoice sbi = new StandByInvoice();
                            sbi.CurrentStatus = "At Customer";
                            if (request.MovedFrom == "Warehouse")
                            {
                                sbi.FromAddress = cm.WarehouseAddress;
                                sbi.FromEmail = cm.WarehouseEmail;
                                sbi.FromMobile = cm.WarehousePhoneNumbers;
                            }
                            else
                            {
                                sbi.FromAddress = cm.ShowroomAddress;
                                sbi.FromEmail = cm.ShowroomEmail;
                                sbi.FromMobile = cm.ShowroomPhoneNumbers;
                            }
                            sbi.IsDeleted = false;
                            sbi.Notes = request.Remarks;
                            sbi.SeqId = cid;
                            sbi.STDate = DateTime.Now;
                            sbi.StandByMasterId = sbm.TrId;
                            sbi.StockTransferNumber = sbm.StockTransferNumber;
                            sbi.DespatchDocumentNo = sbm.StockTransferNumber;
                            sbi.DespatchTrough = sbm.StockTransferNumber;
                            sbi.EWayBillNo = sbm.StockTransferNumber;
                            sbi.TotalValue = totalValue;
                            sbi.ToEmail = cust.EmailId;
                            sbi.ToMobile = cust.MobileNumber;
                            sbi.MovedFrom = request.MovedFrom;
                            sbi.MovedTo = cust.FullName;
                            string BillingAddress = am.HouseNumber + "," + am.StreetName + "," + am.Location + ",<BR/>";
                            BillingAddress += context.cityMasters.Where(a => a.Id == am.CityId).Select(a => a.CityName).FirstOrDefault();
                            BillingAddress += "," + context.stateMasters.Where(a => a.Id == am.StateId).Select(a => a.StateName).FirstOrDefault();
                            BillingAddress += "," + context.countryMasters.Where(a => a.Id == am.CountryId).Select(a => a.CountryName).FirstOrDefault();
                            BillingAddress += " - " + am.PostalCode;
                            sbi.ToAddress = BillingAddress;
                            context.standByInvoices.Add(sbi);
                            context.SaveChanges();
                        }
                    }
                    response.statusCode = 1;
                    response.statusMessage = "success";
                }
                else
                {
                    response.statusCode=0;
                    response.statusMessage = "Selected Customer does not have any address";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = ex.Message;
            }

            return response;
        }
        public List<StockMovementDisplayModel> GetStockMovement(int
            pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "")
        {
            List<StockMovementDisplayModel> response = new List<StockMovementDisplayModel>();

            SqlParameter[] sParams =
           {
                new SqlParameter("pageNumber",pageNumber),
                new SqlParameter("pageSize",pageSize),
                new SqlParameter("search", search ?? ""),
                new SqlParameter("categoryId", catId),
                new SqlParameter("subcategory", subCateId )
            };
            string sp = StoredProcedures.GetStockMovementData + " @pageNumber, @pageSize, @categoryId, @subcategory, @search";
            response = context.Set<StockMovementDisplayModel>().FromSqlRaw(sp, sParams).ToList();
            return response;
        }

        public int GetStockMovement_Count(int catId = 0, int subCateId = 0, string search = "")
        {

            int total = 0;
            SqlParameter[] sParams =
        {

                new SqlParameter("search", search ?? ""),
                new SqlParameter("categoryId", catId),
                new SqlParameter("subcategory", subCateId ) 
            };


            string sp = StoredProcedures.GetStockMovementData_Count + " @categoryId, @subcategory, @search";
            total = context.Set<RecordsCountFromSql>().FromSqlRaw(sp, sParams)
                .AsEnumerable().Select(r => r.cnt).FirstOrDefault();
            return total;
        }

        public List<InventoryModelDrop> GetInventoryModelDrop()
        {
            List<InventoryModelDrop> myList = new List<InventoryModelDrop>();
            try
            {
                myList = context.inventoryMasters.Where(a => a.IsDeleted == false)
                    .Select(b => new InventoryModelDrop
                    {
                        InventoryId = b.InventoryId,
                        ModelNumber = b.ModelNumber
                    }).ToList();
            }
            catch (Exception ex)
            {

            }

            return myList;
        }

        public StockMovmentBasicInfo GetStockMmtBasicInfo(int id)
        {
            return context.inventoryMasters.Where(a => a.InventoryId == id).Select(b => new StockMovmentBasicInfo
            {
                 ShowroomQty = b.ShowroomQty, ProductImage=b.PrimaryImage, WarehouseQty = b.WharehouseQty
            }).FirstOrDefault();
        }

        public List<StockMovmentInvoice> GetStockMovInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10)
        {
            int picPages = (pageNumber - 1) * pageSize;
            List<StockMovmentInvoice> response = new List<StockMovmentInvoice>();
            try
            {
                response = context.stockMovmentInvoices.Where(a => a.IsDeleted == false &&
                a.STDate >= fromDate && a.STDate <= toDate).
                OrderByDescending(b=>b.STDate).
                Skip(picPages).
                Take(pageSize).
                ToList();
            }
            catch(Exception ex)
            {

            }
            return response;
        }
        public List<StandByInvoice> GetStandByInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10)
        {
            int picPages = (pageNumber - 1) * pageSize;
            List<StandByInvoice> response = new List<StandByInvoice>();
            try
            {
                response = context.standByInvoices.Where(a => a.IsDeleted == false &&
                a.STDate >= fromDate && a.STDate <= toDate && a.CurrentStatus== "At Customer").
                OrderByDescending(b => b.STDate).
                Skip(picPages).
                Take(pageSize).
                ToList();
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public List<StandByInvoice> GetPreviousStandByInvoices(DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10)
        {
            int picPages = (pageNumber - 1) * pageSize;
            List<StandByInvoice> response = new List<StandByInvoice>();
            try
            {
                response = context.standByInvoices.Where(a => a.IsDeleted == false &&
                a.STDate >= fromDate && a.STDate <= toDate && a.CurrentStatus == "Returned back").
                OrderByDescending(b => b.STDate).
                Skip(picPages).
                Take(pageSize).
                ToList();
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public int GetStockMovInvoices_Count(DateTime? fromDate, DateTime? toDate)
        {

            int count = 0;
            try
            {
                count = context.stockMovmentInvoices.Where(a => a.IsDeleted == false &&
                a.STDate >= fromDate && a.STDate <= toDate).Select(b => b.SMIId).Count();
            }
            catch (Exception ex)
            {

            }
            return count;
        }

        public int GetCurrentStandByInvoices_Count(DateTime? fromDate, DateTime? toDate)
        {

            int count = 0;
            try
            {
                count = context.standByInvoices.Where(a => a.IsDeleted == false &&
                a.STDate >= fromDate && a.STDate <= toDate && a.CurrentStatus == "At Customer").Select(b => b.SbiId).Count();
            }
            catch (Exception ex)
            {

            }
            return count;
        }
        public int GetPreviousStandByInvoices_Count(DateTime? fromDate, DateTime? toDate)
        {

            int count = 0;
            try
            {
                count = context.standByInvoices.Where(a => a.IsDeleted == false &&
                a.STDate >= fromDate && a.STDate <= toDate && a.CurrentStatus == "Returned back").Select(b => b.SbiId).Count();
            }
            catch (Exception ex)
            {

            }
            return count;
        }
        public StockMovementForPrint GetStockStockMovementByInvoice(int id)
        {
            StockMovementForPrint response = new StockMovementForPrint();
            try
            {
                StockMovmentInvoice inv = new StockMovmentInvoice();
                inv = context.stockMovmentInvoices.Where(a => a.SMIId == id).FirstOrDefault();
                if(inv!=null)
                {
                    StockMovementMaster sr = new StockMovementMaster();
                    sr = context.stockMovementMasters.Where(a => a.SMRId == inv.SMRId).FirstOrDefault();
                    if(sr!=null)
                    {
                        List<StockMovementRegisterForPrint> sm = new List<StockMovementRegisterForPrint>();
                        sm = (from s in context.stockMovementRegisters
                              join i in context.inventoryMasters on s.InventoryId equals i.InventoryId
                              where s.SMRId == sr.SMRId
                              select new StockMovementRegisterForPrint
                              {
                                  InventoryId = s.InventoryId,
                                  SMRId = s.SMRId,
                                  IsDeleted = s.IsDeleted, ItemPrice = s.ItemPrice,
                                  MovedBy = s.MovedBy,
                                  MovedFrom = s.MovedFrom, MovedOn = s.MovedOn,
                                  MovedTo = s.MovedTo,
                                  Notes = s.Notes,
                                  ProductImage = i.PrimaryImage,
                                  Qty = s.Qty,
                                  TotalPrice = s.TotalPrice,
                                  TRId = s.TRId,
                                  Description = i.ItemDescription,
                                  ModelNumber = i.ModelNumber
                              }).ToList();
                        StockMovementMasterForPrint srPrint = new StockMovementMasterForPrint();
                        CloneObjects.CopyPropertiesTo(srPrint, sr);
                        srPrint.items = sm;
                        CloneObjects.CopyPropertiesTo(inv, response);
                        response.AmountInWords = AppSettings.NumberToWords.ConvertAmount((double)response.TotalValue);
                        response.stockMaster = srPrint;
                    }
                }
            }
            catch(Exception )
            {

            }

            return response;
        }
        public StockMovementForPrint GetStandByInvoice(int id)
        {
            StockMovementForPrint response = new StockMovementForPrint();
            try
            {
                StandByInvoice inv = context.standByInvoices.Where(a => a.SbiId == id).FirstOrDefault();
                if (inv != null)
                {
                    StandByMaster sr = context.standByMasters.Where(a => a.TrId == inv.SbiId).FirstOrDefault();
                    if (sr != null)
                    {
                        List<StockMovementRegisterForPrint> sm = new List<StockMovementRegisterForPrint>();
                        sm = (from s in context.standByDetails
                              join i in context.inventoryMasters on s.InventoryId equals i.InventoryId
                              join invoice in context.standByInvoices on s.MasterId equals invoice.StandByMasterId
                              where s.MasterId == sr.TrId
                              select new StockMovementRegisterForPrint
                              {
                                  ItemPrice = s.ItemPrice,
                                  MovedBy = sr.MovedBy,
                                  MovedFrom = sr.MovedFrom,
                                  MovedOn = sr.MovedOn,
                                  MovedTo = invoice.MovedTo,
                                  Notes = s.Notes,
                                  ProductImage = i.PrimaryImage,
                                  Qty = s.Quantity,
                                  TotalPrice = s.TotalPrice,                                  
                                  Description = i.ItemDescription,
                                  ModelNumber = i.ModelNumber
                              }).ToList();
                        StockMovementMasterForPrint srPrint = new StockMovementMasterForPrint();
                        CloneObjects.CopyPropertiesTo(srPrint, sr);
                        srPrint.items = sm;
                        CloneObjects.CopyPropertiesTo(inv, response);
                        response.AmountInWords = AppSettings.NumberToWords.ConvertAmount((double)response.TotalValue);
                        response.stockMaster = srPrint;
                    }
                }
            }
            catch (Exception)
            {

            }
            return response;
        }
        public void UpdateProductImage(InventoryImages request)
        {
            throw new NotImplementedException();
        }
        public void UpdateStockMovementInvoiceEBill(int smId,string ebill)
        {
            try
            {
                StockMovmentInvoice sm = context.stockMovmentInvoices.Where(a => a.IsDeleted == false && a.SMIId == smId).FirstOrDefault();
                sm.EWayBillNo = ebill;
                StockMovmentInvoice oldsm = context.stockMovmentInvoices.Where(a => a.IsDeleted == false && a.SMIId == smId).FirstOrDefault();
                context.Entry(oldsm).CurrentValues.SetValues(sm);
                context.SaveChanges();
            }
            catch { }
        }
        public void UpdateStockMovementInvoiceDispatch(int smId, string dispthr)
        {
            try
            {
                StockMovmentInvoice sm = context.stockMovmentInvoices.Where(a => a.IsDeleted == false && a.SMIId == smId).FirstOrDefault();
                sm.DespatchThrough = dispthr;
                StockMovmentInvoice oldsm = context.stockMovmentInvoices.Where(a => a.IsDeleted == false && a.SMIId == smId).FirstOrDefault();
                context.Entry(oldsm).CurrentValues.SetValues(sm);
                context.SaveChanges();
            }
            catch { }
        }
        public void UpdateStandByInvoiceEBill(int smId, string ebill)
        {
            try
            {
                StandByInvoice sm = context.standByInvoices.Where(a => a.IsDeleted == false && a.SbiId == smId).FirstOrDefault();
                sm.EWayBillNo = ebill;
                StandByInvoice oldsm = context.standByInvoices.Where(a => a.IsDeleted == false && a.SbiId == smId).FirstOrDefault();
                context.Entry(oldsm).CurrentValues.SetValues(sm);
                context.SaveChanges();
            }
            catch { }
        }
        public void UpdateStandByInvoiceDispatch(int smId, string dispthr)
        {
            try
            {
                StandByInvoice sm = context.standByInvoices.Where(a => a.IsDeleted == false && a.SbiId == smId).FirstOrDefault();
                sm.DespatchTrough = dispthr;
                StandByInvoice oldsm = context.standByInvoices.Where(a => a.IsDeleted == false && a.SbiId == smId).FirstOrDefault();
                context.Entry(oldsm).CurrentValues.SetValues(sm);
                context.SaveChanges();
            }
            catch { }
        }
        public SalesProductDetailModel GetProductForSales(int id)
        {
            SalesProductDetailModel response = new SalesProductDetailModel();
            try
            {
                response = (from i in context.inventoryMasters
                            join cat in context.categoryMasters on i.CategoryId equals cat.CategoryId
                            join sub in context.subCategoryMasters on i.SubCategoryId equals sub.SubCategoryId
                            where i.IsDeleted == false && i.InventoryId ==id
                            select new SalesProductDetailModel 
                            {
                                InventoryId=i.InventoryId,
                                CategoryId=(int)i.CategoryId,
                                SubCategoryId=(int)i.SubCategoryId,
                                Title=i.Title,
                                PrimaryImage=i.PrimaryImage,
                                ModelNumber=i.ModelNumber,
                                Brand=i.Brand,
                                Height=i.Height,
                                Width=i.Width,
                                Breadth=i.Breadth,
                                CategoryName=cat.CategoryName,
                                SubCategoryName=sub.SubCategoryName,
                                MRPPrice=i.MRPPrice,
                                ShowroomQty=(int)i.ShowroomQty,
                                WharehouseQty= (int)i.WharehouseQty,
                                ReservedQty= context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.ProductId == i.InventoryId && a.CurrentStatus != "Despatched").Sum(b => b.Quantity),
                                ColorName =i.ColorName,
                                ColorImage=i.ColorImage,
                                ItemDescription=i.ItemDescription
                            }).FirstOrDefault();
            }
            catch { }
            return response;
        }
        public ProcessResponse SaveREservedQtyMaster(ReservedQtyMaster req)
        {
            ProcessResponse p = new ProcessResponse();
            try
            {
                if (req.TRId > 0)
                {
                    context.reservedQtyMasters.Add(req);
                    context.SaveChanges();
                    p.statusCode = 1;
                    p.statusMessage = "Saved";
                }
                else
                {
                    ReservedQtyMaster rem = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.TRId == req.TRId).FirstOrDefault();
                    context.Entry(rem).CurrentValues.SetValues(req);
                    context.SaveChanges();
                    p.statusCode = 1;
                    p.statusMessage = "Updated";
                }
            }catch (Exception e)
            {
                p.statusCode = 0;
                p.statusMessage = "Failed";
            }
            return p;
        }
        public void GetBackFromStandBack(int id)
        {
            try
            {
                StandByInvoice sbi = context.standByInvoices.Where(a => a.IsDeleted == false && a.SbiId == id).FirstOrDefault();
                StandByInvoice old = sbi;
                sbi.CurrentStatus = "Returned back";
                context.Entry(old).CurrentValues.SetValues(sbi);
                context.SaveChanges();
                StandByMaster sbm = context.standByMasters.Where(a => a.IsDeleted == false && a.TrId == sbi.StandByMasterId).FirstOrDefault();
                StandByMaster oldsbm = sbm;
                sbm.currentStatus = "Returned back";
                context.Entry(oldsbm).CurrentValues.SetValues(sbm);
                List<ReservedQtyMaster> sbms = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.StandById == sbm.TrId).ToList();
                foreach(ReservedQtyMaster r in sbms)
                {
                    ReservedQtyMaster o = r;
                    r.CurrentStatus = "Returned back";
                    context.Entry(o).CurrentValues.SetValues(r);
                    InventoryMaster im = context.inventoryMasters.Where(a => a.IsDeleted == false && a.InventoryId == r.ProductId).FirstOrDefault();
                    InventoryMaster ol = im;
                    if(sbi.MovedFrom== "Showroom")
                    {
                        im.ShowroomQty += r.Quantity;
                    }
                    else {
                        im.WharehouseQty += r.Quantity;
                    }
                    im.Qty += r.Quantity;
                    context.Entry(ol).CurrentValues.SetValues(im);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
        public List<IncentoryCatWiseModel> GetCategoryWiswReports()
        {
            List<IncentoryCatWiseModel> res = new List<IncentoryCatWiseModel>();
            try
            {
                var cats = context.categoryMasters.Where(a => a.IsDeleted == false).ToList();
                if (cats.Count>0)
                {
                    foreach(var c in cats)
                    {
                        IncentoryCatWiseModel a = new IncentoryCatWiseModel();
                        a.CagoryId_Name = c.CategoryName;
                        a.CategoryId = c.CategoryId;
                        a.ShowroomQty = 0;
                        a.ShowroomAmount = 0;
                        a.WharehouseQty = 0;
                        a.WareHouseAmount = 0;
                        a.INTransitQty = 0;
                        a.InTransitAmount=0;
                        //a.ShowroomQty = context.inventoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == c.CategoryId).Sum(b => b.ShowroomQty);
                        List<InventoryMaster> cqty = context.inventoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == c.CategoryId).ToList();
                        InventoryStatusMaster ism = context.inventoryStatusMasters.Where(a => a.IsDeleted == false && a.StatusName == "In Transit").FirstOrDefault();
                        foreach(InventoryMaster i in cqty)
                        {
                            a.ShowroomQty += i.ShowroomQty;
                            a.ShowroomAmount += (i.ShowroomQty * i.ActualPrice);
                            if (i.InventoryStatusId != ism.StatusId) 
                            {
                                a.WharehouseQty += i.WharehouseQty;
                                a.WareHouseAmount += (i.WharehouseQty * i.ActualPrice);
                            }
                            else
                            {
                                a.INTransitQty += i.WharehouseQty;
                                a.InTransitAmount += (i.WharehouseQty * i.ActualPrice);
                            }
                            
                        }
                        //a.WharehouseQty = context.inventoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == c.CategoryId && a.InventoryStatusId != ism.StatusId).Sum(b => b.WharehouseQty);
                        //a.INTransitQty = context.inventoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == c.CategoryId && a.InventoryStatusId == ism.StatusId).Sum(b => b.WharehouseQty);
                        res.Add(a);
                    }
                }
            }catch(Exception e)
            {

            }
            return res;
        }
        public bool IsCart(int cartId,int pId)
        {
            bool reslt = false;
            try
            {
                var crtmast = context.cartDetailsEntities.Where(a => a.IsDeleted == false && a.CartId == cartId && a.InventoryId == pId).FirstOrDefault();
                if (crtmast == null)
                {
                    reslt = true;
                }
            }
            catch(Exception e)
            {

            }
            return reslt;
        }
        public List<InventoryCategoryPrintModel> GetProdsCatWiseToPrint(int catId,int SubCatId,int LocationType)
        {
            List<InventoryCategoryPrintModel> res = new List<InventoryCategoryPrintModel>();
            try
            {
                string query = @"select im.InventoryId,im.Title,im.ModelNumber,cm.CategoryName,
                    sm.subcategoryName,im.PrimaryImage,im.ItemDescription,im.Width,
                    im.Height,im.Breadth,im.ColorName,im.ColorImage,im.Qty,im.ActualPrice,
                    im.MRPPrice,im.WharehouseQty,im.ShowroomQty,im.InventoryStatusId,im.Brand
                    from inventorymaster im
                    join categorymaster cm on im.categoryid=cm.categoryid
                    join SubCategoryMaster sm on im.subcategoryid=sm.subcategoryid
                    where im.IsDeleted=0 and cm.IsDeleted=0 and sm.IsDeleted=0 ";
                if (catId > 0)
                {
                    query += " and im.CategoryId= " + catId;
                }
                if (SubCatId > 0)
                {
                    query += " and im.SubCategoryId= " + SubCatId;
                }
                if (LocationType == 2)
                {
                    query += " and im.ShowroomQty > 0";
                }
                else if (LocationType == 3)
                {
                    query += " and im.WharehouseQty > 0";
                }
                else if (LocationType == 0)
                {
                    query += " and im.Qty < -100 ";
                }
                query += " order by im.InventoryId";
                res = context.Set<InventoryCategoryPrintModel>().FromSqlRaw(query).ToList();

            }
            catch(Exception e) { }
            return res;
        }
    }
}
