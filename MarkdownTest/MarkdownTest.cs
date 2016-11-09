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

        [TestCase("_apple", ExpectedResult = new FieldType[] { FieldType.ItalicBegin })]
        [TestCase("apple_pen", ExpectedResult = new FieldType[] { FieldType.ItalicBegin, FieldType.ItalicEnd })]
        [TestCase("apple__", ExpectedResult = new FieldType[] { FieldType.StrongEnd })]
        [TestCase("_apple__", new FieldType[] { FieldType.ItalicBegin, FieldType.StrongEnd })]
        [TestCase("_apple__", new FieldType[] { FieldType.ItalicBegin, FieldType.StrongEnd }
        )]
        [TestCase("apple_pen__pineapple", new FieldType[] {
            FieldType.ItalicBegin,
            FieldType.ItalicEnd,
            FieldType.StrongBegin,
            FieldType.StrongEnd }
        )]
        [TestCase("penpineappleapplepen", new FieldType[] { FieldType.Simple})]
        [TestCase("*apple** pen", ExpectedResult = "<i>apple</i>* pen")]
        [TestCase("**apple__ pen", ExpectedResult = "<b>apple</b> pen")]
        public FieldType[] GetFieldType(string field) {
            return base.GetFieldTypes(field);
        }

        [TestCase("_apple_ pen", ExpectedResult = "<i>apple</i> pen")]
        [TestCase("_apple__ pen", ExpectedResult = "<i>apple</i>_ pen")]
        public string ConvertToItalic(string field) {
            return field.ConvertToItalic();
        }

        [TestCase("__apple__ pen", ExpectedResult = "<b>apple</b> pen")]
        [TestCase("__apple_pen__", ExpectedResult = "<b>apple_pen</b>")]
        public string ConvertToStrong(string field) {
            return field.ConvertToStrong();
        }

        [TestCase("~~apple~~ pen", ExpectedResult = "<strike>apple</strike> pen")]
        [TestCase("~~__apple_pen__~~", ExpectedResult = "<strike>__apple_pen__</strike>")]
        public string ConvertToStrike(string field) {
            return field.ConvertToStrong();
        }
    }
}
