using System;
using EasyConstructor;
using FluentAssertions;
using Xunit;

namespace test
{
    public class InterfaceTests
    {
        private readonly Initializer initializer = new Initializer();

        [Fact]
        public void Create_ConcreteObjectsForInterfaces_CreatesObject()
        {
            TestClass first = new TestClass(42, "Answer", DateTime.MinValue);
            MultipleConstructors second = new MultipleConstructors(44, "Other");
            NestedClassWithInterfaces expected = new NestedClassWithInterfaces(first, second);
            var values = new
            {
                ChildOne = first,
                ChildTwo = second
            };

            var actual = initializer.Create<NestedClassWithInterfaces>(values);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_EmptyValues_CreatesObjectWithDefaults()
        {
            NestedClassWithInterfaces expected = new NestedClassWithInterfaces(null, null);

            var actual = initializer.Create<NestedClassWithInterfaces>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_ValueNotRegistered_CreatesObjectWithDefaults()
        {
            var values = new
            {
                ChildOne = new { Foo = 42, Bar = "answer", Baz = new DateTime() },
                ChildTwo = new { Foo = 42, Bar = "answer" }
            };
            NestedClassWithInterfaces expected = new NestedClassWithInterfaces(null, null);

            var actual = initializer.Create<NestedClassWithInterfaces>(values);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_RegisteredInterface_CreatesConcreteObjectForInterface()
        {
            var values = new
            {
                ChildOne = new { Foo = 42, Bar = "answer", Baz = new DateTime() },
                ChildTwo = new { Foo = 42, Bar = "answer" }
            };
            NestedClassWithInterfaces expected =
                new NestedClassWithInterfaces(
                    new TestClass(values.ChildOne.Foo, values.ChildOne.Bar, values.ChildOne.Baz),
                    new MultipleConstructors(values.ChildTwo.Foo, values.ChildTwo.Bar)
                );

            initializer.RegisterInterface<ITestClass, TestClass>();
            initializer.RegisterInterface<IMultipleConstructors, MultipleConstructors>();
            var actual = initializer.Create<NestedClassWithInterfaces>(values);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}