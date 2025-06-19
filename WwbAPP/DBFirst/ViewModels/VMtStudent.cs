using DBFirst.Models;

namespace DBFirst.ViewModels
{
    public class VMtStudent
    {
        public List<tStudent>? tStudent { get; set; } // 學生資料
        public List<Department>? Department { get; set; } // 學生所屬系所資料


        //public Department Department { get; set; } => 單筆資料
        //public List<Department> Department { get; set; } =>多筆資料，所以用List<>
    }
}
