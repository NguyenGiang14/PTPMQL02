using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
namespace MvcNet.Controllers
{
public class  PersonController :  Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}