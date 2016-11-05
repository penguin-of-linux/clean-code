using System;
using NUnit.Framework;
using Markdown;


namespace MarkdownTests {
    [TestFixture]
    public class MarkdownTest : Md {

        [Test]
        public void Render_EmptyText() {
            var text = Render("");
            Assert.AreEqual("", text);
        }

        [Test]
        public void GetAllFields() {
            var text = "";
            var fields = GetAllFields(text);
            Assert.AreEqual(0, fields.Length);
        }
    }
}
