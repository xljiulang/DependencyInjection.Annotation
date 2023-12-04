using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyInjection.Annotation.SourceGenerator
{
    sealed class SyntaxReceiver : ISyntaxReceiver
    {
        private readonly List<TypeDeclarationSyntax> typeSyntaxList = new();
        private static readonly string serviceAttributeTypeName = "Microsoft.Extensions.DependencyInjection.ServiceAttribute";
        private static readonly string optionsAttributeTypeName = "Microsoft.Extensions.DependencyInjection.OptionsAttribute";

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax)
            {
                this.typeSyntaxList.Add(classSyntax);
            }
            else if (syntaxNode is RecordDeclarationSyntax recordSyntax)
            {
                this.typeSyntaxList.Add(recordSyntax);
            }
        }

        /// <summary>
        /// 获取Options描述
        /// </summary>
        /// <param name="compilation"></param>
        /// <returns></returns>
        public IEnumerable<OptionsDescriptor> GetOptionsDescriptors(Compilation compilation)
        {
            var optionsAttributeClass = compilation.GetTypeByMetadataName(optionsAttributeTypeName);
            if (optionsAttributeClass == null)
            {
                yield break;
            }

            foreach (var syntax in this.typeSyntaxList)
            {
                var symbol = compilation.GetSemanticModel(syntax.SyntaxTree).GetDeclaredSymbol(syntax);
                if (symbol is ITypeSymbol @class)
                {
                    foreach (var descriptor in GetOptionsDescriptors(@class, optionsAttributeClass))
                    {
                        yield return descriptor;
                    }
                }
            }
        }

        private static IEnumerable<OptionsDescriptor> GetOptionsDescriptors(ITypeSymbol @class, INamedTypeSymbol optionsAttributeClass)
        {
            foreach (var attr in @class.GetAttributes())
            {
                var attrClass = attr.AttributeClass;
                if (attrClass != null && attrClass.Equals(optionsAttributeClass, SymbolEqualityComparer.Default))
                {
                    var args = attr.ConstructorArguments;
                    var declaredType = new TypeSymbol(@class);

                    yield return args.Length > 0 && args[0].Value is string section
                        ? new OptionsDescriptor(declaredType, section)
                        : new OptionsDescriptor(declaredType, @class.Name);
                }
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

            foreach (var syntax in this.typeSyntaxList)
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
                        ServiceDescriptor descriptor;
                        if (args.Length > 1)
                        {
                            descriptor = new ServiceDescriptor(lifetime, new TypeSymbol(@class), GetKeyString(args.Last()));
                            for (var i = 1; i < args.Length - 1; i++)
                            {
                                if (args[i].Value is ITypeSymbol serviceType)
                                {
                                    descriptor.ServiceTypes.Add(new TypeSymbol(serviceType));
                                }
                            }
                        }
                        else
                        {
                            descriptor = new ServiceDescriptor(lifetime, new TypeSymbol(@class), null);
                        }
                        yield return descriptor;
                    }
                }
            }
        }

        private static string? GetKeyString(TypedConstant keyTypedConstant)
        {
            object? value = keyTypedConstant.Value;
            if (value is null)
            {
                return null;
            }
            else if (keyTypedConstant.Kind == TypedConstantKind.Enum)
            {
                return $"global::{keyTypedConstant.ToCSharpString()}";
            }
            else if (value is string stringValue)
            {
                return $"\"{stringValue}\"";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
