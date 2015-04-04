namespace Creek.Database.Core.Query.Criteria.Evaluations
{
    internal interface IEvaluation
    {
        bool Evaluate(object candidate);
    }
}