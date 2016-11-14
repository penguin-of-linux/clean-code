using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest {
    [TestFixture]
    public class FieldTest {

        [TestCase("_apple", ExpectedResult = new TagType[] { TagType.Italic})]
        [TestCase("apple_pen", ExpectedResult = new TagType[] { TagType.Italic })]
        [TestCase("apple__", ExpectedResult = new TagType[] { TagType.Strong })]
        [TestCase("_apple__", ExpectedResult = new TagType[] { TagType.Italic, TagType.Strong})]
        [TestCase("apple_pen__pineapple", ExpectedResult = new TagType[] { TagType.Italic, TagType.Strong })]
        [TestCase("penpineappleapplepen", ExpectedResult = new TagType[] { })]
        [TestCase("__", ExpectedResult = new TagType[] { })]
        [TestCase("___", ExpectedResult = new TagType[] { })]
        [TestCase("___apple", ExpectedResult = new TagType[] { TagType.Strong, TagType.Italic})]
        [TestCase("apple\\_pen\\_", ExpectedResult = new TagType[] { })]
        [TestCase("_pen_apple_pen", ExpectedResult = new TagType[] {TagType.Italic, TagType.Italic, TagType.Italic})]
        public TagType[] InitFieldTypes(string text) {
            return new Field(text).tags.Select(t => t.type).ToArray();
        }
    }
}
