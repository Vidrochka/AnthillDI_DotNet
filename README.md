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
        public int I;
        public A()
        {
            I = 10;
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

    public class E
    {
        public string EStr;

        public E(string str)
        {
            Estr = str;
        }
    }

    public class F
    {
        public string FStr;
        public D DField;

        public F(string str, D d)
        {
            Fstr = str;
            DField = d;
        }
    }

    public static void Main()
    {
        AHDI di = new AHDI()

        // Обращаю внимание что тут singleton
        di.SetSingletonObject<A>();

        di.SetRequestedObject<B>();
        di.SetRequestedObject<C>();
        di.SetRequestedObject<D>();

        // d будет проинициализирован С, С содержит B, а B содержит A
        // A содержит поле I = 10
        D d = di.GetObject<D>();

        d.CField.BField.AField.I = 99

        // Новый объект D будет проинициализирован новым С, навый С содержит новый B, а новый B содержит старый A, т.к. A - singleton
        // A содержит поле I = 99 (поменяли выше)
        D d2 = di.GetObject<D>();

        di.SetRequestedObject<E>((context) => new E("test"));

        //e содержит поле EStr = "test"
        E e = di.GetObject<E>();

        di.SetRequestedObject<F>((context) => new F("test2", context.GetObject<D>()));

        // f будет содержать новый объект D, собранный из DI контекста
        F f = di.GetObject<F>();
    }
}
```
