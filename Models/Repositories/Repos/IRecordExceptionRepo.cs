using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IRecordExceptionRepo
    {
        void LogError(Exception ex);
    }
}
