using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown {
    public class Md {
        public Settings settings = new Settings("", "");
        public string Render(string text) {
            if (IsUnderscoreString(text)) return text;
            var tags = new Stack<Tag>();
            for(int i = 0; i < text.Length; i++) {
                if (text[i] == '\\') {
                    text = text.Remove(i, 1);
                    i++;
                }
                var tag = CreateTag(ref text, i);
                if (tag == null) continue;

                if (Tag.IsEndTag(text, tag.Pos)) {
                    if (TryConvertTag(ref text, tags, tag)) continue;
                }

                if (Tag.IsBeginTag(text, tag.Pos)) {
                    tags.Push(tag);
                }
            }
            
            return text;
        }

        private bool TryConvertTag(ref string text, Stack<Tag> tags, Tag tag) {
            if (tags.Any(t => t.Type == tag.Type)) {
                while (tags.Peek().Type != tag.Type) tags.Pop();
                var otherTag = tags.Pop();
                ConvertTwoTagsToHtmlTag(ref text, otherTag, tag);
                return true;
            }
            return false;
        }

        private string CutLink(ref string text, int pos) {
            var result = new StringBuilder();
            while (true) {
                result.Append(text[pos]);
                text = text.Remove(pos, 1);
                if (text[pos] == ')') {
                    text = text.Remove(pos, 1);
                    break;
                }
            }
            result = result.Replace(" ", "").Replace("(", "");
            return String.Format(" href=\"{0}\"", result.ToString());
        }

        protected void ConvertTwoTagsToHtmlTag(ref string text, Tag tag1, Tag tag2) {
            string htmlTag = null;
            int tagLength = 0;
            switch (tag1.Type) {
                case TagType.Italic: htmlTag = "<i>"; tagLength = Tag.ItalicTagLength; break;
                case TagType.Strong: htmlTag = "<b>"; tagLength = Tag.StrongTagLength; break;
                case TagType.Link: htmlTag = "<a>"; tagLength = Tag.LinkTagLength; break;
            }
            if (settings.CSSClass != "")
                tag2.AdvancedInfo += String.Format(" class=\"{0}\"", settings.CSSClass);
            tag2.Pos += tag2.AdvancedInfo.Length + 3 - tagLength;    //3 - length of html tag

            text = text
                .Remove(tag1.Pos, tagLength)
                .Insert(tag1.Pos, htmlTag)
                .Insert(tag1.Pos + 2, tag2.AdvancedInfo);
            text = text
                .Remove(tag2.Pos, tagLength)
                .Insert(tag2.Pos, htmlTag.Insert(1, "/"));
        }

        private bool IsTagStrong(string field, int pos) {
            return field[pos] == '_' && pos < field.Length - 1 && field[pos + 1] == '_';
        }

        private bool IsTagItalic(string field, int pos) {
            return field[pos] == '_';
        }

        private bool IsUnderscoreString(string text) {
            return text.All(s => s == '_');
        }

        private bool IsTagLink(string field, int pos) {
            return field[pos] == '[' || field[pos] == ']';
        }

        private Tag CreateTag(ref string text, int pos) {
            if (IsTagStrong(text, pos))
                return new Tag(pos, TagType.Strong);
            if (IsTagItalic(text, pos))
                return new Tag(pos, TagType.Italic);
            if (IsTagLink(text, pos)) {
                if (Tag.IsEndTag(text, pos)) {
                    var i = pos;
                    while (text[i+1] != '(') i++;
                    var advancedInfo = CutLink(ref text, i);
                    return new Tag(pos, TagType.Link, advancedInfo);
                }
                return new Tag(pos, TagType.Link);
            }
                
            return null;
        }
    }
}
