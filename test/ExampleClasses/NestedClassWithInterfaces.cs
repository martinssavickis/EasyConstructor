namespace test
{
    public class NestedClassWithInterfaces
    {
        public ITestClass ChildOne { get; private set; }
        public IMultipleConstructors ChildTwo { get; private set; }

        public NestedClassWithInterfaces(ITestClass childOne, IMultipleConstructors childTwo)
        {
            ChildOne = childOne;
            ChildTwo = childTwo;
        }
    }
}