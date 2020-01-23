using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSA.Data.Models;
using BSA.Service.ServiceModel;
using BSA.Utility;    

namespace BSA.Service.Interface
{
    public interface ICustomerService : IDisposable
    {
        List<CustomerModel> GetKttlCustomers( string searchText); 
        List<CustomerModel> GetKttlAllCustomers( string searchText); 
        CustomerModel GetKttlCustomer(long id); 
        bool SaveKttlCustomer(long id, CustomerModel model);
        bool DeleteKttlCustomer(long id); 
    }
}
