using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arc.Core.IoC.Contracts;

namespace Arc.Core.IoC.Structures
{
    [DebuggerDisplay("Type = {Type.FullName}")]
    public class ContainerRegistry
    {
        internal ActivationMethod Method { get; }
        internal Type Type { get; }

        internal ContainerRegistry(ActivationMethod method, Type type)
        {
            Method = method;
            Type = type;
        }

        public override int GetHashCode()
        {
            return Type.FullName?.GetHashCode() ?? 0;
        }
    }

    public class ContainerRegistryComparer : IEqualityComparer<ContainerRegistry>
    {
        public bool Equals(ContainerRegistry x, ContainerRegistry y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(ContainerRegistry obj)
        {
            return obj.GetHashCode();
        }
    }
}
