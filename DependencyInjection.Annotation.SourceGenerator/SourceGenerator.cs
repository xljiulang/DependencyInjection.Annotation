using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DependencyInjection.Annotation.SourceGenerator
{
    [Generator]
    public sealed class SourceGenerator : ISourceGenerator
    {      
        private static readonly string fileName = "ServiceCollectionExtensions.g.cs";
        private static readonly string className = "ServiceCollectionExtensions_G";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(GeneratorExecutionContext context)
        {
            //  System.Diagnostics.Debugger.Launch();

            if (context.SyntaxReceiver is SyntaxReceiver receiver)
            {
                var assemblyName = GetAssemblyName(context.Compilation);
                var code = GenerateCode(receiver, context.Compilation, assemblyName);
                context.AddSource(fileName, code);
            }
        }

        private static string GetAssemblyName(Compilation compilation)
        {
            var assemblyName = compilation.AssemblyName ?? string.Empty;
            return new string(assemblyName.Where(IsAllowChar).ToArray());

            static bool IsAllowChar(char c)
            {
                return ('0' <= c && c <= '9') || ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
            }
        }

        private static string GenerateCode(SyntaxReceiver receiver, Compilation compilation, string assemblyName)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using Microsoft.Extensions.Configuration;");
            builder.AppendLine("namespace Microsoft.Extensions.DependencyInjection");
            builder.AppendLine("{");
            builder.AppendLine("    /// <summary>IServiceCollection扩展</summary>");
            builder.AppendLine($"    public static partial class {className}");
            builder.AppendLine("    {");

            var serviceDescriptors = receiver.GetServiceDescriptors(compilation).ToArray();
            if (serviceDescriptors.Length > 0)
            {
                builder.AppendLine($"""
                        /// <summary>
                        /// 将程序集{compilation.AssemblyName}的所有ServiceAttribute标记类型注册到DI
                        /// </summary>
                        /// <param name="services"></param>
                        /// <returns></returns>
                """);
                builder.AppendLine($"        public static IServiceCollection Add{assemblyName}(this IServiceCollection services)");
                builder.AppendLine("        {");


                foreach (var descriptor in serviceDescriptors)
                {
                    if (descriptor.ServiceTypes.Count == 1)
                    {
                        var serviceType = descriptor.ServiceTypes.First();
                        builder.AppendLine($"            services.Add(ServiceDescriptor.Describe(typeof({serviceType}), typeof({descriptor.DeclaredType}), ServiceLifetime.{descriptor.Lifetime}));");
                    }
                    else
                    {
                        builder.AppendLine($"            services.Add(ServiceDescriptor.Describe(typeof({descriptor.DeclaredType}), typeof({descriptor.DeclaredType}), ServiceLifetime.{descriptor.Lifetime}));");
                        foreach (var serviceType in descriptor.ServiceTypes)
                        {
                            if (serviceType.Equals(descriptor.DeclaredType) == false)
                            {
                                builder.AppendLine($"            services.Add(ServiceDescriptor.Describe(typeof({serviceType}), serviceProvider => serviceProvider.GetRequiredService<{descriptor.DeclaredType}>(), ServiceLifetime.{descriptor.Lifetime}));");
                            }
                        }
                    }
                }

                builder.AppendLine("            return services;");
                builder.AppendLine("        }");
            }

            var optionsDescriptors = receiver.GetOptionsDescriptors(compilation).ToArray();
            if (optionsDescriptors.Length > 0)
            {
                builder.AppendLine($"""
                        /// <summary>
                        /// 将程序集{compilation.AssemblyName}的所有OptionsAttribute标记类型绑定到配置
                        /// </summary>
                        /// <param name="services"></param>
                        /// <param name="configuration"></param>
                        /// <returns></returns>
                """);
                builder.AppendLine($"        public static IServiceCollection Add{assemblyName}Options(this IServiceCollection services, IConfiguration configuration)");
                builder.AppendLine("        {");
                foreach (var descriptor in optionsDescriptors)
                {
                    builder.AppendLine($"            {descriptor.ToString("services", "configuration")}");
                }

                builder.AppendLine("            return services;");
                builder.AppendLine("        }");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}
