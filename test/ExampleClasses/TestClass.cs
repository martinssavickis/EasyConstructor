using System;

namespace test
{
    public class TestClass
    {
        public int Foo { get; private set; }

        public string Bar { get; private set; }

        public DateTime Baz { get; private set; }

        public TestClass(int foo, string bar, DateTime baz)
        {
            this.Foo = foo;
            this.Bar = bar;
            this.Baz = baz;
        }
    }
}