using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public class Field {
        public string text;
        public List<Tuple<int, TagType>> tags = new List<Tuple<int, TagType>>();
        public Field(string text) {
            this.text = text;
            InitFieldTypes();
        }

        protected TagType[] InitFieldTypes() {
            if (IsDashString(text)) return new TagType[0];
            var result = new List<TagType>();
            for (int i = 0; i < text.Length; i++) {
                if (text[i] == '\\') {
                    i++;
                    continue;
                }
                if (text[i] == '_') {
                    if (IsTagStrong(text, i)) {
                        tags.Add(new Tuple<int, TagType>(i, TagType.Strong));
                        i++;
                    }
                    else {
                        tags.Add(new Tuple<int, TagType>(i, TagType.Italic));
                    }
                }
            }
            return result.ToArray();
        }

        private bool IsTagStrong(string field, int pos) {
            return pos < field.Length - 1 && field[pos + 1] == '_';
        }

        private bool IsDashString(string field) {
            return field.All(sym => sym == '_');
        }
    }
}
