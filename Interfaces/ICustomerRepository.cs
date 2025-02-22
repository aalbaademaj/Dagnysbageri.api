using Dagnysbageri.api.ViewModels.Customer;

namespace Dagnysbageri.api.Interfaces;

public interface ICustomerRepository
{
    public Task<bool> Update(int id, UpdateContactPersonViewModel model);
    public Task<IList<CustomersViewModel>> List();
    public Task<CustomerViewModel> Get(int id);
    public Task<bool> Add(CustomerPostViewModel model);
}
