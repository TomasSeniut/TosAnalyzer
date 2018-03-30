using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TosDiagnostics.DataModel
{
    public class ParamWithTokens
    {
        public SyntaxToken TrailingToken { get; set; }

        public SyntaxToken LeadingToken { get; set; }

        public ParameterSyntax Parameter { get; set; }
    }
}
