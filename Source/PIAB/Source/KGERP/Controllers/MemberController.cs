using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KGERP.Service.Implementation;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using KGERP.Utility.Util;
using KGERP.ViewModel;
using PagedList;
using System.Threading.Tasks;

namespace KGERP.Controllers
{
    public class MemberController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMemberService memberService = new MemberService();
        ICompanyService companyService = new CompanyService();
        IDropDownItemService dropDownItemService = new DropDownItemService();   
        IDesignationService designationService = new DesignationService();
        
        [SessionExpire] 
        public ActionResult Index(int? Page_No, string searchText)
        {
            List<MemberModel> members = memberService.GetMembers(searchText ?? "");
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(members.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        [SessionExpire]
        public ActionResult MemberSearch(int? Page_No, string searchText)
        {
            List<MemberModel> members = memberService.MemberSearch(searchText ?? "");
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(members.ToPagedList(No_Of_Page, Size_Of_Page));
        }
        
        [SessionExpire]
        public async Task<ActionResult> PreviousMembers(int? Page_No, string searchText)
        {
            searchText = searchText ?? string.Empty;
            List<MemberModel> members = await memberService.GetPreviousMembersAsync(searchText);
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(members.ToPagedList(No_Of_Page, Size_Of_Page));

        }
        public ActionResult GetBirthday(int? Page_No, string searchText)
        {
            searchText = searchText ?? string.Empty;
            List<MemberModel> members = memberService.GetMemberEvent();
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(searchText))
            {
                var members1 = members.Where(
                    x => x.Anniversary.Contains(searchText)
                || x.Name.Contains(searchText)
                || x.EDepartment.Contains(searchText)
                || x.EDesignation.Contains(searchText));
                return View(members1.ToPagedList(No_Of_Page, Size_Of_Page));
            }
            else
            {
                return View(members.ToPagedList(No_Of_Page, Size_Of_Page));
            }

        }

        [SessionExpire]
        public ActionResult Details()
        {

            MemberModel model = memberService.GetMember(Convert.ToInt64(Session["Id"].ToString()));
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
            vm.Member = memberService.GetMember(id);

            var request = HttpContext.Request;
            var baseUrl = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);
            if (string.IsNullOrEmpty(vm.Member.ImageFileName))
            {
                vm.Member.ImageFileName = "default.png";
            }
            var imageUrl = baseUrl + "/Images/Picture/" + vm.Member.ImageFileName;
            vm.Member.ImagePath = imageUrl;

            vm.Managers = memberService.GetMemberSelectModels();
            vm.Companies = companyService.GetCompanySelectModels();
            vm.Religions = dropDownItemService.GetDropDownItemSelectModels(9);
            vm.BloodGroups = dropDownItemService.GetDropDownItemSelectModels(5);
            vm.Countries = dropDownItemService.GetDropDownItemSelectModels(14);
            vm.MaritalTypes = dropDownItemService.GetDropDownItemSelectModels(2);
            vm.Genders = dropDownItemService.GetDropDownItemSelectModels(3);
            vm.MemberCategories = dropDownItemService.GetDropDownItemSelectModels(8);
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
                picture = vm.Member.MemberId + fileExtension;
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
                vm.Member.ImageFileName = picture;
            }


            if (vm.Member.OID <= 0)
            {
                result = memberService.SaveMember(0, vm.Member);
            }
            else
            {
                result = memberService.SaveMember(vm.Member.OID, vm.Member);
            }
            return RedirectToAction("CreateOrEdit", new { id = vm.Member.OID });
        }
        [SessionExpire]
        public ActionResult Delete(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberModel member = memberService.GetMember(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [SessionExpire]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            bool result = memberService.DeleteMember(id);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public JsonResult MemberAutoComplete(string prefix)
        {
            var member = memberService.GetMemberAutoComplete(prefix);
            return Json(member);
        }
    }
}
