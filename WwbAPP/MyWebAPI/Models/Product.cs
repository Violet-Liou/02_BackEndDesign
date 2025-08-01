using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MyWebAPI.Models;

public partial class Product
{
    [Key]
    [StringLength(5)]
    public string ProductID { get; set; } = null!;

    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [StringLength(12)]
    public string Picture { get; set; } = null!;

    [StringLength(2)]
    public string CateID { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CateID")]
    [InverseProperty("Product")]
    public virtual Category? Cate { get; set; } = null!;
    //如果用[FromForm]的方式傳遞POST，就需要修改Model(因有些欄位是非必填，只需要在那些非必填的欄位上加上「?」即可)
    //但需要注意，在Swagger上測試時，欄位值刪除後，會自動勾選「Send empty value」，需要把這個勾掉，資料才會傳送正確!!
    //推論：無法傳遞一個空的序列值，要直接沒有資料，傳遞時會整個欄位NULL掉。

    [JsonIgnore]
    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail>? OrderDetail { get; set; } = new List<OrderDetail>();
}
