using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcNet.Models;

namespace MvcNet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
public IActionResult Index( string Fullname, string  Address)
{
    string strOutput ="xin chào" + Fullname + " Đến từ" + Address ;
    ViewBag.Message = strOutput;
    return View();
}
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
