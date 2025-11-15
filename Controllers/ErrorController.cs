using Microsoft.AspNetCore.Mvc;
using WebRehabScheduler.Filters;

namespace WebRehabScheduler.Controllers
{
    [SkipTherapistAuthorization] // 排除權限檢查
    public class ErrorController : Controller
    {
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
