using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UnitOfWorkTrial.Models
{
    public class Employee
    {
        //1. Setting Properties in Employee Model to create the database table using these entities. 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public int Gender { get; set; }

        [Display(Name = "Department Name")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
