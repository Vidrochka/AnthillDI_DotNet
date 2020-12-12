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

        // ��� ������� ������������� singleton, � ������ ��������� ����� �������� �� SetRequestedObject
        di.SetSingletonObject<A>();

        di.SetRequestedObject<B>();
        di.SetRequestedObject<C>();
        di.SetRequestedObject<D>();

        // d ����� ������������������ �, � �������� B, � B �������� A
        D d = di.GetObject<D>();
    }
}
```
