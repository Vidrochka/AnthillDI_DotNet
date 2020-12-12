using AnthillDI_DotNet.Attribute;

namespace AnthillDI_DotNet.Tests.TestClasses
{
    public class ClassWithPropertyInjection
    {
        [DIProperty]
        public ClassWithSingleParameterInConstructor TestPropertyInjection { get; set; }
    }
}