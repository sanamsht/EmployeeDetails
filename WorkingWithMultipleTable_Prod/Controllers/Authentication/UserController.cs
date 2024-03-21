using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkingWithMultipleTable_Prod.Data;
using WorkingWithMultipleTable_Prod.Migrations.DB;
using WorkingWithMultipleTable_Prod.Models.IdentityModel;

namespace WorkingWithMultipleTable_Prod.Controllers.Authentication
{
    public class UserController : Controller
    {
       
        private readonly DBContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public UserController(DBContext context, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            //_identityUser = identityUser;
            _context = context;
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
        }
        public IActionResult Index()
        {

            return View(_context.Users.ToList()) ;
        }

        public IActionResult EditUserGet(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            return Json(user);
        }
       
        public async Task<IActionResult> EditUserPost(string Firstname, string Lastname, string Gender, string DateofBirth, string Password, string Status, string Email, string Username, string CPassword)
        {
            if(Firstname != null && Lastname != null && Gender != null && DateofBirth != null && Password !=null && CPassword != null) {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == Email || e.UserName == Username);
                if(user == null)
                {
                    TempData["error"] = "User doesn't exist";
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    if(Password == user.PasswordHash)
                    {
                        Password = user.PasswordHash;
                    }
                    else
                    {
                        Password = passwordHasher.HashPassword(user, Password);
                    }

                    user.FirstName = Firstname;
                    user.LastName = Lastname;
                    user.Gender = Gender;
                    user.BirthDate = DateTime.Parse(DateofBirth).ToUniversalTime();
                    user.PasswordHash = Password;
                    user.ModifiedOn = DateTime.UtcNow;
                    user.Status = Boolean.Parse(Status);
                        
                   
                     _context.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "User Updated";
                }
            }
            return  Json("");
        }

        public IActionResult EditStatus(string id, string status)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if(user != null)
            {
                user.Status = Convert.ToBoolean(status);
                int result = _context.SaveChanges();
                if (result == 0)
                {
                    TempData["error"] = "Failed to update status";
                }
                else
                {
                    TempData["success"] = "Status Updated";
                }
            }
            return Json(user);
        }

        public IActionResult Delete(string id) {
            var user = _context.Users.SingleOrDefault(e=>e.Id== id);
            if(user!= null)
            {
                _context.Remove(user);
                _context.SaveChanges();
                TempData["success"] = "User Deleted Successfully";
            }
            else
            {
                TempData["error"] = "User doesn't exist";
            }
            return RedirectToAction("Index");

        }

    }
}
