using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using TosDiagnostics.DataModel;

namespace TosDiagnostics.Helpers
{
    public class TriviaHelper
    {
        public SyntaxTriviaList GenerateLeadingTriviaForNewLine(SyntaxNode node)
        {
            var leadingTrivia = new List<SyntaxTrivia>
            {
                SyntaxFactory.CarriageReturnLineFeed
            };

            if (FormattingOptions.UseTabs.DefaultValue)
            {
                leadingTrivia.Add(SyntaxFactory.Tab);
            }
            else
            {
                leadingTrivia.AddRange(Enumerable.Repeat(SyntaxFactory.Space, FormattingOptions.TabSize.DefaultValue));
            }

            leadingTrivia.AddRange(node.GetLeadingTrivia());

            return leadingTrivia.ToSyntaxTriviaList();
        }

        public bool IsParamIsAfterNewLine(ParamWithTokens paramWithTokens)
        {
            var param = paramWithTokens.Parameter;
            var trivia = paramWithTokens.TrailingToken.TrailingTrivia;

            return !trivia.Select(x => x.Kind()).Contains(SyntaxKind.EndOfLineTrivia);
        }
    }
}
