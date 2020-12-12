using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AnthillDI_DotNet.Attribute;
using AnthillDI_DotNet.Exceptions;

namespace AnthillDI_DotNet
{
    public class AHDI
    {
        private readonly Dictionary<Type, Func<object>> _requestedInjectedObjects = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, SingletonContainer> _singletonInjectedObject = new Dictionary<Type, SingletonContainer>();

        private object GetObject(Type type)
        {
            if (_requestedInjectedObjects.ContainsKey(type))
            {
                return _requestedInjectedObjects[type].Invoke();
            }

            if (_singletonInjectedObject.ContainsKey(type))
            {
                if (_singletonInjectedObject[type].Type is null)
                {
                    _singletonInjectedObject[type].Type =
                        _singletonInjectedObject[type].InitializationFunction.Invoke();

                    _singletonInjectedObject[type].InitializationFunction = null;
                }

                return _singletonInjectedObject[type].Type;
            }

            throw new UndefinedTypeException($"We cant find requested type. Type [{type.FullName}]");
        }

        public TType GetObject<TType>() where TType : class => GetObject(typeof(TType)) as TType;

        public void SetRequestedObject<TType>() where TType : class => SetRequestedObject<TType,TType>();

        public void SetRequestedObject<TInterface, TType>() where TType : class
        {
            if (VerifyExistence<TInterface>())
            {
                throw new DuplicateInjectionException($"Type [{typeof(TType).FullName}] yet exist");
            }

            _requestedInjectedObjects.Add(typeof(TInterface), () => InitializeType<TType>() as object);
        }

        public void SetRequestedObject<TType>(Func<AHDI, TType> initializer) where TType : class
        {
            if (VerifyExistence<TType>())
            {
                throw new DuplicateInjectionException($"Type [{typeof(TType).FullName}] yet exist");
            }

            _requestedInjectedObjects.Add(typeof(TType), () => initializer(this) as object);
        }

        public void SetSingletonObject<TType>() where TType : class => SetSingletonObject<TType, TType>();

        public void SetSingletonObject<TInterface, TType>() where TType : class
        {
            if (VerifyExistence<TType>())
            {
                throw new DuplicateInjectionException($"Type [{typeof(TType).FullName}] yet exist");
            }

            _singletonInjectedObject.Add(typeof(TInterface),
                new SingletonContainer(null, () => InitializeType<TType>() as object));
        }

        public void SetSingletonObject<TType>(Func<AHDI, TType> initializer) where TType : class
        {
            if (VerifyExistence<TType>())
            {
                throw new DuplicateInjectionException($"Type [{typeof(TType).FullName}] yet exist");
            }

            _singletonInjectedObject.Add(typeof(TType),
                new SingletonContainer(null, () => initializer.Invoke(this) as object));
        }

        public bool VerifyExistence<TType>() => VerifyExistence(typeof(TType));
        private bool VerifyExistence(Type type) => VerifyExistenceAsSingleton(type) || VerifyExistenceAsRequested(type);

        public bool VerifyExistenceAsSingleton<TType>() => VerifyExistenceAsSingleton(typeof(TType));
        private bool VerifyExistenceAsSingleton(Type type) => _singletonInjectedObject.ContainsKey(type);

        public bool VerifyExistenceAsRequested<TType>() => VerifyExistenceAsRequested(typeof(TType));
        private bool VerifyExistenceAsRequested(Type type) => _requestedInjectedObjects.ContainsKey(type);

        private TType InitializeType<TType>() where TType : class
        {
            Type type = typeof(TType);
            ConstructorInfo[] constructors = type.GetConstructors();

            IEnumerable<ConstructorInfo> markedCtr
                = constructors.Where(ctr => ctr.GetCustomAttributes<DIConstructorAttribute>().Any());

            TType injectedObject = null;

            if (!markedCtr.Any())
            {
                if (!constructors.Any(ctr => !ctr.GetParameters().Any()))
                    throw new UnknownConstructorException(
                        $"Constructor attribute not defined. Type [{typeof(TType).FullName}]");

                injectedObject = Activator.CreateInstance(typeof(TType)) as TType;
            }
            else
            {
                IEnumerable<ConstructorInfo> suitableCtr = markedCtr.Where(ctr
                    => ctr.GetParameters().All(param
                        => VerifyExistence(param.ParameterType)));

                if (!suitableCtr.Any())
                    throw new UnknownConstructorException(
                        $"We cant initialize any of constructor. Type [{typeof(TType).FullName}]");

                if (suitableCtr.Count() > 1)
                    throw new UnknownConstructorException(
                        $"We cant determine the correct constructor. Type [{typeof(TType).FullName}]");

                injectedObject = Activator.CreateInstance(typeof(TType),
                        suitableCtr.Single()
                            .GetParameters()
                            .Select(param => GetObject(param.ParameterType)).ToArray())
                    as TType;
            }

            FillProperty(ref injectedObject);
            FillField(ref injectedObject);

            return injectedObject;
        }

        private void FillProperty<TType>(ref TType injectedObject)
        {
            IEnumerable<PropertyInfo> properties = typeof(TType).GetProperties().Where(property => property.GetCustomAttributes<DIPropertyAttribute>().Any());

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(injectedObject, GetObject(property.PropertyType));
            }
        }

        private void FillField<TType>(ref TType injectedObject)
        {
            IEnumerable<FieldInfo> fields = typeof(TType).GetFields().Where(field => field.GetCustomAttributes<DIFieldAttribute>().Any());

            foreach (FieldInfo field in fields)
            {
                field.SetValue(injectedObject, GetObject(field.FieldType));
            }
        }
    }
}
