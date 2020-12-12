using AnthillDI_DotNet.Attribute;

namespace AnthillDI_DotNet.Tests.TestClasses
{
    public class ClassWithMultipleParameterInConstructor
    {
        public ClassWithoutParamsInConstructor WithoutParam;
        public ClassWithSingleParameterInConstructor WithSingleParam;

        [DIConstructor]
        public ClassWithMultipleParameterInConstructor(ClassWithoutParamsInConstructor testInjectWithoutParam, 
            ClassWithSingleParameterInConstructor testInjectWithSingleParam)
        {
            WithoutParam = testInjectWithoutParam;
            WithSingleParam = testInjectWithSingleParam;
        }
    }
}