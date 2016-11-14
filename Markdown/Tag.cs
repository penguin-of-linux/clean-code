namespace Markdown {
    public class Tag {
        public int pos;
        public TagType type;
        public Tag(int pos, TagType type) {
            this.pos = pos;
            this.type = type;
        }
    }
}
