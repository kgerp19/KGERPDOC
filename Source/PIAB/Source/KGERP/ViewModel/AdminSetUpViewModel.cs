using KGERP.Service.ServiceModel;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KGERP.ViewModel
{
    public class AdminSetUpViewModel
    {
        public AdminSetUpModel AdminSetUp { get; set; }
        public List<SelectModel> Members { get; set; }
        public List<SelectModel> StatusSelectModels { get; set; }
    }
}