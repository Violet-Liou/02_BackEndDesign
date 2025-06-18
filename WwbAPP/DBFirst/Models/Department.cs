using System.ComponentModel.DataAnnotations;

namespace DBFirst.Models
{
    public class Department
    {
        [Key]
        [Display(Name = "科系代碼")]
        [StringLength(2,ErrorMessage ="最多2碼")]
        [Required(ErrorMessage ="必填")]
        public string DeptID { get; set; } = null!;

        [Display(Name = "科系名稱")]
        [StringLength(30, ErrorMessage = "最多30個字")]
        [Required(ErrorMessage = "必填")]
        public string DeptName { get; set; } = null!;

        public virtual List<tStudent>? tStudents { get; set; } //這是一對多關聯，表示一個科系可以有多個學生
    }
}
