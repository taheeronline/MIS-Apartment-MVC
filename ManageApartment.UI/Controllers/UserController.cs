using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using ManageApartment.UI.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace ManageApartment.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly iUserRepo _userRepo;

        public UserController(iUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Register(vmUser vm)
        {
            var model = new User
            {
                Name = vm.Name,
                LoginName = vm.LoginName,
                Password = vm.Password
            };
            _userRepo.RegisterUser(model);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(vmUser vm)
        {
            var userInfo = await _userRepo.GetUserInfo(vm.LoginName, vm.Password);
            if (userInfo == null)
            {
                // Add error message to the ModelState
                ModelState.AddModelError(string.Empty, "Invalid login credentials.");

                // Return the same view with the error message
                return View(vm);
            }
            HttpContext.Session.SetInt32("UserId", userInfo.Id);
            HttpContext.Session.SetString("UserName", userInfo.Name);
            return RedirectToAction("Index", "Home");
        }

    }
}
