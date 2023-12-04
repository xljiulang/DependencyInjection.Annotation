using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 表示服务特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ServiceAttribute : Attribute
    {
        /// <summary>
        /// 将当前实现类型注册为服务的特性
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        public ServiceAttribute(ServiceLifetime lifetime)
        {
        }

        /// <summary>
        /// 将当前实现类型注册为指定服务的特性
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="serviceType1">注册的服务类型</param>
        /// <param name="key">注册的服务key</param>
        public ServiceAttribute(ServiceLifetime lifetime,
            Type serviceType1,
            object? key = null)
        {
        }

        /// <summary>
        /// 将当前实现类型注册为指定多个服务的特性
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="serviceType1">注册的服务类型1</param>
        /// <param name="serviceType2">注册的服务类型2</param>
        /// <param name="key">注册的服务key</param>
        public ServiceAttribute(ServiceLifetime lifetime,
            Type serviceType1,
            Type serviceType2,
            object? key = null)
        {
        }

        /// <summary>
        /// 将当前实现类型注册为指定多个服务的特性
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="serviceType1">注册的服务类型1</param>
        /// <param name="serviceType2">注册的服务类型2</param>
        /// <param name="serviceType3">注册的服务类型3</param>
        /// <param name="key">注册的服务key</param>
        public ServiceAttribute(ServiceLifetime lifetime,
            Type serviceType1,
            Type serviceType2,
            Type serviceType3,
            object? key = null)
        {
        }

        /// <summary>
        /// 将当前实现类型注册为指定多个服务的特性
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="serviceType1">注册的服务类型1</param>
        /// <param name="serviceType2">注册的服务类型2</param>
        /// <param name="serviceType3">注册的服务类型3</param>
        /// <param name="serviceType4">注册的服务类型4</param>
        /// <param name="key">注册的服务key</param>
        public ServiceAttribute(ServiceLifetime lifetime,
            Type serviceType1,
            Type serviceType2,
            Type serviceType3,
            Type serviceType4,
            object? key = null)
        {
        }

        /// <summary>
        /// 将当前实现类型注册为指定多个服务的特性
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="serviceType1">注册的服务类型1</param>
        /// <param name="serviceType2">注册的服务类型2</param>
        /// <param name="serviceType3">注册的服务类型3</param>
        /// <param name="serviceType4">注册的服务类型4</param>
        /// <param name="serviceType5">注册的服务类型5</param>
        /// <param name="key">注册的服务key</param>
        public ServiceAttribute(ServiceLifetime lifetime,
            Type serviceType1,
            Type serviceType2,
            Type serviceType3,
            Type serviceType4,
            Type serviceType5,
            object? key = null)
        {
        }

        /// <summary>
        /// 将当前实现类型注册为指定多个服务的特性
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="serviceType1">注册的服务类型1</param>
        /// <param name="serviceType2">注册的服务类型2</param>
        /// <param name="serviceType3">注册的服务类型3</param>
        /// <param name="serviceType4">注册的服务类型4</param>
        /// <param name="serviceType5">注册的服务类型5</param>
        /// <param name="serviceType6">注册的服务类型6</param>
        /// <param name="key">注册的服务key</param>
        public ServiceAttribute(ServiceLifetime lifetime,
            Type serviceType1,
            Type serviceType2,
            Type serviceType3,
            Type serviceType4,
            Type serviceType5,
            Type serviceType6,
            object? key = null)
        {
        }
    }
}
