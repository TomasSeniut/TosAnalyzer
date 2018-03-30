using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TestHelper;
using TosDiagnostics;

namespace TosDiagnostics.Test
{
    [TestClass]
    public class ConstructorLengthUnitTests : CodeFixVerifier
    {
        private string _commonPath = "..\\..\\..\\TestClasses\\ConstructorLength\\";
        private string _oneLineConstructorBefore;
        private string _oneLineConstructorAfter;

        public ConstructorLengthUnitTests()
        {
            _oneLineConstructorBefore = File.ReadAllText(_commonPath + "ClassBeforeFix.cs")
                .Replace("ClassBeforeFix", "ClassUnderTest");

            _oneLineConstructorAfter = File.ReadAllText(_commonPath + "ClassAfterFix.cs")
                .Replace("ClassAfterFix", "ClassUnderTest");
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new TosDiagnosticsCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new TosDiagnosticsAnalyzer();
        }

        [TestMethod]
        public void NoDiagnose()
        {
            var emptyTest = @"";

            VerifyCSharpDiagnostic(emptyTest);
        }

        [TestMethod]
        public void CheckConstructor_Diagnostics()
        {
            var expected = new DiagnosticResult
            {
                Id = "ConstructorParamLength",
                Message = "Parameter list is too long to stay on the same line",
                Severity = DiagnosticSeverity.Info,
                Locations = new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 9, 9)
                        }
            };

            VerifyCSharpDiagnostic(_oneLineConstructorBefore, expected);
        }

        [TestMethod]
        public void CheckConstrutorAfterFix_Diagnostics()
        {
            var expected = new DiagnosticResult[0];

            VerifyCSharpDiagnostic(_oneLineConstructorAfter, expected);
        }

        [TestMethod]
        public void CheckConstructor_CodeFix()
        {
            VerifyCSharpFix(_oneLineConstructorBefore, _oneLineConstructorAfter);
        }
    }
}
