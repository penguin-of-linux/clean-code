using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown {
    public class Md {
        public Settings settings = new Settings("", "");

        public void SetSettings(string baseURL, string CSSClass) {
            settings = new Settings(baseURL, CSSClass);
        }

        public string Render(string text) {
            if (IsUnderscoreString(text)) return text;

            var result = ConvertAllTags(text);
            result = InsertCode(result);
            result = InsertLists(result);
            result = InsertHeaders(result);
            result = InsertParagraphs(result);
            result = InsertBreaks(result);

            return result;
        }

        private bool IsUnderscoreString(string text) {
            return text.All(s => s == '_');
        }

        protected string ConvertAllTags(string text) {
            var tags = new Stack<Tag>();
            for(int i = 0; i < text.Length; i++) {
                if (text[i] == '\\') {
                    text = text.Remove(i, 1);
                    i++;
                }
                var tag = Tag.CreateTag(ref text, i);
                if (tag == null) continue;

                if (Tag.IsEndTag(text, tag.Pos) && tags.Any(t => t.Type == tag.Type)) {
                        text = ConvertTag(text, tags, tag);
                        continue;
                }

                if (Tag.IsBeginTag(text, tag.Pos)) {
                    tags.Push(tag);
                }
            }
            
            return text;
        }

        private string ConvertTag(string text, Stack<Tag> tags, Tag tag) {
            while (tags.Peek().Type != tag.Type) tags.Pop();
            var otherTag = tags.Pop();
            ConvertTwoTagsToHtmlTag(ref text, otherTag, tag);
            return text;
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


        protected string InsertBreaks(string text) {
            var result = new StringBuilder();
            var spaceCount = 0;
            for (int i = 0; i < text.Length; i++) {
                if (char.IsWhiteSpace(text[i])) spaceCount++;
                if (text[i] == '\n' && spaceCount > 1) {
                    result.Append("<br/>");
                    break;
                }
                result.Append(text[i]);
            }
            return result.ToString();
        }

        protected string InsertHeaders(string text) {
            var result = new StringBuilder();
            foreach(var line in text.Split('\n')) {
                var headerLevel = 0;
                for (int i = 0; i < line.Length && line[i] == '#'; i++) headerLevel++;
                if (headerLevel < 1 || headerLevel > 6) {
                    result.Append(line + "\n");
                    continue;
                }

                var endHeader = line.Length - 1;
                while (line[endHeader] == '#') endHeader--;
                var prost = line.Substring(headerLevel, endHeader - headerLevel + 1);
                result.Append(string.Format("<h{0}>{1}</h{0}>\n", 
                                            headerLevel, 
                                            prost));
            }
            return result.Remove(result.Length-1, 1).ToString();
        }

        protected string InsertLists(string text) {
            return InsertBlockTags(text, "<ol>", "</ol>", "li", IsListLine);
        }

        protected string InsertParagraphs(string text) {
            return InsertBlockTags(text, "<p>", "</p>", "", IsParagraphLine);
        }

        protected string InsertCode(string text) {
            return InsertBlockTags(text, "<pre><code>", "</code></pre>", "", IsCodeLine);
        }

        private string InsertBlockTags(string text, 
                                  string beginBlockTag,
                                  string endBlockTag,
                                  string lineTag,
                                  Func<string, bool> IsLineInBlock) {
            var result = new StringBuilder();
            var inBlock = false;
            foreach (var line in text.Split('\n')) {
                if (IsLineInBlock(line)) {
                    if (!inBlock) {
                        result.Append(beginBlockTag + "\n");
                        inBlock = true;
                    }
                    if (lineTag == "") result.Append(ShortLine(line) + "\n");
                    else result.Append(string.Format("<{0}>{1}</{0}>\n", lineTag, ShortLine(line)));
                }
                else {
                    if (inBlock) {
                        result.Append(endBlockTag + "\n");
                        inBlock = false;
                    }
                    if (line != "") result.Append(line + "\n");
                }
            }
            result = result.Remove(result.Length - 1, 1);
            if (inBlock) result.Append("\n" + endBlockTag);
            return result.ToString();
        }

        private bool IsParagraphLine(string line) {
            return !string.IsNullOrWhiteSpace(line);
        }

        private bool IsListLine(string line) {
            return line.Length > 1 && char.IsDigit(line[0]) && line[1] == '.';
        }

        private bool IsCodeLine(string line) {
            var spacesCount = 0;
            for(int i = 0; i < line.Length && char.IsWhiteSpace(line[i]); i++) {
                if (line[i] == '\t') return true;
                spacesCount++;
                if (spacesCount == 4) return true;
            }
            return false;
        }

        private string ShortLine(string line) {
            if (line[0] == ' ') return line.Substring(4);
            if (line[0] == '\t') return line.Substring(1);
            if (char.IsDigit(line[0]) && line[1] == '.') return line.Substring(2);
            return line;
        }
    }
}
