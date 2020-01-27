using BSA.Service.Interface;
using BSA.Service.ServiceModel;
using BSA.Data.Models;
using BSA.Utility;
using BSA.Utility.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BSA.Service.Implementation
{
    public class MemberService : IMemberService
    {
        private bool disposed = false;

        ERPEntities employeeRepository = new ERPEntities();
        ERPEntities userRepository = new ERPEntities();

        public List<MemberModel> GetEmployees(string searchText)
        {
            IQueryable<Employee> employees = employeeRepository.Employees.Where(x => x.Active && (x.EmployeeId.Contains(searchText) || x.MemberName.Contains(searchText) || x.MobileNo.Contains(searchText) || x.Email.Contains(searchText))).OrderBy(x => x.Id);
            return ObjectConverter<Employee, MemberModel>.ConvertList(employees.ToList()).ToList();
        }

        public async Task<List<MemberModel>> GetPreviousEmployeesAsync(string searchText)
        {
            List<Employee> employees = await employeeRepository.Employees.Where(x => !x.Active && (x.EmployeeId.Contains(searchText) || x.MemberName.Contains(searchText) || x.MobileNo.Contains(searchText) || x.Email.Contains(searchText))).OrderBy(x => x.EmployeeId).ToListAsync();
            return ObjectConverter<Employee, MemberModel>.ConvertList(employees.ToList()).ToList();
        }

        private string GetEmployeeId(string employeeId)
        {
            string kg = employeeId.Substring(0, 3);

            string kgNumber = employeeId.Substring(3);
            int num = 0;
            if (employeeId != string.Empty)
            {
                num = Convert.ToInt32(kgNumber);
                ++num;
            }
            string newKgNumber = num.ToString().PadLeft(3, '0');
            return kg + newKgNumber;
        }

        public MemberModel GetEmployee(long id)
        {
            if (id <= 0)
            {
                Employee lastEmployee = employeeRepository.Employees.OrderByDescending(x => x.Id).FirstOrDefault();

                if (lastEmployee == null)
                {
                    return new MemberModel() { EmployeeId = "BSA0001" };
                }
                return new MemberModel()
                {
                    EmployeeId = GetEmployeeId(lastEmployee.EmployeeId)
                };
            }

            Employee employee = employeeRepository.Employees.OrderByDescending(x => x.Id == id).FirstOrDefault();
            return ObjectConverter<Employee, MemberModel>.Convert(employee);
        }

        public bool SaveEmployee(long id, MemberModel model)
        {
            if (model == null)
            {
                throw new Exception(Constants.DATA_NOT_FOUND);
            }
            if (model == null)
            {
                throw new Exception(Constants.DATA_NOT_FOUND);
            }
            Employee employee = ObjectConverter<MemberModel, Employee>.Convert(model);


            if (id > 0)
            {
                employee = employeeRepository.Employees.FirstOrDefault(x => x.Id == id);
                if (employee == null)
                {
                    throw new Exception(Constants.DATA_NOT_FOUND);
                }

                employee.ModifiedDate = DateTime.Now;
                employee.ModifedBy = "";
                if (!string.IsNullOrEmpty(model.ImageFileName))
                {
                    employee.LogoUrl = model.ImageFileName;
                }
            }
            else
            {
                UserModel userModel = new UserModel();
                userModel.UserName = model.EmployeeId;
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

                employee.CreatedBy = 1;
                employee.CreatedDate = DateTime.Now;
                employee.Active = true;

                employeeRepository.Employees.Add(employee);
                try
                {
                    int isEmployeeSaved = employeeRepository.SaveChanges();
                }
                catch (Exception)
                {
                    userRepository.Users.Remove(user);
                    return userRepository.SaveChanges() > 0;
                }
            }

            employee.EmployeeId = model.EmployeeId;
            employee.CompanyChairman = model.CompanyChairman;
            employee.CompanyMD = model.CompanyMD;
            employee.CompanyName = model.CompanyName;
            employee.MemberType = model.MemberType;
            employee.MemberName = model.MemberName;
            employee.ContactPersonName = model.ContactPersonName;
            employee.ConcatPersonEmail = model.ConcatPersonEmail;
            employee.AddressOne = model.AddressOne;
            employee.AddressTwo = model.AddressTwo;
            employee.Telephone = model.Telephone;
            employee.MobileNo = model.MobileNo;
            employee.PBX = model.PBX;
            employee.Nationality = model.Nationality;
            employee.Email = model.Email;
            employee.OfficeEmail = model.OfficeEmail;
            employee.TradeLicense = model.TradeLicense;
            employee.FiscalYear = model.FiscalYear;

            employee.BankId = model.BankId;
            employee.BankBranchId = model.BankBranchId;
            employee.BankAccount = model.BankAccount;
            employee.LicenseNo = model.LicenseNo;
            employee.PassportNo = model.PassportNo;
            employee.NationalId = model.NationalId;
            employee.TinNo = model.TinNo;
            employee.ReligionId = model.ReligionId;
            try
            {
                return employeeRepository.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteEmployee(long id)
        {
            Employee employee = employeeRepository.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                throw new Exception(Constants.DATA_NOT_FOUND);
            }
            employeeRepository.Employees.Remove(employee);
            return employeeRepository.SaveChanges() > 0;
        }

        public List<SelectModel> GetEmployeeSelectModels()
        {
            return employeeRepository.Employees.ToList().OrderBy(x => x.EmployeeId).Select(x => new SelectModel()
            {
                Text = "[" + x.EmployeeId.ToString() + "] " + x.MemberName,
                Value = x.Id.ToString()
            }).ToList();
        }

        public bool SaveAdminSetUp(long v, AdminSetUpModel adminSetUp)
        {
            throw new NotImplementedException();
        }

        public List<MemberModel> EmployeeSearch(string searchText)
        {
            IQueryable<Employee> employees = employeeRepository.Employees.Where(x => x.Active && (x.EmployeeId.Contains(searchText) || x.MemberName.Contains(searchText) || x.PBX.Contains(searchText) || x.MobileNo.Contains(searchText) || x.OfficeEmail.Contains(searchText))).OrderBy(x => x.Id);
            return ObjectConverter<Employee, MemberModel>.ConvertList(employees.ToList()).ToList();
        }

        public List<MemberModel> GetBirthday()
        {
            var b = ObjectConverter<Employee, MemberModel>.ConvertList(
                employeeRepository.Employees.Where(
                e => e.DateOfBirth.Value.Day == DateTime.Now.Day
                && e.DateOfBirth.Value.Month == DateTime.Now.Month).OrderBy(x => x.Id).ToList())
                .ToList();

            var bw = ObjectConverter<Employee, MemberModel>.ConvertList(
                employeeRepository.Employees.Where(
                e => e.DateOfBirth.Value.Day == DateTime.Now.Day
                && e.DateOfBirth.Value.Month == DateTime.Now.Month).OrderBy(x => x.Id).ToList())

                .ToList();
            return b;
        }

        public List<MemberModel> GetEmployeeEvent()
        {
            dynamic result = employeeRepository.Database.SqlQuery<MemberModel>("exec sp_GetEmployeeEvent").ToList();
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
                    employeeRepository.Dispose();
                }
            }
            disposed = true;
        }

        public object GetEmployeeAutoComplete(string prefix)
        {
            return employeeRepository.Employees.Where(x => x.MemberName.Contains(prefix)).Select(x => new
            {
                label = x.MemberName + " [" + x.EmployeeId + "]",
                val = x.Id
            }).OrderBy(x => x.label).Take(10).ToList();

        }
    }
}
