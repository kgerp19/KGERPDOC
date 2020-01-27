using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGERP.Data.Models;
using KGERP.Utility;

namespace KGERP.Service.Interface
{
    public interface IDropDownItemService
    {
        List<DropDownItem> GetDropDownItems();
        List<SelectModel> GetDropDownItemSelectModels(int id);
        // List<SelectModel> getCompanySelectModels();
    }
}
