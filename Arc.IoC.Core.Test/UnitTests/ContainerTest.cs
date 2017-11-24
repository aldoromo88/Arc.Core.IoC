using System;
using System.Linq;
using Arc.Core.IoC.Contracts;
using Arc.Core.IoC.Test.UnitTests.Mocks.ActivationMethods;
using Arc.Core.IoC.Test.UnitTests.Mocks.BasicDependencies;
using Arc.Core.IoC.Test.UnitTests.Mocks.BasicHierarchy;
using Arc.Core.IoC.Test.UnitTests.Mocks.CyclicDependency;
using NUnit.Framework;

namespace Arc.Core.IoC.Test.UnitTests
{
    [TestFixture]
    public class ContainerTest
    {
        [Test]
        public void ResolveUnregisteredConcreteTypeTest()
        {
            var container = new Container();
            var letterB = container.Resolve<LetterB>();

            Assert.IsNotNull(letterB);
            Assert.AreEqual(letterB.GetChar, 'B');
        }

        [Test]
        public void ResolveAllTest()
        {
            var container = new Container();
            container.Register<IChar, LetterA>();
            container.Register<IChar, LetterB>();
            container.Register<IChar, LetterC>();
            container.Register<IChar, LetterD>();

            var chars = container.ResolveAll<IChar>();
            Assert.IsNotNull(chars);
            Assert.AreEqual(chars.Count, 4);
        }

        [Test]
        public void ResolveInterfaceTest()
        {
            var container = new Container();
            container.Register<IChar>();

            Assert.Throws<Exception>(() =>
            {
                container.Resolve<IChar>();
            });
        }

        [Test]
        public void ResolveAbstractClass()
        {
            var container = new Container();

            container.Register<Letter>();

            Assert.Throws<Exception>(() =>
            {
                container.Resolve<Letter>();
            });
        }

        [Test]
        public void ResolveWithDependenciesTest()
        {
            var container = new Container();
            container.Register<IChar, LetterA>();
            container.Register<IChar, LetterB>();
            container.Register<IChar, LetterC>();
            container.Register<IChar, LetterD>();
            container.Register<IChar, LetterE>();
            container.Register<IChar, LetterF>();

            container.Register<IAlphabet, Alphabet>();
            container.Register<WrittenLanguage>("Italian");

            WrittenLanguage writtenLanguage = container.Resolve("Italian");

            Assert.IsNotNull(writtenLanguage);
            Assert.AreEqual(writtenLanguage.Alphabet.Letters.Count(), 6);
        }

        [Test]
        public void ResolveCyclicDependencyTest()
        {

            var container = new Container();

            Assert.Throws<Exception>(() =>
            {
                container.Resolve<Yin>();
            });

            Assert.Throws<Exception>(() =>
            {
                container.Resolve<Yang>();
            });
        }

        [Test]
        public void ActivationMethodPerCallTest()
        {
            var container = new Container();

            container.Register<CommonDependency>();

            var serviceFacade1 = container.Resolve<ServiceFacade>();
            var serviceFacade2 = container.Resolve<ServiceFacade>();


            Assert.IsNotNull(serviceFacade1);
            Assert.IsNotNull(serviceFacade2);


            Assert.AreNotEqual(serviceFacade1.A.GetInternalId, serviceFacade1.B.GetInternalId);
            Assert.AreNotEqual(serviceFacade2.A.GetInternalId, serviceFacade2.B.GetInternalId);

            Assert.AreNotEqual(serviceFacade1.A.GetInternalId, serviceFacade2.A.GetInternalId);
            Assert.AreNotEqual(serviceFacade1.B.GetInternalId, serviceFacade2.B.GetInternalId);
        }

        [Test]
        public void ActivationMethodPerGraphTest()
        {
            var container = new Container();

            container.Register<CommonDependency>(ActivationMethod.PerResolutionGraph);

            var serviceFacade1 = container.Resolve<ServiceFacade>();
            var serviceFacade2 = container.Resolve<ServiceFacade>();


            Assert.IsNotNull(serviceFacade1);
            Assert.IsNotNull(serviceFacade2);


            Assert.AreEqual(serviceFacade1.A.GetInternalId, serviceFacade1.B.GetInternalId);
            Assert.AreEqual(serviceFacade2.A.GetInternalId, serviceFacade2.B.GetInternalId);

            Assert.AreNotEqual(serviceFacade1.A.GetInternalId, serviceFacade2.A.GetInternalId);
            Assert.AreNotEqual(serviceFacade1.B.GetInternalId, serviceFacade2.B.GetInternalId);
        }

        [Test]
        public void ActivationMethodPerContainerTest()
        {
            var container = new Container();

            container.Register<CommonDependency>(ActivationMethod.PerContainer);

            var serviceFacade1 = container.Resolve<ServiceFacade>();
            var serviceFacade2 = container.Resolve<ServiceFacade>();


            Assert.IsNotNull(serviceFacade1);
            Assert.IsNotNull(serviceFacade2);


            Assert.AreEqual(serviceFacade1.A.GetInternalId, serviceFacade1.B.GetInternalId);
            Assert.AreEqual(serviceFacade2.A.GetInternalId, serviceFacade2.B.GetInternalId);

            Assert.AreEqual(serviceFacade1.A.GetInternalId, serviceFacade2.A.GetInternalId);
            Assert.AreEqual(serviceFacade1.B.GetInternalId, serviceFacade2.B.GetInternalId);
        }

        [Test]
        public void RegisterTwiceTest()
        {
            var container = new Container();
            container.Register<ServiceA>();
            container.Register<ServiceA>();

            var servicesA = container.ResolveAll<ServiceA>();

            Assert.IsNotNull(servicesA);
            Assert.AreEqual(1, servicesA.Count);
        }
    }
}