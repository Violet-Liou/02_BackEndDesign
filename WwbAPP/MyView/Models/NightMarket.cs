using System.ComponentModel.DataAnnotations;

namespace MyView.Models
{
    public class NightMarket
    {
        [Display(Name = "編號")]
        [Required(ErrorMessage ="必填")] //自定義錯誤訊息(原：The field Id is required.，改成：必填)
        [RegularExpression("A[0-9]{2}",ErrorMessage ="編號格式有誤")] //A[0-9]{2}，格式有兩碼介於0~9的數字
        public string Id { get; set; } = null!;

        [Display(Name = "名稱")]
        [Required(ErrorMessage = "必填")]
        [StringLength(20, ErrorMessage ="字數不可超過20個字")]
        public string Name { get; set; } = null!;

        [Display(Name = "地址")]
        [Required(ErrorMessage = "必填")]
        public string Address { get; set; } = null!;
    }
}
