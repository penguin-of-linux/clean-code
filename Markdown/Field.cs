using System.Collections.Generic;
using System.Linq;

namespace Markdown {
    public class Field {
        public string Text;
        public List<Tag> Tags = new List<Tag>();
        public Field(string text) {
            this.Text = text;
            InitFieldTypes();
        }

        protected Tag[] InitFieldTypes() {
            if (IsUnderscoreString(Text)) return new Tag[0];
            var result = new List<Tag>();
            for (int i = 0; i < Text.Length; i++) {
                if (Text[i] == '\\') {
                    Text = Text.Remove(i, 1);
                    continue;
                }
                var newTag = CreateTag(i);
                if (newTag == null) continue;
                if (newTag.Type == TagType.Strong) i++;
                Tags.Add(newTag);
            }
            return result.ToArray();
        }

        private bool IsTagStrong(string field, int pos) {
            return field[pos] == '_' && pos < field.Length - 1 && field[pos + 1] == '_';
        }

        private bool IsTagItalic(string field, int pos) {
            return field[pos] == '_';
        }

        private bool IsUnderscoreString(string field) {
            return field.All(sym => sym == '_');
        }

        private Tag CreateTag(int pos) {
            if (IsTagStrong(Text, pos))
                return new Tag(pos, TagType.Strong);
            if (IsTagItalic(Text, pos))
                return new Tag(pos, TagType.Italic);
            return null;
        }
    }
}
