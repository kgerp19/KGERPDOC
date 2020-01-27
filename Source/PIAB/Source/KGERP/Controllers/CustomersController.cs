using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KGERP.Data.Models;
using KGERP.Service.Implementation;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Utility;
using KGERP.Utility.Util;
using PagedList;

namespace KGERP.Controllers
{
    public class CustomersController : Controller
    {
        private ERPEntities db = new ERPEntities();
        ICustomerService kttlCustomerService = new CustomerService();
        IDropDownItemService dropDownItemService = new DropDownItemService();

        // GET: KttlCustomers
        [SessionExpire]
        public ActionResult Index(int? Page_No, string searchText)
        {
            //return View(db.KttlCustomers.ToList());
            searchText = searchText ?? "";
            string memberid = System.Web.HttpContext.Current.User.Identity.Name;
            List<CustomerModel> kttlCustomers = kttlCustomerService.GetKttlCustomers(searchText).Where(x=>x.CreatedBy == memberid).ToList();
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(kttlCustomers.ToPagedList(No_Of_Page, Size_Of_Page));
        }
        [SessionExpire]
        public ActionResult Create(int id)
        {
            CustomerModel model = kttlCustomerService.GetKttlCustomer(id);
            model.CustomerTypes = dropDownItemService.GetDropDownItemSelectModels(2);
            model.CustStatus = dropDownItemService.GetDropDownItemSelectModels(3);
            model.PaymentsStatus = dropDownItemService.GetDropDownItemSelectModels(3);
            //model.Districts = DistrictService.GetDistrictSelectModels();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerModel model, HttpPostedFileBase file)
        {
            bool exist = false;
            string picture = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png", "bmp" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    string ErrorMessage = Constants.FileType;
                    throw new Exception(ErrorMessage);
                }
                else
                { 
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/Picture"), fileName);
                        file.SaveAs(path); 
                        string fileName1 = Path.GetPathRoot(file.FileName);
                        string extention = Path.GetExtension(file.FileName).ToLower();
                        model.LogoUrl = path; 
                }
            }


            if (model.OID <= 0)
            {
                 exist = db.Customers.Where(x => x.CustomerName == model.CustomerName).Any();
                if (exist)
                {
                    TempData["errMessage"] = "Exists";
                    return RedirectToAction("Create");
                }
                else
                {
                    kttlCustomerService.SaveKttlCustomer(0, model);
                }
            }
            else
            {
                Customer kttlCustomer = db.Customers.FirstOrDefault(x => x.OID == model.OID);
                if (kttlCustomer == null)
                {
                    TempData["errMessage1"] = "Client not found!";
                    return RedirectToAction("Create");
                }
                kttlCustomerService.SaveKttlCustomer(model.OID, model);
                TempData["DataUpdate"] = "Data Save Successfully!";
            }

            return RedirectToAction("Index");
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer kttlCustomer = db.Customers.Find(id);
            if (kttlCustomer == null)
            {
                return HttpNotFound();
            }
            return View(kttlCustomer);
        }

        // GET: KttlCustomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer kttlCustomer = db.Customers.Find(id);
            if (kttlCustomer == null)
            {
                return HttpNotFound();
            }
            return View(kttlCustomer);
        }

        // POST: KttlCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer kttlCustomer = db.Customers.Find(id);
            db.Customers.Remove(kttlCustomer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        public byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        public FileResult CustomerStatusReport( string NID)
        {
            if (NID == "000039930")
            {
                string filepath = Server.MapPath("/UserManual/CustomerStatusReportsBlock.pdf");
                byte[] pdfByte = GetBytesFromFile(filepath);
                return File(pdfByte, "application/pdf", "CustomerStatusReportsBlock.pdf");
            }
            else if (NID == "000021120")
            {
                string filepath = Server.MapPath("/UserManual/CustomerStatusReportsIrregular.pdf");
                byte[] pdfByte = GetBytesFromFile(filepath);
                return File(pdfByte, "application/pdf", "CustomerStatusReportsIrregular.pdf");
            }
            else
            {
                string filepath = Server.MapPath("/UserManual/CustomerStatusReportsRegular.pdf");
                byte[] pdfByte = GetBytesFromFile(filepath);
                return File(pdfByte, "application/pdf", "CustomerStatusReportsRegular.pdf");
            }
        }

        public ActionResult CustomerStatusReports()
        {
            //string filepath = Server.MapPath("/UserManual/CustomerStatusReport.pdf");
            //byte[] pdfByte = GetBytesFromFile(filepath);
            //return File(pdfByte, "application/pdf", "CustomerStatusReport.pdf");
           return View(CustomerStatusReportsActionResult());
        }

        public FileResult CustomerStatusReportsActionResult()
        {
            string filepath = Server.MapPath("/UserManual/CustomerStatusReport.pdf");
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf", "CustomerStatusReport.pdf");
        }
      
        public FileResult AllCustomerList()
        {
            string filepath = Server.MapPath("/UserManual/AllCustomerList.pdf");
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf", "AllCustomerList.pdf");
        }
        [SessionExpire]
        public ActionResult CustomerList(int? Page_No, string searchText)
        {
            //return View(db.KttlCustomers.ToList());
            searchText = searchText ?? "";
            string memberid = System.Web.HttpContext.Current.User.Identity.Name;
            List<CustomerModel> kttlCustomers = kttlCustomerService.GetKttlAllCustomers(searchText).ToList();
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(kttlCustomers.ToPagedList(No_Of_Page, Size_Of_Page));
        }


        [HttpGet]
        [SessionExpire]
        public ActionResult CustomerAll(string ReportName, string ReportDescription)
        {
            int index = ReportDescription.IndexOf('?');
            string description = ReportDescription.Substring(0, index);

            var rptInfo = new ReportInfo
            {
                ReportName = ReportName,
                ReportDescription = description,
                ReportURL = String.Format("../../Reports/ReportTemplate.aspx?ReportName={0}&Height={1}", ReportName, 650),
                Width = 100,
                Height = 650
            };

            return View(rptInfo);
        }

    }
}