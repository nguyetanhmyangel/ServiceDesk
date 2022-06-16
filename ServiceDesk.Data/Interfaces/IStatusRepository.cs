using ServiceDesk.Data.Features.Status;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IStatusRepository //where T : BaseEntity
    {
        //bool Add(StatusViewModel model);

        //bool Remove(int id);

        //bool Update(StatusViewModel model);

        IEnumerable<StatusResponse> FindById(int id);

        IEnumerable<StatusResponse> FindAll();

        //IEnumerable<StatusViewModel> FindAllByFunction();
    }
}