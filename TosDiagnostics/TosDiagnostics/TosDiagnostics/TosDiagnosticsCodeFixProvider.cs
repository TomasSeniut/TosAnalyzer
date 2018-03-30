using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using TosDiagnostics.Helpers;

namespace TosDiagnostics
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TosDiagnosticsCodeFixProvider)), Shared]
    public class TosDiagnosticsCodeFixProvider : CodeFixProvider
    {
        private readonly TriviaHelper _triviaHelper = new TriviaHelper();

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticDescriptions.Ids); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            foreach (var diag in context.Diagnostics)
            {
                var diagnosticSpan = diag.Location.SourceSpan;

                if (diag.Id == "ConstructorParamLength")
                {
                    var constructorDeclaration = (ConstructorDeclarationSyntax)root.FindNode(diagnosticSpan);

                    context.RegisterCodeFix(CodeAction.Create(
                        title: DiagnosticDescriptions.Titles["ConstructorParamLength"],
                        createChangedDocument: c => SeperateByNewLinesAsync(context.Document, constructorDeclaration, c)),
                    diag);
                }
            }
        }

        private async Task<Document> SeperateByNewLinesAsync(Document document, ConstructorDeclarationSyntax declaration, CancellationToken c)
        {
            var editor = await DocumentEditor.CreateAsync(document, c);

            var leadingTrivia = _triviaHelper.GenerateLeadingTriviaForNewLine(declaration);
            var paramsWithTokens = declaration.ParameterList.ChildNodesAndTokens().AggregateParamsWithTokens();

            foreach (var elem in paramsWithTokens)
            {
                var param = elem.Parameter;

                if (_triviaHelper.IsParamIsAfterNewLine(elem))
                {
                    editor.ReplaceNode(param, param.WithLeadingTrivia(leadingTrivia));
                }
            }

            return editor.GetChangedDocument();
        }
    }
}
