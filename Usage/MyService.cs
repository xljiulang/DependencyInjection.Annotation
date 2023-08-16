using Microsoft.Extensions.DependencyInjection;

namespace Usage
{
    [Service(ServiceLifetime.Singleton, typeof(IMyService))]
    class MyService
    {
    }
}
