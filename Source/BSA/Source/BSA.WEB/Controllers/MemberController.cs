using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BSA.Service.Implementation;
using BSA.Service.Interface;
using BSA.Service.ServiceModel;
using BSA.Utility;
using BSA.Utility.Util;
using BSA.ViewModel;
using PagedList;
using System.Threading.Tasks;

namespace BSA.Controllers
{
    public class MemberController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMemberService employeeService = new MemberService();
        ICompanyService companyService = new CompanyService();
        IDropDownItemService dropDownItemService = new DropDownItemService();
   
        IDesignationService designationService = new DesignationService();
   
     
        [SessionExpire] 
        public ActionResult Index(int? Page_No, string searchText)
        {
            List<MemberModel> employees = employeeService.GetEmployees(searchText ?? "");
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(employees.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        [SessionExpire]
        public ActionResult EmployeeSearch(int? Page_No, string searchText)
        {
            List<MemberModel> employees = employeeService.EmployeeSearch(searchText ?? "");
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(employees.ToPagedList(No_Of_Page, Size_Of_Page));
        }


        [SessionExpire]
        public async Task<ActionResult> PreviousEmployees(int? Page_No, string searchText)
        {
            searchText = searchText ?? string.Empty;
            List<MemberModel> employees = await employeeService.GetPreviousEmployeesAsync(searchText);
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(employees.ToPagedList(No_Of_Page, Size_Of_Page));

        }
        public ActionResult GetBirthday(int? Page_No, string searchText)
        {
            searchText = searchText ?? string.Empty;
            List<MemberModel> employees = employeeService.GetEmployeeEvent();
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(searchText))
            {
                var employees1 = employees.Where(
                    x => x.Anniversary.Contains(searchText)
                || x.Name.Contains(searchText)
                || x.EDepartment.Contains(searchText)
                || x.EDesignation.Contains(searchText));
                return View(employees1.ToPagedList(No_Of_Page, Size_Of_Page));
            }
            else
            {
                return View(employees.ToPagedList(No_Of_Page, Size_Of_Page));
            }

        }

        [SessionExpire]
        public ActionResult Details()
        {

            MemberModel model = employeeService.GetEmployee(Convert.ToInt64(Session["Id"].ToString()));
            if (model == null)
            {
                return HttpNotFound();
            }
            if (model.ImageFileName == null)
            {
                model.ImageFileName = "default.png";
            }
            model.ImagePath = string.Format("{0}://{1}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority) + "/Images/Picture/" + model.ImageFileName;

            return View(model);
        }

        [SessionExpire]
        [HttpGet]
        public ActionResult CreateOrEdit(long id)
        {
            MemberViewModel vm = new MemberViewModel();
            vm.Employee = employeeService.GetEmployee(id);

            var request = HttpContext.Request;
            var baseUrl = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);
            if (string.IsNullOrEmpty(vm.Employee.ImageFileName))
            {
                vm.Employee.ImageFileName = "default.png";
            }
            var imageUrl = baseUrl + "/Images/Picture/" + vm.Employee.ImageFileName;
            vm.Employee.ImagePath = imageUrl;

            vm.Managers = employeeService.GetEmployeeSelectModels();
            vm.Companies = companyService.GetCompanySelectModels();
            vm.Religions = dropDownItemService.GetDropDownItemSelectModels(9);
            vm.BloodGroups = dropDownItemService.GetDropDownItemSelectModels(5);
            vm.Countries = dropDownItemService.GetDropDownItemSelectModels(14);
            vm.MaritalTypes = dropDownItemService.GetDropDownItemSelectModels(2);
            vm.Genders = dropDownItemService.GetDropDownItemSelectModels(3);
            vm.EmployeeCategories = dropDownItemService.GetDropDownItemSelectModels(8);
            vm.Designations = designationService.GetDesignationSelectModels();
            vm.OfficeTypes = dropDownItemService.GetDropDownItemSelectModels(12);
            vm.DisverseMethods = dropDownItemService.GetDropDownItemSelectModels(13);
            vm.JobCategories = dropDownItemService.GetDropDownItemSelectModels(15);
            vm.JobTypes = dropDownItemService.GetDropDownItemSelectModels(10);
            return View(vm);
        }
        
        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit(MemberViewModel vm, HttpPostedFileBase file)
        {
            bool result = false;
            string picture = string.Empty;
            if (file != null)
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png", "bmp" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    string ErrorMessage = Constants.FileType;
                    throw new Exception(ErrorMessage);
                }
                int count = 1;
                string fileExtension = Path.GetExtension(file.FileName);
                picture = vm.Employee.EmployeeId + fileExtension;
                string fullPath = Path.Combine(Server.MapPath("~/Images/Picture"), picture);
                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                string extension = Path.GetExtension(fullPath);
                string path = Path.GetDirectoryName(fullPath);
                string newFullPath = fullPath;

                while (System.IO.File.Exists(newFullPath))
                {
                    picture = string.Format("{0}({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(path, picture + extension);
                    picture = picture + extension;
                }
                file.SaveAs(Path.Combine(path, picture));
                vm.Employee.ImageFileName = picture;
            }


            if (vm.Employee.Id <= 0)
            {
                result = employeeService.SaveEmployee(0, vm.Employee);
            }
            else
            {
                result = employeeService.SaveEmployee(vm.Employee.Id, vm.Employee);
            }
            return RedirectToAction("CreateOrEdit", new { id = vm.Employee.Id });
        }
        [SessionExpire]
        public ActionResult Delete(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberModel employee = employeeService.GetEmployee(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [SessionExpire]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            bool result = employeeService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public JsonResult EmployeeAutoComplete(string prefix)
        {
            var employee = employeeService.GetEmployeeAutoComplete(prefix);
            return Json(employee);
        }
    }
}
