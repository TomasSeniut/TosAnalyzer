using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using TosDiagnostics.Helpers;

namespace TosDiagnostics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TosDiagnosticsAnalyzer : DiagnosticAnalyzer
    {
        private TriviaHelper _triviaHelper = new TriviaHelper();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(DiagnosticDescriptions.DescriptionDefinitions);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(SyntaxNodeConstructorLengthAnalysis, SyntaxKind.ConstructorDeclaration);
        }

        private void SyntaxNodeConstructorLengthAnalysis(SyntaxNodeAnalysisContext context)
        {
            var node = (ConstructorDeclarationSyntax)context.Node;

            var paramWithTokens = node.ParameterList.ChildNodesAndTokens().AggregateParamsWithTokens();

            if (paramWithTokens.Count() < 4)
            {
                return;
            }

            var areParamsTrailingEndOfTheLine = true;

            foreach (var elem in paramWithTokens)
            {
                if (_triviaHelper.IsParamIsAfterNewLine(elem))
                {
                    areParamsTrailingEndOfTheLine = false;
                    break;
                }
            }

            if (!areParamsTrailingEndOfTheLine)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptions.Descriptions["ConstructorParamLength"], node.GetLocation()));
            }
        }
    }
}
