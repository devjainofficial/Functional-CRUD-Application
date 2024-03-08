using Microsoft.Data.SqlClient;
using UnitOfWorkTrial.Repository;

namespace UnitOfWorkTrial.UOW
{
    public interface IUnitOfWork
    {
        EmployeeRepository Employees { get; }
        DepartmentRepository Departments { get; }

        void CreateTransaction();
        void Commit();
        void Rollback();
        Task Save();
    }
}
