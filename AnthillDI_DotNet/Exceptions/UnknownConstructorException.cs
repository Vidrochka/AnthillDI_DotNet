namespace AnthillDI_DotNet.Exceptions
{
    public class UnknownConstructorException : System.Exception
    {
        public UnknownConstructorException(string message) : base(message)
        { }
    }
}