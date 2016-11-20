using System;
using NUnit.Framework;
using Markdown;


namespace MarkdownTests {
    [TestFixture]
    public class MarkdownTest : Md {

        [TestCase("_pineapple_", ExpectedResult = "<i>pineapple</i>", TestName = "Simple italic")]
        [TestCase("__pineapple__", ExpectedResult = "<b>pineapple</b>", TestName = "Simple bold")]
        [TestCase("_pen pineapple apple pen_", 
            ExpectedResult = "<i>pen pineapple apple pen</i>",
            TestName = "Long italic")]
        [TestCase("__pen pineapple apple pen__", 
            ExpectedResult = "<b>pen pineapple apple pen</b>",
            TestName = "Long bold")]
        [TestCase("_pen_apple_pen", ExpectedResult = "<i>pen</i>apple_pen", TestName = "Italic without spaces")]
        [TestCase("_____", ExpectedResult = "_____", TestName = "String of dashes")]
        [TestCase("__apple _pineapple pen_ pen__", 
            ExpectedResult = "<b>apple <i>pineapple pen</i> pen</b>",
            TestName = "Inserted italic")]
        [TestCase("_app\\_le_", ExpectedResult = "<i>app_le</i>", TestName = "Slesh test")]
        public string Render(string text) {
            return base.Render(text);
        }

        [TestCase("_apple_", 0, ExpectedResult = true, TestName = "Begin italic tag is begin tag")]
        [TestCase("_apple_", 6, ExpectedResult = false, TestName = "End italic tag is not begin tag")]
        [TestCase("__apple__", 0, ExpectedResult = true, TestName = "Begin strong tag is begin tag")]
        [TestCase("__apple__", 8, ExpectedResult = false, TestName = "End strong tag is not begin tag")]
        [TestCase("__apple_pen__", 7, ExpectedResult = true, TestName = "Middle tag is begin tag")]
        public bool IsBeginTag(string field, int pos) {
            return base.IsBeginTag(field, pos);
        }


        [TestCase("_apple_", 0, ExpectedResult = false, TestName = "Begin italic tag is not end tag")]
        [TestCase("_apple_", 6, ExpectedResult = true, TestName = "End italic tag is end tag")]
        [TestCase("__apple__", 0, ExpectedResult = false, TestName = "Begin strong tag is not end tag")]
        [TestCase("__apple__", 8, ExpectedResult = true, TestName = "End strong tag is end tag")]
        [TestCase("__apple_pen__", 7, ExpectedResult = true, TestName = "Middle tag is end tag")]
        public bool IsEndTag(string field, int pos) {
            return base.IsEndTag(field, pos);
        }

        [TestCase("_apple", "pen_", 0, 3, TagType.Italic,
            ExpectedResult = "<i>apple pen</i>",
            TestName = "Simple Convert two fields to tag test")]
        public string ConvertTwoFieldsToTag(string text1, string text2, int pos1, int pos2, TagType type) {
            var field1 = new Field(text1);
            var field2 = new Field(text2);
            base.ConvertTwoFieldsToHtmlTag(field1, field2, new Tag(pos1, type), new Tag(pos2, type));
            return field1.Text + " " + field2.Text;
        }
    }
}
