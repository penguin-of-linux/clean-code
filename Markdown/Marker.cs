using System.Linq;
using System.Text;

namespace Markdown {
    public class Marker {
        public static readonly int ItalicTagLength = 1;
        public static readonly int StrongTagLength = 2;
        public static readonly int LinkTagLength = 1;
        public static readonly char[] BeginTags = { '_', '[' };
        public static readonly char[] CloseTags = { '_', ']' };

        public int Pos;
        public MarkerType Type;
        public string AdvancedInfo;
        public int Length;

        public Marker(int pos, MarkerType type, string advancedInfo = "") {
            Pos = pos;
            Type = type;
            AdvancedInfo = advancedInfo;
            switch (type) {
                case MarkerType.Italic:
                case MarkerType.Link: Length = 1; break;
                case MarkerType.Strong: Length = 2; break;
            }
        }

        public static bool IsBeginTag(string text, int pos) {
            return pos != text.Length - 1
                && !string.IsNullOrWhiteSpace(text[pos + 1].ToString())
                && BeginTags.Contains(text[pos]);
        }

        public static bool IsEndTag(string text, int pos) {
            return pos != 0
                && !string.IsNullOrWhiteSpace(text[pos - 1].ToString())
                && CloseTags.Contains(text[pos]);
        }

        private static bool IsTagStrong(string text, int pos) {
            return text[pos] == '_' && pos < text.Length - 1 && text[pos + 1] == '_';
        }

        private static bool IsTagItalic(string text, int pos) {
            return text[pos] == '_';
        }


        private static bool IsTagLink(string text, int pos) {
            return text[pos] == '[' || text[pos] == ']';
        }

        public static Marker CreateTag(ref string text, int pos) {
            if (IsTagStrong(text, pos))
                return new Marker(pos, MarkerType.Strong);
            if (IsTagItalic(text, pos))
                return new Marker(pos, MarkerType.Italic);
            if (IsTagLink(text, pos)) {
                if (Marker.IsEndTag(text, pos)) {
                    var i = pos;
                    while (text[i + 1] != '(') i++;
                    var advancedInfo = CutLink(ref text, i);
                    return new Marker(pos, MarkerType.Link, advancedInfo);
                }
                return new Marker(pos, MarkerType.Link);
            }

            return null;
        }

        private static string CutLink(ref string text, int pos) {
            var result = new StringBuilder();
            while (text[pos] != ')') {
                result.Append(text[pos]);
                text = text.Remove(pos, 1);
            }
            text = text.Remove(pos, 1);
            result = result.Replace(" ", "").Replace("(", "");
            return $" href=\"{result.ToString()}\"";
        }
    }
}
