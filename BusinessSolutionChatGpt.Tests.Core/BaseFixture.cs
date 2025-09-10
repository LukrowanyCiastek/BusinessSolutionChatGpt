using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NUnit.Framework;

namespace BusinessSolutionChatGpt.Tests.Core
{
    [TestFixture]
    public class BaseFixture
    {
#pragma warning disable CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ dodanie modyfikatora „required” lub zadeklarowanie go jako dopuszczającego wartość null.
        protected IFixture Fixture { get; private set; }
#pragma warning restore CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ dodanie modyfikatora „required” lub zadeklarowanie go jako dopuszczającego wartość null.

        [SetUp]
        public void SetUp() 
        {
            Fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        }
    }
}
