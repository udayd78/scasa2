using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class Transform
    {
        public static ApiResponse<T> ConvertResultToApiResonse<T>(T result)
        {
            var ApiResponse = new ApiResponse<T>();
            ApiResponse.Response = result;
            ApiResponse.Succeded = true;

            return ApiResponse;
        }

        public static ApiResponse<T> GetErrorResponse<T>(T Result, List<string> errors)
        {
            var errorObject = Transform.ConvertResultToApiResonse(Result);
            errorObject.Errors = errors.ToArray();
            errorObject.Succeded = false;
            return errorObject;
        }
    }
}
