using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WorkingWithMultipleTable_Prod.Data;
using WorkingWithMultipleTable_Prod.Models;

namespace WorkingWithMultipleTable_Prod.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationContext _context;

        public DepartmentController(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.OrderBy(d => d.DepartmentId).ToListAsync());
        }
        public async Task<IActionResult> Create(int? id)
        {
            Department dept = new Department();
            if (id != null && id != 0)
            {
                dept = await _context.Departments.FindAsync(id);
            }
            return View(dept);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Failed to Add Department";
                return View(department);
            }
            else
            {
                try
                {
                    if (department.DepartmentId == 0)
                    {
                        await _context.Departments.AddAsync(department);

                        TempData["success"] = "Department Added Successfully";

                    }
                    else
                    {
                        _context.Departments.Update(department);

                        TempData["success"] = "Department Updated Successfully";

                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Failed to perform operation"+ ex.Message;
                    return View(department);
                }
            }

        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                bool status = _context.Employees.Any(e => e.DepartmentId == id);
                if (status)
                {
                    TempData["error"] = "Some Employees are assigned to this Department.\n Failed to Delete Department";
                    return RedirectToAction("Index");
                }
                else
                {
                    var data = await _context.Departments.Where(e => e.DepartmentId == id).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        _context.Departments.Remove(data);
                        await _context.SaveChangesAsync();
                        TempData["success"] = "Record Deleted Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
