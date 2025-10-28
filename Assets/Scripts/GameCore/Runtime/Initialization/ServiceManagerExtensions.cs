using System;

namespace GameCore
{
    public static class ServiceManagerExtensions
    {
        public static T GetService<T>(this ServiceManager serviceManager) where T : class
        {
            return (T)serviceManager.GetService(typeof(T));
        }

        public static void SetService(this ServiceManager serviceManager, Type serviceType, object service, bool markPersistent)
        {
            serviceManager.SetService(serviceType, service, markPersistent, false);
        }

        public static void OverwriteService(this ServiceManager serviceManager, Type serviceType, object service, bool markPersistent)
        {
            serviceManager.SetService(serviceType, service, markPersistent, true);
        }
    }
}