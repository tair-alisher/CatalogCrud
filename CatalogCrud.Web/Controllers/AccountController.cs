using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Infrastructure;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CatalogCrud.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { UserName = model.UserName, Password = model.Password };
                var claim = await UserService.Authenticate(userDTO);
                if (claim == null)
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    }, claim);
                    return RedirectToAction("Index", "Catalog");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "app_admin"
                };
                OperationDetails result = await UserService.Create(userDTO);
                if (result.Succeeded)
                    return RedirectToAction("Login");
                else
                    ModelState.AddModelError(result.Property, result.Message);
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> ChangeEmail()
        {
            var email = await UserService.GetUserEmail(User.Identity.GetUserId());
            var model = new ChangeEmailModel
            {
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailModel model)
        {
            if (ModelState.IsValid)
            {
                var modelDTO = new UserDTO
                {
                    Id = User.Identity.GetUserId(),
                    Email = model.Email
                };
                var result = await UserService.ChangeEmail(modelDTO);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Catalog");
                else
                    ModelState.AddModelError(result.Property, result.Message);
            }

            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var modelDTO = new ChangePasswordDTO
                {
                    UserId = User.Identity.GetUserId(),
                    OldPassword = model.OldPassword,
                    NewPassword = model.NewPassword
                };
                var result = await UserService.ChangePassword(modelDTO);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Catalog");
                else
                    ModelState.AddModelError(result.Property, result.Message);
            }

            return View(model);
        }
    }
}