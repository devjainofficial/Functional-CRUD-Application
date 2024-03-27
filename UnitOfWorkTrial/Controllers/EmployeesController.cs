using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using UnitOfWorkTrial.Models;
using UnitOfWorkTrial.Models.Filters;
using UnitOfWorkTrial.Models.ViewModels;
using UnitOfWorkTrial.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace UnitOfWorkTrial.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Get Employees
        //[AllowAnonymous]
        [AuthorizationExceptionFilter]      //Custom Exception Filter
        [ResponseCacheAttribute(NoStore = true, Duration = 10)]     //Result Filter
        [CustomAuthorizationFilter]        //Action Filter
        public async Task<IActionResult> Index(Employee employee, int[] DepId, string searchText = "", int page = 1, int size = 10)
        {
            if(HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserSession");
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }

            

            string ArrayString = String.Join(",", DepId);

            int recsCount = await _unitOfWork.Employees.GetAllEmployeesCountAsync(searchText, DepId);
            if (recsCount <= (page - 1) * size)
            {
                page = 1;
            }

            var sp = await _unitOfWork.Employees.GetEmployeeDataStoredProcedure(page, size, searchText, ArrayString);

            int totalPages = (int)Math.Ceiling((decimal)recsCount / (decimal)size);
            int currentPage = page;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }


            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.SearchText = searchText;
            ViewData["DepartmentId"] = new SelectList(await _unitOfWork.Departments.GetAllAsync(), "DepartmentId", "Name");
            ViewBag.DepId = DepId;
            ViewBag.ArrayString = ArrayString;
            var defaultLength = !string.IsNullOrEmpty(searchText) ? 1 : 0;
            string[] AppliedFilters = new string[DepId.Length + defaultLength];


            if (!string.IsNullOrEmpty(searchText))
            {
                AppliedFilters[0] = searchText;
            }



            if (DepId.Length > 0)
            {
                var start = !string.IsNullOrEmpty(searchText) ? 1 : 0;
                foreach (var item in DepId)
                {
                    var departmentIdAsString = item.ToString();
                    var departmentName = ((SelectList)ViewData["DepartmentId"]).FirstOrDefault(d => d.Value == departmentIdAsString)?.Text;
                    if (!string.IsNullOrEmpty(departmentName))
                    {
                        AppliedFilters[start] = departmentName;
                    }
                    start++;
                }

            }

            ViewBag.Data = sp;
            var result = new EmployeeViewModel()
            {
                Employees = sp,
                AppliedFilters = AppliedFilters
            };
            return View(result);

        }

        //[Authorize]
        //Get Employees/Details
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            //Using Employee Repository to Fetching Employee along with thier Department Data by Employee Id
            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(Convert.ToInt32(Id));
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        //Get Employees/Create
        public async Task<IActionResult> Create()
        {
            ViewData["DepartmentId"] = new SelectList(await _unitOfWork.Departments.GetAllAsync(), "DepartmentId", "Name");
            return View();
        }

        //Post Employees/Create
        [HttpPost]
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [ValidateAntiForgeryToken]      //Action Filter
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CreateTransaction();

                    await _unitOfWork.Employees.InsertAsync(employee);

                    await _unitOfWork.Save();

                    _unitOfWork.Commit();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                }
            }
            ViewData["DepartmentId"] = new SelectList(await _unitOfWork.Departments.GetAllAsync(), "DepartmentId", "Name", employee.Department);
            return View(employee);
        }

        //Get Employees/Edit
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(Convert.ToInt32(Id));

            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(await _unitOfWork.Departments.GetAllAsync(), "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        //Post Employees/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, Employee employee)
        {
            if (Id != employee.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CreateTransaction();

                    await _unitOfWork.Employees.UpdateAsync(employee);

                    await _unitOfWork.Save();

                    _unitOfWork.Commit();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    _unitOfWork.Rollback();
                }
            }

            ViewData["DepartmentId"] = new SelectList(await _unitOfWork.Departments.GetAllAsync(), "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }
        // GET: Employees/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Use Employee Repository to Fetch Employees along with the Department Data by Employee Id
            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(Convert.ToInt32(id));
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Begin The Transaction
            _unitOfWork.CreateTransaction();
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee != null)
            {
                try
                {
                    await _unitOfWork.Employees.DeleteAsync(id);
                    //Save Changes to database
                    await _unitOfWork.Save();
                    //Commit the Changes to database
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //Rollback Transaction
                    _unitOfWork.Rollback();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
