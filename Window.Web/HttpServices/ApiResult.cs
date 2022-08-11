using CRM.Domain.DTOs.StructuredApiDtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace Window.Web.HttpServices
{
    public static class ApiResult
    {
        public static JsonResult SetResponse(ApiResultEnum status, List<ApiErrorDto> errors, object? data, string message = null)
        {
            return new JsonResult(new
            {
                status = Enum.GetName(status),
                errors = errors,
                data = data,
                message = message
            });
        }

        public static object GetObject(ApiResultEnum status, List<ApiErrorDto> errors, object? data, string message = null)
        {
            return new
            {
                status = Enum.GetName(status),
                errors = errors,
                data = data,
                message = message
            };
        }
    }


    public enum ApiResultEnum
    {
        Success,
        Info,
        Warning,
        Error
    }
}
