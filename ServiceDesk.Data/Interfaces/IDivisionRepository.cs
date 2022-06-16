using ServiceDesk.Data.Features.Division;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IDivisionRepository //where T : BaseEntity
    {
        IEnumerable<DivisionResponse> FindById(int id);

        IEnumerable<DivisionResponse> FindAll();

        IEnumerable<DivisionResponse> FindAllByFunction();
    }
}