using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class ShiftMasterMgmtService : IShiftMasterMgmtService
    {
        private readonly IShiftMasterMgmtRepo sRepo;
        public ShiftMasterMgmtService(IShiftMasterMgmtRepo _sRepo)
        {
            sRepo = _sRepo;
        }
        public ProcessResponse SaveShift(ShiftMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.SMId > 0)
                {
                    ShiftMaster um = new ShiftMaster();
                    um = sRepo.GetShiftById(request.SMId);
                    um.ShiftName = request.ShiftName;
                    um.StartTime = request.StartTime;
                    um.EndTime = request.EndTime;

                    response = sRepo.UpdateShifts(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = sRepo.SaveShift(request);
                }
            }
            catch (Exception ex)
            {
                sRepo.LogError(ex);
            }
            return response;
        }
        public List<ShiftMaster> GetAllShifts()
        {
            return sRepo.GetAllShifts();
        }
        public ShiftMaster GetShiftById(int id)
        {
            return sRepo.GetShiftById(id);
        }
    }
}



