using System.ComponentModel.DataAnnotations;

namespace DBFirst.Models
{
    public class Department
    {
        [Key]
        public string DeptID { get; set; } = null!;
        public string DeptName { get; set; } = null!;

        public virtual List<tStudent>? tStudents { get; set; } //這是一對多關聯，表示一個科系可以有多個學生
    }
}
