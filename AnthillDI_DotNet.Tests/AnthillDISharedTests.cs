using AnthillDI_DotNet.Tests.TestClasses;
using Xunit;

namespace AnthillDI_DotNet.Tests
{
    public class AnthillDISharedTests
    {
        [Fact]
        public void TestInjectFuncInitialization_Ok()
        {
            AHDI di = new AHDI();

            di.SetSingletonObject<ClassWithoutParamsInConstructor>();
            di.SetRequestedObject((context)
                => new ClassWithMultipleParameterInConstructor(
                    context.GetObject<ClassWithoutParamsInConstructor>(),
                    new ClassWithSingleParameterInConstructor(context.GetObject<ClassWithoutParamsInConstructor>())));

            ClassWithMultipleParameterInConstructor type = di.GetObject<ClassWithMultipleParameterInConstructor>();

            Assert.Equal(1, type.WithoutParam.TestNum);
            Assert.Equal(2, type.WithoutParam.TestNum2);
            Assert.Equal("1", type.WithoutParam.TestStr);
            Assert.Equal("2", type.WithoutParam.TestStr2);
            Assert.Equal(1, type.WithSingleParam.TestInjectedClass.TestNum);
            Assert.Equal(2, type.WithSingleParam.TestInjectedClass.TestNum2);
            Assert.Equal("1", type.WithSingleParam.TestInjectedClass.TestStr);
            Assert.Equal("2", type.WithSingleParam.TestInjectedClass.TestStr2);

            Assert.Equal(type.WithoutParam, type.WithSingleParam.TestInjectedClass);
        }

        [Fact]
        public void TestInjectProperty_Ok()
        {
            AHDI di = new AHDI();

            di.SetRequestedObject<ClassWithPropertyInjection>();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();
            di.SetSingletonObject<ClassWithSingleParameterInConstructor>();

            ClassWithPropertyInjection type = di.GetObject<ClassWithPropertyInjection>();

            Assert.Equal(1, type.TestPropertyInjection.TestInjectedClass.TestNum);
            Assert.Equal("1", type.TestPropertyInjection.TestInjectedClass.TestStr);
        }

        [Fact]
        public void TestInjectField_Ok()
        {
            AHDI di = new AHDI();

            di.SetRequestedObject<ClassWithFieldInjection>();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();
            di.SetSingletonObject<ClassWithSingleParameterInConstructor>();

            ClassWithFieldInjection type = di.GetObject<ClassWithFieldInjection>();

            Assert.Equal(1, type.TestPropertyInjection.TestInjectedClass.TestNum);
            Assert.Equal("1", type.TestPropertyInjection.TestInjectedClass.TestStr);
        }
    }
}