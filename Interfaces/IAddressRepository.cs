using Dagnysbageri.api.Entities;
using Dagnysbageri.api.ViewModels.Address;

namespace Dagnysbageri.api.Interfaces;

public interface IAddressRepository
{
    public Task<Address> Add(AddressPostViewModel model);
}
