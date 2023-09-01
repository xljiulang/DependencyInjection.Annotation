using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 表示选项绑定配置的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class OptionsAttribute : Attribute
    {
        /// <summary>
        /// 标记该选项类型绑定到IConfiguration的同名section
        /// </summary>
        public OptionsAttribute()
        {
        }

        /// <summary>
        /// 标记该选项类型绑定到IConfiguration的指定section
        /// </summary>
        /// <param name="section">配置的section名</param>
        public OptionsAttribute(string section)
        {
        }
    }
}
