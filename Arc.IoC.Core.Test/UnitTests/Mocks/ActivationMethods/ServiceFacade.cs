namespace Arc.Core.IoC.Test.UnitTests.Mocks.ActivationMethods
{
    public class ServiceFacade
    {
        public ServiceA A { get; }
        public ServiceB B { get; }

        public ServiceFacade(ServiceA a, ServiceB b)
        {
            A = a;
            B = b;
        }
    }
}
