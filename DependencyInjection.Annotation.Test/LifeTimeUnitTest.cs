using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Annotation.Test
{
    public class LifeTimeUnitTest
    {
        [Service(ServiceLifetime.Scoped)]
        public class ScopedService
        {
        }

        [Service(ServiceLifetime.Singleton)]
        public class SingletonService
        {
        }

        [Service(ServiceLifetime.Transient)]
        public class TransientService
        {
        }

        [Fact]
        public void ScopedTest()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjectionAnnotationTest();
            var root = services.BuildServiceProvider();

            using var scope1 = root.CreateScope();
            var instance11 = scope1.ServiceProvider.GetRequiredService<ScopedService>();
            var instance12 = scope1.ServiceProvider.GetRequiredService<ScopedService>();

            using var scope2 = root.CreateScope();
            var instance21 = scope2.ServiceProvider.GetRequiredService<ScopedService>();
            var instance22 = scope2.ServiceProvider.GetRequiredService<ScopedService>();


            Assert.Equal(instance11, instance12);
            Assert.Equal(instance21, instance22);
            Assert.NotEqual(instance11, instance21);
        }


        [Fact]
        public void SingletonTest()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjectionAnnotationTest();
            var root = services.BuildServiceProvider();

            using var scope1 = root.CreateScope();
            var instance11 = scope1.ServiceProvider.GetRequiredService<SingletonService>();
            var instance12 = scope1.ServiceProvider.GetRequiredService<SingletonService>();

            using var scope2 = root.CreateScope();
            var instance21 = scope2.ServiceProvider.GetRequiredService<SingletonService>();
            var instance22 = scope2.ServiceProvider.GetRequiredService<SingletonService>();


            Assert.Equal(instance11, instance12);
            Assert.Equal(instance21, instance22);
            Assert.Equal(instance11, instance21);
        }

        [Fact]
        public void TransientTest()
        {
            var services = new ServiceCollection();
            services.AddDependencyInjectionAnnotationTest();
            var root = services.BuildServiceProvider();

            using var scope1 = root.CreateScope();
            var instance11 = scope1.ServiceProvider.GetRequiredService<TransientService>();
            var instance12 = scope1.ServiceProvider.GetRequiredService<TransientService>();

            using var scope2 = root.CreateScope();
            var instance21 = scope2.ServiceProvider.GetRequiredService<TransientService>();
            var instance22 = scope2.ServiceProvider.GetRequiredService<TransientService>();


            Assert.NotEqual(instance11, instance12);
            Assert.NotEqual(instance21, instance22);
            Assert.NotEqual(instance11, instance21);
        }
    }
}