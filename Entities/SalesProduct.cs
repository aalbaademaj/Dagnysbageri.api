using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.Entities;

public class SalesProduct
{
    public int Id { get; set; }
    public string ItemNumber { get; set; }
    public string ProductName { get; set; }
    public double PiecePrice { get; set; }
    public double Weight { get; set; }
    public int PackQuantity { get; set; }
    public DateOnly ProductionDate { get; set; }
    public DateOnly ExpireDate { get; set; }
}
