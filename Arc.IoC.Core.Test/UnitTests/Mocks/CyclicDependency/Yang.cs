namespace Arc.Core.IoC.Test.UnitTests.Mocks.CyclicDependency
{
    public class Yang
    {
        public Yin Yin { get; }

        public Yang(Yin yin)
        {
            Yin = yin;
        }
    }
}