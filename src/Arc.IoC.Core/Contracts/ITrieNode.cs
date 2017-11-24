using System.Collections.Generic;

namespace Arc.Core.IoC.Contracts
{
    public interface ITrieNode<T>
    {
        ISet<T> Registries { get; }
    }
}
