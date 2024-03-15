namespace UnitOfWorkTrial.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public string[] AppliedFilters { get; set; }
    }
}
