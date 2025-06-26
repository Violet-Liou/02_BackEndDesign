using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthWindStore.Models;

public partial class Categories
{
    [Display(Name = "商品類別ID")]
    public int CategoryID { get; set; }

    [Display(Name = "商品類別")]
    public string CategoryName { get; set; } = null!;

    [Display(Name = "描述")]
    public string Description { get; set; } = null!;

    [Display(Name = "圖示")]
    public byte[] Picture { get; set; } = null!;

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
