using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.ViewModels;

public class UpdatePriceViewModel
{
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public double newPrice { get; set; }
}
