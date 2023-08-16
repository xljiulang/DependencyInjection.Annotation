using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DependencyInjection.Annotation.SourceGenerator
{
    [Generator]
    public class ServiceSourceGenerator : ISourceGenerator
    {
        private static readonly string hintName = "DependencyInjection.Annotation";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ServiceSyntaxReceiver());
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(GeneratorExecutionContext context)
        {
            //  System.Diagnostics.Debugger.Launch();

            if (context.SyntaxReceiver is ServiceSyntaxReceiver receiver)
            {
                var methodName = GetMethodName(context.Compilation);
                var code = GenerateCode(receiver, context.Compilation, methodName);
                context.AddSource(hintName, code);
            }
        }

        private static string GetMethodName(Compilation compilation)
        {
            var methodName = $"Add{compilation.AssemblyName}";
            return new string(methodName.Where(IsAllowChar).ToArray());

            static bool IsAllowChar(char c)
            {
                return ('0' <= c && c <= '9') || ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
            }
        }

        private static string GenerateCode(ServiceSyntaxReceiver receiver, Compilation compilation, string methodName)
        {
            var builder = new StringBuilder();
            builder.AppendLine("namespace Microsoft.Extensions.DependencyInjection");
            builder.AppendLine("{");
            builder.AppendLine($"    public static partial class ServiceCollectionExtensions");
            builder.AppendLine("    {");

            builder.AppendLine($"""
                        /// <summary>
                        /// 将程序集{compilation.AssemblyName}的所有ServiceAttribute标记类型注册到DI
                        /// </summary>
                        /// <param name="services"></param>
                        /// <returns></returns>
                """);

            builder.AppendLine($"        public static IServiceCollection {methodName}(this IServiceCollection services)");
            builder.AppendLine("        {");

            foreach (var descriptor in receiver.GetServiceDescriptors(compilation))
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
                        if (serviceType.Equals(descriptor.DeclaredType, SymbolEqualityComparer.Default) == false)
                        {
                            builder.AppendLine($"            services.Add(ServiceDescriptor.Describe(typeof({serviceType}), serviceProvider => serviceProvider.GetRequiredService<{descriptor.DeclaredType}>(), ServiceLifetime.{descriptor.Lifetime}));");
                        }
                    }
                }
            }

            builder.AppendLine("            return services;");
            builder.AppendLine("        }");
            builder.AppendLine("    }");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}
