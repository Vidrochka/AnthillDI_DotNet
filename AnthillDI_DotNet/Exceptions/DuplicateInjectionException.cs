namespace AnthillDI_DotNet.Exceptions
{
    public class DuplicateInjectionException : System.Exception
    {
        public DuplicateInjectionException(string message) : base(message)
        { }
    }
}