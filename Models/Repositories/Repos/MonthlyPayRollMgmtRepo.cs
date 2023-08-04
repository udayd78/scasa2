using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class MonthlyPayRollMgmtRepo : IMonthlyPayRollMgmtRepo
    {
        private readonly MyDbContext context;
        public MonthlyPayRollMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }
        public ProcessResponse SaveMonthPay(MonthlyPayRoll request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.PayRollId > 0)
                {
                    MonthlyPayRoll mpr = context.monthlyPayRolls.Where(a => a.IsDeleted == false && a.PayRollId==request.PayRollId).FirstOrDefault();
                    context.Entry(mpr).CurrentValues.SetValues(request);
                    context.SaveChanges();
                    response.currentId = request.PayRollId;
                    response.statusCode = 1;
                    response.statusMessage = "Success";
                }
                else
                {
                    request.IsDeleted = false;
                    context.monthlyPayRolls.Add(request);
                    context.SaveChanges();
                    response.currentId = request.PayRollId;
                    response.statusCode = 1;
                    response.statusMessage = "success";

                }
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }
            return response;
        }
        public List<MPayRollDisplayModel> GetAllMonthPayRolls()
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            try
            {
                response = (from m in context.monthlyPayRolls
                            join u in context.userMasters on m.StaffId equals u.UserId
                            join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                            where (m.IsDeleted == false)
                            select new MPayRollDisplayModel
                            {
                                PayRollId = m.PayRollId,
                                StaffId = m.StaffId,
                                ECode = u.EmployeeCode,
                                EName = u.UserName,
                                EmployeeType = ut.TypeName,
                                MonthNumber = m.MonthNumber,
                                YearNumber = m.YearNumber,
                                NumberOfWorkingDays = m.NumberOfWorkingDays,
                                NumberPresentDays = m.NumberPresentDays,
                                NumberOfAbsentDays = (int)m.NumberOfWorkingDays-(int)m.NumberPresentDays,
                                BasicSalary = m.BasicSalary,
                                HRA = m.HRA,
                                DearnessAllowance = m.DearnessAllowance,
                                FoodAllowance = m.FoodAllowance,
                                Conveyance = m.Conveyance,
                                MedicalAllowances = m.MedicalAllowances,
                                OtherAllowances = m.OtherAllowances,
                                ProvidentFund = m.ProvidentFund,
                                ProfessionalTax = m.ProfessionalTax,
                                ESIFund = m.ESIFund,
                                TDS = m.TDS,
                                Bonus = m.Bonus,
                                GrossSalary = m.GrossSalary,
                                TotalDeductions = m.TotalDeductions,
                                NetSalary = m.NetSalary,
                                Remarks = m.Remarks,
                                IsPostedToFinance=m.IsPostedToFinance
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public MonthlyPayRoll GetMPayRollBypayRollId(int id)
        {
            MonthlyPayRoll response = new MonthlyPayRoll();
            try
            {
                response = context.monthlyPayRolls.Where(a => a.IsDeleted == false && a.PayRollId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public MPayRollDisplayModel GetMPayRollByStaffIdWithDate(int sid, int month,int year)
        {
            MPayRollDisplayModel response = new MPayRollDisplayModel();
            try
            {
                response = (from m in context.monthlyPayRolls
                            join u in context.userMasters on m.StaffId equals u.UserId
                            join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                            where (m.IsDeleted == false && m.MonthNumber ==month && m.YearNumber ==year &&m.StaffId == sid)
                            select new MPayRollDisplayModel
                            {
                                PayRollId=m.PayRollId,
                                StaffId = m.StaffId,
                                ECode = u.EmployeeCode,
                                EName = u.UserName,
                                EmployeeType = ut.TypeName,
                                MonthNumber = m.MonthNumber,
                                YearNumber = m.YearNumber,
                                NumberOfWorkingDays = m.NumberOfWorkingDays,
                                NumberPresentDays = m.NumberPresentDays,
                                NumberOfAbsentDays = (int)m.NumberOfWorkingDays - (int)m.NumberPresentDays,
                                BasicSalary = m.BasicSalary,
                                HRA = m.HRA,
                                DearnessAllowance = m.DearnessAllowance,
                                FoodAllowance = m.FoodAllowance,
                                Conveyance = m.Conveyance,
                                MedicalAllowances = m.MedicalAllowances,
                                OtherAllowances = m.OtherAllowances,
                                ProvidentFund = m.ProvidentFund,
                                ProfessionalTax = m.ProfessionalTax,
                                ESIFund = m.ESIFund,
                                TDS = m.TDS,
                                Bonus = m.Bonus,
                                GrossSalary = m.GrossSalary,
                                TotalDeductions = m.TotalDeductions,
                                NetSalary = m.NetSalary,
                                Remarks = m.Remarks,
                                IsPostedToFinance = m.IsPostedToFinance
                            }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List<MPayRollDisplayModel> GetMPayRollByStaffId(int sid)
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            try
            {
                response = (from m in context.monthlyPayRolls
                            join u in context.userMasters on m.StaffId equals u.UserId
                            join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                            where (m.IsDeleted == false && m.StaffId ==sid)
                            select new MPayRollDisplayModel
                            {
                                PayRollId = m.PayRollId,
                                StaffId = m.StaffId,
                                ECode = u.EmployeeCode,
                                EName = u.UserName,
                                EmployeeType = ut.TypeName,
                                MonthNumber = m.MonthNumber,
                                YearNumber = m.YearNumber,
                                NumberOfWorkingDays = m.NumberOfWorkingDays,
                                NumberPresentDays = m.NumberPresentDays,
                                NumberOfAbsentDays = 0,
                                BasicSalary = m.BasicSalary,
                                HRA = m.HRA,
                                DearnessAllowance = m.DearnessAllowance,
                                FoodAllowance = m.FoodAllowance,
                                Conveyance = m.Conveyance,
                                MedicalAllowances = m.MedicalAllowances,
                                OtherAllowances = m.OtherAllowances,
                                ProvidentFund = m.ProvidentFund,
                                ProfessionalTax = m.ProfessionalTax,
                                ESIFund = m.ESIFund,
                                TDS = m.TDS,
                                Bonus = m.Bonus,
                                GrossSalary = m.GrossSalary,
                                TotalDeductions = m.TotalDeductions,
                                NetSalary = m.NetSalary,
                                Remarks = m.Remarks,
                                IsPostedToFinance = m.IsPostedToFinance
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public List<MPayRollDisplayModel> GetPayRollByMonth(int month, int year)
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            try
            {
                response = (from m in context.monthlyPayRolls
                            join u in context.userMasters on m.StaffId equals u.UserId
                            join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                            where (m.IsDeleted == false &&m.MonthNumber ==month&&m.YearNumber ==year)
                            select new MPayRollDisplayModel
                            {
                                PayRollId = m.PayRollId,
                                StaffId = m.StaffId,
                                ECode = u.EmployeeCode,
                                EName = u.UserName,
                                EmployeeType = ut.TypeName,
                                MonthNumber = m.MonthNumber,
                                YearNumber = m.YearNumber,
                                NumberOfWorkingDays = m.NumberOfWorkingDays,
                                NumberPresentDays = m.NumberPresentDays,
                                NumberOfAbsentDays = 0,
                                BasicSalary = m.BasicSalary,
                                HRA = m.HRA,
                                DearnessAllowance = m.DearnessAllowance,
                                FoodAllowance = m.FoodAllowance,
                                Conveyance = m.Conveyance,
                                MedicalAllowances = m.MedicalAllowances,
                                OtherAllowances = m.OtherAllowances,
                                ProvidentFund = m.ProvidentFund,
                                ProfessionalTax = m.ProfessionalTax,
                                ESIFund = m.ESIFund,
                                TDS = m.TDS,
                                Bonus = m.Bonus,
                                GrossSalary = m.GrossSalary,
                                TotalDeductions = m.TotalDeductions,
                                NetSalary = m.NetSalary,
                                Remarks = m.Remarks,
                                IsPostedToFinance = m.IsPostedToFinance
                            }).ToList();
            }catch(Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public List<MPayRollDisplayModel> GetPayRollsByStaffOfYear(int sid,int year)
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            try
            {
                response= (from m in context.monthlyPayRolls
                           join u in context.userMasters on m.StaffId equals u.UserId
                           join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                           where (m.IsDeleted == false && m.StaffId == sid && (m.YearNumber == year || m.MonthNumber ==year))
                           select new MPayRollDisplayModel
                           {
                               PayRollId = m.PayRollId,
                               StaffId = m.StaffId,
                               ECode = u.EmployeeCode,
                               EName = u.UserName,
                               EmployeeType = ut.TypeName,
                               MonthNumber = m.MonthNumber,
                               YearNumber = m.YearNumber,
                               NumberOfWorkingDays = m.NumberOfWorkingDays,
                               NumberPresentDays = m.NumberPresentDays,
                               NumberOfAbsentDays = 0,
                               BasicSalary = m.BasicSalary,
                               HRA = m.HRA,
                               DearnessAllowance = m.DearnessAllowance,
                               FoodAllowance = m.FoodAllowance,
                               Conveyance = m.Conveyance,
                               MedicalAllowances = m.MedicalAllowances,
                               OtherAllowances = m.OtherAllowances,
                               ProvidentFund = m.ProvidentFund,
                               ProfessionalTax = m.ProfessionalTax,
                               ESIFund = m.ESIFund,
                               TDS = m.TDS,
                               Bonus = m.Bonus,
                               GrossSalary = m.GrossSalary,
                               TotalDeductions = m.TotalDeductions,
                               NetSalary = m.NetSalary,
                               Remarks = m.Remarks,
                               IsPostedToFinance = m.IsPostedToFinance
                           }).ToList();
            }
            catch(Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List<MPayRollDisplayModel> GetPayRollsOfYear(int year)
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            try
            {
                response = (from m in context.monthlyPayRolls
                            join u in context.userMasters on m.StaffId equals u.UserId
                            join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                            where (m.IsDeleted == false && (m.YearNumber == year || m.MonthNumber ==year))
                            select new MPayRollDisplayModel
                            {
                                PayRollId = m.PayRollId,
                                StaffId = m.StaffId,
                                ECode = u.EmployeeCode,
                                EName = u.UserName,
                                EmployeeType = ut.TypeName,
                                MonthNumber = m.MonthNumber,
                                YearNumber = m.YearNumber,
                                NumberOfWorkingDays = m.NumberOfWorkingDays,
                                NumberPresentDays = m.NumberPresentDays,
                                NumberOfAbsentDays = 0,
                                BasicSalary = m.BasicSalary,
                                HRA = m.HRA,
                                DearnessAllowance = m.DearnessAllowance,
                                FoodAllowance = m.FoodAllowance,
                                Conveyance = m.Conveyance,
                                MedicalAllowances = m.MedicalAllowances,
                                OtherAllowances = m.OtherAllowances,
                                ProvidentFund = m.ProvidentFund,
                                ProfessionalTax = m.ProfessionalTax,
                                ESIFund = m.ESIFund,
                                TDS = m.TDS,
                                Bonus = m.Bonus,
                                GrossSalary = m.GrossSalary,
                                TotalDeductions = m.TotalDeductions,
                                NetSalary = m.NetSalary,
                                Remarks = m.Remarks,
                                IsPostedToFinance = m.IsPostedToFinance
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
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
    }
}
