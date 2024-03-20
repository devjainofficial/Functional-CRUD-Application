using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Drawing;
using UnitOfWorkTrial.GenericRepository;
using UnitOfWorkTrial.Models;

namespace UnitOfWorkTrial.Repository
{
    //Creating EmployeeRepository class to  
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        //
        public EmployeeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(int Page, int Size)
        {
            return await _context.Employees.FromSqlRaw($"sp_Employee @Pageindex = '{Page}', @Pagesize = '{Size}'").ToListAsync();
        }

        public async Task<int> GetAllEmployeesCountAsync(string searchString, int[] deps)
        {
            var query = _context.Employees.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(x => x.Name.StartsWith(searchString));
            if (deps.Length > 0)
               query = query.Where(x => deps.Contains(x.DepartmentId));
            return await query.Include(e => e.Department).CountAsync();
        }
       
        public async Task<List<Employee>> GetEmployeeDataStoredProcedure(int Page, int Size, string SearchText, string arrayString)
        {
            var query = $"sp_Employees @Pageindex = '{Page}', @Pagesize = '{Size}', @SearchName = '{SearchText}', @Arraystring = '{arrayString}'";
            var empData = await _context.Employees.FromSqlRaw(query).ToListAsync();
            return (empData);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int EmployeeID)
        {
            var employee = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(m => m.Id == EmployeeID);
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int EmployeeDepartmentId)
        {
            return await _context.Employees
                .Where(emp => emp.DepartmentId == EmployeeDepartmentId)
                .Include(e => e.Department).ToListAsync();
        }

    }
}