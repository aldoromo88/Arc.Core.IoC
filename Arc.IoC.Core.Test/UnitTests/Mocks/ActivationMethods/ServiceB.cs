using System;

namespace Arc.Core.IoC.Test.UnitTests.Mocks.ActivationMethods
{
    public class ServiceB : ISimpleService
    {
        private readonly CommonDependency _dep;
        public Guid GetInternalId => _dep.Id;

        public ServiceB(CommonDependency dep)
        {
            _dep = dep;
        }
    }
}