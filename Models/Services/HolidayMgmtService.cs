using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class HolidayMgmtService : IHolidayMgmtService
    {

        private readonly IHolidayMgmtRepo hRepo;


        public HolidayMgmtService(IHolidayMgmtRepo _hRepo)
        {
            hRepo = _hRepo;

        }

        public ProcessResponse SaveHolidays(HolidayMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.HMId > 0)
                {
                    HolidayMaster um = new HolidayMaster();
                    um = hRepo.GetHolidayById(request.HMId);
                    um.HolidayDate = request.HolidayDate;
                    um.HolidayName = request.HolidayName;
                    um.HolidayDesc = request.HolidayDesc;

                    response = hRepo.UpdateHolidays(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = hRepo.SaveHoliday(request);
                }
            }
            catch (Exception ex)
            {
                hRepo.LogError(ex);
            }
            return response;
        }
        public List<HolidayMaster> GetAllHolidays()
        {
            return hRepo.GetAllHolidays();
        }
        public HolidayMaster GetHolidayById(int id)
        {
            return hRepo.GetHolidayById(id);
        }
        public ProcessResponse UpdateHolidays(HolidayMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = hRepo.UpdateHolidays(request);
            }
            catch (Exception ex)
            {
                hRepo.LogError(ex);
            }
            return response;
        }
    }
}
