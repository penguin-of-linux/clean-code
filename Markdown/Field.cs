using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public class Field {
        public string text;
        public List<Tag> tags = new List<Tag>();
        public Field(string text) {
            this.text = text;
            InitFieldTypes();
        }

        protected Tag[] InitFieldTypes() {
            if (IsDashString(text)) return new Tag[0];
            var result = new List<Tag>();
            for (int i = 0; i < text.Length; i++) {
                if (text[i] == '\\') {
                    text = text.Remove(i, 1);
                    continue;
                }
                var newTag = CreateTag(text, i);
                if (newTag == null) continue;
                if (newTag.type == TagType.Strong) i++;
                tags.Add(newTag);
            }
            return result.ToArray();
        }

        private bool IsTagStrong(string field, int pos) {
            return pos < field.Length - 1 && field[pos + 1] == '_';
        }

        private bool IsDashString(string field) {
            return field.All(sym => sym == '_');
        }

        private Tag CreateTag(string text, int pos) {
            if (text[pos] == '_') {
                if (IsTagStrong(text, pos)) {
                    return new Tag(pos, TagType.Strong);
                }
                else return new Tag(pos, TagType.Italic);
            }
            return null;
        }
    }
}
