using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
     public interface IAttendanceMgmtService
     {

        ProcessResponse SaveAttendanceType(AttendanceMaster request);

        List<AttendanceMaster> GetAllAttendanceType();

        AttendanceMaster GetAllAttendanceTypeById(int id);
        ProcessResponse UpdateAttendanceType(AttendanceMaster request);

     }
}
