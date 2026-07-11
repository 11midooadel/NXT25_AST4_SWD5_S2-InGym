using Microsoft.AspNetCore.Mvc;

namespace NXT25_AST4_SWD5_S2_InGym.Controllers
{
    public class GymManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
