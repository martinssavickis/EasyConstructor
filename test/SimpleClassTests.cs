using System;
using FluentAssertions;
using EasyConstructor;
using Xunit;

namespace test
{
    public class SimpleClassTests
    {
        private readonly Initializer initializer;
        public SimpleClassTests()
        {
            initializer = new Initializer();
        }

        [Fact]
        public void Create_WithoutParameters_ReturnswithDefaultValues()
        {
            var expected = new TestClass(0, null, new DateTime());

            var actual = initializer.Create<TestClass>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_WithCreateParameters_ReturnsExpectedValues()
        {
            var parameters = new
            {
                Foo = 42,
                Bar = "Answer",
                Baz = new DateTime(1979, 1, 1)
            };
            var expected = new TestClass(parameters.Foo, parameters.Bar, parameters.Baz);

            var actual = initializer.Create<TestClass>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_WithPartialCreateParameters_ReturnsExpectedValues()
        {
            var parameters = new
            {
                Foo = 42,
                Baz = new DateTime(1979, 1, 1)
            };
            var expected = new TestClass(parameters.Foo, null, parameters.Baz);

            var actual = initializer.Create<TestClass>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AddDefaultParameters_WithParameters_ReturnsExpectedValues()
        {
            var defaults = new
            {
                Foo = 42,
                Bar = "Answer",
                Baz = new DateTime(1979, 1, 1)
            };
            var expected = new TestClass(defaults.Foo, defaults.Bar, defaults.Baz);

            initializer.AddDefaultParameters<TestClass>(defaults);
            var actual = initializer.Create<TestClass>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AddDefaultParameters_WithPartialParameters_ReturnsExpectedValues()
        {
            var defaults = new
            {
                Bar = "Answer",
                Baz = new DateTime(1979, 1, 1)
            };
            var expected = new TestClass(default(int), defaults.Bar, defaults.Baz);

            initializer.AddDefaultParameters<TestClass>(defaults);
            var actual = initializer.Create<TestClass>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AddDefaultParameters_OverridesParameters_ReturnsExpectedValues()
        {
            var defaults = new
            {
                Foo = 42,
                Bar = "Answer",
                Baz = new DateTime(1979, 1, 1)
            };
            var defaultsOverride = new
            {
                Foo = 44,
                Bar = "Question",
                Baz = new DateTime(1999, 1, 1)
            };
            var expected = new TestClass(defaultsOverride.Foo, defaultsOverride.Bar, defaultsOverride.Baz);

            initializer.AddDefaultParameters<TestClass>(defaults);
            initializer.AddDefaultParameters<TestClass>(defaultsOverride);
            var actual = initializer.Create<TestClass>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AddDefaultParameters_OverlaysParameters_ReturnsExpectedValues()
        {
            var defaults = new
            {
                Bar = "Answer",
                Baz = new DateTime(1979, 1, 1)
            };
            var defaultsOverride = new
            {
                Foo = 44,
                Bar = "Question",
            };
            var expected = new TestClass(defaultsOverride.Foo, defaultsOverride.Bar, defaults.Baz);

            initializer.AddDefaultParameters<TestClass>(defaults);
            initializer.AddDefaultParameters<TestClass>(defaultsOverride);
            var actual = initializer.Create<TestClass>();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_OverridesDefaultParameters_ReturnsExpectedValues()
        {
            var defaults = new
            {
                Bar = "Answer",
                Baz = new DateTime(1979, 1, 1)
            };
            var parameters = new
            {
                Foo = 44,
                Bar = "Question",
            };
            var expected = new TestClass(parameters.Foo, parameters.Bar, defaults.Baz);

            initializer.AddDefaultParameters<TestClass>(defaults);
            var actual = initializer.Create<TestClass>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}