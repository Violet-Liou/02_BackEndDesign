﻿using System;
using System.Collections.Generic;

namespace NorthWindStore.Models;

public partial class OrderDetails
{
    public int OrderID { get; set; }

    public int ProductID { get; set; }

    public decimal UnitPrice { get; set; }

    public short Quantity { get; set; }

    public float Discount { get; set; }

    public virtual Orders Order { get; set; } = null!;

    public virtual Products Product { get; set; } = null!;
}
