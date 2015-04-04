namespace Creek.UI.EFML.Base
{
    public abstract class IValidator
    {
        public abstract string Name { get; }
        public abstract string Pattern { get; }
    }
}