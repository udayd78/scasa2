using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IRecordExceptionService
    {
        void LogError(Exception ex);
    }
}
