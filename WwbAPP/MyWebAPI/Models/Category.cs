using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyWebAPI.Models;

public partial class Category
{
    [Key]
    [StringLength(2)]
    public string CateID { get; set; } = null!;

    [StringLength(20)]
    public string CateName { get; set; } = null!;

    //不用加ignore的原因：雖然Category對Product是一對多，但是不會形成循環。
    /// /如果會形成循環就一定要加(EX：Product>OrderDetail>Product>OrderDetail....)。
    [InverseProperty("Cate")]
    public virtual ICollection<Product>? Product { get; set; } = new List<Product>();
}
