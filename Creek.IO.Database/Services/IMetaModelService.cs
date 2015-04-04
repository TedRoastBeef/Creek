using System.Collections.Generic;
using Creek.Database.Meta;

namespace Creek.Database.Services
{
    public interface IMetaModelService
    {
        IEnumerable<ClassInfo> GetAllClasses();
    }
}