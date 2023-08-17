using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Annotation.Test
{
    public class ServiceUnitTest
    {
        public interface IOneService { }

        [Service(ServiceLifetime.Singleton, typeof(IOneService))]
        public class OneService : IOneService
        {
        }

        public interface ITwoService1 { }
        public interface ITwoService2 { }

        [Service(ServiceLifetime.Singleton, typeof(ITwoService1), typeof(ITwoService2))]
        public class TwoService : ITwoService1, ITwoService2
        {
        }

        [Fact]
        public void OneServiceTest()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjectionAnnotationTest();
            var root = services.BuildServiceProvider();

            var service1 = root.GetService<OneService>();
            var service2 = root.GetService<IOneService>();

            Assert.Null(service1);
            Assert.NotNull(service2);
        }


        [Fact]
        public void TwoServiceTest()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjectionAnnotationTest();
            var root = services.BuildServiceProvider();

            var service1 = root.GetService<ITwoService1>();
            var service2 = root.GetService<ITwoService2>();

            Assert.NotNull(service1);
            Assert.True(service1 == service2);
        }
    }
}