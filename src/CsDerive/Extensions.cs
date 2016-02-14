using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Dnx.Compilation.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CsDerive
{
    public static class Extensions
    {
        public static IEnumerable<T> OfSubtype<TSuper, T>(this IEnumerable<TSuper> source) 
            where T: TSuper
        {
            return source.OfType<T>();
        }

        public static TResult Switch<TSource, TResult>(this Optional<TSource> optional, Func<TSource, TResult> ifValue, Func<TResult> ifNoValue) {
            if (optional.HasValue)
                return ifValue(optional.Value);
            return ifNoValue();
        }

        public static T GetOrElse<T>(this Optional<T> optional, T defaultValue) {
            if (optional.HasValue)
                return optional.Value;
            return defaultValue;
        }

        public static bool IsReadOnly(this FieldDeclarationSyntax member) {
            //return member.Modifiers.Contains(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
            // TODO there has to be a better way
            return member.Modifiers.Any(x => x.Text == "readonly");
        }

        public static bool IsReadOnly(this PropertyDeclarationSyntax member) {
            return !member.AccessorList.Accessors.Any(x => x.Keyword.Equals(SyntaxFactory.Token(SyntaxKind.SetKeyword)));
        }

        public static void AddError(this BeforeCompileContext ctx, string msg) {
            var dd = new DiagnosticDescriptor(
                id: "CSDERIVE001",
                messageFormat: msg,
                title: "",
                category: "",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                description: "");

            ctx.Diagnostics.Add(Diagnostic.Create(dd, Location.None));
        }
    }
}
