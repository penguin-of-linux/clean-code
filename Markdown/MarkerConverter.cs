using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public static class MarkerConverter {
        public static string ConvertLine(string line) {
            var tags = new Stack<Marker>();
            for (int i = 0; i < line.Length; i++) {
                if (line[i] == '\\') {
                    line = line.Remove(i, 1);
                    i++;
                }
                var tag = Marker.CreateTag(ref line, i);
                if (tag == null) continue;
                i += tag.Length - 1;

                if (Marker.IsEndTag(line, tag.Pos) && tags.Any(t => t.Type == tag.Type)) {
                    line = ConvertTag(line, tags, tag);
                    continue;
                }

                if (Marker.IsBeginTag(line, tag.Pos)) {
                    tags.Push(tag);

                }

            }

            return line;
        }

        private static string ConvertTag(string text, Stack<Marker> tags, Marker tag) {
            while (tags.Peek().Type != tag.Type) tags.Pop();
            var otherTag = tags.Pop();
            ConvertTwoMarkersToHtmlTag(ref text, otherTag, tag);
            return text;
        }

        public static void ConvertTwoMarkersToHtmlTag(ref string text, Marker tag1, Marker tag2) {
            string htmlTag = null;
            int tagLength = 0;
            switch (tag1.Type) {
                case MarkerType.Italic: htmlTag = "<i>"; tagLength = Marker.ItalicTagLength; break;
                case MarkerType.Strong: htmlTag = "<b>"; tagLength = Marker.StrongTagLength; break;
                case MarkerType.Link: htmlTag = "<a>"; tagLength = Marker.LinkTagLength; break;
            }
            tag2.Pos += tag2.AdvancedInfo.Length + 3 - tagLength;    //3 - length of html tag

            text = text
                .Remove(tag1.Pos, tagLength)
                .Insert(tag1.Pos, htmlTag)
                .Insert(tag1.Pos + 2, tag2.AdvancedInfo);
            text = text
                .Remove(tag2.Pos, tagLength)
                .Insert(tag2.Pos, htmlTag.Insert(1, "/"));
        }

    }
}
