using System;
using System.Collections.Generic;

namespace NorthWindStore.Models;

public partial class Customers
{
    public string CustomerID { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string ContactTitle { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? PostalCode { get; set; }

    public string Phone { get; set; } = null!;

    public string? Fax { get; set; }

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
