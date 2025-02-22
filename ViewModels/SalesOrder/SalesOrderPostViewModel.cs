using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.ViewModels;

public class SalesOrderPostViewModel
{
    public int CustomerId { get; set; }
    public DateOnly OrderDate { get; set; }

    public IList<OrderItemViewModel> OrderItems { get; set; }
}
