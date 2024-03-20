namespace UnitOfWorkTrial.Models
{
    public class Department
    {
        //2. Setting Properties in Department Model to create the database table using these entities. 

        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
