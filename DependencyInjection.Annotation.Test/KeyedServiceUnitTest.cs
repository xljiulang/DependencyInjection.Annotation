using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Annotation.Test;

public class KeyedServiceUnitTest
{

    private const string keyOne = "one";
    private const string keyTwo = "two";

    [Service(ServiceLifetime.Singleton, typeof(IKeyedServices), keyOne)]
    public class OneKeyService : IKeyedServices
    {
        public string Message { get; } = keyOne;
    }

    [Service(ServiceLifetime.Singleton, typeof(IKeyedServices), keyTwo)]
    public class TwoKeyService : IKeyedServices
    {
        public string Message { get; } = keyTwo;
    }

    [Service(ServiceLifetime.Singleton, typeof(IEnumKeyedServices), typeof(IKeyedServices), EnumKey.One)]
    public class OneEnumKeyService : IEnumKeyedServices, IKeyedServices
    {
        public EnumKey Key { get; } = EnumKey.One;
        public string Message { get; } = keyOne;
    }

    [Service(ServiceLifetime.Singleton, typeof(IEnumKeyedServices), typeof(IKeyedServices), EnumKey.Two)]
    public class TwoEnumKeyService : IEnumKeyedServices, IKeyedServices
    {
        public EnumKey Key { get; } = EnumKey.Two;
        public string Message { get; } = keyOne;
    }

    [Service(ServiceLifetime.Singleton, typeof(IEnumKeyedServices), typeof(IKeyedServices), EnumKey.Three)]
    public class ThreeEnumKeyService : IEnumKeyedServices, IKeyedServices
    {
        public EnumKey Key { get; } = EnumKey.Three;
        public string Message { get; } = keyOne;
    }

    [Fact]
    public void KeyedServiceTest()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjectionAnnotationTest();
        var root = services.BuildServiceProvider();

        var service1 = root.GetKeyedService<IKeyedServices>(keyOne);
        var service2 = root.GetKeyedService<IKeyedServices>(keyTwo);

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Equal(keyOne, service1.Message);
        Assert.Equal(keyTwo, service2.Message);
    }

    [Fact]
    public void EnumKeyedServiceTest()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjectionAnnotationTest();
        var root = services.BuildServiceProvider();
        var service1 = root.GetKeyedService<IEnumKeyedServices>(EnumKey.One);
        var service2 = root.GetKeyedService<IEnumKeyedServices>(EnumKey.Two);
        var service3 = root.GetKeyedService<IEnumKeyedServices>(EnumKey.Three);
        var service4 = root.GetKeyedService<IKeyedServices>(EnumKey.One);
        var service5 = root.GetKeyedService<IKeyedServices>(EnumKey.Two);
        var service6 = root.GetKeyedService<IKeyedServices>(EnumKey.Three);

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.NotNull(service3);
        Assert.NotNull(service4);
        Assert.NotNull(service5);
        Assert.NotNull(service6);
        Assert.Equal(EnumKey.One, service1.Key);
        Assert.Equal(EnumKey.Two, service2.Key);
        Assert.Equal(EnumKey.Three, service3.Key);
    }
}

public enum EnumKey
{
    One,
    Two,
    Three
}

public interface IKeyedServices
{
    public string Message { get; }
}

public interface IEnumKeyedServices
{
    public EnumKey Key { get; }
}
