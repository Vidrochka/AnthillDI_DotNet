# AnthillDI_DotNet
Simple .net dependency injection

### ������� �������� DI:
1. ���������� ���������� AnthillDI_DotNet
2. ������� ������ DI - AHDI
3. �������� � �������� DI ����������� ����
4. ������������ �������� DI ��� ��������������� ����������� ��������

### ������������� ��������� ������
1. ������ ������ ������� - ����� ������
* void SetRequestedObject<TType>()
* void SetRequestedObject<TInterface, TType>()
* void SetRequestedObject<TType>(Func<AHDI, TType> initializer)

2. ����� singleton ������
* void SetSingletonObject<TType>()
* void SetSingletonObject<TInterface, TType>()
* void SetSingletonObject<TType>(Func<AHDI, TType> initializer)

- Singleton ��������� ��� ������ �������. Requested ��� ������ �������.<br/>
�.�. ������� ����� ��������� � ����� ������������������ �� �� ������

- ����� ������� � 1 generic-���������� ��������� ������ ��� ��� TType � ������� �������� ��� ��� ��� TType
- ����� ������� � 2 generic-����������� ��������� ������ ��� ��� TInterface � ������� �������� ��� ��� ��� TInterface
- ����� ������� � 1 generic-���������� � 1 ���������� Func<AHDI, TType> ��������� ������ ��� ��� TType,<br/>
������� ������ ������� ���������� ������� � ������� �������� ��� ��� ��� TType

### ������������� �����
* TType GetObject<TType>() - ����� ������� ��� ��������������� ������������������ �����

### �������������� �������
* bool VerifyExistence<TType>() - ��������� ������� ������������������� ���� TType ��� Singleton ��� � Requested
* bool VerifyExistenceAsSingleton<TType>() - ��������� ������� ������������������� ���� TType ��� Singleton
* bool VerifyExistenceAsRequested<TType>() - ��������� ������� ������������������� ���� TType ��� Requested

### ��� �������� ���������� ��������� �������� (AnthillDI_DotNet.Attribute)
* DIConstructorAttribute - �� ������������ ��������� �������� (���� ���, �� ����������� ��� ����������)
* DIFieldAttribute - �� ���� ��������� ��������
* DIPropertyAttribute - �� �������� ��������� ��������

### ������

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

        // ������� �������� ��� ��� singleton
        di.SetSingletonObject<A>();

        di.SetRequestedObject<B>();
        di.SetRequestedObject<C>();
        di.SetRequestedObject<D>();

        // d ����� ������������������ �, � �������� B, � B �������� A
        // A �������� ���� I = 10
        D d = di.GetObject<D>();

        d.CField.BField.AField.I = 99

        // ����� ������ D ����� ������������������ ����� �, ����� � �������� ����� B, � ����� B �������� ������ A, �.�. A - singleton
        // A �������� ���� I = 99 (�������� ����)
        D d2 = di.GetObject<D>();

        di.SetRequestedObject<E>((context) => new E("test"));

        //e �������� ���� EStr = "test"
        E e = di.GetObject<E>();

        di.SetRequestedObject<F>((context) => new F("test2", context.GetObject<D>()));

        // f ����� ��������� ����� ������ D, ��������� �� DI ���������
        F f = di.GetObject<F>();
    }
}
```
