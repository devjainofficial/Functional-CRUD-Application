using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UnitOfWorkTrial.Models;
using UnitOfWorkTrial.Repository;

namespace UnitOfWorkTrial.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public ApplicationDbContext? Context;

        private IDbContextTransaction? _objTran = null;

        public EmployeeRepository Employees { get; private set; }

        public DepartmentRepository Departments { get; private set; }

        public UnitOfWork(ApplicationDbContext _Context)
        {
            Context = _Context;
            Employees = new EmployeeRepository(Context);
            Departments = new DepartmentRepository(Context);
        }

        //The CreateTransaction() method will create a database Transaction so that we can do database operations
        //by applying do everything and do nothing principle
        public void CreateTransaction()
        {
            //It will Begin the transaction on the underlying connection
            _objTran = Context.Database.BeginTransaction();
        }
        //If all the Transactions are completed successfully then we need to call this Commit() 
        //method to Save the changes permanently in the database
        public void Commit()
        {
            //Commits the underlying store transaction
            _objTran?.Commit();
        }
        //If at least one of the Transaction is Failed then we need to call this Rollback() 
        //method to Rollback the database changes to its previous state
        public void Rollback()
        {
            //Rolls back the underlying store transaction
            _objTran?.Rollback();
            //The Dispose Method will clean up this transaction object and ensures Entity Framework
            //is no longer using that transaction.
            _objTran?.Dispose();
        }
        //The Save() Method will Call the DbContext Class SaveChanges method 
        //So whenever we do a transaction we need to call this Save() method 
        //so that it will make the changes in the database permanently
        public async Task Save()
        {
            try
            {
                //Calling DbContext Class SaveChanges method 
                await Context.SaveChangesAsync();
            }
            //DbUpdateException thrown when exception occurs in database while saving changes of transactions. 
            catch (DbUpdateException ex)
            {
                // Handle the exception, possibly logging the details
                // The InnerException often contains more specific details
                throw new Exception(ex.Message, ex);
            }
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
