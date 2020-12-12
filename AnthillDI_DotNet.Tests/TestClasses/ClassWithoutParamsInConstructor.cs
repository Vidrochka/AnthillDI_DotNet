namespace AnthillDI_DotNet.Tests.TestClasses
{
    public class ClassWithoutParamsInConstructor
    {
        public int TestNum;
        public int TestNum2 { get; set; }
        public string TestStr;
        public string TestStr2 { get; set; }

        public ClassWithoutParamsInConstructor()
        {
            TestNum = 1;
            TestNum2 = 2;
            TestStr = "1";
            TestStr2 = "2";
        }
    }
}