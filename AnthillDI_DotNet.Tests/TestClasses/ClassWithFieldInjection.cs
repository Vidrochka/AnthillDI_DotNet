using AnthillDI_DotNet.Attribute;

namespace AnthillDI_DotNet.Tests.TestClasses
{
    public class ClassWithFieldInjection
    {
        [DIField] public ClassWithSingleParameterInConstructor TestPropertyInjection;
    }
}