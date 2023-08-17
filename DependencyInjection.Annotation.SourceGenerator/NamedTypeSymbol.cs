using Microsoft.CodeAnalysis;
using System;

namespace DependencyInjection.Annotation.SourceGenerator
{
    sealed class NamedTypeSymbol : IEquatable<NamedTypeSymbol>
    {
        private readonly INamedTypeSymbol typeSymbol;

        public NamedTypeSymbol(INamedTypeSymbol typeSymbol)
        {
            this.typeSymbol = typeSymbol;
        }

        public bool Equals(NamedTypeSymbol? other)
        {
            return other != null && this.typeSymbol.Equals(other.typeSymbol, SymbolEqualityComparer.Default);
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as NamedTypeSymbol);
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
