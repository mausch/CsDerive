using Microsoft.Dnx.Compilation.CSharp;
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace CsDerive
{

    public abstract class ClassAttributeProcessorCompileModule<T> : ICompileModule where T: Attribute {
        public abstract ClassDeclarationSyntax ProcessClass(ClassDeclarationSyntax classDeclaration);

        public void BeforeCompile(BeforeCompileContext context) {

            var attrTypeCtor = context.Compilation
                .GetTypeByMetadataName(typeof(T).FullName)
                .InstanceConstructors.Single();

            var classes =
                from tree in context.Compilation.SyntaxTrees
                from classDecl in tree.GetRoot().DescendantNodes().OfSubtype<SyntaxNode, ClassDeclarationSyntax>()
                let model = context.Compilation.GetSemanticModel(tree, true)
                where (
                    from attrList in classDecl.AttributeLists
                    from attr in attrList.Attributes
                    let symbol = model.GetSymbolInfo(attr).Symbol
                    where symbol.Equals(attrTypeCtor)
                    select symbol
                ).Any()
                select new {
                    classDecl,
                    tree,
                };

            var classesList = classes.ToList();

            if (classesList.Count == 0)
                return;

            var changes = 
                from g in classesList.GroupBy(x => x.tree)
                select new {
                    oldTree = g.Key,
                    newTree = g.Aggregate(g.Key, (accTree, ctx) => ProcessClassTree(ctx.classDecl, accTree))
                };

            //Debugger.Launch();

            context.Compilation = changes.Aggregate(context.Compilation, (compilation, change) => compilation.ReplaceSyntaxTree(change.oldTree, change.newTree));
        }

        SyntaxTree ProcessClassTree(ClassDeclarationSyntax clazz, SyntaxTree tree) {
            var existingClass = tree.GetRoot()
                .DescendantNodes()
                .OfSubtype<SyntaxNode, ClassDeclarationSyntax>()
                .Where(c => c.ToString().Equals(clazz.ToString())) // TODO ugly as hell
                .FirstOrDefault();

            if (existingClass == null) {
                //Debugger.Launch();
                throw new Exception($"class not found: {clazz}"); // TODO move to diagnostics
            }

            var newClass = ProcessClass(existingClass);
            var newRoot = (CSharpSyntaxNode)tree.GetRoot().ReplaceNode(existingClass, newClass);
            var newTree = CSharpSyntaxTree.Create(newRoot);
            return newTree;
        }

        public void AfterCompile(AfterCompileContext context) {}
    }
}
