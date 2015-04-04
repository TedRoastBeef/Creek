using Creek.Database.Meta;

namespace Creek.Database.Core.Query
{
    internal interface IQueryExecutionPlan
    {
        bool UseIndex();

        ClassInfoIndex GetIndex();

        string GetDetails();

        void Start();

        void End();
    }
}
