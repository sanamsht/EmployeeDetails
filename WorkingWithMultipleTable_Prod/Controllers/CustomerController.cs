using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WorkingWithMultipleTable_Prod.Data;

using WorkingWithMultipleTable_Prod.Models;
using WorkingWithMultipleTable_Prod.Utility;

namespace WorkingWithMultipleTable_Prod.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationContext _context;
        

        public CustomerController(ApplicationContext context)
        {
            _context = context;
          
            
        }

        
         
        public IActionResult Index()
        {
            var data = _context.Customers.ToList();
            return View(data);
        }

        public IActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            List<Customer> cust = new();
            try
            {
                if (file != null && file.Length > 0)
                {
                    using(var pkg = new ExcelPackage(file.OpenReadStream()))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var worksheet = pkg.Workbook.Worksheets[0];
                        for ( int row = 2; row<=worksheet.Dimension.End.Row; row++)
                        {
                            cust.Add(new Customer
                            {
                                CustomerId = worksheet.Cells[row, 1].Text,
                                FirstName = worksheet.Cells[row, 2].Text,
                                LastName = worksheet.Cells[row, 3].Text,
                                Country = worksheet.Cells[row, 4].Text,
                                Gender = worksheet.Cells[row, 5].Text,
                                Email = worksheet.Cells[row, 6].Text,
                                Age = Int32.Parse(worksheet.Cells[row, 7].Text)
                           
                            });
                        }
                    }
                }
                else
                {
                    return View(file);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return View("DisplayExcel", cust);
        }

        public IActionResult DisplayExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ImportExcel(List<Customer> customers)
        {
            var data =  customers.Where(c=>c.Active).ToList();
           
                if (data != null)
                {
                    foreach (var item in data)
                    {
                    item.CustomerId = Helper.encryptText(item.CustomerId);
                        await _context.Customers.AddAsync(item);
                    }
                   await _context.SaveChangesAsync();
                }
                else
                {
                    BadRequest();
                }
           
            return RedirectToAction("Index");
        }
    }
}
