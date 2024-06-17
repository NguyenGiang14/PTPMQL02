using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

public class  EmployeeController :  Controller
{
    public IActionResult Index ()
    {
        return View();
    }
    public string Welcome()
    {return" this is the ";}
    
}