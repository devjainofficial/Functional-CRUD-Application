﻿using Microsoft.EntityFrameworkCore;
using System.Drawing;
using UnitOfWorkTrial.GenericRepository;
using UnitOfWorkTrial.Models;

namespace UnitOfWorkTrial.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(int Page, int Size)
        {
            return await _context.Employees.FromSqlRaw($"sp_Employee @Pageindex = '{Page}', @Pagesize = '{Size}'").ToListAsync();
        }

        public async Task<int> GetAllEmployeesCountAsync()
        {
            return await _context.Employees.Include(e => e.Department).CountAsync();
        }
        public async Task<int> GetAllEmployeesCountAsync(string searchString, int[] deps)
        {
            return await _context.Employees.Where(x => x.Name.StartsWith(searchString) && deps.Contains(x.DepartmentId)).Include(e => e.Department).CountAsync();
        } 
        public async Task<List<Employee>> GetEmployeeAsync(string SearchText)
        {
            var employee =  await _context.Employees.Include(e => e.Department).Where(e => e.Name.Contains(SearchText)).ToListAsync();
            return employee;
        }
        public async Task<List<Employee>> GetStoredProcedure(int Page, int Size, string SearchText, string arrayString)
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
