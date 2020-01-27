using AutoMapper;
using BSA.Service.ServiceModel;
using BSA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSA.Models
{
    public partial class ModelMapper
    {
        public static void SetUp()
        {
            Mapper.Initialize(cfg =>
            {
                //***************** Entity to Model********************* 

                cfg.CreateMap<Customer, CustomerModel>(); 
                cfg.CreateMap<DropDownType, DropDownTypeModel>();
                cfg.CreateMap<DropDownItem, DropDownItemModel>();
                cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<Employee, MemberModel>();
                cfg.CreateMap<YearlyHoliday, YearlyHolidayModel>();

                //*****************Model to Entity*********************
           
                cfg.CreateMap<CustomerModel,  Customer>(); 
                cfg.CreateMap<DropDownTypeModel, DropDownType>();
                cfg.CreateMap<DropDownItemModel, DropDownItem>(); 
                cfg.CreateMap<MemberModel, Employee>();
                cfg.CreateMap<UserModel, User>(); 
                cfg.CreateMap<YearlyHolidayModel, YearlyHoliday>(); 
                 
            });
        }
    }
}


