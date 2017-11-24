namespace Arc.Core.IoC.Test.UnitTests.Mocks.CyclicDependency
{
    public class Yin
    {
        public Yang Yang { get; }

        public Yin(Yang yang)
        {
            Yang = yang;
        }
    }
}
