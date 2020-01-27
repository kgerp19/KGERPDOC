using KGERP.Data.Models;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
  public  interface IDesignationService
    {
        List<Designation> GetDesignations();
        List<SelectModel> GetDesignationSelectModels();
    }
}
