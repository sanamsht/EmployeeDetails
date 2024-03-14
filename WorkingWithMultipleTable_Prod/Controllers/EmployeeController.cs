using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WorkingWithMultipleTable_Prod.Data;
using WorkingWithMultipleTable_Prod.Models;
using WorkingWithMultipleTable_Prod.Models.Combined;
using WorkingWithMultipleTable_Prod.Models.ViewModel;
using WorkingWithMultipleTable_Prod.Utility;

namespace WorkingWithMultipleTable_Prod.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext _context;


        public EmployeeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //var data = _context.Employees.OrderBy(e=>e.EmployeeId).ToList();
            //CombinedViewModel emp = new CombinedViewModel();
            //emp.Employees = _context.Employees.ToList();
            //emp.Departments = _context.Departments.ToList();
            //return View(emp) ;

            var data = from e in _context.Employees
                       join d in _context.Departments
                       on e.DepartmentId equals d.DepartmentId
                       orderby e.EmployeeId
                       select new EmployeeDepartmentModel
                       {
                           EmployeeId = e.EmployeeId,
                           FirstName = Utilities.EveryStringFirstCapital(e.FirstName),
                           LastName = Utilities.EveryStringFirstCapital(e.LastName),
                           MiddleName = Utilities.EveryStringFirstCapital(e.MiddleName),
                           Gender = Utilities.EveryStringFirstCapital(e.Gender),
                           DepartmentCode = d.DepartmentCode.ToUpper(),
                           DepartmentName = Utilities.EveryStringFirstCapital(d.DepartmentName)
                       };
            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            ViewData["Department"] = await _context.Departments.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDepartmentModel employee)
        {
            ViewData["Department"] = await _context.Departments.ToListAsync();
            ModelState.Remove("DepartmentName");
            ModelState.Remove("DepartmentCode");


            if (ModelState.IsValid)
            {
                try
                {
                    if (employee.EmployeeId != 0)
                    {
                        //var emp = await _context.Employees.FindAsync(employee.EmployeeId);
                        //if (emp != null)
                        //{

                            //emp.EmployeeId = employee.EmployeeId;
                            //emp.FirstName = employee.FirstName;
                            //emp.LastName = employee.LastName;
                            //emp.MiddleName = employee.MiddleName;
                            //emp.Gender = employee.Gender;
                            //emp.DepartmentId = employee.DepartmentId;
                            var emp = new Employee()
                            {
                                EmployeeId = employee.EmployeeId,
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                MiddleName = employee.MiddleName,
                                Gender = employee.Gender,
                                DepartmentId = employee.DepartmentId,
                            };

                            _context.Employees.Update(emp);

                            TempData["success"] = "Record updated successfully";

                        //}
                        //else
                        //{
                        //    return NotFound();
                        //}


                    }
                    else
                    {
                        var data = new Employee()
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            MiddleName = employee.MiddleName,
                            Gender = employee.Gender,
                            DepartmentId = employee.DepartmentId,
                        };
                        await _context.Employees.AddAsync(data);

                        TempData["success"] = "Record saved successfully";

                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["Error"] = "Failed to perform operation.\n" + ex.Message;
                    return View(employee);
                }

            }
            else
            {

                return BadRequest();
            }


        }

        public async Task<IActionResult> Delete(int[] id)
        {
            string result = string.Empty;
            try
            {
                if (id.Length == 0)
                {
                    TempData["error"] = "No Records to Delete";
                    result = "success";
                }
                else
                {
                    foreach (var item in id)
                    {
                        if (item == 0)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            var data = await _context.Employees.FindAsync(item);
                            if (data == null)
                            {
                                NotFound();
                            }
                            else
                            {
                                _context.Employees.Remove(data);

                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Record(s) Deleted Successfully";
                    result = "success";
                }
            }
            catch (Exception)
            {

                throw;
            }
                       
            return new JsonResult(result);

        }
        public async Task<IActionResult> Edit(int id)
        {
            EmployeeDepartmentModel employeeDepartment = new();
            ViewData["Department"] = _context.Departments.ToList();
            employeeDepartment = await (from e in _context.Employees
                       join d in _context.Departments
                       on e.DepartmentId equals d.DepartmentId
                       where e.EmployeeId == id
                       select new EmployeeDepartmentModel
                       {
                           EmployeeId = e.EmployeeId,
                           FirstName = e.FirstName,
                           LastName = e.LastName,
                           MiddleName = e.MiddleName,
                           Gender = e.Gender,
                           DepartmentId = d.DepartmentId,
                           DepartmentCode = d.DepartmentCode,
                           DepartmentName = d.DepartmentName
                       }).FirstOrDefaultAsync();
            return View("Create", employeeDepartment);
        }

        public async Task<IActionResult> Details(int id)
        {
            EmployeeDepartmentModel employeeDepartment = new EmployeeDepartmentModel();
            try
            {
                if (id == 0)
                {
                    BadRequest();
                }
                else
                {
                    employeeDepartment = await (from e in _context.Employees.Where(e => e.EmployeeId == id)
                               join d in _context.Departments
                               on e.DepartmentId equals d.DepartmentId
                               select new EmployeeDepartmentModel
                               {
                                   EmployeeId = e.EmployeeId,
                                   DepartmentId = d.DepartmentId,
                                   FirstName = Utilities.EveryStringFirstCapital(e.FirstName),
                                   LastName = Utilities.EveryStringFirstCapital(e.LastName),
                                   MiddleName = Utilities.EveryStringFirstCapital(e.MiddleName),
                                   Gender = Utilities.EveryStringFirstCapital(e.Gender),
                                   DepartmentCode = d.DepartmentCode.ToUpper(),
                                   DepartmentName = Utilities.EveryStringFirstCapital(d.DepartmentName)
                               }).FirstOrDefaultAsync();

                    if(employeeDepartment == null)
                    {
                        NotFound();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(employeeDepartment);
        }
    }
}
