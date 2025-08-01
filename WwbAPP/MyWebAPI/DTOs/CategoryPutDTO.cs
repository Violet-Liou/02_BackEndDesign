using System.ComponentModel.DataAnnotations;
using MyWebAPI.Models;

namespace MyWebAPI.DTOs
{
    //不是每個欄位都可以被修改
    //每個人可以修改的權限也不同
    public class CategoryPutDTO
    {
        //CateID不應該可以被修改

        [Required]
        [StringLength(20)]
        [CateDulicateCheck]
        public string CateName { get; set; } = null!;
    }

}
