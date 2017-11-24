namespace Arc.Core.IoC.Test.UnitTests.Mocks.BasicHierarchy
{
    public abstract class Letter : IChar
    {
        public abstract char GetChar { get; }

        public Letter()
        {
            
        }
    }
}
