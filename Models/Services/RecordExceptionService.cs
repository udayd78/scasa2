using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class RecordExceptionService : IRecordExceptionService
    {
        private readonly IRecordExceptionRepo recordExceptionRep;
        public RecordExceptionService(IRecordExceptionRepo _repo)
        {
            recordExceptionRep = _repo;
        }
        public void LogError(Exception ex)
        {
            recordExceptionRep.LogError(ex);
        }
    }
}
