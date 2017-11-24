namespace Arc.Core.IoC.Test.UnitTests.Mocks.BasicDependencies
{
    public class WrittenLanguage
    {
        public IAlphabet Alphabet { get; }

        public WrittenLanguage(IAlphabet alphabet)
        {
            Alphabet = alphabet;
        }
    }
}
