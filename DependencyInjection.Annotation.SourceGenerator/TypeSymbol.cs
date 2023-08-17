using Microsoft.CodeAnalysis;
using System;

namespace DependencyInjection.Annotation.SourceGenerator
{
    sealed class TypeSymbol : IEquatable<TypeSymbol>
    {
        private readonly ITypeSymbol typeSymbol;

        public TypeSymbol(ITypeSymbol typeSymbol)
        {
            this.typeSymbol = typeSymbol;
        }

        public bool Equals(TypeSymbol? other)
        {
            return other != null && this.typeSymbol.Equals(other.typeSymbol, SymbolEqualityComparer.Default);
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as TypeSymbol);
        }

        public override int GetHashCode()
        {
            return this.typeSymbol.GetHashCode();
        }

        public override string ToString()
        {
            return this.typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }
    }
}
