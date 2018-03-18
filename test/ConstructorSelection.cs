using System;
using System.Linq;
using EasyConstructor;
using FluentAssertions;
using Xunit;

namespace test
{
    public class ConstructorSelection
    {
        private readonly Initializer initializer;
        public ConstructorSelection()
        {
            initializer = new Initializer();
        }

        [Fact]
        public void UseConstructor_WhenCalled_SelectsConstructorToUse()
        {
            var parameters = new
            {
                Foo = 47,
                Bar = "Correct",
                Baz = new DateTime(1991, 1, 1)
            };
            var expected = new MultipleConstructors(parameters.Foo, parameters.Bar, parameters.Baz);

            //TODO still wondering what happens if only default is available
            initializer.UseConstructor<MultipleConstructors>(c => c.OrderByDescending(x => x.GetParameters().Count()).First());
            var actual = initializer.Create<MultipleConstructors>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void UseConstructor_WhenCalled_SelectsConstructorToUse_2()
        {
            var expected = new MultipleConstructors(0, null, DateTime.MinValue);

            //TODO still wondering what happens if only default is available
            initializer.UseConstructor<MultipleConstructors>(c => c.OrderByDescending(x => x.GetParameters().Count()).First());
            var actual = initializer.Create<MultipleConstructors>();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}