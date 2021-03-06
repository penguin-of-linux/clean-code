﻿using System;
using NUnit.Framework;
using Markdown;


namespace MarkdownTests {
    [TestFixture]
    public class MarkdownTest : Md {

        [SetUp]
        public void SetSettings() {
            settings = new Settings("", "");
        }

        [TestCase("", ExpectedResult = "", TestName = "Empty string")]
        [TestCase("a", ExpectedResult = "<p>\na\n</p>")]
        [TestCase("a  \na  \n", ExpectedResult = "<p>\na  \na  \n</p>")]
        [TestCase("\\\a a_", ExpectedResult = "<p>\n\a a_\n</p>")]
        [TestCase("a\n\tcodeline1\n\tcode_line_2",
            ExpectedResult = "<p>\na\n</p>\n<pre><code>\ncodeline1\ncode_line_2\n</code></pre>")]
        [TestCase("#apple", ExpectedResult = "<h1>apple</h1>", TestName = "Simple header insert")]
        [TestCase("1.One\n2.Two\n3.Three",
            ExpectedResult = "<ol>\n<li>One</li>\n<li>Two</li>\n<li>Three</li>\n</ol>",
            TestName = "Simple lists insert test")]
        [TestCase("\tcode",
            ExpectedResult = "<pre><code>\ncode\n</code></pre>",
            TestName = "Simple code block insert test")]
        [TestCase("    code1\n\t\tcode2\n    code3\n",
            ExpectedResult = "<pre><code>\ncode1\n\tcode2\ncode3\n</code></pre>",
            TestName = "Hard code block insert test")]
        public string Render(string text) {
            if (text == "a  \na  \n")
                ;
            return base.Render(text);
        }

        [TestCase("_pineapple_", ExpectedResult = "<i>pineapple</i>", TestName = "Simple italic")]
        [TestCase("__pineapple__", ExpectedResult = "<b>pineapple</b>", TestName = "Simple bold")]
        [TestCase("_pen pineapple apple pen_",
            ExpectedResult = "<i>pen pineapple apple pen</i>",
            TestName = "Long italic")]
        [TestCase("__pen pineapple apple pen__",
            ExpectedResult = "<b>pen pineapple apple pen</b>",
            TestName = "Long bold")]
        [TestCase("_pen_apple_pen", ExpectedResult = "<i>pen</i>apple_pen", TestName = "Italic without spaces")]
        //[TestCase("_____", ExpectedResult = "_____", TestName = "String of dashes")]
        [TestCase("__apple _pineapple pen_ pen__",
            ExpectedResult = "<b>apple <i>pineapple pen</i> pen</b>",
            TestName = "Inserted italic")]
        [TestCase("_app\\_le_", ExpectedResult = "<i>app_le</i>", TestName = "Slash test")]
        [TestCase("[apple pen] (google.com)",
            ExpectedResult = "<a href=\"google.com\">apple pen</a>",
            TestName = "Link")]
        [TestCase("\\\a a_", ExpectedResult = "\a a_", TestName = "Slash test")]
        [TestCase("___a a___",
            ExpectedResult = "<b>_a a</b>_",
            TestName = "3 dashes")] // иначе тип тегa должен зависеть от уже существующих
        [TestCase("_a __a a__", ExpectedResult = "_a <b>a a</b>")]
        [TestCase("a_a a_", ExpectedResult = "a<i>a a</i>")]
        [TestCase("___a a__", ExpectedResult = "<b>_a a</b>")]
        [TestCase("__a __a a__", ExpectedResult = "__a <b>a a</b>")]
        [TestCase("_a __a__ a_", ExpectedResult = "<i>a <b>a</b> a</i>")]
        [TestCase("__a _a_ __", ExpectedResult = "__a <i>a</i> __")]
        public string ConvertAllTags(string text) {
            return MarkerConverter.ConvertLine(text);
        }

        /*[TestCase("_applepen_", "", "myclass",
            ExpectedResult = "<i class=\"myclass\">applepen</i>",
            TestName = "Simple CSS class test")]
        [TestCase("_apple_ __pen__ [link] (google.com)", "", "myclass",
            ExpectedResult = "<i class=\"myclass\">apple</i> <b class=\"myclass\">pen</b> <a href=\"google.com\" class=\"myclass\">link</a>",
            TestName = "Hard CSS class test")]
        public string ConvertAllTags_WithSettings(string text, string baseURL, string CSSClass) {
            base.SetSettings(baseURL, CSSClass);
            return base.ConvertAllTags(text);
        }*/

        [TestCase("_apple_", 0, ExpectedResult = true, TestName = "Begin italic tag is begin tag")]
        [TestCase("_apple_", 6, ExpectedResult = false, TestName = "End italic tag is not begin tag")]
        [TestCase("__apple__", 0, ExpectedResult = true, TestName = "Begin strong tag is begin tag")]
        [TestCase("__apple__", 8, ExpectedResult = false, TestName = "End strong tag is not begin tag")]
        [TestCase("__apple_pen__", 7, ExpectedResult = true, TestName = "Middle tag is begin tag")]
        public bool IsBeginTag(string field, int pos) {
            return Marker.IsBeginTag(field, pos);
        }


        [TestCase("_apple_", 0, ExpectedResult = false, TestName = "Begin italic tag is not end tag")]
        [TestCase("_apple_", 6, ExpectedResult = true, TestName = "End italic tag is end tag")]
        [TestCase("__apple__", 0, ExpectedResult = false, TestName = "Begin strong tag is not end tag")]
        [TestCase("__apple__", 8, ExpectedResult = true, TestName = "End strong tag is end tag")]
        [TestCase("__apple_pen__", 7, ExpectedResult = true, TestName = "Middle tag is end tag")]
        public bool IsEndTag(string field, int pos) {
            return Marker.IsEndTag(field, pos);
        }

        [TestCase("_apple pen_", 0, 10, MarkerType.Italic,
            ExpectedResult = "<i>apple pen</i>",
            TestName = "Simple Convert text to italic tag test")]
        [TestCase("__apple pen__", 0, 11, MarkerType.Strong,
            ExpectedResult = "<b>apple pen</b>",
            TestName = "Simple Convert text to strong tag test")]
        public string ConvertTwoFieldsToTag(string text, int pos1, int pos2, MarkerType type) {
            var tag1 = new Marker(pos1, type);
            var tag2 = new Marker(pos2, type);
            MarkerConverter.ConvertTwoMarkersToHtmlTag(ref text, tag1, tag2);
            return text;
        }

        /*[TestCase("apple\n\npen",
            ExpectedResult = "<p>\napple\n</p>\n<p>\npen\n</p>",
            TestName = "Simple insert paragraphs test")]
        public string InsertParagraphs(string text) {
            return base.InsertParagraphs(text);
        }

        [TestCase("apple pen  \n", ExpectedResult = "apple pen  <br/>", TestName = "Simple break insert")]
        [TestCase("apple     pen\t\t\n",
            ExpectedResult = "apple     pen\t\t<br/>",
            TestName = "Hard break insert")]
        public string InsertBreaks(string text) {
            return base.InsertBreaks(text);
        }

        [TestCase("#apple", ExpectedResult = "<h1>apple</h1>\n", TestName = "Simple header insert")]
        [TestCase("#header1 ## ##\n   #non-header1\n###header3#\ntext\n",
            ExpectedResult = "<h1>header1 ## </h1>\n   #non-header1\n<h3>header3</h3>\ntext\n",
            TestName = "Hard headers insert test")]
        public string InsertHeaders(string text) {
            return base.InsertHeaders(text);
        }

        [TestCase("\tcode",
            ExpectedResult = "<pre><code>\ncode\n</code></pre>",
            TestName = "Simple code block insert test")]
        [TestCase("    code1\n\t\tcode2\n    code3\n",
            ExpectedResult = "<pre><code>\ncode1\n\tcode2\ncode3\n</code></pre>",
            TestName = "Hard code block insert test")]
        public string InsertCodeBlocks(string text) {
            return base.InsertCode(text);
        }

        [TestCase("1.One\n2.Two\n3.Three",
            ExpectedResult = "<ol>\n<li>One</li>\n<li>Two</li>\n<li>Three</li>\n</ol>",
            TestName = "Simple lists insert test")]
        public string InsertLists(string text) {
            return base.InsertLists(text);
        }*/
    }
}
