using Creek.Database.Meta.Introspector;

namespace Creek.Database.Meta
{
    public interface IClassInfoProvider
    {
        IObjectIntrospectionDataProvider GetClassInfoProvider();
    }
}