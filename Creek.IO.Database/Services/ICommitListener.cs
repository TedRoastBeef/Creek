namespace Creek.Database.Services
{
    public interface ICommitListener
    {
        void BeforeCommit();

        void AfterCommit();
    }
}