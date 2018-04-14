using Microsoft.VisualStudio.TestTools.UnitTesting;
using HalloJoe.Text.Casing;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestCasing
    {
        [TestMethod]
        public void has_lower() => Assert.AreEqual("this-has-lower".HasLower(), true);

        [TestMethod]
        public void has_upper() => Assert.AreEqual("ThisHasUpper".HasUpper(), true);

        [TestMethod]
        public void is_upper() => Assert.AreEqual("THISISUPPER".IsUpper(), true);

        [TestMethod]
        public void is_upper_edge() => Assert.AreEqual("TH1S-ÍSUPPE´R".IsUpper(true), true);

        [TestMethod]
        public void is_upper_edge_revert() => Assert.AreEqual("TH1S-ÍSUPPE´R".IsUpper(), false);

        [TestMethod]
        public void is_lower() => Assert.AreEqual("thisislower".IsLower(), true);

        [TestMethod]
        public void detect_kebab() => Assert.AreEqual("this-is-kebab".DetectCase(), Case.Kebab);

        [TestMethod]
        public void detect_pascal() => Assert.AreEqual("ThisIsPascal".DetectCase(), Case.Pascal);

        [TestMethod]
        public void detect_camel() => Assert.AreEqual("thisIsCamel".DetectCase(), Case.Camel);

        [TestMethod]
        public void normalize_uppers() => Assert.AreEqual("IPascalToKebabAPI".NormalizeUppers(), "IpascalToKebabApi");

        [TestMethod]
        public void pascal_to_kebab() => Assert.AreEqual("PascalToKebab".ToKebab(), "pascal-to-kebab");

        [TestMethod]
        public void camel_to_kebab() => Assert.AreEqual("camelToKebab".ToKebab(), "camel-to-kebab");

        [TestMethod]
        public void kebab_to_pascal() => Assert.AreEqual("kebab-to-pascal".ToPascal(), "KebabToPascal");

        [TestMethod]
        public void camel_to_pascal() => Assert.AreEqual("camelToPascal".ToPascal(), "CamelToPascal");

        [TestMethod]
        public void pascal_to_camel() => Assert.AreEqual("PascalToCamel".ToCamel(), "pascalToCamel");

        [TestMethod]
        public void kebab_to_camel() => Assert.AreEqual("kebab-to-camel".ToCamel(), "kebabToCamel");
    }
}
