using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TosDiagnostics.DataModel;

namespace TosDiagnostics.Helpers
{
    public static class AggregateHelper
    {
        public static IEnumerable<ParamWithTokens> AggregateParamsWithTokens(this ChildSyntaxList childSyntaxList)
        {
            var array = childSyntaxList.ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                var elem = array[i];

                if (elem.IsToken)
                {
                    continue;
                }

                yield return new ParamWithTokens
                {
                    Parameter = (ParameterSyntax)elem,
                    TrailingToken = (SyntaxToken)array[i - 1],
                    LeadingToken = (SyntaxToken)array[i + 1]
                };
            }
        }
    }
}
