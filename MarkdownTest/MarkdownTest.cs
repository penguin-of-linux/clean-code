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
        [TestCase("[apple pen] (google.com)", ExpectedResult = "<a href=\"google.com\">apple pen</a>")]
        public string Render(string text) {
            if (text == "[apple pen] (google.com)")
                ;
            return base.Render(text);
        }

        [TestCase("_applepen_", "", "myclass", 
            ExpectedResult = "<i class=\"myclass\">applepen</i>",
            TestName = "Simple CSS class test")]
        [TestCase("_apple_ __pen__ [link] (google.com)", "", "myclass", 
            ExpectedResult = "<i class=\"myclass\">apple</i> <b class=\"myclass\">pen</b> <a href=\"google.com\" class=\"myclass\">link</a>",
            TestName = "Hard CSS class test")]
        public string Render_WithSettings(string text, string baseURL, string CSSClass) {
            settings = new Settings(baseURL, CSSClass);
            return base.Render(text);
        }

        [TestCase("_apple_", 0, ExpectedResult = true, TestName = "Begin italic tag is begin tag")]
        [TestCase("_apple_", 6, ExpectedResult = false, TestName = "End italic tag is not begin tag")]
        [TestCase("__apple__", 0, ExpectedResult = true, TestName = "Begin strong tag is begin tag")]
        [TestCase("__apple__", 8, ExpectedResult = false, TestName = "End strong tag is not begin tag")]
        [TestCase("__apple_pen__", 7, ExpectedResult = true, TestName = "Middle tag is begin tag")]
        public bool IsBeginTag(string field, int pos) {
            return Tag.IsBeginTag(field, pos);
        }


        [TestCase("_apple_", 0, ExpectedResult = false, TestName = "Begin italic tag is not end tag")]
        [TestCase("_apple_", 6, ExpectedResult = true, TestName = "End italic tag is end tag")]
        [TestCase("__apple__", 0, ExpectedResult = false, TestName = "Begin strong tag is not end tag")]
        [TestCase("__apple__", 8, ExpectedResult = true, TestName = "End strong tag is end tag")]
        [TestCase("__apple_pen__", 7, ExpectedResult = true, TestName = "Middle tag is end tag")]
        public bool IsEndTag(string field, int pos) {
            return Tag.IsEndTag(field, pos);
        }

        [TestCase("_apple pen_", 0, 10, TagType.Italic,
            ExpectedResult = "<i>apple pen</i>",
            TestName = "Simple Convert text to italic tag test")]
        [TestCase("__apple pen__", 0, 11, TagType.Strong,
            ExpectedResult = "<b>apple pen</b>",
            TestName = "Simple Convert text to strong tag test")]
        public string ConvertTwoFieldsToTag(string text, int pos1, int pos2, TagType type) {
            var tag1 = new Tag(pos1, type);
            var tag2 = new Tag(pos2, type);
            base.ConvertTwoTagsToHtmlTag(ref text, tag1, tag2);
            return text;
        }
    }
}
