using System;

namespace test
{
    public class InheritingClassBase
    {
        public DateTime Baz { get; private set; }

        public InheritingClassBase(DateTime baz)
        {
            Baz = baz;
        }
    }
}