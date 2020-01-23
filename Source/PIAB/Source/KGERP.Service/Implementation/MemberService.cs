using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using KGERP.Data.Models;
using KGERP.Utility;
using KGERP.Utility.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KGERP.Service.Implementation
{
    public class MemberService : IMemberService
    {
        private bool disposed = false;

           ERPEntities memberRepository = new ERPEntities();
        ERPEntities userRepository = new ERPEntities();

        public List<MemberModel> GetMembers(string searchText)
        {
            IQueryable<Member> members = memberRepository.Members.Where(x => x.Active && (x.MemberId.Contains(searchText) || x.MemberName.Contains(searchText)  || x.MobileNo.Contains(searchText) || x.Email.Contains(searchText))).OrderBy(x => x.OID);
            return ObjectConverter<Member, MemberModel>.ConvertList(members.ToList()).ToList();
        }

        public async Task<List<MemberModel>> GetPreviousMembersAsync(string searchText)
        {
            List<Member> members =  await memberRepository.Members.Where(x => !x.Active && (x.MemberId.Contains(searchText) || x.MemberName.Contains(searchText) || x.MobileNo.Contains(searchText) || x.Email.Contains(searchText))).OrderBy(x => x.MemberId).ToListAsync();
            return ObjectConverter<Member, MemberModel>.ConvertList(members.ToList()).ToList();
        }

        private string GetMemberId(string memberId)
        {
            string kg = memberId.Substring(0, 4);

            string kgNumber = memberId.Substring(4);
            int num = 0;
            if (memberId != string.Empty)
            {
                num = Convert.ToInt32(kgNumber);
                ++num;
            }
            string newKgNumber = num.ToString().PadLeft(4, '0');
            return kg + newKgNumber;
        }

        public MemberModel GetMember(long id)
        {
            if (id <= 0)
            {
                Member lastMember = memberRepository.Members.OrderByDescending(x => x.OID).FirstOrDefault();

                if (lastMember == null)
                {
                    return new MemberModel() { MemberId = "PIAB0001" };
                }
                return new MemberModel()
                {
                    MemberId = GetMemberId(lastMember.MemberId)
                };
            }
            //Member member = memberRepository.Members.Include("Manager").Include("Company").Include("Department").Include("Designation").Include("District").Include("Shift").Include("Grade").Include("Bank").Include("BankBranch1").Include("DropDownItem").Include("DropDownItem1").Include("DropDownItem2").Include("DropDownItem3").Include("DropDownItem4").Include("DropDownItem5").Include("DropDownItem6").Include("DropDownItem7").Include("DropDownItem8").Include("DropDownItem9").OrderByDescending(x => x.Id == id).FirstOrDefault();
            Member member = memberRepository.Members.OrderByDescending(x => x.OID == id).FirstOrDefault();

            return ObjectConverter<Member, MemberModel>.Convert(member);
        }

        public bool SaveMember(long id, MemberModel model)
        {
            if (model == null)
            {
                throw new Exception(Constants.DATA_NOT_FOUND);
            }
            if (model == null)
            {
                throw new Exception(Constants.DATA_NOT_FOUND);
            }
            Member member = ObjectConverter<MemberModel, Member>.Convert(model);


            if (id > 0)
            {
                member = memberRepository.Members.FirstOrDefault(x => x.OID == id);
                if (member == null)
                {
                    throw new Exception(Constants.DATA_NOT_FOUND);
                }

                member.ModifiedDate = DateTime.Now;
                member.ModifedBy = "";
                if (!string.IsNullOrEmpty(model.ImageFileName))
                {
                    member.LogoUrl = model.ImageFileName;
                }
            }
            else
            {
                UserModel userModel = new UserModel();
                userModel.UserName = model.MemberId;
                userModel.Email = model.Email;
                userModel.IsEmailVerified = true;
                userModel.Active = true;
                userModel.Password = Crypto.Hash(userModel.UserName.ToLower());
                userModel.ConfirmPassword = userModel.Password;

                User user = ObjectConverter<UserModel, User>.Convert(userModel);

                userRepository.Users.Add(user);
                int isUserSaved = userRepository.SaveChanges();
                if (isUserSaved <= 0)
                    throw new Exception(Constants.OPERATION_FAILE);

                member.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                member.CreatedDate = DateTime.Now;
                member.Active = true;

                memberRepository.Members.Add(member);
                try
                {
                    int isMemberSaved = memberRepository.SaveChanges();
                }
                catch (Exception)
                {

                    userRepository.Users.Remove(user);
                    return userRepository.SaveChanges() > 0;
                }
            }

            member.MemberId = model.MemberId;
            member.CompanyChairman = model.CompanyChairman;
            member.CompanyMD = model.CompanyMD;
            member.CompanyName = model.CompanyName;
            member.MemberType = model.MemberType;
            member.MemberName = model.MemberName; 
            member.ContactPersonName = model.ContactPersonName;
            member.ConcatPersonEmail = model.ConcatPersonEmail;
            member.CompanyAddress = model.CompanyAddress;
            member.ContactPersonAddress = model.ContactPersonAddress;  
            member.Telephone = model.Telephone;
            member.MobileNo = model.MobileNo;
            member.PBX = model.PBX;
            member.Nationality = model.Nationality;
            member.Email = model.Email;
            member.OfficeEmail = model.OfficeEmail; 
            member.TradeLicense = model.TradeLicense;  
          
            member.BankId = model.BankId;
            member.BankBranchId = model.BankBranchId;
            member.BankAccount = model.BankAccount;
            member.LicenseNo = model.LicenseNo;
            member.PassportNo = model.PassportNo;
            member.NationalId = model.NationalId;
            member.TinorBin = model.TinorBin;  
            try
            {
                return memberRepository.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public bool DeleteMember(long id)
        {
            Member member = memberRepository.Members.FirstOrDefault(x => x.OID == id);
            if (member == null)
            {
                throw new Exception(Constants.DATA_NOT_FOUND);
            }

            memberRepository.Members.Remove(member);
            return memberRepository.SaveChanges() > 0;

        }

        public List<SelectModel> GetMemberSelectModels()
        {
            return memberRepository.Members.ToList().OrderBy(x => x.MemberId).Select(x => new SelectModel()
            {
                Text = "[" + x.MemberId.ToString() + "] " + x.MemberName,
                Value = x.OID.ToString()
            }).ToList();
        }

        public bool SaveAdminSetUp(long v, AdminSetUpModel adminSetUp)
        {
            throw new NotImplementedException();
        }

        public List<MemberModel> MemberSearch(string searchText)
        {
            IQueryable<Member> members = memberRepository.Members.Where(x => x.Active && (x.MemberId.Contains(searchText) || x.MemberName.Contains(searchText) || x.PBX.Contains(searchText) || x.MobileNo.Contains(searchText) || x.OfficeEmail.Contains(searchText))).OrderBy(x => x.OID);
            return ObjectConverter<Member, MemberModel>.ConvertList(members.ToList()).ToList();
        }

        public List<MemberModel> GetBirthday()
        {
            var b = ObjectConverter<Member, MemberModel>.ConvertList(
                memberRepository.Members.Where(
                e => e.DateOfBirth.Value.Day == DateTime.Now.Day
                && e.DateOfBirth.Value.Month == DateTime.Now.Month).OrderBy(x => x.OID).ToList())
                .ToList();

            var bw = ObjectConverter<Member, MemberModel>.ConvertList(
                memberRepository.Members.Where(
                e => e.DateOfBirth.Value.Day == DateTime.Now.Day
                && e.DateOfBirth.Value.Month == DateTime.Now.Month).OrderBy(x => x.OID).ToList())

                .ToList();
            return b;
        }

        public List<MemberModel> GetMemberEvent()
        {
            dynamic result = memberRepository.Database.SqlQuery<MemberModel>("exec sp_GetMemberEvent").ToList();
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    memberRepository.Dispose();
                }
            }
            disposed = true;
        }

        public object GetMemberAutoComplete(string prefix)
        {
            return memberRepository.Members.Where(x =>  x.MemberName.Contains(prefix)).Select(x => new
            {
                label = x.MemberName + " ["+x.MemberId+"]",
                val = x.OID
            }).OrderBy(x=>x.label).Take(10).ToList();

        }
    }
}
