using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.ViewModels.Customer;

public class CustomersViewModel
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string ContactPerson { get; set; }

}
