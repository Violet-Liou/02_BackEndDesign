using System;
using System.Collections.Generic;

namespace NorthWindStore.Models;

public partial class Shippers
{
    public int ShipperID { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
