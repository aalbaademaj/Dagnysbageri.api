using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dagnysbageri.api.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }
    IAddressRepository AddressRepository { get; }

    Task<bool> Complete();
    bool HasChanges();
}
