using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace TosDiagnostics
{
    public static class DiagnosticDescriptions
    {
        private static IDictionary<string, DiagnosticDescriptor> _rules;
        private static IDictionary<string, string> _titles;

        public static string[] Ids => _rules?.Keys.ToArray();

        public static DiagnosticDescriptor[] DescriptionDefinitions => _rules?.Values.ToArray();

        public static IDictionary<string, DiagnosticDescriptor> Descriptions => _rules;

        public static IDictionary<string, string> Titles => _titles;

        static DiagnosticDescriptions()
        {
            _rules = new Dictionary<string, DiagnosticDescriptor>
            {
                ["ConstructorParamLength"] = new DiagnosticDescriptor(
                   "ConstructorParamLength",
                   "Constructor Params Length on single line",
                   "Parameter list is too long to stay on the same line",
                   "Formating",
                   DiagnosticSeverity.Info,
                   isEnabledByDefault: true)
            };

            _titles = new Dictionary<string, string>
            {
                ["ConstructorParamLength"] = "Seperate by new lines"
            };
        }
    }
}
