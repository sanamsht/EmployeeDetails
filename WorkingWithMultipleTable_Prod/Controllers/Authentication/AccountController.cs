using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkingWithMultipleTable_Prod.Models.ViewModel;

namespace WorkingWithMultipleTable_Prod.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signinManager;

        public AccountController(UserManager<IdentityUser>userManager, SignInManager<IdentityUser>signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    IdentityUser chkEmail = await userManager.FindByEmailAsync(register.Email);
                    if (chkEmail != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email already exist");
                        return View(register);
                    }
                    var user = new IdentityUser
                    {
                        Email = register.Email,
                        UserName = register.Email
                    };
                    var result = await userManager.CreateAsync(user, register.Password);
                    if (result.Succeeded)
                    {
                       
                        TempData["success"] = "Account has bee successfully created!";
                        return RedirectToAction("Login", "Account");
                    }
                    if(result.Errors.Count() > 0)
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(register);
        }
        public IActionResult Login() {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    IdentityUser chkEmail = await userManager.FindByEmailAsync(model.Email);
                    if(chkEmail == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email not found");
                        return View(model);
                    }
                    if(await userManager.CheckPasswordAsync(chkEmail, model.Password)==false)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Credentials");
                        return View(model);
                    }
                    var result = await signinManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if(result.Succeeded)
                    {
                        TempData["success"] = "Login Successful";
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);

        }
        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
