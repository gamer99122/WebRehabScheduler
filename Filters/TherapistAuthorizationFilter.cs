using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebRehabScheduler.Filters
{
    public class TherapistAuthorizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 檢查是否有 SkipTherapistAuthorization 屬性
            var skipAuthorization = context.ActionDescriptor.EndpointMetadata
                .Any(m => m is SkipTherapistAuthorizationAttribute);

            if (skipAuthorization)
            {
                return; // 跳過檢查
            }

            var session = context.HttpContext.Session;
            var request = context.HttpContext.Request;

            // 檢查 1: URL 是否有 therapistId 參數
            if (request.Query.ContainsKey("therapistId"))
            {
                var therapistId = request.Query["therapistId"].ToString();

                if (!string.IsNullOrEmpty(therapistId))
                {
                    // 將 therapistId 設定到 Session
                    session.SetString("TherapistId", therapistId);
                    return; // 繼續執行
                }
            }

            // 檢查 2: Session 是否已有 TherapistId
            var sessionTherapistId = session.GetString("TherapistId");

            if (string.IsNullOrEmpty(sessionTherapistId))
            {
                // 都沒有，導向無權限頁面
                context.Result = new RedirectToActionResult("Unauthorized", "Error", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 不需要處理
        }
    }
}