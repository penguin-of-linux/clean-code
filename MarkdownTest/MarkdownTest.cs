using System;
using NUnit.Framework;
using Markdown;


namespace MarkdownTests {
    [TestFixture]
    public class MarkdownTest : Md {

        [TestCase("_pen pineapple apple pen_", ExpectedResult = "<i>pen pineapple apple pen</i>")]
        public string Render(string text) {
            return Render(text);
        }

        [TestCase("pen apple _apple pen_ __pineapple __" , 
            ExpectedResult = new string[] { "pen", "apple", "_apple", "pen_", "__pineapple", "__" })]
        public string[] GetAllFields(string text) {
            return base.GetAllFields(text);
        }

        [TestCase("_apple", ExpectedResult = FieldType.Italic)]
        [TestCase("apple__", ExpectedResult = FieldType.Strong)]
        [TestCase("_apple__", ExpectedResult = FieldType.StrongItalic)]
        public FieldType GetFieldType(string field) {
            return base.GetFieldType(field);
        }
    }
}
