using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace DependencyInjection.Annotation.SourceGenerator
{
    sealed class ServiceSyntaxReceiver : ISyntaxReceiver
    {
        private readonly List<ClassDeclarationSyntax> classSyntaxList = new();
        private static readonly string serviceAttributeTypeName = "Microsoft.Extensions.DependencyInjection.ServiceAttribute";

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax syntax)
            {
                this.classSyntaxList.Add(syntax);
            }
        }

        /// <summary>
        /// 获取服务描述
        /// </summary>
        /// <param name="compilation"></param>
        /// <returns></returns>
        public IEnumerable<ServiceDescriptor> GetServiceDescriptors(Compilation compilation)
        {
            var serviceAttributeClass = compilation.GetTypeByMetadataName(serviceAttributeTypeName);
            if (serviceAttributeClass == null)
            {
                yield break;
            }

            foreach (var syntax in this.classSyntaxList)
            {
                var symbol = compilation.GetSemanticModel(syntax.SyntaxTree).GetDeclaredSymbol(syntax);
                if (symbol is ITypeSymbol @class)
                {
                    foreach (var descriptor in GetServiceDescriptors(@class, serviceAttributeClass))
                    {
                        yield return descriptor;
                    }
                }
            }
        }

        private static IEnumerable<ServiceDescriptor> GetServiceDescriptors(ITypeSymbol @class, INamedTypeSymbol serviceAttributeClass)
        {
            foreach (var attr in @class.GetAttributes())
            {
                var attrClass = attr.AttributeClass;
                if (attrClass != null && attrClass.Equals(serviceAttributeClass, SymbolEqualityComparer.Default))
                {
                    var args = attr.ConstructorArguments;
                    if (args.Length > 0 &&
                        args[0].Kind == TypedConstantKind.Enum &&
                        args[0].Value is int intValue &&
                        Enum.IsDefined(typeof(ServiceLifetime), intValue))
                    {
                        var lifetime = (ServiceLifetime)intValue;
                        var descriptor = new ServiceDescriptor(lifetime, new TypeSymbol(@class));

                        for (var i = 1; i < args.Length; i++)
                        {
                            if (args[i].Value is ITypeSymbol serviceType)
                            {
                                descriptor.ServiceTypes.Add(new TypeSymbol(serviceType));
                            }
                        }
                        yield return descriptor;
                    }
                }
            }
        }
    }
}
