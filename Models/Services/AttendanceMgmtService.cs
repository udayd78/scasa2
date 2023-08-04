using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class AttendanceMgmtService : IAttendanceMgmtService
    {

        private readonly IAttendanceMgmtRepo atrepo;

        public AttendanceMgmtService(IAttendanceMgmtRepo attendrepo)
        {
            atrepo = attendrepo;
        }

        public ProcessResponse SaveAttendanceType(AttendanceMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.TRId > 0)
                {
                    AttendanceMaster myObj = new AttendanceMaster();
                    myObj = atrepo.GetAllAttendanceTypeById(request.TRId);
                    myObj.AttStatus = request.AttStatus;

                }
                else
                {
                    AttendanceMaster myObj = atrepo.GetAllAttendanceTypeByStaffAndDate((int)request.StaffId, (DateTime)request.DateofAttendance);
                    if (myObj == null)
                    {
                        request.IsDeleted = false;
                        response = atrepo.SaveAttendanceType(request);
                    }
                    else
                    {
                        myObj.Remarks = request.Remarks;
                        myObj.AttStatus = request.AttStatus;
                        response = atrepo.UpdateAttendanceType(myObj);
                    }
                }
            }
            catch(Exception ex)
            {
                atrepo.LogError(ex);
            }

            return response;
            
        }

        public List<AttendanceMaster> GetAllAttendanceType()
        {
            return atrepo.GetAllAttendanceType();
        }

        public AttendanceMaster GetAllAttendanceTypeById(int id)
        {
            return atrepo.GetAllAttendanceTypeById(id);
        }

        public ProcessResponse UpdateAttendanceType(AttendanceMaster request)
        {
            ProcessResponse response = new ProcessResponse();

            try
            {
                response = atrepo.UpdateAttendanceType(request);
            }
            catch(Exception ex)
            {
                atrepo.LogError(ex);
            }
            return response;
                
        }
            
    }
}
