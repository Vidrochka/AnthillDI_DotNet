using AnthillDI_DotNet.Attribute;

namespace AnthillDI_DotNet.Tests.TestClasses
{
    public class ClassWithSingleParameterInConstructor
    {
        public ClassWithoutParamsInConstructor TestInjectedClass;

        [DIConstructor]
        public ClassWithSingleParameterInConstructor(ClassWithoutParamsInConstructor testInjectedParameter)
        {
            TestInjectedClass = testInjectedParameter;
        }
    }
}