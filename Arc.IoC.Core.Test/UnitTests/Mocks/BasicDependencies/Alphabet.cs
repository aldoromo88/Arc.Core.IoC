using System.Collections.Generic;
using Arc.Core.IoC.Test.UnitTests.Mocks.BasicHierarchy;

namespace Arc.Core.IoC.Test.UnitTests.Mocks.BasicDependencies
{
    public interface IAlphabet
    {
        IEnumerable<IChar> Letters { get; }
    }

    public class Alphabet : IAlphabet
    {
        public IEnumerable<IChar> Letters { get; }

        public Alphabet(Queue<IChar> letters)
        {
            Letters = letters;
        }
    }
}