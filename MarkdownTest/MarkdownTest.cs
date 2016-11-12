using System;
using NUnit.Framework;
using Markdown;


namespace MarkdownTests {
    [TestFixture]
    public class MarkdownTest : Md {

        [TestCase("_pen pineapple apple pen_", ExpectedResult = "<i>pen pineapple apple pen</i>")]
        [TestCase("_pen_apple_pen", ExpectedResult = "<i>pen</i>apple_pen")]
        [TestCase("__", ExpectedResult = "__")]
        public string Render(string text) {
            return base.Render(text);
        }

        [TestCase("_apple_", 0, ExpectedResult = true)]
        [TestCase("_apple_", 6, ExpectedResult = false)]
        [TestCase("__apple_pen__", 7, ExpectedResult = true)]
        [TestCase("__apple_", 0, ExpectedResult = true)]
        public bool IsBeginTag(string field, int pos) {
            return base.IsBeginTag(field, pos);
        }

        [TestCase("_apple_", 0, ExpectedResult = false)]
        [TestCase("_apple_", 6, ExpectedResult = true)]
        [TestCase("__apple_pen__", 7, ExpectedResult = true)]
        public bool IsEndTag(string field, int pos) {
            return base.IsEndTag(field, pos);
        }

        [Test]
        public void ConvertTwoFieldsToTag() {
            var field1 = new Field("_apple");
            var field2 = new Field("pen_");
            ConvertTwoFieldsToTag(field1, field2, 0, 3, TagType.Italic);
            Assert.AreEqual("<i>apple pen</i>", field1.text + " " + field2.text);
        }
    }
}
