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

    [InverseProperty("Cate")]
    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
