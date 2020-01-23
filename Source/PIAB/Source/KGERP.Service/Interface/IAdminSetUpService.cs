using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGERP.Service.ServiceModel;
using KGERP.Data.Models;
using KGERP.Utility;

namespace KGERP.Service.Interface
{
    public interface IAdminSetUpService
    {
        List<AdminSetUpModel> GetAdminSetUps(string searchText);
        List<SelectModel> GetEmployeeSelectModels();
        AdminSetUpModel GetAdminSetUp(long id);
        List<SelectModel> StatusSelectModels();
        bool SaveAdminSetUp(long id, AdminSetUpModel adminSetUp);
    }
}
