using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Models.Process;
using OfficeOpenXml;
using X.PagedList;


namespace MvcMovie.Controllers
{
    public class PersonController : Controller
    {
        private  readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();
        private object excelPackage;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page,int?  PageSize)
        {
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() {Value="3", Text="3" },
                new SelectListItem() {Value="5", Text="5" },
                new SelectListItem() {Value="10", Text="10" },
                new SelectListItem() {Value="15", Text="15" },
                new SelectListItem() {Value="25", Text="25" },
                new SelectListItem() {Value="50", Text="50" },
            };
            int pagesize = (PageSize ?? 3);
            ViewBag.psize = pagesize;
            var model = _context.Person.ToPagedList(page ?? 1, pagesize);
            return View(model);

        }
            //     public async Task<IActionResult> Index()
    //     {
    //         var model = await _context.Person.ToListAsync();
    //     return View (model);
    //     }
      [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Index(string KeySearch)
       {
         if(_context.Person.Count()==0)
         {
             return Problem("Enity set MvcMovieContext.Person is null");
       }
       var person =from m in _context.Person select m;
         person = _context.Person.Where(s => s.FullName!.Contains(KeySearch));
 return  View (await person.ToListAsync());
         }
        public IActionResult Create()
        {
            return View ();
        }      private bool PersonExists(string id)
        {
            return (_context.Person?.Any(e => e.PersonId ==id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind("PersonId","FullName","Address")]Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        public async Task<IActionResult> Edit(string PersonId)
        {
            if (PersonId==null || _context.Person == null)
            {
                return NotFound();
            }
            var person = await _context.Person.FindAsync(PersonId);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string PersonId ,[Bind("PersonId","FullName","Address")]Person person)
        {
             if (PersonId != person.PersonId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Person.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        public async Task<IActionResult> Delete(string PersonId)
        {
            if (PersonId==null || _context.Person == null)
            {
                return NotFound();
            }
            var person = await _context.Person
            .FirstOrDefaultAsync(m => m.PersonId == PersonId);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        public async Task<IActionResult> DeleteConfirmed(string personID)
        {
            if (_context.Person == null)
            {
                return Problem("Danh sach rong");
            }
            var result = await _context.Person.FindAsync(personID);
            if (result != null)
            {
                _context.Person.Remove(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(result);

        }
        public async Task<IActionResult> Upload()
{
    return View();
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Upload(IFormFile file)
{
    if (file!=null)
    {
        string fileExtension = Path.GetExtension(file.FileName);
        if(fileExtension !=".xls" && fileExtension.ToLower() != ".xlsx")
        {
            ModelState.AddModelError("","Please choose excel file to upload!");
            }
            else
            {
                //rename file when upload to sever
                var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels" , fileName);
                var fileLocation = new FileInfo(filePath).ToString();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    //save file to sever
                    await file.CopyToAsync(stream);
                    //read data from excel file fill DataTable
                    var dt = _excelProcess.ExcelToDataTable(fileLocation);
                    //using for loop to read data form dt
                    for (int i = 0; i < dt.Rows.Count; i++ )
                    {
                        //create new Person object
                        var ps = new Person();
                        //set value to attributes
                        ps.PersonId = dt.Rows[i][0].ToString();
                        ps.FullName = dt.Rows[i][1].ToString();
                        ps.Address = dt.Rows[i][2].ToString();
                        //add objext to context
                        _context.Add(ps);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                
                }
            }
    }
return View();

}
public ActionResult Download()
        {
            var fileName = "YourFileName" + ".xlsx";
            using(ExcelPackage excelPackge = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackge.Workbook.Worksheets.Add("Sheet 1");
                //add some text to ceel A1
                worksheet.Cells["A1"].Value = "PersonId";
                worksheet.Cells["B1"].Value = "Fullname";
                worksheet.Cells["C1"].Value = "Address";
                //get all Person
                var personList = _context.Person.ToList();
                //fill data tp worksheet
                worksheet.Cells["A2"].LoadFromCollection(personList);
                var stream = new  MemoryStream(excelPackge.GetAsByteArray());
                //download file
                return File (stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

    }

}

