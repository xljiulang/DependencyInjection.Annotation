using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DependencyInjection.Annotation.Test;

[Options]
public class Test1Options
{
    public int Age { get; set; }
}

[Options("Test2")]
public record Test2Options
{
    public int Age { get; set; }
}

public class OptionsUnitTest
{
    [Fact]
    public void OpionsBindTest()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string,string?>("Test1Options:Age","10"),
                new KeyValuePair<string,string?>("Test2:Age","20")
            }).Build();

        services.AddDependencyInjectionAnnotationTestOptions(configuration);
        var root = services.BuildServiceProvider();
        var test1Options = root.GetRequiredService<IOptions<Test1Options>>().Value;
        var test2Options = root.GetRequiredService<IOptions<Test2Options>>().Value;

        Assert.Equal(10, test1Options.Age);
        Assert.Equal(20, test2Options.Age);
    }
}
