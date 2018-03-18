using System;
using EasyConstructor;
using FluentAssertions;
using Xunit;

namespace test
{
    public class BestConstructorSimpleTests
    {
        private readonly Initializer initializer;
        public BestConstructorSimpleTests()
        {
            initializer = new Initializer();
        }

        [Fact]
        public void Create_AllArgumets_ChoosesCorrectConstructor()
        {
            var parameters = new
            {
                Foo = 44,
                Bar = "Other",
                Baz = new DateTime(1999, 1, 1)
            };
            var expected = new MultipleConstructors(44, "Other", new DateTime(1999, 1, 1));

            var actual = initializer.Create<MultipleConstructors>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_FewerArgumets_ChoosesCorrectConstructor()
        {
            var parameters = new
            {
                Foo = 44,
                Bar = "Other"
            };
            var expected = new MultipleConstructors(44, "Other", new DateTime(1984, 1, 1));

            var actual = initializer.Create<MultipleConstructors>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_IncorrectArgumets_ChoosesDefaultConstructor()
        {
            var parameters = new
            {
                Fooz = 44,
                Barz = "Other",
                Bazz = DateTime.Now
            };
            var expected = new MultipleConstructors(42, "Answer", new DateTime(1989, 1, 1));

            var actual = initializer.Create<MultipleConstructors>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_IncorrectArgumetTypes_ChoosesDefaultConstructor()
        {
            var parameters = new
            {
                Foo = "wrong",
                Bar = -1,
                Baz = "still wrong"
            };
            var expected = new MultipleConstructors(42, "Answer", new DateTime(1989, 1, 1));

            var actual = initializer.Create<MultipleConstructors>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_SomeIncorrectArgumetTypes_ChoosesCorrectConstructor()
        {
            //should choose MultipleConstructors(int foo, string bar) 
            //because it has the least arguments and most matched arguments
            var parameters = new
            {
                Foo = "wrong",
                Bar = "Correct",
                Baz = "still wrong"
            };
            var expected = new MultipleConstructors(0, "Correct", new DateTime(1984, 1, 1));

            var actual = initializer.Create<MultipleConstructors>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_SomeIncorrectArguments_ChoosesCorrectConstructor()
        {
            //should choose MultipleConstructors(int foo, string bar, DateTime baz)
            //because it has the least arguments and most matched arguments
            var parameters = new
            {
                Fooz = "wrong",
                Bar = "Correct",
                Baz = new DateTime(1999, 1,1)
            };
            var expected = new MultipleConstructors(0, "Correct", new DateTime(1999, 1, 1));

            var actual = initializer.Create<MultipleConstructors>(parameters);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}