using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dagnysbageri.api.ViewModels.Address;

namespace Dagnysbageri.api.ViewModels.Customer;

public class CustomerPostViewModel
{
    public string StoreName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string ContactPerson { get; set; }


    public IList<AddressPostViewModel> Addresses { get; set; }
}
