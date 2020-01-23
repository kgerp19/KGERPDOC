using KGERP.Service.Implementation;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KGERP.Controllers
{
    public class CompanyController : Controller
    {
        ICompanyService companyService = new CompanyService();
        public ActionResult Index()
        {
            List<CompanyModel> companies = companyService.GetCompanies();
            return View(companies);
        }
    }
}