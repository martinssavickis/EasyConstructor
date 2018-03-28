using System;

namespace test
{
    public class InheritingClass : InheritingClassBase
    {
        public int Foo { get; private set; }

        public string Bar { get; private set; }

        public InheritingClass(int foo, string bar, DateTime baz) : base(baz)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}