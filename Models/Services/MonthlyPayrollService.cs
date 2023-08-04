using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class MonthlyPayrollService : IMonthlyPayrollService
    {
        private readonly IMonthlyPayRollMgmtRepo mPRepo;
        private readonly IPayRollMgmtRepo pRRepo;
        private readonly IAttendanceMgmtRepo aRepo;
        private readonly IMWorkingDaysMgmtRepo mDRepo;
        private readonly IUserMgmtRepo uRepo;
        private readonly IStaffLoansMgmtRepo sLRepo;
        public MonthlyPayrollService(IMonthlyPayRollMgmtRepo _mPRepo,IPayRollMgmtRepo _pRepo,IAttendanceMgmtRepo _aRepo,
                                     IMWorkingDaysMgmtRepo _mDRepo,IUserMgmtRepo _uRepo, IStaffLoansMgmtRepo _sLRepo)
        {
            mPRepo = _mPRepo;
            pRRepo = _pRepo;
            aRepo = _aRepo;
            mDRepo = _mDRepo;
            uRepo = _uRepo;
            sLRepo = _sLRepo;
        }
        public ProcessResponse SaveMonthlyPayRoll(int staffId,int month,int year, int createdById)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                UserMaster um = uRepo.GetUserById(staffId);
                DateTime jD = (DateTime)um.DateOfJoin;
                MPayRollDisplayModel myObj = mPRepo.GetMPayRollByStaffIdWithDate(staffId, month, year);
                if (myObj == null)
                {
                    PayRollMaster prm = pRRepo.GetPayRollByUserId(staffId);
                    if (prm.BasicSalary != 0)
                    {
                        //UserMaster created = uRepo.GetUserById(createdById);
                        //MWorkingDaysDisplay mwd = mDRepo.GetDaysByMonYer(month, year);
                        int presentDays = 0;
                        int leaveDays = 0;
                        int weakoffDays = 0;
                        int AbsentDays = 0;
                        int allDays = 0;
                        string att;
                        var startDate = new DateTime(year, month, 1);
                        var endDate = startDate.AddMonths(1).AddDays(-1);
                        int todayDay = endDate.Day;
                        int noOfconsideredDays = endDate.Day;
                        if ((year<jD.Year)||(year==jD.Year && month < jD.Month))
                        {
                            response.statusCode = 0;
                            response.statusMessage = "Salary Before Joining date is not possible";
                            return response;
                        }
                        else if(year == jD.Year && month == jD.Month &&jD.Day>1)
                        {
                            startDate = new DateTime(year, month, jD.Day);
                            noOfconsideredDays -= jD.Day;
                        }
                        for(int i = startDate.Day; i <= todayDay; i++)
                        {
                            DateTime cDay = new DateTime(year, month, i);
                            att = uRepo.GetPresentDayAttendance(staffId, cDay);
                            if (att == "P")
                            {
                                presentDays++;
                                allDays++;
                            }
                            if (att == "A")
                            {
                                AbsentDays++;
                                allDays++;
                            }
                            if (att == "L")
                            {
                                leaveDays++;
                                allDays++;
                            }
                            if (att == "W")
                            {
                                weakoffDays++;
                                presentDays++;
                                allDays++;
                            }
                        }
                        if (allDays != (noOfconsideredDays+1))
                        {                        
                            response.statusCode = 0;
                            response.statusMessage = "Please Give attendance for all days to generate salary";
                            return response;
                        }
                        MonthlyPayRoll mpr = new MonthlyPayRoll();
                        mpr.StaffId = staffId;
                        mpr.MonthNumber = month;
                        mpr.YearNumber = year;
                        mpr.CreatedOn = DateTime.Now;
                        mpr.CreatedBy = createdById;
                        mpr.NumberOfWorkingDays = todayDay;
                        mpr.NumberPresentDays = presentDays;
                        if (prm.BasicSalary == null)
                        {
                            mpr.BasicSalary = 0;
                        }
                        else {
                            mpr.BasicSalary = prm.BasicSalary;
                        }
                        if (prm.HRA == null)
                        {
                            mpr.HRA = 0;
                        }
                        else
                        {
                            mpr.HRA = prm.HRA;
                        }
                        if (prm.DearnessAllowance == null)
                        {
                            mpr.DearnessAllowance = 0;
                        }
                        else
                        {
                            mpr.DearnessAllowance = prm.DearnessAllowance;
                        }
                        if (prm.FoodAllowance == null)
                        {
                            mpr.FoodAllowance = 0;
                        }
                        else
                        {
                            mpr.FoodAllowance = prm.FoodAllowance;
                        }
                        if (prm.Conveyance == null)
                        {
                            mpr.Conveyance = 0;
                        }
                        else
                        {
                            mpr.Conveyance = prm.Conveyance;
                        }
                        if (prm.MedicalAllowances == null)
                        {
                            mpr.MedicalAllowances = 0;
                        }
                        else
                        {
                            mpr.MedicalAllowances = prm.MedicalAllowances;
                        }
                        if (prm.OtherAllowances == null)
                        {
                            mpr.OtherAllowances = 0;
                        }
                        else
                        {
                            mpr.OtherAllowances = prm.OtherAllowances;
                        }
                        mpr.GrossSalary = (mpr.BasicSalary + mpr.HRA + mpr.DearnessAllowance + mpr.FoodAllowance + mpr.Conveyance + mpr.MedicalAllowances + mpr.OtherAllowances);
                        if (prm.ProvidentFund == null)
                        {
                            mpr.ProvidentFund = 0;
                        }
                        else
                        {
                            mpr.ProvidentFund = prm.ProvidentFund;
                        }
                        if (prm.ProfessionalTax == null)
                        {
                            mpr.ProfessionalTax = 0;
                        }
                        else
                        {
                            mpr.ProfessionalTax = prm.ProfessionalTax;
                        }
                        if (prm.ESIFund == null)
                        {
                            mpr.ESIFund = 0;
                        }
                        else
                        {
                            mpr.ESIFund = prm.ESIFund;
                        }
                        if (prm.TDS == null)
                        {
                            mpr.TDS = 0;
                        }
                        else
                        {
                            mpr.TDS = prm.TDS;
                        }
                        mpr.TotalDeductions = (mpr.ProvidentFund + mpr.ProfessionalTax + mpr.ESIFund + mpr.TDS);                   
                    
                        decimal temp = (decimal)(mpr.GrossSalary);
                        decimal dailySalary = (decimal)(temp / mpr.NumberOfWorkingDays);
                        int noOfSalaryCutDays = 0;
                        if (leaveDays > 1)
                        {
                            noOfSalaryCutDays = --leaveDays;
                        }
                        if (AbsentDays >= 1)
                        {
                            noOfSalaryCutDays += AbsentDays;
                        }
                        if (startDate.Day != 1)
                        {
                            noOfSalaryCutDays += (startDate.Day - 1);
                        }
                        if (noOfSalaryCutDays != 0)
                        {
                            mpr.TotalDeductions += (dailySalary*noOfSalaryCutDays);
                        }


                        List<StaffLoans> sLoans = sLRepo.GetAllLoansByUserId(staffId);
                        if (sLoans != null)
                        {
                            foreach(StaffLoans l in sLoans)
                            {
                                mpr.TotalDeductions += l.MonthlyEMI;
                            }
                        }

                        mpr.NetSalary = mpr.GrossSalary-mpr.TotalDeductions;
                        mpr.PaidBy = createdById;
                        mpr.PaidDate = DateTime.Now;
                        mpr.IsPostedToFinance = false;
                        response = mPRepo.SaveMonthPay(mpr);
                        if (response.statusCode == 1)
                        {
                            if (sLoans != null)
                            {
                                foreach (StaffLoans l in sLoans)
                                {
                                    StaffLoanReceipts slr = new StaffLoanReceipts();
                                    slr.LoanId = l.LoanId;
                                    slr.IsDeleted = false;
                                    slr.AmountDeducted = l.MonthlyEMI;
                                    slr.DeductedOn = DateTime.Now;
                                    slr.MonthNumber = month;
                                    sLRepo.SaveLoanReceipts(slr);
                                    if (l.RepaymentMode == "Single")
                                    {
                                        StaffLoans sl = sLRepo.GetStaffLoanById(l.LoanId);
                                        sl.LoanStatus = "Closed";
                                    }
                                    else if (l.RepaymentMode == "EMI")
                                    {
                                        List<StaffLoanReceipts> slrl = sLRepo.GetReciptsByLoanId(l.LoanId);
                                        if (slrl != null)
                                        {
                                            int i = 0;
                                            foreach(StaffLoanReceipts r in slrl)
                                            {
                                                i += (int)r.AmountDeducted;
                                            }
                                            int diff = (int)l.AmountTaken - i;
                                            if (i < 50)
                                            {
                                                StaffLoans sl = sLRepo.GetStaffLoanById(l.LoanId);
                                                sl.LoanStatus = "Closed";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Basic Salary does not Exists";
                    }
                }
                else
                {
                    response.statusCode = 0;
                    response.statusMessage = "Already Exists";
                }
            }
            catch(Exception ex)
            {
                mPRepo.LogError(ex);
            }
            return response;
        }
        public ProcessResponse UpdateMonthlyPayroll(MonthlyPayRoll request)
        {
            return mPRepo.SaveMonthPay(request);
        }
        public List<MPayRollDisplayModel> GetMPayRollDisplays(int staffId, int month, int year)
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            if(staffId != 0)
            {
                if(month !=0 && year != 0)
                {
                    MPayRollDisplayModel myObj = mPRepo.GetMPayRollByStaffIdWithDate(staffId, month, year);
                    if (myObj != null)
                    {
                        response.Add(myObj);
                    }
                }
                else if(month==0 && year == 0)
                {
                    response = mPRepo.GetMPayRollByStaffId(staffId);
                }else if(month==0 && year != 0)
                {
                    response = mPRepo.GetPayRollsByStaffOfYear(staffId, year);
                }else if(month!=0 && year == 0)
                {
                    response = mPRepo.GetPayRollsByStaffOfYear(staffId, month);
                }
            }
            else
            {
                if(month != 0 && year != 0)
                {
                    response = mPRepo.GetPayRollByMonth(month, year);
                }
                else if(month == 0 && year == 0)
                {
                    response = mPRepo.GetAllMonthPayRolls();
                }else if(month==0 && year != 0)
                {
                    response = mPRepo.GetPayRollsOfYear(year);
                }
                else if (month != 0 && year == 0)
                {
                    response = mPRepo.GetPayRollsOfYear(month);
                }
            }
            return response;
        }
        public MonthlyPayRoll GetMonthlyPayRollById(int mId)
        {
            MonthlyPayRoll response = new MonthlyPayRoll();
            response = mPRepo.GetMPayRollBypayRollId(mId);
            return response;
        }
    }
}
