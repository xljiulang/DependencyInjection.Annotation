namespace DependencyInjection.Annotation.SourceGenerator
{
    sealed class OptionsDescriptor
    {
        /// <summary>
        /// 声明类型
        /// </summary>
        public TypeSymbol DeclaredType { get; }

        /// <summary>
        /// section名称
        /// </summary>
        public string Section { get; }


        public OptionsDescriptor(TypeSymbol declaredType, string section)
        {
            this.DeclaredType = declaredType;
            this.Section = section;
        }

        public string ToString(string services, string configuration)
        {
            configuration = @$"{configuration}.GetSection(""{this.Section}"")";
            return $"{services}.AddOptions<{this.DeclaredType}>().Bind({configuration}).ValidateDataAnnotations();";
        }
    }
}
