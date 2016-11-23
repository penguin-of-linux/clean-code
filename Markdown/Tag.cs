using System.Linq;


namespace Markdown {
    public class Tag {
        public static readonly int ItalicTagLength = 1;
        public static readonly int StrongTagLength = 2;
        public static readonly int LinkTagLength = 1;
        public static readonly char[] BeginTags = new char[] { '_', '[' };
        public static readonly char[] CloseTags = new char[] { '_', ']' };

        public int Pos;
        public TagType Type;
        public string AdvancedInfo;

        public Tag(int pos, TagType type, string advancedInfo = "") {
            Pos = pos;
            Type = type;
            AdvancedInfo = advancedInfo;
        }

        public static bool IsBeginTag(string field, int pos) {
            return pos != field.Length - 1
                && !string.IsNullOrWhiteSpace(field[pos + 1].ToString())
                && BeginTags.Contains(field[pos]);
        }

        public static bool IsEndTag(string field, int pos) {
            return pos != 0
                && !string.IsNullOrWhiteSpace(field[pos - 1].ToString())
                && CloseTags.Contains(field[pos]);
        }
    }
}
