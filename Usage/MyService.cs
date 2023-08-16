using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Usage
{
    /// <summary>
    /// 注册为IMyService1和IMyService2
    /// </summary>
    [Service(ServiceLifetime.Singleton, typeof(IMyService1), typeof(IMyService2))]
    sealed class MyService : IMyService1, IMyService2
    {
        private readonly ILogger<MyService> logger;

        public MyService(ILogger<MyService> logger)
        {
            this.logger = logger;
        }

        public void LogInformation(string value)
        {
            this.logger.LogInformation(value);
        }

        public void LogWarning(string value)
        {
            this.logger.LogWarning(value);
        }
    }
}
