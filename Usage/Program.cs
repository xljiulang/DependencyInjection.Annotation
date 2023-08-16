using Microsoft.Extensions.DependencyInjection;

namespace Usage
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddUsage();

            var root = services.BuildServiceProvider();
            var service = root.GetRequiredService<IMyService>();

            Console.WriteLine("Hello, World!");
        }

    }
}
