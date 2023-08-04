using SCASA.Models.Utilities;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class MWorkingDaysMgmtService : IMWorkingDaysMgmtService
    {
        private readonly IMWorkingDaysMgmtRepo mRepo;

        public MWorkingDaysMgmtService(IMWorkingDaysMgmtRepo _mrepo)
        {
            mRepo = _mrepo;
        }

        public ProcessResponse SaveDays(MonthlyWorkingDays request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                
                if (request.TRId>0)
                {
                    MonthlyWorkingDays md = mRepo.GetDaysById(request.TRId);
                    md.MonthNumber = request.MonthNumber;
                    md.YearNumber = request.YearNumber;

                    response = mRepo.UpdateDays(request);
                }
                else
                {
                    MWorkingDaysDisplay md = new MWorkingDaysDisplay();
                    md = mRepo.GetDaysByMonYer(request.MonthNumber, request.YearNumber);
                    if (md == null)
                    {
                        response = mRepo.SaveWorkingDays(request);

                    }
                    else
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Data Already Existed with Same Month and year";

                    }
                }
            }
            catch(Exception ex)
            {
                mRepo.LogError(ex);
            }
            return response;
        }

        public List<MWorkingDaysDisplay> GetAllDays(int MonthNumber, int YearNumber)
        {
            List<MWorkingDaysDisplay> myObj = new List<MWorkingDaysDisplay>();
            if (MonthNumber == 0 && YearNumber == 0)
            {
                myObj = mRepo.GetAllWorkingDays();
                foreach (MWorkingDaysDisplay m in myObj)
                {
                    switch (m.MonthNumber)
                    {
                        case 1:
                            m.MonthName = "January";
                            break;

                        case 2:
                            m.MonthName = "February";
                            break;
                        case 3:
                            m.MonthName = "March";
                            break;
                        case 4:
                            m.MonthName = "April";
                            break;
                        case 5:
                            m.MonthName = "May";
                            break;

                        case 6:
                            m.MonthName = "June";
                            break;

                        case 7:
                            m.MonthName = "July";
                            break;

                        case 8:
                            m.MonthName = "August";
                            break;

                        case 9:
                            m.MonthName = "September";
                            break;

                        case 10:
                            m.MonthName = "October";
                            break;

                        case 11:
                            m.MonthName = "November";
                            break;

                        case 12:
                            m.MonthName = "December";
                            break;
                    }
                }

                
            }
            else
            {

                MWorkingDaysDisplay mdd = mRepo.GetDaysByMonYer(MonthNumber, YearNumber);
                if (mdd != null)
                {
                    switch (mdd.MonthNumber)
                    {
                        case 1:
                            mdd.MonthName = "January";
                            break;

                        case 2:
                            mdd.MonthName = "February";
                            break;
                        case 3:
                            mdd.MonthName = "March";
                            break;
                        case 4:
                            mdd.MonthName = "April";
                            break;
                        case 5:
                            mdd.MonthName = "May";
                            break;

                        case 6:
                            mdd.MonthName = "June";
                            break;

                        case 7:
                            mdd.MonthName = "July";
                            break;

                        case 8:
                            mdd.MonthName = "August";
                            break;

                        case 9:
                            mdd.MonthName = "September";
                            break;

                        case 10:
                            mdd.MonthName = "October";
                            break;

                        case 11:
                            mdd.MonthName = "November";
                            break;

                        case 12:
                            mdd.MonthName = "December";
                            break;

                    }

                    myObj.Add(mdd);
                }
            }

            return myObj;
        }
        
        public MonthlyWorkingDays GetDaysById(int id)
        {
            MonthlyWorkingDays myObj = new MonthlyWorkingDays();
            myObj = mRepo.GetDaysById(id);

            return myObj;
        }
        
    }

}
