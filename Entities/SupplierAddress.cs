using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.Entities;

public class SupplierAddress
{
    public int SupplierId { get; set; }
    public int AddressId { get; set; }

    public Supplier Supplier { get; set; }
    public Address Address { get; set; }
}
