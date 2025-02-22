using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dagnysbageri.api.ViewModels.Address;

namespace Dagnysbageri.api.ViewModels.Customer;

public class CustomerViewModel
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string ContactPerson { get; set; }


    public IList<AddressViewModel> Addresses { get; set; }
    public IList<SalesOrderViewModel> SalesOrders { get; set; }
}
