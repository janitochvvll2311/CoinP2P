using Microsoft.AspNetCore.Mvc;

namespace CoinP2P.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

}