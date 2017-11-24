using System;

namespace Arc.Core.IoC.Test.UnitTests.Mocks.ActivationMethods
{
    public class CommonDependency
    {
        public Guid Id { get; }

        public CommonDependency()
        {
            Id = Guid.NewGuid();
        }
    }
}
