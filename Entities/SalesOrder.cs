using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.Entities;

public class SalesOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateOnly OrderDate { get; set; }

    public IList<OrderItem> OrderItems { get; set; }
    public Customer Customer { get; set; }
}
