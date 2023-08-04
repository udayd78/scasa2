using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class AttendanceMgmtRepo : IAttendanceMgmtRepo
    {
        private readonly MyDbContext context;

        public AttendanceMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveAttendanceType(AttendanceMaster request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.attendanceMasters.Add(request);
                context.SaveChanges();
                responce.currentId = request.TRId;
                responce.statusCode = 1;
                responce.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce;

        }
        public List<AttendanceMaster> GetAllAttendanceType()
        {
            List<AttendanceMaster> response = new List<AttendanceMaster>();
            try
            {
                response = context.attendanceMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public AttendanceMaster GetAllAttendanceTypeById(int id)
        {
            AttendanceMaster response = new AttendanceMaster();
            try
            {
                response = context.attendanceMasters.Where(a => a.IsDeleted == false && a.TRId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public AttendanceMaster GetAllAttendanceTypeByStaffAndDate(int staffid,DateTime aDate)
        {
            AttendanceMaster response = new AttendanceMaster();
            try
            {
                response = context.attendanceMasters.Where(a => a.IsDeleted == false && a.StaffId == staffid && a.DateofAttendance==aDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateAttendanceType(AttendanceMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.TRId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }
      
    }
}

        
    
