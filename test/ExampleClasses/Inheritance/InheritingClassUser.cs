namespace test
{
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
}