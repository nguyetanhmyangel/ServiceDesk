using ServiceDesk.Data.Features.MenuIcon;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IMenuIconRepository //where T : BaseEntity
    {
        bool Add(MenuIconCommand model);

        bool Remove(int id);

        bool Update(MenuIconCommand model);

        IEnumerable<MenuIconResponse> FindById(int id);

        IEnumerable<MenuIconResponse> FindAll();

        IEnumerable<MenuIconResponse> FindAllByFunction();
    }
}