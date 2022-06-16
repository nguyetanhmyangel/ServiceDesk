using ServiceDesk.Data.Features.TransferTasks;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface ITransferTaskRepository //where T : BaseEntity
    {
        //bool Add(StatusViewModel model);

        //bool Remove(int id);

        //bool Update(StatusViewModel model);

        IEnumerable<TransferTaskResponse> FindById(int id);

        IEnumerable<TransferTaskResponse> FindAll();

        IEnumerable<TransferTaskResponse> FindByTaskId(int taskId);

        //IEnumerable<StatusViewModel> FindAllByFunction();
    }
}