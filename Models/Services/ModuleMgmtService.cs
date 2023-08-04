using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class ModuleMgmtService : IModuleMgmtService
    {

        private readonly IModuleMasterMgmtRepo mRepo;


        public ModuleMgmtService(IModuleMasterMgmtRepo _mRepo)
        {
            mRepo = _mRepo;

        }

        public ProcessResponse SaveModules(ModulesMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.MoMId > 0)
                {
                    ModulesMaster um = new ModulesMaster();
                    um = mRepo.GetModuleTypeById(request.MoMId);
                    um.ModuleName = request.ModuleName;

                    response = mRepo.UpdateModules(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveModule(request);
                }
            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }
            return response;
        }

        public List<ModulesMaster> GetAllModules()
        {
            return mRepo.GetAllModules();
        }
        public ModulesMaster GetModuleTypeById(int id)
        {
            return mRepo.GetModuleTypeById(id);
        }
        public ProcessResponse UpdateModules(ModulesMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = mRepo.UpdateModules(request);
            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }
            return response;
        }
    }
}
