using AutoMapper;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Data.Models;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class CompanyService : ICompanyService
    {
        ERPEntities companyRepository = new ERPEntities();
        public List<CompanyModel> GetCompanies()
        {
            List<CompanyModel> models = ObjectConverter<Company, CompanyModel>.ConvertList(companyRepository.Companies.ToList()).ToList();
            return models;
        }

        public List<SelectModel> GetCompanySelectModels()
        {
            return companyRepository.Companies.ToList().Select(x => new SelectModel()
            {
                Text = x.Name.ToString(),
                Value = x.CompanyId
            }).ToList();
        }
    }
}
