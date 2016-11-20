namespace Markdown {
    public class Tag {
        public static readonly int ItalicTagLength = 1;
        public static readonly int StrongTagLength = 2;
        public int Pos;
        public TagType Type;
        public Tag(int pos, TagType type) {
            Pos = pos;
            Type = type;
        }
    }
}
