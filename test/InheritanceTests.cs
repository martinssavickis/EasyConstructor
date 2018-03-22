using System;
using EasyConstructor;
using FluentAssertions;
using Xunit;

namespace test
{
    public class InheritanceTests
    {
        private readonly Initializer initializer = new Initializer();

        [Fact]
        public void Create_WithConcreteObjects_CreatesObject()
        {
            InheritingClass inheriting = new InheritingClass(42, "Name", DateTime.MinValue);
            InheritingClassBase inheritingBase = new InheritingClassBase(DateTime.MinValue);
            InheritingClassUser expected = new InheritingClassUser(inheriting, inheritingBase);
            var values = new
            {
                inheriting,
                inheritingBase
            };

            var actual = initializer.Create<InheritingClassUser>(values);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_WithDerivedAsBase_CreatesObject()
        {
            InheritingClass inheriting = new InheritingClass(42, "Name", DateTime.MinValue);
            InheritingClass inheritingBase = new InheritingClass(42, "Name", DateTime.MinValue);
            InheritingClassUser expected = new InheritingClassUser(inheriting, inheritingBase);
            var values = new
            {
                inheriting,
                inheritingBase
            };

            var actual = initializer.Create<InheritingClassUser>(values);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_WithAnonymousObjects_CreatesObject()
        {
            InheritingClass inheriting = new InheritingClass(42, "Name", DateTime.MinValue);
            InheritingClassBase inheritingBase = new InheritingClassBase(DateTime.MinValue);
            InheritingClassUser expected = new InheritingClassUser(inheriting, inheritingBase);
            var values = new
            {
                inheriting = new
                {
                    Foo = inheriting.Foo,
                    Bar = inheriting.Bar,
                    Baz = inheriting.Baz
                },
                inheritingBase = new
                {
                    Baz = DateTime.MinValue
                }
            };

            var actual = initializer.Create<InheritingClassUser>(values);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}