using AnthillDI_DotNet.Exceptions;
using AnthillDI_DotNet.Tests.TestClasses;
using Xunit;

namespace AnthillDI_DotNet.Tests
{
    public class AnthillDISingletonLifetimeTests
    {
        [Fact]
        public void TestInjectWithoutParamsInConstructor_Ok()
        {
            AHDI di = new AHDI();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();

            ClassWithoutParamsInConstructor type = di.GetObject<ClassWithoutParamsInConstructor>();

            Assert.Equal(1, type.TestNum);
            Assert.Equal(2, type.TestNum2);
            Assert.Equal("1", type.TestStr);
            Assert.Equal("2", type.TestStr2);
        }

        [Fact]
        public void TestInjectWithSingleParameterInConstructor_Ok()
        {
            AHDI di = new AHDI();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();
            di.SetRequestedObject<ClassWithSingleParameterInConstructor>();

            ClassWithSingleParameterInConstructor type = di.GetObject<ClassWithSingleParameterInConstructor>();

            Assert.Equal(1, type.TestInjectedClass.TestNum);
            Assert.Equal(2, type.TestInjectedClass.TestNum2);
            Assert.Equal("1", type.TestInjectedClass.TestStr);
            Assert.Equal("2", type.TestInjectedClass.TestStr2);
        }

        [Fact]
        public void TestInjectWithNestedInjectionInConstructor_Ok()
        {
            AHDI di = new AHDI();
            di.SetRequestedObject<ClassWithoutParamsInConstructor>();
            di.SetSingletonObject<ClassWithMultipleParameterInConstructor>();
            di.SetRequestedObject<ClassWithSingleParameterInConstructor>();

            ClassWithMultipleParameterInConstructor type = di.GetObject<ClassWithMultipleParameterInConstructor>();

            Assert.Equal(1, type.WithoutParam.TestNum);
            Assert.Equal(2, type.WithoutParam.TestNum2);
            Assert.Equal("1", type.WithoutParam.TestStr);
            Assert.Equal("2", type.WithoutParam.TestStr2);
            Assert.Equal(1, type.WithSingleParam.TestInjectedClass.TestNum);
            Assert.Equal(2, type.WithSingleParam.TestInjectedClass.TestNum2);
            Assert.Equal("1", type.WithSingleParam.TestInjectedClass.TestStr);
            Assert.Equal("2", type.WithSingleParam.TestInjectedClass.TestStr2);
        }

        [Fact]
        public void TestInjectSecondTime_Exception()
        {
            AHDI di = new AHDI();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();
            di.SetSingletonObject<ClassWithSingleParameterInConstructor>();
            di.SetSingletonObject<ClassWithMultipleParameterInConstructor>();

            Assert.Throws<DuplicateInjectionException>(() =>
                di.SetRequestedObject<ClassWithoutParamsInConstructor>());
            Assert.Throws<DuplicateInjectionException>(() =>
                di.SetRequestedObject<ClassWithSingleParameterInConstructor>());
            Assert.Throws<DuplicateInjectionException>(() =>
                di.SetRequestedObject<ClassWithMultipleParameterInConstructor>());
        }

        [Fact]
        public void TestInjectCheckExistence_Ok()
        {
            AHDI di = new AHDI();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();
            di.SetSingletonObject<ClassWithSingleParameterInConstructor>();

            Assert.True(di.VerifyExistenceAsSingleton<ClassWithoutParamsInConstructor>());
            Assert.True(di.VerifyExistenceAsSingleton<ClassWithSingleParameterInConstructor>());
            Assert.False(di.VerifyExistenceAsSingleton<ClassWithMultipleParameterInConstructor>());
        }

        [Fact]
        public void TestInjectCheckConstructorWithUnregisteredParameter_Exception()
        {
            AHDI di = new AHDI();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();
            di.SetSingletonObject<ClassWithMultipleParameterInConstructor>();

            Assert.Throws<UnknownConstructorException>(() => di.GetObject<ClassWithMultipleParameterInConstructor>());
        }

        [Fact]
        public void TestInjectTypeUnregistered_Exception()
        {
            AHDI di = new AHDI();

            Assert.Throws<UndefinedTypeException>(() => di.GetObject<ClassWithMultipleParameterInConstructor>());
        }

        [Fact]
        public void TestInjectCheckSingletonLifeTime_Ok()
        {
            AHDI di = new AHDI();
            di.SetSingletonObject<ClassWithoutParamsInConstructor>();

            ClassWithoutParamsInConstructor type = di.GetObject<ClassWithoutParamsInConstructor>();
            type.TestNum = 99;
            type.TestStr = "99";

            ClassWithoutParamsInConstructor type2 = di.GetObject<ClassWithoutParamsInConstructor>();

            Assert.Equal(99, type2.TestNum);
            Assert.Equal("99", type2.TestStr);
            Assert.Equal(type, type2);
        }
    }
}