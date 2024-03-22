using Microsoft.AspNetCore.Mvc;
using UnitOfWorkTrial.Models;
using UnitOfWorkTrial.UOW;

namespace UnitOfWorkTrial.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var User = _unitOfWork.Users.VerifyUser(user.Email, user.Password);

            if (User != null)
            {
                HttpContext.Session.SetString("UserSession", user.Email);
                return RedirectToAction("Index", "Employees");
            }
            else
            {
                ViewBag.Message = "Retry Login";
            }
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }
        public async Task<IActionResult> Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            if (!String.IsNullOrEmpty(user.Name))
            {
                try
                {
                    _unitOfWork.CreateTransaction();

                    await _unitOfWork.Users.InsertAsync(user);

                    await _unitOfWork.Save();

                    _unitOfWork.Commit();

                    return RedirectToAction("Index", "Employees");
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                }
            }
            return View(user);
        }
    }
}
