using System;

namespace test
{
    public class NestedClass
    {
        public int Foo { get; private set; }
        public TestClass ChildOne { get; set; }
        public MultipleConstructors ChildTwo { get; set; }
    }

    public class InheritingClassUser
    {
        public InheritingClass Inheriting { get; private set; }
        public InheritingClassBase InheritingBase { get; set; }

        public InheritingClassUser(InheritingClass inheriting, InheritingClassBase inheritingBase)
        {
            Inheriting = inheriting;
            InheritingBase = inheritingBase;
        }
    }

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

    public class InheritingClassBase
    {
        public DateTime Baz { get; private set; }

        public InheritingClassBase(DateTime baz)
        {
            Baz = baz;
        }
    }
}