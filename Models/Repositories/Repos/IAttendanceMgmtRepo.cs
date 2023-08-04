using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IAttendanceMgmtRepo
    {
        ProcessResponse SaveAttendanceType(AttendanceMaster request);

        List<AttendanceMaster> GetAllAttendanceType();

        AttendanceMaster GetAllAttendanceTypeById(int id);
        AttendanceMaster GetAllAttendanceTypeByStaffAndDate(int staffid, DateTime aDate);

        ProcessResponse UpdateAttendanceType(AttendanceMaster request);

        void LogError(Exception ex);
        
    }
}
