using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.Entities;

public class Address
{
    public int Id { get; set; }
    public int AddressTypeId { get; set; }
    public int PostalAddressId { get; set; }
    public string AddressLine { get; set; }

    public PostalAddress PostalAddress { get; set; }
    public AddressType AddressType { get; set; }

    public IList<CustomerAddress> CustomerAddresses { get; set; }
    public IList<SupplierAddress> SupplierAddresses { get; set; }

}
