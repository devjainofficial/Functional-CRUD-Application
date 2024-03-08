using UnitOfWorkTrial.GenericRepository;
using UnitOfWorkTrial.Models;

namespace UnitOfWorkTrial.Repository
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //Here, Can add methods specific to the Department Entity
    }
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository<Department>
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
