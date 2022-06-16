using ServiceDesk.Data.Features.Menu;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IMenuRepository//<T> where T : BaseEntity
    {
        bool Add(MenuCommand model);

        bool Remove(int id);

        bool Update(MenuCommand model);

        IEnumerable<MenuResponse> FindById(int id);

        IEnumerable<MenuResponse> FindAll();

        IEnumerable<MenuResponse> FindAllByFunction();

        IEnumerable<MenuResponse> FindByName(string menuName);

        IEnumerable<SidebarResponse> SidebarList(string languageId, string userId, int roleId);
    }
}