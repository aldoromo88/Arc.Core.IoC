using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arc.Core.IoC.Contracts;
using Arc.Core.IoC.Structures;

namespace Arc.Core.IoC
{
    public class Container : IContainer
    {
        #region Fields
        private readonly Dictionary<Type, dynamic> perContainerCache;
        private readonly Dictionary<Type, dynamic> perGraphCache;
        private readonly ITrie<ContainerRegistry> registries;
        #endregion

        #region Constructor
        public Container()
        {
            registries = new Trie();
            perContainerCache = new Dictionary<Type, dynamic>();
            perGraphCache = new Dictionary<Type, dynamic>();

            InnerRegister(typeof(IContainer), typeof(Container), null, ActivationMethod.PerContainer);
        }
        #endregion

        #region Public Methods
        #region Register<T>
        public Container Register<T>()
        {
            return InnerRegister(typeof(T), typeof(T), null, ActivationMethod.PerCall);
        }

        public Container Register<T>(ActivationMethod activationMethod)
        {
            return InnerRegister(typeof(T), typeof(T), null, activationMethod);
        }

        public Container Register<T>(string name)
        {
            return InnerRegister(typeof(T), typeof(T), name, ActivationMethod.PerContainer);
        }

        public Container Register<T>(string name, ActivationMethod activationMethod)
        {
            return InnerRegister(typeof(T), typeof(T), name, activationMethod);
        }
        #endregion

        #region Register<T1,T2>
        public Container Register<T1, T2>() where T2 : T1
        {
            return InnerRegister(typeof(T1), typeof(T2), null, ActivationMethod.PerContainer);
        }

        public Container Register<T1, T2>(ActivationMethod activationMethod) where T2 : T1
        {
            return InnerRegister(typeof(T1), typeof(T2), null, activationMethod);
        }

        public Container Register<T1, T2>(string name) where T2 : T1
        {
            return InnerRegister(typeof(T1), typeof(T2), name, ActivationMethod.PerContainer);
        }

        public Container Register<T1, T2>(string name, ActivationMethod activationMethod) where T2 : T1
        {
            return InnerRegister(typeof(T1), typeof(T2), name, activationMethod);
        }
        #endregion

        #region Resitrer(Type target)
        public Container Register(Type target)
        {
            return InnerRegister(target, target, null, ActivationMethod.PerCall);
        }

        public Container Register(Type target, string name)
        {
            return InnerRegister(target, target, name, ActivationMethod.PerCall);
        }

        public Container Register(Type target, ActivationMethod activationMethod)
        {
            return InnerRegister(target, target, null, activationMethod);
        }

        public Container Register(Type target, string name, ActivationMethod activationMethod)
        {
            return InnerRegister(target, target, name, activationMethod);
        }
        #endregion

        #region Register(Type key, Type target)
        public Container Register(Type key, Type target)
        {
            return InnerRegister(key, target, null, ActivationMethod.PerCall);
        }

        public Container Register(Type key, Type target, string name)
        {
            return InnerRegister(key, target, name, ActivationMethod.PerCall);
        }

        public Container Register(Type key, Type target, ActivationMethod activationMethod)
        {
            return InnerRegister(key, target, null, activationMethod);
        }

        public Container Register(Type key, Type target, string name, ActivationMethod activationMethod)
        {
            return InnerRegister(key, target, name, activationMethod);
        }
        #endregion

        #region Resolve
        public T Resolve<T>()
        {
            return ResolveByType(typeof(T), new HashSet<Type>());
        }

        public dynamic Resolve(Type type)
        {
            return ResolveByType(type, new HashSet<Type>());
        }

        public dynamic Resolve(string name)
        {
            return ResolveByName(name, new HashSet<Type>());
        }
        #endregion

        #region ResolveAll
        public IList<T> ResolveAll<T>()
        {
            return ResolveAllByType(typeof(T), new HashSet<Type>()).Cast<T>().ToList();
        }

        public IList<dynamic> ResolveAll(Type type)
        {
            return ResolveAllByType(type, new HashSet<Type>());
        }

        public IList<dynamic> ResolveAll(string name)
        {
            return ResolveAllByName(name, new HashSet<Type>());
        }
        #endregion
        #endregion

        #region Private Methods
        private Container InnerRegister(Type key, Type target, string name, ActivationMethod activationMethod)
        {
            if (key != target && target.IsAssignableFrom(key))
            {
                throw new Exception($"Unnable to register type {target} with key {key}, target must be assignable to key.");
            }

            var containerRegistry = new ContainerRegistry(activationMethod, target);

            registries.Add(key.FullName, containerRegistry);

            if (key != target)
            {
                registries.Add(target.FullName, containerRegistry);
            }

            if (!string.IsNullOrEmpty(name))
            {
                registries.Add(name, containerRegistry);
            }


            return this;
        }

        private IList<dynamic> ResolveAllByType(Type type, HashSet<Type> previousGrahpNodes)
        {
            if (type == null) return new List<object>();

            var objs = ResolveAllByName(type.FullName, previousGrahpNodes);
            if (objs.Count != 0) return objs;

            //If not registered type, trying to create an instance as PerCall activation method
            var obj = ResolvePerCall(new ContainerRegistry(ActivationMethod.PerCall, type), previousGrahpNodes);
            if (obj != null)
            {
                objs.Add(obj);
            }

            return objs;
        }

        private IList<dynamic> ResolveAllByName(string name, HashSet<Type> previousGrahpNodes)
        {
            return registries.Get(name)
                    .Select(entry => ResolveByRegistry(entry, previousGrahpNodes))
                    .Where(entryResolved => entryResolved != null)
                    .ToList();
        }

        private dynamic ResolveByType(Type type, HashSet<Type> previousGrahpNodes)
        {
            var obj = ResolveByName(type.FullName, previousGrahpNodes);

            //If not registered type, trying to create an instance as PerCall activation method
            return obj ?? ResolvePerCall(new ContainerRegistry(ActivationMethod.PerCall, type), previousGrahpNodes);
        }

        private dynamic ResolveByName(string name, HashSet<Type> previousGrahpNodes)
        {
            var registry = registries.Get(name).FirstOrDefault();
            return ResolveByRegistry(registry, previousGrahpNodes);
        }

        private dynamic ResolveByRegistry(ContainerRegistry registry, HashSet<Type> previousGrahpNodes)
        {
            if (previousGrahpNodes.Count == 0) //Root graph
            {
                perGraphCache.Clear();
            }

            if (registry == null)
            {
                return null;
            }

            switch (registry.Method)
            {
                case ActivationMethod.PerContainer:
                    return ResolvePerContainer(registry, previousGrahpNodes);
                case ActivationMethod.PerCall:
                    return ResolvePerCall(registry, previousGrahpNodes);
                case ActivationMethod.PerResolutionGraph:
                    return ResolvePerGraph(registry, previousGrahpNodes);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private dynamic ResolvePerCall(ContainerRegistry registry, HashSet<Type> previousGrahpNodes)
        {
            ConstructorInfo constructor = GetValidConstructor(registry.Type);




            previousGrahpNodes.Add(registry.Type);



            var parameters = constructor.GetParameters();

            List<object> args = new List<object>();

            foreach (var p in parameters)
            {
                if (previousGrahpNodes.Contains(p.ParameterType))
                {
                    throw new Exception($"Error resolving type {registry.Type}, it has a cyclic dependency to his constructor's parameter {p.Name}:{p.ParameterType}");
                }
                var containedType = p.ParameterType.GenericTypeArguments.FirstOrDefault();
                var arg = p.ParameterType.GetInterface("System.Collections.IEnumerable") == null
                    ? ResolveByType(p.ParameterType, previousGrahpNodes)
                    : ConvertList(ResolveAllByType(containedType, previousGrahpNodes), p.ParameterType);

                args.Add(arg);
            }


            previousGrahpNodes.Remove(registry.Type);
            return Activator.CreateInstance(registry.Type, args.ToArray());
        }

        private static ConstructorInfo GetValidConstructor(Type type)
        {
            if (type.IsInterface)
            {
                throw new Exception($"Type {type} is an interface, can not create instances of an interface");
            }

            if (type.IsAbstract)
            {
                throw new Exception($"Type {type} is abstract, can not create instances of an abstract class");
            }


            //Get frist public constructor whit less parameters
            ConstructorInfo constructor = type.GetConstructors().Where(d => d.IsPublic).OrderBy(d => d.GetParameters().Length).FirstOrDefault();
            if (constructor == null)
            {
                throw new Exception($"Type {type} does not have a public constructor");
            }
            return constructor;
        }

        private dynamic ResolvePerContainer(ContainerRegistry registry, HashSet<Type> previousGrahpNodes)
        {
            if (!perContainerCache.ContainsKey(registry.Type))
            {
                perContainerCache.Add(registry.Type, ResolvePerGraph(registry, previousGrahpNodes));
            }
            return perContainerCache[registry.Type];
        }

        private dynamic ResolvePerGraph(ContainerRegistry registry, HashSet<Type> previousGrahpNodes)
        {

            if (!perGraphCache.ContainsKey(registry.Type))
            {
                perGraphCache.Add(registry.Type, ResolvePerCall(registry, previousGrahpNodes));
            }
            return perGraphCache[registry.Type];

        }

        private static dynamic ConvertList(IEnumerable<dynamic> value, Type type)
        {
            var listType = type.IsInterface ? typeof(List<>).MakeGenericType(type.GenericTypeArguments) : type;

            IList list = (IList)Activator.CreateInstance(listType);
            foreach (var item in value)
            {
                list.Add(item);
            }
            return list;
        }
        #endregion
    }
}
