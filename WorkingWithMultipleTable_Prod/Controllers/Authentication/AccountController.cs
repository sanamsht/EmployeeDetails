using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using WorkingWithMultipleTable_Prod.Data;
using WorkingWithMultipleTable_Prod.Models.IdentityModel;
using WorkingWithMultipleTable_Prod.Models.ViewModel;
using WorkingWithMultipleTable_Prod.Repository.Interface;

namespace WorkingWithMultipleTable_Prod.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        private readonly IEmailSender _emailSender;
        private readonly DBContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IEmailSender emailSender, DBContext context)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
            _emailSender = emailSender;
            _context = context;
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
            Response response = new();
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiesOn");

            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser chkEmail = await userManager.FindByEmailAsync(register.Email);
                    if (chkEmail != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email already exist");
                        return View(register);
                    }
                    var userName = _context.Users.Where(u => u.UserName == register.Username).Any();
                    if(userName)
                    {
                        ModelState.AddModelError(string.Empty, "Username already taken");
                        return View(register);
                    }
                    var user = new ApplicationUser
                    {
                        Email = register.Email,
                        UserName = register.Username,
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Gender = register.Gender,
                        BirthDate = Convert.ToDateTime(register.BirthDate).ToUniversalTime(),
                        CreatedOn = DateTime.UtcNow,
                        Status = register.Status
                    };
                    var result = await userManager.CreateAsync(user, register.Password);
                    if (result.Succeeded)
                    {
                        var userId = await userManager.GetUserIdAsync(user);
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmLink = Url.Action("ConfirmMail", "Account", new { userId = userId, Token = code }, protocol: Request.Scheme);
                        bool status = await _emailSender.SendEmailAsync(register.Email, "Successfully Registered", _emailSender.GetEmailBody(register.Email, "ConfirmEmail", confirmLink, "Confirm Email"));
                        if (status)
                        {
                            
                            TempData["success"] = "Email has been sent to your registered email. Please check";
                            return RedirectToAction("Login", "Account");
                        }




                    }
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
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
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser chkEmail = await userManager.FindByEmailAsync(model.Email);
                    if (chkEmail == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email not found");
                        return View(model);
                    }
                    if (!chkEmail.Status)
                    {
                        ModelState.AddModelError(string.Empty, "User deactivated");
                        return View(model);
                    }
                    if (await userManager.CheckPasswordAsync(chkEmail, model.Password) == false)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Credentials");
                        return View(model);
                    }

                    bool confirmStatus = await userManager.IsEmailConfirmedAsync(chkEmail);
                    if (!confirmStatus)
                    {
                        ModelState.AddModelError(string.Empty, "Email not verified yet");
                        return View(model);
                    }
                    var result = await signinManager.PasswordSignInAsync(chkEmail.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
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



        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordOrUsernameVM model)
        {

            if (model.Email == null)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, Token = code }, protocol: Request.Scheme);
                bool isSendEmail = await _emailSender.SendEmailAsync(model.Email, "Reset Password",  _emailSender.GetEmailBody("", "ResetPassword", callbackUrl, "Reset Password"));
                if (isSendEmail)
                {
                    Response response = new();
                    response.Message = "Reset Password Link";
                    response.StatusCode = "Reset";
                    return RedirectToAction("ForgetPasswordConfirmation", "Account", response)
;
                }
            }
            return View();
        }


        public IActionResult ForgetPasswordConfirmation(Response response)
        {
            return View(response);
        }

        public IActionResult ResetPassword(string userId, string Token)
        {
            var model = new ForgetPasswordOrUsernameVM
            {
                UserId = userId,
                Token = Token
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ForgetPasswordOrUsernameVM model)
        {
            Response response = new();
            ModelState.Remove("Email");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return View(model);
            }
            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                response.Message = "Password Reset Successfully";
                response.StatusCode = "Success";
                return RedirectToAction("ForgetPasswordConfirmation", response);

            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmMail(string userId, string Token)
        {
            Response response = new();
            if (userId != null && Token != null)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return View("Error");
                }
                var result = await userManager.ConfirmEmailAsync(user, Token);
                if (result.Succeeded)
                {
                    response.Message = "Thank You for confirming your email";
                    response.StatusCode = "Success";
                    
                    return RedirectToAction("ForgetPasswordConfirmation", "Account", response);
                }
            }
            return View("Error");
        }
    }
}
