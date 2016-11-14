using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public class Md {
        public string Render(string text) {
            /* Изначально была выбрана неправильная стретегия, из-за этого не успел вовремя,
             * да и не вовремя тоже не успел.
             * А может просто из-за кривых рук :(
            */
            var fields = GetAllFields(text);
            var tagFieldStack = new Stack<Tuple<Field, Tag>>();

            foreach (var field in fields) {
                foreach (var tag in field.tags) {
                    if (IsEndTag(field.text, tag.pos)) {
                        if (tagFieldStack.Any(t => t.Item2.type == tag.type)) {
                            while (tagFieldStack.Peek().Item2.type != tag.type) tagFieldStack.Pop();
                            var otherPair = tagFieldStack.Pop();
                            ConvertTwoFieldsToHtmlTag(otherPair.Item1, field, otherPair.Item2, tag);
                            continue;
                        }
                    }
                    if (IsBeginTag(field.text, tag.pos)) {
                        tagFieldStack.Push(new Tuple<Field, Tag>(field, tag));
                    }
                }
            }

            return fields.Select(f => f.text).Aggregate((x, y) => x + " " + y);
        }

        protected void ConvertTwoFieldsToHtmlTag(Field field1, Field field2, Tag tag1, Tag tag2) {
            string htmlTag = null;
            int tagLength = 0;
            switch (tag1.type) {
                case TagType.Italic: htmlTag = "<i>"; tagLength = 1; break;
                case TagType.Strong: htmlTag = "<b>"; tagLength = 2; break;
            }

            if (field1 == field2) tag2.pos += 3 - tagLength;    //3 - length of html tag

            field1.text = field1.text.Remove(tag1.pos, tagLength).Insert(tag1.pos, htmlTag);
            field2.text = field2.text.Remove(tag2.pos, tagLength).Insert(tag2.pos, htmlTag.Insert(1, "/"));
        }

        protected Field[] GetAllFields(string text) {
            return text.Split(' ').Select(s => new Field(s)).ToArray();
        }

        protected bool IsBeginTag(string field, int pos) {
            return pos != field.Length - 1 && !string.IsNullOrWhiteSpace(field[pos + 1].ToString());
        }

        protected bool IsEndTag(string field, int pos) {
            return pos != 0 && !string.IsNullOrWhiteSpace(field[pos - 1].ToString());
        }


    }
}
