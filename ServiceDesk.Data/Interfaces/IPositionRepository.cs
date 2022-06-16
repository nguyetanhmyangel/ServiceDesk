using ServiceDesk.Data.Features.Position;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IPositionRepository //where T : BaseEntity
    {
        //bool Add(PositionViewModel model);

        //bool Remove(int id);

        //bool Update(PositionViewModel model);

        //IEnumerable<PositionViewModel> FindById(int id);

        IEnumerable<PositionResponse> FindAll();

        //IEnumerable<PositionViewModel> FindAllByFunction();
    }
}