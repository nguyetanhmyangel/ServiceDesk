using ServiceDesk.Data.Features.Tag;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface ITagRepository //where T : BaseEntity
    {
        //bool Add(PositionViewModel model);

        //bool Remove(int id);

        //bool Update(PositionViewModel model);

        //IEnumerable<PositionViewModel> FindById(int id);

        IEnumerable<TagResponse> FindAll(string languageId);

        //IEnumerable<PositionViewModel> FindAllByFunction();
    }
}