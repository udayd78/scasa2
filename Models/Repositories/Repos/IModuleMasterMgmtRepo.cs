using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IModuleMasterMgmtRepo
    {
        ProcessResponse SaveModule(ModulesMaster request);
        List<ModulesMaster> GetAllModules();

        ModulesMaster GetModuleTypeById(int id);
        ProcessResponse UpdateModules(ModulesMaster request);
        void LogError(Exception ex);


    }
}
