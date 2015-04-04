namespace Creek.Tools
{
    public class Singleton<TInstance>
        where TInstance : new()
    {
        private static TInstance instance;

        // Note: Constructor is 'protected' 
        protected Singleton()
        {
        }

        public static TInstance Instance
        {
            get
            {
                // Use 'Lazy initialization' 
                if (instance == null)
                {
                    instance = new TInstance();
                }

                return instance;
            }
        }
    }
}
