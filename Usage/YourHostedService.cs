using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Usage
{
    // 注册为IHostedService
    [Service(ServiceLifetime.Singleton, typeof(IHostedService))]
    sealed class YourHostedService : BackgroundService
    {
        private readonly IMyService1 myService1;
        private readonly IMyService2 myService2;

        public YourHostedService(IMyService1 myService1, IMyService2 myService2)
        {
            Debug.Assert(myService1 == myService2);

            this.myService1 = myService1;
            this.myService2 = myService2;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.myService1.LogInformation("Hi, I'm MyService!");
            this.myService2.LogWarning("Hi, I'm also MyService!");
            return Task.CompletedTask;
        }
    }
}
