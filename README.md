# DependencyInjection.Annotation
基于注解的服务自动查找与注册的DI扩展。使用SourceGenerator无反射，完美支持AOT。

### 食用步骤
#### 1 nuget引用 
```xml
<PackageReference Include="DependencyInjection.Annotation" Version="1.1.2" />
```

#### 2 服务标记
```c#
[Service(ServiceLifetime.Singleton)]
class MyService 
{    
}
```

```c#
[Service(ServiceLifetime.Singleton, typeof(IMyService1))]
class MyService : IMyService1
{    
}
```

```c#
[Service(ServiceLifetime.Singleton, typeof(IMyService1), typeof(IMyService2))]
class MyService : IMyService1, IMyService2, IDisposable
{    
}
```

#### 3 服务注册
```c#
builder.Services.Add{AssemblyName}();
```
其中{AssemblyName}为包含服务的程序集名。
