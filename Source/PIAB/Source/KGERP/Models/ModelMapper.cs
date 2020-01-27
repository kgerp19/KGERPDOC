using AutoMapper;
using KGERP.Service.ServiceModel;
using KGERP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KGERP.Models
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
                cfg.CreateMap<Member, MemberModel>(); 

                //*****************Model to Entity*********************
           
                cfg.CreateMap<CustomerModel,  Customer>(); 
                cfg.CreateMap<DropDownTypeModel, DropDownType>();
                cfg.CreateMap<DropDownItemModel, DropDownItem>(); 
                cfg.CreateMap<MemberModel, Member>();
                cfg.CreateMap<UserModel, User>();  
                 
            });
        }
    }
}


