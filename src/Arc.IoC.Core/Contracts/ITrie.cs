using System.Collections.Generic;
using Arc.Core.IoC.Structures;

namespace Arc.Core.IoC.Contracts
{
    public interface ITrie<in T> where T : class
    {
        void Add(string name, T element);
        ISet<ContainerRegistry> Get(string name);
    }
}