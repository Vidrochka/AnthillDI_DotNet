# AnthillDI_DotNet
Simple .net dependency injection

### Процесс создания DI:
1. Подключить библиотеку AnthillDI_DotNet
2. Создать объект DI - AHDI
3. Добавить в контекст DI необходимые типы
4. Использовать контекст DI для инстанцирования необходимых объектов

### Инициализация контекста типами
1. Каждый запрос объекта - новый объект
* void SetRequestedObject<TType>()
* void SetRequestedObject<TInterface, TType>()
* void SetRequestedObject<TType>(Func<AHDI, TType> initializer)

2. Общий singleton объект
* void SetSingletonObject<TType>()
* void SetSingletonObject<TInterface, TType>()
* void SetSingletonObject<TType>(Func<AHDI, TType> initializer)

- Singleton создается при первом запросе. Requested при каждом запросе.<br/>
Т.е. объекты можно добавлять в любой последовательности до их вызова

- Вызов функции с 1 generic-параметром добавляет объект как тип TType и обязует получать его как тип TType
- Вызов функции с 2 generic-параметрами добавляет объект как тип TInterface и обязует получать его как тип TInterface
- Вызов функции с 1 generic-параметром и 1 параметром Func<AHDI, TType> добавляет объект как тип TType,<br/>
создает объект вызовом переданной функции и обязует получать его как тип TType

### Генерирования типов
* TType GetObject<TType>() - общая функция для инстанцирования зарегистрированных типов

### Дополнительные функции
* bool VerifyExistence<TType>() - проверяет наличие зарегистрированного типа TType как Singleton так и Requested
* bool VerifyExistenceAsSingleton<TType>() - проверяет наличие зарегистрированного типа TType как Singleton
* bool VerifyExistenceAsRequested<TType>() - проверяет наличие зарегистрированного типа TType как Requested

### Для инъекции необходимо указывать атрибуты (AnthillDI_DotNet.Attribute)
* DIConstructorAttribute - на конструкторе требующем инъекции (если нет, то конструктор без параметров)
* DIFieldAttribute - на поле требующем инъекции
* DIPropertyAttribute - на свойстве требующем инъекции

### Пример

```C#
using AnthillDI_DotNet;
using AnthillDI_DotNet.Attribute;

namespace Test
{
    public class A
    {
        public int i;
        public A()
        {
            i = 10;
        }
    }

    public class B
    {
        public A AField;

        [DIConstructor]
        public B(A a)
        {
            AField = a;
        }
    }

    public class C
    {
        [DIField]
        public B BField;
    }

    public class D
    {
        [DIProperty]
        public C CField { get; set; }
    }

    public static void Main()
    {
        AHDI di = new AHDI()

        // Для примера использования singleton, в данном контексте можно заменить на SetRequestedObject
        di.SetSingletonObject<A>();

        di.SetRequestedObject<B>();
        di.SetRequestedObject<C>();
        di.SetRequestedObject<D>();

        // d будет проинициализирован С, С содержит B, а B содержит A
        D d = di.GetObject<D>();
    }
}
```
