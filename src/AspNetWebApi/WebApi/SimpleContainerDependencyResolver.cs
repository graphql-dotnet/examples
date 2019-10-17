using IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http.Dependencies;

namespace WebApi
{
    public class SimpleContainerDependencyResolver : IDependencyResolver
    {
        private readonly ISimpleContainer _container;

        public SimpleContainerDependencyResolver(ISimpleContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Get(serviceType);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
