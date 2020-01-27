using System.Collections.Generic;
using KGERP.Service.ServiceModel;
using KGERP.Data.Models;
using KGERP.Utility;
using System;
using System.Threading.Tasks;

namespace KGERP.Service.Interface
{
    public interface IMemberService : IDisposable
    {
        List<MemberModel> GetMembers(string searchText);
        MemberModel GetMember(long id);
        bool SaveMember(long id, MemberModel employee);
        bool DeleteMember(long id);
        List<SelectModel> GetMemberSelectModels();
        List<MemberModel> MemberSearch(string v);
        List<MemberModel> GetMemberEvent();
        Task<List<MemberModel>> GetPreviousMembersAsync(string searchText);
        object GetMemberAutoComplete(string prefix);
    }
}
