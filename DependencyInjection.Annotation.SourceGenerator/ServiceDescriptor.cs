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
        public TypeSymbol DeclaredType { get; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public HashSet<TypeSymbol> ServiceTypes { get; } = new HashSet<TypeSymbol>();

        /// <summary>
        /// 服务描述
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="declaredType">声明类型</param>
        public ServiceDescriptor(ServiceLifetime lifetime, TypeSymbol declaredType)
        {
            this.Lifetime = lifetime;
            this.DeclaredType = declaredType;
        }
    }
}
