using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.Entities;

public class OrderItem
{
    public int SalesOrderId { get; set; }
    public int SalesProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public SalesProduct SalesProduct { get; set; }
    public SalesOrder SalesOrder { get; set; }
}
