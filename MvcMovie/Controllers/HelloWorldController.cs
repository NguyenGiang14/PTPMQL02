using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
namespace MvcMovie.Controllers;

    public class HelloWorldController : Controller
  {
 // GET: /HelloWorld/
        public IActionResult  Index()
        {
            return View();
        }
        // GET: /HelloWorld/Welcome/
        [HttpPost]
public IActionResult Index (Person Ps)
{
    string strResult = "Xin chao" +Ps.PersonId +Ps.FullName ;
ViewBag.info =strResult ;
return View();
}
  }