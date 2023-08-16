using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Usage
{
    class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {  
                    services.AddUsage();
                })
                .Build()
                .Run();
        }

    }
}
