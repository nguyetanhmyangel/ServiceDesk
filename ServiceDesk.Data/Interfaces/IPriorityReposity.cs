using ServiceDesk.Data.Features.Priority;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IPriorityRepository //where T : BaseEntity
    {
        //bool Add(PositionViewModel model);

        //bool Remove(int id);

        //bool Update(PositionViewModel model);

        //IEnumerable<PositionViewModel> FindById(int id);

        IEnumerable<PriorityResponse> FindAll();

        //IEnumerable<PositionViewModel> FindAllByFunction();
    }
}