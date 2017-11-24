using System;

namespace Arc.Core.IoC.Test.UnitTests.Mocks.ActivationMethods
{
    public class ServiceA : ISimpleService
    {
        private readonly CommonDependency _dep;
        public Guid GetInternalId => _dep.Id;

        public ServiceA(CommonDependency dep)
        {
            _dep = dep;
        }
    }
}