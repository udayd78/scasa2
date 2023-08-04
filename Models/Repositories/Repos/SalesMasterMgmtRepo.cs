using Microsoft.EntityFrameworkCore;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class SalesMasterMgmtRepo : ISalesMasterMgmtRepo
    {
        private readonly MyDbContext context;

        public SalesMasterMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }
        #region CRFQ Master
        public ProcessResponse SaveCRFQ(CRFQMaster request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.cRFQMasters.Add(request);
                context.SaveChanges();
                responce.currentId = request.CRFQId;
                responce.statusCode = 1;
                responce.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce;

        }
        public List<CRFQMaster> GetAllCRFQ()
        {
            List<CRFQMaster> response = new List<CRFQMaster>();
            try
            {
                response = context.cRFQMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public CRFQMaster GetCRFQById(int id)
        {
            CRFQMaster response = new CRFQMaster();
            try
            {
                response = context.cRFQMasters.Where(a => a.IsDeleted == false && a.CRFQId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateCRFQ(CRFQMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                CRFQMaster cm = new CRFQMaster();
                cm = context.cRFQMasters.Where(a => a.CRFQId == request.CRFQId).FirstOrDefault();
                context.Entry(cm).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.CRFQId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Quote Master
        public ProcessResponse SaveQuote(QuoteMaster request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.quoteMasters.Add(request);
                context.SaveChanges();
                responce.currentId = request.QuoteId;
                responce.statusCode = 1;
                responce.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce;

        }
        public List<QuoteMaster> GetAllQuote()
        {
            List<QuoteMaster> response = new List<QuoteMaster>();
            try
            {
                response = context.quoteMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public QuoteMaster GetQuoteById(int id)
        {
            QuoteMaster response = new QuoteMaster();
            try
            {
                response = context.quoteMasters.Where(a => a.IsDeleted == false && a.QuoteId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateQuote(QuoteMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                QuoteMaster qm = new QuoteMaster();
                qm = context.quoteMasters.Where(a => a.QuoteId == request.QuoteId).FirstOrDefault();
                context.Entry(qm).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.QuoteId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region SO Master
        public ProcessResponse SaveSalesOrder(SalesOrderMaster request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.salesOrderMasters.Add(request);
                context.SaveChanges();
                responce.currentId = request.SOMId;
                responce.statusCode = 1;
                responce.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce;

        }

        public List<SalesOrderMaster> GetAllSales()
        {
            List<SalesOrderMaster> response = new List<SalesOrderMaster>();
            try
            {
                response = context.salesOrderMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public SalesOrderMaster GetSalesById(int id)
        {
            SalesOrderMaster response = new SalesOrderMaster();
            try
            {
                response = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.SOMId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateSales(SalesOrderMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                SalesOrderMaster sm = new SalesOrderMaster();
                sm = context.salesOrderMasters.Where(a => a.SOMId == request.SOMId).FirstOrDefault();
                context.Entry(sm).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.SOMId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        public ProcessResponse DeleteSaleOrder(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                SalesOrderMaster som = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.SOMId == id).FirstOrDefault();
                som.IsDeleted = true;
                context.Entry(som).CurrentValues.SetValues(som);
                context.SaveChanges();
                pr.currentId =id;
                pr.statusCode = 1;
                pr.statusMessage = "Success";
                List<ReservedQtyMaster> res = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.SOMId == id).ToList();
                foreach(ReservedQtyMaster r in res)
                {
                    InventoryMaster im = context.inventoryMasters.Where(a => a.IsDeleted == false && a.InventoryId == r.ProductId).FirstOrDefault();
                    ReservedQtyMaster rc = r;
                    r.IsDeleted = true;
                    InventoryMaster imne = im;
                    im.Qty += r.Quantity;
                    if(rc.WQty==0 && rc.SQty == 0)
                    {
                        im.WharehouseQty += r.Quantity;
                    }
                    else
                    {
                        if (rc.WQty > 0)
                        {
                            im.WharehouseQty += rc.WQty;
                        }
                        if (rc.SQty > 0)
                        {
                            im.ShowroomQty += rc.SQty;
                        }
                    }
                    context.Entry(rc).CurrentValues.SetValues(r);
                    context.Entry(imne).CurrentValues.SetValues(im);
                    context.SaveChanges();
                }
            }catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "failed";
                LogError(e);
            }
            return pr;
        }
        #endregion

        public List<ProductsDisplaySales> GetProductsByCats(int catid, int pageNumber = 1, int pageSize =12)
        {
            List<ProductsDisplaySales> response = new List<ProductsDisplaySales>();
            try
            {
                if (pageNumber > 1)
                {
                    response = (from i in context.inventoryMasters
                                join sc in context.subCategoryMasters on i.SubCategoryId equals sc.SubCategoryId
                                where i.CategoryId == catid && i.IsDeleted == false && i.MRPPrice > 0 && sc.IsDeleted==false
                                select new ProductsDisplaySales
                                {
                                    colorImages = i.ColorImage,
                                    InventroyId = i.InventoryId,
                                    ItemDescription = i.ItemDescription,
                                    MainImages = i.PrimaryImage,
                                    ModelNumber = i.ModelNumber,
                                    ColorName = i.ColorName,
                                    MRP = i.MRPPrice,
                                    Title = i.Title,
                                    AvaliableQtyW = i.WharehouseQty,
                                    AvailableQtyS = i.ShowroomQty,
                                    AvaliableQtyR=context.reservedQtyMasters.Where(a=>a.IsDeleted==false && a.ProductId==i.InventoryId && a.CurrentStatus!= "Despatched").Sum(b=>b.Quantity),
                                }).Skip((pageNumber - 1) * pageSize).Take(pageSize).
                                ToList();
                }
                else
                {
                    response = (from i in context.inventoryMasters
                                join sc in context.subCategoryMasters on i.SubCategoryId equals sc.SubCategoryId
                                where i.CategoryId == catid && i.IsDeleted == false && i.MRPPrice > 0 && sc.IsDeleted == false
                                select new ProductsDisplaySales
                                {
                                    colorImages = i.ColorImage,
                                    InventroyId = i.InventoryId,
                                    ItemDescription = i.ItemDescription,
                                    MainImages = i.PrimaryImage,
                                    ModelNumber = i.ModelNumber,
                                    ColorName = i.ColorName,
                                    MRP = i.MRPPrice,
                                    Title = i.Title,
                                    AvaliableQtyW = i.WharehouseQty,
                                    AvailableQtyS = i.ShowroomQty,
                                    AvaliableQtyR = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.ProductId == i.InventoryId && a.CurrentStatus != "Despatched").Sum(b => b.Quantity),
                                }).Take(pageSize).
                                ToList();
                }
            }catch(Exception e) { }
                return response;
        }
        public int GetCatProdsCount(int catid)
        {
            int cnt = 0;
            try
            {
                cnt = (from i in context.inventoryMasters
                       join sc in context.subCategoryMasters on i.SubCategoryId equals sc.SubCategoryId
                       where i.CategoryId == catid && i.IsDeleted == false && i.MRPPrice > 0 && sc.IsDeleted == false
                       select new ProductsDisplaySales
                       {
                           InventroyId = i.InventoryId
                       }).Count();
            }
            catch(Exception e) { }
            return cnt;
        }
        public List<ProductsDisplaySales> SearchProducts(int catid = 0,int subCatId=0, string search = "",int pageNumber=1, int pageSize = 12)
        {
            List<ProductsDisplaySales> response = new List<ProductsDisplaySales>();
            try
            {
                int skipamt = 0;
                if (pageNumber != 1)
                {
                    skipamt = (pageNumber - 1) * pageSize;
                }
                string query = @"select i.ColorImage colorImages, i.Title, i.ColorName, i.ItemDescription, i.PrimaryImage MainImages, 
                    i.ModelNumber, i.MRPPrice MRP, i.ShowroomQty AvailableQtyS, i.WharehouseQty AvaliableQtyW, i.InventoryId InventroyId ,i.InventoryId AvaliableQtyR
                    from InventoryMaster i
                    join SubCategoryMaster s on i.SubCategoryId = s.SubCategoryId
                    where ";

                if (!string.IsNullOrEmpty(search))
                {
                    query += " (i.Title like '%" + search + "%' or i.ItemDescription like '%" + search + "%' or i.ModelNumber like '%" + search + "%') and ";
                }
                if(catid> 0)
                {
                    query += " i.CategoryId = " + catid + " and ";
                }
                if(subCatId > 0)
                {
                    query += " i.SubCategoryId = " + subCatId + " and ";
                }
                query += " i.IsDeleted = 0 and s.IsDeleted = 0 order by i.Title ";
                query += " offset " + skipamt +" ROWS FETCH NEXT " +  pageSize + "  ROWS ONLY"; 
                response = context.Set<ProductsDisplaySales>().FromSqlRaw(query).ToList();


                //int skipamt = 0;
                //if (pageNumber != 1)
                //{
                //    skipamt = (pageNumber - 1) * pageSize;
                //}
                //if (catid > 0)
                //{
                //    if (subCatId > 0)
                //    {
                //        response = (from i in context.inventoryMasters
                //                    join s in context.subCategoryMasters on i.SubCategoryId equals s.SubCategoryId
                //                    where (i.IsDeleted == false && i.CategoryId == catid && s.IsDeleted == false && i.SubCategoryId==subCatId &&
                //             (i.Title.StartsWith(search) || i.ItemDescription.Contains(search) || i.ModelNumber.Contains(search)))
                //                    select new ProductsDisplaySales
                //                    {
                //                        colorImages = i.ColorImage,
                //                        Title = i.Title,
                //                        ColorName = i.ColorName,
                //                        InventroyId = i.InventoryId,
                //                        ItemDescription = i.ItemDescription,
                //                        MainImages = i.PrimaryImage,
                //                        ModelNumber = i.ModelNumber,
                //                        MRP = i.MRPPrice
                //                    }).Skip(skipamt).Take(pageSize).ToList();
                //    }
                //    else
                //    {
                //        response = (from i in context.inventoryMasters
                //                    join s in context.subCategoryMasters on i.SubCategoryId equals s.SubCategoryId
                //                    where (i.IsDeleted == false && i.CategoryId == catid && s.IsDeleted == false &&
                //             (i.Title.StartsWith(search) || i.ItemDescription.Contains(search) || i.ModelNumber.Contains(search) || s.SubCategoryName.Contains(search)))
                //                    select new ProductsDisplaySales
                //                    {
                //                        colorImages = i.ColorImage,
                //                        Title = i.Title,
                //                        ColorName = i.ColorName,
                //                        InventroyId = i.InventoryId,
                //                        ItemDescription = i.ItemDescription,
                //                        MainImages = i.PrimaryImage,
                //                        ModelNumber = i.ModelNumber,
                //                        MRP = i.MRPPrice
                //                    }).Skip(skipamt).Take(pageSize).ToList();
                //    }
                //}
                //else
                //{                   
                //        response = (from i in context.inventoryMasters
                //                    join s in context.subCategoryMasters on i.SubCategoryId equals s.SubCategoryId
                //                    where (i.IsDeleted == false &&
                //             (i.Title.StartsWith(search) || i.ItemDescription.Contains(search) || i.ModelNumber.Contains(search) || s.SubCategoryName.Contains(search)))
                //                    select new ProductsDisplaySales
                //                    {
                //                        colorImages = i.Title,
                //                        Title = i.Title,
                //                        ColorName = i.ColorName,
                //                        InventroyId = i.InventoryId,
                //                        ItemDescription = i.ItemDescription,
                //                        MainImages = i.PrimaryImage,
                //                        ModelNumber = i.ModelNumber,
                //                        MRP = i.MRPPrice
                //                    }).Skip(skipamt).Take(pageSize).ToList();                    
                //}

            }
            catch (Exception ex)
            {

            }

            return response;
        }
        public int SearchProdsCount(int catid=0, int subCatId = 0, string search = "")
        {
            int cnt = 0;
            try
            {
                string query = @"select count(i.InventoryId) as cnt
                    from InventoryMaster i
                    join SubCategoryMaster s on i.SubCategoryId = s.SubCategoryId
                    where ";

                if (!string.IsNullOrEmpty(search))
                {
                    query += " (i.Title like '%" + search + "%' or i.ItemDescription like '%" + search + "%' or i.ModelNumber like '%" + search + "%') and ";
                }
                if (catid > 0)
                {
                    query += " i.CategoryId = " + catid + " and ";
                }
                if (subCatId > 0)
                {
                    query += " i.SubCategoryId = " + subCatId + " and ";
                }
                query += " i.IsDeleted = 0 and s.IsDeleted = 0";
                cnt = context.Set<RecordsCountFromSql>().FromSqlRaw(query).AsEnumerable().Select(r => r.cnt).FirstOrDefault();
                //if (search == null)
                //{
                //    search = "";
                //}
                //if (catid > 0)
                //{
                //    if (subCatId > 0)
                //    {
                //        cnt = (from i in context.inventoryMasters
                //               join s in context.subCategoryMasters on i.SubCategoryId equals s.SubCategoryId
                //               where (i.IsDeleted == false && i.CategoryId == catid && i.SubCategoryId==subCatId &&
                //        (i.Title.StartsWith(search) || i.ItemDescription.Contains(search) || i.ModelNumber.Contains(search)))
                //               select new ProductsDisplaySales
                //               {
                //                   InventroyId = i.InventoryId,
                //               }).Count();
                //    }
                //    else
                //    {
                //        cnt = (from i in context.inventoryMasters
                //               join s in context.subCategoryMasters on i.SubCategoryId equals s.SubCategoryId
                //               where (i.IsDeleted == false && i.CategoryId == catid &&
                //        (i.Title.StartsWith(search) || i.ItemDescription.Contains(search) || i.ModelNumber.Contains(search) || s.SubCategoryName.Contains(search)))
                //               select new ProductsDisplaySales
                //               {
                //                   InventroyId = i.InventoryId,
                //               }).Count();
                //    }
                //}
                //else
                //{
                //    cnt = (from i in context.inventoryMasters
                //           join s in context.subCategoryMasters on i.SubCategoryId equals s.SubCategoryId
                //           where (i.IsDeleted == false &&
                //    (i.Title.StartsWith(search) || i.ItemDescription.Contains(search) || i.ModelNumber.Contains(search) || s.SubCategoryName.Contains(search)))
                //           select new ProductsDisplaySales
                //           {
                //               InventroyId = i.InventoryId,
                //           }).Count();
                //}
            }
            catch(Exception e) { }
            return cnt;
        }
        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }
        public List<MPayRollDisplayModel> GetMonthlyPayRoll(int staffId)
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            response = context.monthlyPayRolls.Where(a => a.IsDeleted == false && a.StaffId == staffId)
                .Select(b => new MPayRollDisplayModel
                {
                    BasicSalary = b.BasicSalary,
                    StaffId = b.StaffId,
                    Bonus = b.Bonus,
                    Conveyance = b.Conveyance,
                    CreatedBy = b.CreatedBy,
                    CreatedOn = b.CreatedOn,
                    DearnessAllowance = b.DearnessAllowance,
                    ECode = "",
                    EmployeeType = "",
                    EName = "",
                    ESIFund = b.ESIFund,
                    FoodAllowance = b.FoodAllowance,
                    GrossSalary = b.GrossSalary,
                    HRA = b.HRA,
                    IsDeleted = b.IsDeleted,
                    MedicalAllowances = b.MedicalAllowances,
                    MonthNumber = b.MonthNumber,
                    NetSalary = b.NetSalary,
                    NumberOfAbsentDays = 0,
                    NumberOfWorkingDays = b.NumberOfWorkingDays,
                    NumberPresentDays = b.NumberPresentDays,
                    OtherAllowances = b.OtherAllowances,
                    PaidBy = b.PaidBy,
                    PaidDate = b.PaidDate,
                    PayRollId = b.PayRollId,
                    ProfessionalTax = b.ProfessionalTax,
                    ProvidentFund = b.ProvidentFund,
                    Remarks = b.Remarks,
                    TDS = b.TDS,
                    TotalDeductions = b.TotalDeductions,
                    YearNumber = b.YearNumber
                }).OrderBy(b => b.YearNumber).ThenBy(c => c.MonthNumber).ToList();
            return response;
        }

        public List<LeadInfo> GetLeadInfo(int staffId)
        {
            UserMaster um = context.userMasters.Where(a => a.IsDeleted == false && a.UserId == staffId).FirstOrDefault();
            DateTime joinDate = (DateTime)um.DateOfJoin;
            List<LeadInfo> response = new List<LeadInfo>();
            DateTime dt = DateTime.Now;
            int Year = dt.Year;
            int CurrentMonth = dt.Month;
            int totalDays = dt.Day;
            int y = 1;
            if (joinDate.Year == dt.Year && joinDate.Month == dt.Month &&joinDate.Day<=dt.Day)
            {
                y = joinDate.Day;
            }

            for (int i = y; i <= totalDays; i++)
            {
                DateTime sDate = new DateTime(Year, CurrentMonth, i);
                DateTime eDate = sDate.AddDays(1);
                var att = context.customerSalesActivities.Where(a => a.IsDeleted == false && a.AttendedOn >= sDate
                && a.AttendedOn < eDate && a.StaffId == staffId).Select(b => b.CustomerId).Count();
                var timeDuration = context.customerSalesActivities.Where(a => a.IsDeleted == false && a.AttendedOn >= sDate
                && a.AttendedOn < eDate && a.StaffId == staffId).ToList();
                TimeSpan? difTime = new TimeSpan();
                if(timeDuration !=null)
                {
                    if(timeDuration.Count > 0)
                    {
                        foreach(var v in timeDuration)
                        {
                            if(v.EndTime != null)
                            {
                                difTime += v.EndTime - v.AttendedOn;
                            }
                            
                        }
                    }
                }
                var crfq = context.cRFQMasters.Where(a => a.StaffId == staffId && a.IsDeleted == false).Select(b => b.CustomerId).Count();
                var quotes = context.quoteMasters.Where(a => a.StaffId == staffId && a.IsDeleted == false).Select(b => b.CustomerId).Count();
                sDate = sDate.AddDays(-1);
                var sales = context.salesOrderMasters.Where(a => a.StaffId == staffId && a.IsDeleted == false && a.CreatedOn >= sDate && a.CreatedOn <= eDate).Select(b => b.CustomerId).Count();
                LeadInfo lead = new LeadInfo();
                lead.attendedCount = att;
                lead.crfqCount = crfq;
                lead.date = new DateTime(dt.Year, CurrentMonth, i);
                lead.hotCount = 0;
                lead.qouteCount = quotes;
                lead.Year = Year;
                lead.salesCount = sales;
                lead.attendedTime = difTime;
                lead.Month = i;
                response.Add(lead);
            }
            return response;
        }
        public List<LoansInformation> GetLoansInformation(int staffId)
        {
            List<LoansInformation> response = new List<LoansInformation>();
            response = context.staffLoans.Where(a => a.StaffId == staffId && a.IsDeleted == false)
                .Select(b => new LoansInformation
                {
                    AmountTaken = b.AmountTaken,
                    MonthlyEMI = b.MonthlyEMI,
                    NoofMonths = b.NoofMonths,
                    Remarks = b.Remarks,
                    RepaymentMode = b.RepaymentMode,
                    TakenOn = b.TakenOn,
                     Status=b.LoanStatus

                }).ToList();

            return response;
        }
        public List<AttendanceInfo> GetStaffAttendance(int staffId)
        {
            UserMaster um = context.userMasters.Where(a => a.IsDeleted == false && a.UserId==staffId).FirstOrDefault();
            DateTime joinDate = (DateTime)um.DateOfJoin;
            List<AttendanceInfo> response = new List<AttendanceInfo>();
            DateTime dt = DateTime.Now;
            int Year = dt.Year;
            int CurrentMonth = dt.Month;
            int FirstMonth = 1;
            if (joinDate.Year == dt.Year && joinDate.Month <= dt.Month)
            {
                FirstMonth = joinDate.Month;
            }
            for (int i = FirstMonth; i <= CurrentMonth; i++)
            {
                DateTime sDate = new DateTime(Year, i, 1);
                DateTime eDate = sDate.AddMonths(1).AddDays(-1);
                AttendanceInfo info = new AttendanceInfo();
                var p = context.attendanceMasters.Where(a => a.IsDeleted == false &&
                a.DateofAttendance >= sDate && a.DateofAttendance < eDate && a.AttStatus == "P" && a.StaffId == staffId)
                    .Select(b => b.StaffId).Count();
                //var pdays = context.mworkingDaysMasters.Where(a => a.IsDeleted == false &&
                //a.MonthNumber == i && a.YearNumber == Year).Select(b => b.NumberOfDays).FirstOrDefault();
                var a = context.attendanceMasters.Where(a => a.IsDeleted == false &&
                a.DateofAttendance >= sDate && a.DateofAttendance < eDate && a.AttStatus == "A" && a.StaffId == staffId)
                    .Select(b => b.StaffId).Count();
                info.AbsentDays =  a;
                info.Month = i;
                info.PresentDays = p;
                info.WorkingDays = eDate.Day;
                info.Year = Year;
                response.Add(info);
            }
            return response;
        }

        public void SaveActivity(CustomerSalesActivity ca)
        {
            context.customerSalesActivities.Add(ca);
            context.SaveChanges();
        }

        public CustomerSalesActivity GetActivity(int id)
        {
            return context.customerSalesActivities.Where(a => a.IsDeleted == false && a.CustomerId == id && a.EndTime == null).FirstOrDefault();
        }
        public void UpdateActivty(CustomerSalesActivity request)
        {
            CustomerSalesActivity ca = new CustomerSalesActivity();
            ca = context.customerSalesActivities.Where(a => a.ActivityId == request.ActivityId).FirstOrDefault();
            context.Entry(ca).CurrentValues.SetValues(request);
            context.SaveChanges();
        }

        public ProcessResponse SubmitSOForAccounts(int soid, int currentUserId)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                var subSo = context.saleOrdersForAccounts.Where(a => a.SOId == soid && a.IsDeleted == false).FirstOrDefault();
                if(subSo == null)
                {
                    SalesOrderMaster so = context.salesOrderMasters.Where(a => a.SOMId == soid).FirstOrDefault();
                    CustomerMaster cm = context.customerMasters.Where(a => a.Cid == so.CustomerId).FirstOrDefault();
                    UserMaster um = context.userMasters.Where(a => a.UserId == so.StaffId).FirstOrDefault();
                    var sd = context.salesOrderDetails.Where(a => a.SOMId == so.SOMId).ToList();
                    var umCurrent = context.userMasters.Where(a => a.UserId == currentUserId).FirstOrDefault();
                    SaleOrdersForAccounts sa = new SaleOrdersForAccounts();
                    sa.CreatedBy = currentUserId;
                    sa.CreatedOn = DateTime.Now;
                    sa.CurrentStatus = "Submitted for Payment";
                    sa.CustomerId = cm.Cid;
                    sa.DelivaryCharges = so.DelivaryCharges;
                    sa.DiscountBySe =(decimal) sd.Sum(a => a.DisAmtBySE) / sd.Count;
                    sa.DiscountBySHead = (decimal)sd.Sum(a => a.DisAmtByHead) / sd.Count;
                    sa.IsDeleted = false;
                    sa.LastModifiedBy_Id = currentUserId;
                    sa.LastModifiedBy_Name = umCurrent.UserName;
                    sa.LastModifiedOn = DateTime.Now;
                    sa.SOId = so.SOMId;
                    sa.StaffId = so.StaffId;
                    sa.TaxAmount = so.TaxAmount;
                    sa.TaxApplicable = so.TaxApplicable;
                    sa.TaxPercentage = so.TaxPercentage;
                    sa.TotalOrderValue = sd.Sum(a => a.TotalPrice)+so.RoundedValue;
                    sa.RoundedValue = so.RoundedValue;
                    sa.TotalValue = sd.Sum(a => a.TotalPrice) + so.DelivaryCharges+so.RoundedValue;
                    context.saleOrdersForAccounts.Add(sa);
                    context.SaveChanges();
                    so.CurrentStatus = "Submitted for Payment";
                    context.Entry(so).CurrentValues.SetValues(so); 
                    context.SaveChanges();
                    responce.statusCode = 1;
                    responce.statusMessage = "Success";
                }
                else
                {
                    responce.statusCode = 0;
                    responce.statusMessage = "Already Submitted";
                }
               
            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce; 
        }

        public List<SaleOrdersForAccountsModel> GetOrdersSubmittedForAccounts()
        {
            List<SaleOrdersForAccountsModel> myList = new List<SaleOrdersForAccountsModel>();
            try
            {
                myList = (from sa in context.saleOrdersForAccounts
                          join cm in context.customerMasters on sa.CustomerId equals cm.Cid
                          join um in context.userMasters on sa.StaffId equals um.UserId
                          join umL in context.userMasters on sa.LastModifiedBy_Id equals umL.UserId
                          where sa.IsDeleted == false
                          select new SaleOrdersForAccountsModel
                          {
                              IsDeleted = sa.IsDeleted,
                              CreatedBy = um.UserId,
                              CreatedOn = sa.CreatedOn,
                              CurrentStatus = sa.CurrentStatus,
                              CustomerDetails = cm.FullName,
                              CustomerId = sa.CustomerId,
                              DelivaryCharges = sa.DelivaryCharges,
                              DiscountBySe = sa.DiscountBySe,
                              DiscountBySHead = sa.DiscountBySHead,
                              LastModifiedBy_Id = sa.LastModifiedBy_Id,
                              LastModifiedBy_Name = umL.UserName,
                              LastModifiedOn = sa.LastModifiedOn,
                              SOId = sa.SOId,
                              StaffDetails = um.UserName,
                              StaffId = sa.StaffId,
                              TaxAmount = sa.TaxAmount,
                              TaxApplicable = sa.TaxApplicable,
                              TaxPercentage = sa.TaxPercentage,
                              TotalOrderValue = sa.TotalOrderValue,
                              TotalValue = sa.TotalValue,
                               ReceivedAmount = context.salesReceipts.Where(a=>a.SOId == sa.SOId && a.IsDeleted == false).Sum(b=>b.AmountReceived),
                              TRId = sa.TRId ,
                              

                          }).ToList();
            }
            catch(Exception ex)
            {

            }
            return myList;
        }

        public List<FinanceHeads> GetFinanceHeads()
        {
            FinanceGroups fgb = context.financeGroups.Where(a => a.IsDeleted == false && (a.GroupName == "Bank Accounts")).FirstOrDefault();
            FinanceGroups fgc = context.financeGroups.Where(a => a.IsDeleted == false && (a.GroupName == "Cash-in-hand")).FirstOrDefault();
            List<FinanceHeads> myList = new List<FinanceHeads>();
            myList = context.financeHeads.Where(a => a.IsDeleted == false && (a.GroupId == fgb.GroupId || a.GroupId == fgc.GroupId)).ToList();
            return myList;
        }
        public ProcessResponse SaveSalesReceipt(SalesReceipts request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                request.IsPostedToFinance = false;
                request.PaymentMode = context.financeHeads.Where(a => a.HeadId == request.FinanceHeadId).Select(b => b.HeadName).FirstOrDefault();
                context.salesReceipts.Add(request);
                context.SaveChanges();
                    responce.statusCode = 1;
                    responce.statusMessage = "Success";

                // check for full or partial payment

                var paidAmount = context.salesReceipts.Where(a => a.SOId == request.SOId).Sum(b => b.AmountReceived);
                var so = context.saleOrdersForAccounts.Where(a => a.SOId == request.SOId).FirstOrDefault();
                var dchrge = so.DelivaryCharges == null ? 0 : so.DelivaryCharges;
                if(paidAmount < (so.TotalOrderValue+dchrge))
                {
                    SalesOrderMaster sm = context.salesOrderMasters.Where(a => a.SOMId == request.SOId).FirstOrDefault();
                    sm.CurrentStatus = "Partially Paid";
                    context.Entry(sm).CurrentValues.SetValues(sm);
                    context.SaveChanges();

                    so.CurrentStatus = "Partially Paid";
                    context.Entry(so).CurrentValues.SetValues(so);
                    context.SaveChanges();
                }
                if(paidAmount == (so.TotalOrderValue + dchrge))
                {
                    SalesOrderMaster sm = context.salesOrderMasters.Where(a => a.SOMId == request.SOId).FirstOrDefault();
                    sm.CurrentStatus = "Paid Full";
                    context.Entry(sm).CurrentValues.SetValues(sm);
                    context.SaveChanges();

                    so.CurrentStatus = "Paid Full";
                    context.Entry(so).CurrentValues.SetValues(so);
                    context.SaveChanges();
                }
                     
               

            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce;
        }

        public List<SalesReceiptsModel> GetSalesReciepts(int soid)
        {
            List<SalesReceiptsModel> myList = new List<SalesReceiptsModel>();
            try
            {
                myList = (from sr in context.salesReceipts
                          join so in context.salesOrderMasters on sr.SOId equals so.SOMId
                          join c in context.customerMasters on sr.CustomerId equals c.Cid
                          join u in context.userMasters on so.StaffId equals u.UserId
                          join uR in context.userMasters on sr.ReceivedBy equals uR.UserId
                          join f in context.financeHeads on c.FinanceHeadId equals f.HeadId into tempF
                          from fh in tempF.DefaultIfEmpty()
                          where sr.IsDeleted == false && sr.SOId == soid
                          select new SalesReceiptsModel
                          {
                              SOId = sr.SOId,
                              IsDeleted = sr.IsDeleted,
                              AmountReceived = sr.AmountReceived,
                              CustomerDetails = c.FullName,
                              CustomerId = sr.CustomerId,
                              FinanceHeadId = sr.FinanceHeadId,
                              FinanceHeadName = fh.HeadName,
                              InstrumentDate = sr.InstrumentDate,
                              InstrumentDetails = sr.InstrumentDetails,
                              InstrumentNumber = sr.InstrumentNumber,
                              IsPostedToFinance = sr.IsPostedToFinance,
                              PaymentMode = sr.PaymentMode,
                              ReceivedBy = sr.ReceivedBy,
                              RecievedBy_Name = uR.UserName,
                              RecievedOn = sr.RecievedOn,
                              StaffDetails = u.UserName,
                              TRId = sr.TRId
                          }).ToList();
                
            }
            catch (Exception ex)
            { 
                LogError(ex);
            }
            return myList;
        }
         
        public ProcessResponse PostReceiptToFinance(int trid)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                SalesReceipts sr = context.salesReceipts.Where(a => a.TRId == trid).FirstOrDefault();
                if(sr!=null)
                {
                    FinanceTransactions ft = new FinanceTransactions();
                    ft.ChequeDate = sr.InstrumentDate;
                    ft.ChequeDetails = sr.InstrumentDetails;
                    ft.ChequeNo = sr.InstrumentNumber;
                    ft.Credit = sr.AmountReceived;
                    ft.CustomerId = sr.CustomerId;
                    ft.DateOfTransaction = sr.RecievedOn;
                    ft.Debit = 0;
                    ft.Description = "Payment received against order " + sr.SOId;
                    ft.DoneBy = context.userMasters.Where(a => a.UserId == sr.ReceivedBy).Select(b => b.UserName).FirstOrDefault();
                    ft.FromHeadID = sr.FinanceHeadId;
                    ft.remarks = "Payment received against order " + sr.SOId;
                    ft.TTime = DateTime.Now;
                    ft.UserId = sr.ReceivedBy ;
                    ft.VoucherType = "Receipt";
                    int? vNo = context.financeTransactions.OrderByDescending(b => b.TRID).Select(b => b.VoucherNumber).FirstOrDefault();
                    if(vNo == 0)
                    {
                        vNo = 1;
                    }else
                    {
                        vNo = vNo + 1;
                    }
                    ft.VoucherNumber = vNo;
                    //get from heaid 
                    var fromHead = context.financeHeads.Where(a => a.IsDeleted == false && a.CustomerId == sr.CustomerId).FirstOrDefault();
                    if(fromHead != null)
                    {
                        ft.ToHeaID = fromHead.HeadId;
                    }
                    else
                    {
                        CustomerMaster cm = context.customerMasters.Where(a => a.Cid == sr.CustomerId).FirstOrDefault();
                        FinanceHeads fhNew = new FinanceHeads();
                        fhNew.CurrentBallance = 0;
                        fhNew.CustomerId = cm.Cid;
                        var fGroup = context.financeGroups.Where(a => a.IsDeleted == false && a.GroupName == "Sales Accounts").FirstOrDefault();
                        if(fGroup == null)
                        {
                            fGroup = new FinanceGroups();
                            fGroup.GroupName = "Customers";
                            fGroup.AcountType = null;
                            //fGroup.GroupCode = "Sales Accounts";
                            fGroup.IsDeleted = false;
                            context.financeGroups.Add(fGroup);
                            context.SaveChanges();
                            fGroup = context.financeGroups.Where(a => a.IsDeleted == false && a.GroupName == "Sales Accounts").FirstOrDefault();
                        }
                        fhNew.GroupId = fGroup.GroupId;
                        fhNew.HeadCode = "Customer";
                        fhNew.HeadName = cm.FullName;
                        fhNew.IsDeleted = false;
                        fhNew.StartingBallance = 0;
                        context.financeHeads.Add(fhNew);
                        context.SaveChanges();
                        cm.FinanceHeadId = fhNew.HeadId;
                        context.Entry(cm).CurrentValues.SetValues(cm);
                        context.SaveChanges();
                        fromHead = fhNew;
                    }
                    ft.ToHeaID = fromHead.HeadId;
                    context.financeTransactions.Add(ft);
                    context.SaveChanges();
                    {
                        FinanceTransactions ft2 = new FinanceTransactions();
                        ft2.ChequeDate = sr.InstrumentDate;
                        ft2.ChequeDetails = sr.InstrumentDetails;
                        ft2.ChequeNo = sr.InstrumentNumber;
                        ft2.Credit = 0;
                        ft2.CustomerId = sr.CustomerId;
                        ft2.DateOfTransaction = sr.RecievedOn;
                        ft2.Debit = sr.AmountReceived;
                        ft2.Description = "Payment received against order " + sr.SOId;
                        ft2.DoneBy = ft.DoneBy;
                        
                        ft2.remarks = "Payment received against order " + sr.SOId;
                        ft2.TTime = DateTime.Now;
                        ft2.UserId = ft.ToHeaID;
                        ft2.VoucherType = "Receipt";                        
                        ft2.VoucherNumber = ++vNo; 
                        ft2.FromHeadID = fromHead.HeadId;                            
                        ft2.ToHeaID = sr.FinanceHeadId;
                        ft2.RelatedTrId = ft.TRID;
                        context.financeTransactions.Add(ft2);
                        context.SaveChanges();
                        ft.RelatedTrId = ft2.TRID;
                        FinanceTransactions abc = context.financeTransactions.Where(a => a.TRID == ft.TRID).FirstOrDefault();
                        context.Entry(abc).CurrentValues.SetValues(ft);
                        context.SaveChanges();
                    }
                    // update headid balance
                    FinanceHeads fhUPdate = context.financeHeads.Where(a => a.HeadId == ft.ToHeaID).FirstOrDefault();
                    fhUPdate.CurrentBallance -= sr.AmountReceived;
                    context.Entry(fhUPdate).CurrentValues.SetValues(fhUPdate);
                    context.SaveChanges();
                    FinanceHeads thUpdate = context.financeHeads.Where(a => a.HeadId == ft.FromHeadID).FirstOrDefault();
                    thUpdate.CurrentBallance += sr.AmountReceived;
                    context.Entry(thUpdate).CurrentValues.SetValues(thUpdate);
                    context.SaveChanges();

                    //update receit

                    var s = context.salesReceipts.Where(a => a.TRId == trid).FirstOrDefault();
                    s.IsPostedToFinance = true;
                    context.Entry(s).CurrentValues.SetValues(s);
                    context.SaveChanges();
                    pr.statusCode = 1;
                    pr.statusMessage = "Success";
                }
            }
            catch(Exception ex)
            {
                ex.Source = "Post receipt to finance";
                LogError(ex);
                pr.statusCode = 0;
                pr.statusMessage = "Failed to post";
            }
            return pr;
        }
        public CompanyMaster GetCompany()
        {
            return context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
        }
    }
}