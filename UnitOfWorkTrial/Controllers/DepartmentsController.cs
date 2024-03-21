using Microsoft.AspNetCore.Mvc;
using UnitOfWorkTrial.Models;
using UnitOfWorkTrial.UOW;

namespace UnitOfWorkTrial.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> DepIndex()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DepIndex(Department department)
        {
            if (!String.IsNullOrEmpty(department.Name))
            {
                try
                {
                    _unitOfWork.CreateTransaction();

                    await _unitOfWork.Departments.InsertAsync(department);

                    await _unitOfWork.Save();

                    _unitOfWork.Commit();

                    return RedirectToAction("Index", "Employees");
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                }
            }
            return View(department);
        }
    }
}
