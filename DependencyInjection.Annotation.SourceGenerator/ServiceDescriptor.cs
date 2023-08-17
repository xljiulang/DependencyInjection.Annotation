using System.Collections.Generic;

namespace DependencyInjection.Annotation.SourceGenerator
{
    /// <summary>
    /// 服务描述
    /// </summary>
    sealed class ServiceDescriptor
    {
        /// <summary>
        /// 生命周期
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// 声明类型
        /// </summary>
        public NamedTypeSymbol DeclaredType { get; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public HashSet<NamedTypeSymbol> ServiceTypes { get; } = new HashSet<NamedTypeSymbol>();

        /// <summary>
        /// 服务描述
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="declaredType">声明类型</param>
        public ServiceDescriptor(ServiceLifetime lifetime, NamedTypeSymbol declaredType)
        {
            this.Lifetime = lifetime;
            this.DeclaredType = declaredType;
        }
    }
}
