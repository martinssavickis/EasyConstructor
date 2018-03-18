using System;

namespace test
{
    public class MultipleConstructors
    {
        public int Foo { get; private set; }

        public string Bar { get; private set; }

        public DateTime Baz { get; private set; }

        public MultipleConstructors(int foo, string bar, DateTime baz)
        {
            this.Foo = foo;
            this.Bar = bar;
            this.Baz = baz;
        }

        public MultipleConstructors(int foo, string bar)
        {
            this.Foo = foo;
            this.Bar = bar;
            this.Baz = new DateTime(1984, 1, 1);
        }

        public MultipleConstructors()
        {
            this.Foo = 42;
            this.Bar = "Answer";
            this.Baz = new DateTime(1989, 1, 1);
        }
    }
}