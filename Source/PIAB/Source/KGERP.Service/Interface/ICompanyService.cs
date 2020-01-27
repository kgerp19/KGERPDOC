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
    public interface ICompanyService
    {
        List<CompanyModel> GetCompanies();
        List<SelectModel> GetCompanySelectModels();
    }
}
