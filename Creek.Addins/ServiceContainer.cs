namespace Creek.Extensibility.Addins
{
    using System;
    using System.Linq;

    public class ServiceContainer
    {
        #region Public Methods and Operators

        public IServiceProvider GetService(Type type)
        {
            return
                ServiceProviderContainer.ToArray()
                    .FirstOrDefault(serviceProvider => serviceProvider.GetType().Name == type.Name);
        }

        public T GetService<T>() where T : IServiceProvider
        {
            return (T)this.GetService(typeof(T));
        }

        #endregion
    }
}