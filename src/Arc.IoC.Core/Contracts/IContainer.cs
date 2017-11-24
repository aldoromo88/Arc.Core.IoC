using System;
using System.Collections.Generic;

namespace Arc.Core.IoC.Contracts
{
    public interface IContainer
    {
        Container Register<T>();
        Container Register<T>(ActivationMethod activationMethod);
        Container Register<T>(string name);
        Container Register<T>(string name, ActivationMethod activationMethod);
        Container Register<T1, T2>() where T2 : T1;
        Container Register<T1, T2>(ActivationMethod activationMethod) where T2 : T1;
        Container Register<T1, T2>(string name) where T2 : T1;
        Container Register<T1, T2>(string name, ActivationMethod activationMethod) where T2 : T1;
        Container Register(Type target);
        Container Register(Type target, string name);
        Container Register(Type target, ActivationMethod activationMethod);
        Container Register(Type target, string name, ActivationMethod activationMethod);
        Container Register(Type key, Type target);
        Container Register(Type key, Type target, string name);
        Container Register(Type key, Type target, ActivationMethod activationMethod);
        Container Register(Type key, Type target, string name, ActivationMethod activationMethod);
        T Resolve<T>();
        dynamic Resolve(Type type);
        dynamic Resolve(string name);
        IList<T> ResolveAll<T>();
        IList<object> ResolveAll(Type type);
        IList<object> ResolveAll(string name);
    }
}