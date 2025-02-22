using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.ViewModels;

public class OrderItemViewModel
{
    public string SalesProductName { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
