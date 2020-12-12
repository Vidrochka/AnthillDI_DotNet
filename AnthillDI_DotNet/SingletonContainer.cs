using System;

namespace AnthillDI_DotNet
{
    internal class SingletonContainer
    {
        internal object Type;
        internal Func<object> InitializationFunction;

        internal SingletonContainer(object type, Func<object> initializationFunction)
        {
            Type = type;
            InitializationFunction = initializationFunction;
        }
    }
}