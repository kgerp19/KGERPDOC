using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KGERP.Service.ServiceModel
{
    public class MemberModel 
    {
        public string ButtonName
        {
            get
            {
                return OID > 0 ? "Update" : "Save";
            }

        }


        public long OID { get; set; }
        [DisplayName("Employee ID")]
        public string EmployeeId { get; set; }
        [DisplayName("Manager")]
        public Nullable<long> ManagerId { get; set; }
        [DisplayName("Company")]
        public Nullable<int> CompanyId { get; set; }
 
       
        public string Name { get; set; }
        [DisplayName("Short Name")]
        public string ShortName { get; set; }

        [DisplayName("Present Address")]
        public string PresentAddress { get; set; }
        [DisplayName("Father's Name")]
        public string FatherName { get; set; }
        [DisplayName("Mother's Name")]
        public string MotherName { get; set; }
        [DisplayName("Spouse")]
    
        public string Telephone { get; set; }
        [DisplayName("PBX No")]
        public string PBX { get; set; }
        [DisplayName("Mobile No")]
        public string MobileNo { get; set; }
        [DisplayName("Fax No")]
        public string FaxNo { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Personal Email")]
        public string Email { get; set; }
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string OfficeEmail { get; set; }
        [DisplayName("Mailing Address")]
        public string MailAddress { get; set; }
        [DisplayName("Permanent Address")]
        public string PermanentAddress { get; set; }
        [DisplayName("Department")]
        public Nullable<int> DepartmentId { get; set; }
        [DisplayName("Designation")]
        public Nullable<int> DesignationId { get; set; }
 
  
        public bool Active { get; set; }
        [DisplayName("Shift")]
        public Nullable<int> ShiftId { get; set; }
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Date of Marriage")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public Nullable<System.DateTime> DateOfMarriage { get; set; }
        [DisplayName("Grade")]
        public Nullable<int> GradeId { get; set; }
        [DisplayName("Country")]
        public Nullable<int> CountryId { get; set; }
        [DisplayName("Holiday Name")]
        public string HolidayName { get; set; }
        [DisplayName("Previous Gender")]
        public string Gender { get; set; }
        [DisplayName("Gender")]
        public Nullable<int> GenderId { get; set; }
        [DisplayName("Previous Marital Type")]
        public string MaritalType { get; set; }
        [DisplayName("Marital Type")]
        public Nullable<int> MaritalTypeId { get; set; }
        [DisplayName("District")]
        public Nullable<int> DistrictId { get; set; }
        [DisplayName("Bank")]
        public Nullable<int> BankId { get; set; }
        [DisplayName("Bank Branch")]
        public Nullable<int> BankBranchId { get; set; }

        [DisplayName("Bank Account")]
        public string BankAccount { get; set; }
        [DisplayName("License No")]
        public string LicenseNo { get; set; }
        [DisplayName("Passport No")]
        public string PassportNo { get; set; }
        [DisplayName("NID")]
        public string NationalId { get; set; }
        [DisplayName("TIN/BIN")]
        public string TinorBin { get; set; }
        public string Religion { get; set; }
        [DisplayName("Religion")]
        public Nullable<int> ReligionId { get; set; }
        [DisplayName("Blood Group")]
        public string BloodGroup { get; set; }
        [DisplayName("Blood Group")]
        public Nullable<int> BloodGroupId { get; set; }
        [DisplayName("PF Status")]
        public string PfStatus { get; set; }
        [DisplayName("PF Amount")]
        public Nullable<double> PfAmount { get; set; }
        [DisplayName("PF Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public Nullable<System.DateTime> PfDate { get; set; }
        [DisplayName("JC Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public Nullable<System.DateTime> JCDate { get; set; }
        [DisplayName("Employee Category")]
        public Nullable<int> EmployeeCategoryId { get; set; }
        [DisplayName("Job Type")]
        public Nullable<int> JobTypeId { get; set; }
        [DisplayName("Disburse Method")]
        public string DisverseMethod { get; set; }
        [DisplayName("Disburse Method")]
        public Nullable<int> DisverseMethodId { get; set; }

        public string ImageFileName { get; set; }
        [DisplayName("Signature File Name")]
        public string SignatureFileName { get; set; }
        [DisplayName("Office Type")]
        public string OfficeType { get; set; }
        [DisplayName("Office Type")]
        public Nullable<int> OfficeTypeId { get; set; }
        [DisplayName("Created By")]
        public Nullable<int> CreatedBy { get; set; }
        [DisplayName("Created Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [DisplayName("Modified By")]
        public string ModifedBy { get; set; }
        [DisplayName("Modified Date")]
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [DisplayName("Event Date")]
        public string EventDate { get; set; }
        public string EDepartment { get; set; }
        public string EDesignation { get; set; }
        public string Anniversary { get; set; }  
        public virtual CompanyModel Company { get; set; } 
        public virtual DropDownItemModel DropDownItem { get; set; }
        public virtual DropDownItemModel DropDownItem1 { get; set; }
        public virtual DropDownItemModel DropDownItem2 { get; set; }
        public virtual DropDownItemModel DropDownItem3 { get; set; }
        public virtual DropDownItemModel DropDownItem4 { get; set; }
        public virtual DropDownItemModel DropDownItem5 { get; set; }
        public virtual DropDownItemModel DropDownItem6 { get; set; }
        public virtual DropDownItemModel DropDownItem7 { get; set; }
        public virtual DropDownItemModel DropDownItem8 { get; set; }
        public virtual DropDownItemModel DropDownItem9 { get; set; }
        public virtual MemberModel HrAdmin { get; set; }

        public virtual MemberModel Manager { get; set; }
        //public virtual ShiftModel Shift { get; set; }
        public string ImagePath { get; set; }

        //====================================================== 

        [DisplayName("Member Id")]
        public string MemberId { get; set; }
        [DisplayName("Member Type")]
        [Required]
        public string MemberType { get; set; }
        [DisplayName("Member Name")]
        public string MemberName { get; set; }
        
        [DisplayName("Company Chairman")]
        public string CompanyChairman { get; set; }


        [DisplayName("Managing Director")]
        public string CompanyMD { get; set; }

        [DisplayName("Company Name")] 
        public string CompanyName { get; set; }

        [DisplayName("Contact Person")]
        public string ContactPersonName { get; set; } 
        public string Nationality { get; set; } 
        [DisplayName("Email")]
        public string ConcatPersonEmail { get; set; }

        [DisplayName("Contact Person Photo")]
        public string ImageUrl { get; set; }

        [DisplayName("Company Logo")]
        public string LogoUrl { get; set; } 

        [DisplayName("Company Address")]
        public string CompanyAddress { get; set; }
        [DisplayName("Contact Person Address")]
        public string ContactPersonAddress { get; set; }
        public string Division { get; set; } 
        public string Upzilla { get; set; } 
        [DisplayName("Date Of Registration")]
        public Nullable<System.DateTime> DateOfRegistration { get; set; }
        [DisplayName("Corporate Office")]
        public string CorporateOffice { get; set; }
        [DisplayName("Fiscal Year")]
        public string FiscalYear { get; set; }
        [DisplayName("Trade License")]
        public string TradeLicense { get; set; }
        [DisplayName("Website URL")]
        public string Website { get; set; }

    }
}
