using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace CsDerive
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DeriveConstructorAttribute : Attribute { }

    public class DeriveConstructorProcessor : ClassAttributeProcessorCompileModule<DeriveConstructorAttribute> {
        public override ClassDeclarationSyntax ProcessClass(ClassDeclarationSyntax classDeclaration) {
            var className = classDeclaration.Identifier.ValueText;

            //context.CompileContext.AddError(className);

            var properties = classDeclaration.Members
                 .Select(m => m.WithoutTrivia())
                 .OfSubtype<MemberDeclarationSyntax, PropertyDeclarationSyntax>()
                 //.Select(m => { Debugger.Launch(); return m; })
                 .Where(m => m.IsReadOnly())
                 .Select(m => {
                     var name = m.Identifier.ValueText;
                     var type = m.Type.ToString();
                     return new { name, type };
                 });

            var fields = classDeclaration.Members
                .Select(m => m.WithoutTrivia())
                .OfSubtype<MemberDeclarationSyntax, FieldDeclarationSyntax>()
                .Where(m => m.IsReadOnly())
                .SelectMany(m => 
                    from variable in m.Declaration.Variables
                    let name = variable.Identifier.ValueText
                    let type = m.Declaration.Type.ToString()
                    select new { name, type }
                 );

            var propertiesAndFields = new[] { properties, fields }.SelectMany(x => x).ToList();

            var ctorParams = propertiesAndFields
                .Select(m => SyntaxFactory.Parameter(SyntaxFactory.Identifier(m.name.ToLowerInvariant()))
                                .WithType(SyntaxFactory.ParseTypeName(m.type)))
                .ToArray();

            var assignments = propertiesAndFields
                .Select(m => SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    kind: SyntaxKind.SimpleAssignmentExpression,
                                    left: SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                                        SyntaxFactory.Token(SyntaxKind.DotToken),
                                        SyntaxFactory.IdentifierName(m.name)),
                                    right: SyntaxFactory.IdentifierName(m.name.ToLowerInvariant())
                                )))
                .ToArray();

            var ctor =
                SyntaxFactory.ConstructorDeclaration(className)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(ctorParams)
                    .AddBodyStatements(assignments);

            var newClass = classDeclaration.AddMembers(ctor);
            return newClass;
        }
    }
}
