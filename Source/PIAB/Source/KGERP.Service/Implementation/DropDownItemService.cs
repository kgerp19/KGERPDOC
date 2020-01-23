using KGERP.Service.Interface;
using KGERP.Data.Models;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGERP.Service.Implementation
{
    public class DropDownItemService : IDropDownItemService
    {
        ERPEntities dropdownItemRepository = new ERPEntities();
        public List<DropDownItem> GetDropDownItems()
        {
            return dropdownItemRepository.DropDownItems.ToList();
        }

        

        public List<SelectModel> GetDropDownItemSelectModels(int id)
        {
            return dropdownItemRepository.DropDownItems.ToList().Where(x=>x.DropDownTypeId==id).Select(x => new SelectModel()
            {
                Text = x.Name.ToString(),
                Value = x.DropDownItemId.ToString()
            }).ToList();
        }
    }
}
