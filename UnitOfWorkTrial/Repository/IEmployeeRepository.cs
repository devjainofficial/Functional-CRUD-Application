using System.Buffers.Text;
using UnitOfWorkTrial.GenericRepository;
using UnitOfWorkTrial.Models;

namespace UnitOfWorkTrial.Repository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //Here, you need to define the operations which are specific to Employee Entity
        //This method returns all the Employee entities along with Department data
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<int> GetAllEmployeesCountAsync();
        Task<int> GetAllEmployeesCountAsync(string searchString, int[] deps);
        Task<List<Employee>> GetStoredProcedure(int Page, int Size, string SearchText, string arrayString);
        //This method returns the one Employee along with Department data based on the EmployeeId
        Task<Employee?> GetEmployeeByIdAsync(int EmployeeId);


        //This method will return Employees by DepartmentId
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int DepartmentId);    
    }
}
