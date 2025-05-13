using IKEA.BLL.Common.Services.EmailSettings;
using IKEA.DAL.Models.Identity;
using IKEA.PL.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IEmailSettings emailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
        }

        #region Register
        #region  Get 
        [HttpGet]
        public async  Task< IActionResult> SignUp()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async  Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //Check If The User Name  already Exists
            var existingUser = await _userManager.FindByNameAsync(signUpViewModel.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(SignUpViewModel.UserName), "This User Name Is Already Taken");
                return View(signUpViewModel);
            }
            var User = new ApplicationUser()
            {
                FName= signUpViewModel.FirstName,
                LName= signUpViewModel.LastName,
                UserName= signUpViewModel.UserName,
                Email= signUpViewModel.Email,
                IsAgree= signUpViewModel.IsAgree,

            };
            var result = await _userManager.CreateAsync(User, signUpViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }
            foreach( var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(signUpViewModel);

        }
        #endregion



        #endregion

        #region Login
        #region  Get
        [HttpGet]
        //P@ssw0rd
        public  async Task<IActionResult> SignIn()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();

            }
            var User = await _userManager.FindByEmailAsync(signInViewModel.Email);
            if ( User is { })
            {
                var flag = await _userManager.CheckPasswordAsync(User, signInViewModel.Password);
            if (flag)
                {
                    var result = await _signInManager.PasswordSignInAsync(User, signInViewModel.Password, signInViewModel.RememberMe,true);
                    if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Is Not Confirmed Yet");

                    }
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Is Locked");

                    }
                      if ( result.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                   

                }
            
            }


            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View(signInViewModel);
        }
        #endregion
        #endregion

        #region LogOut
        
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password
        #region Get
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        #endregion
        #region  Post
        [HttpPost]
        public async Task <IActionResult> SendRessetPasswordUrl(ForgetPasswordViewModel  forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                 if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    // var token = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                    //https://localhost:7289/Account/ResetPassword/ranaHatem447@gmail.com
                    var url = Url.Action("ResetPassword", "Account", new { email = forgetPasswordViewModel.Email, token = token  }, Request.Scheme);
                    // To , Subject, Body 
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = url


                    };
                    // Send Email
                    _emailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");

                }
                ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try");
                 
               
            }
            return View(forgetPasswordViewModel);
        }
        #endregion


        #endregion

        #region Check Your Inbox
        [HttpGet]
         public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        //NewPassword,ConfirmPassword
        //ResetPassword
        [HttpGet]
         public  IActionResult ResetPassword( string  email,string token)
        {
            
                //pass email - token
                TempData["email"] = email;
                TempData["token"] = token;
                return View();
           
        }

        //-----------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {

            #region Old
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }

                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Opration Please TRy Again");
            return View(resetPasswordViewModel);
            #endregion

            #endregion

        }
    }
}
