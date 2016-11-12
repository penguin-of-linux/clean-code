using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public class Md {
        private Stack<Tuple<Field, int, TagType>> tagsStack = new Stack<Tuple<Field, int, TagType>>();
        public string Render(string text) {
            var fields = GetAllFields(text);

            foreach(var field in fields) {
                foreach (var tag in field.tags) {
                    if (IsBeginTag(field.text, tag.Item1)) {
                        tagsStack.Push(new Tuple<Field, int, TagType>(field, tag.Item1, tag.Item2));
                    }

                    if (IsEndTag(field.text, tag.Item1)) {
                        if (tagsStack.Any(t => t.Item3 == tag.Item2))
                            while (tagsStack.Peek().Item3 != tag.Item2)
                                tagsStack.Pop();
                        var temp = tagsStack.Pop();
                        ConvertTwoFieldsToTag(temp.Item1, field, temp.Item2, tag.Item1, tag.Item2);
                    }
                }
            }

            var result = fields[0].text;
            foreach (var field in fields)
                result += " " + field.text;
            return result;
        }

        protected void ConvertTwoFieldsToTag(Field field1, Field field2, int pos1, int pos2, TagType tagType) {
            string type = null;
            switch (tagType) {
                case TagType.Italic: type = "<i>"; break;
                case TagType.Strong: type = "<b>"; break;
            }
            field1.text = field1.text.Remove(pos1, 1).Insert(pos1, type);
            field2.text = field2.text.Remove(pos2, 1).Insert(pos2, type.Insert(1, "/"));
        }

        /*protected void ConvertTwoFieldsToTag(ref string first, ref string second, string tag) {
            var index = -1;
            for (int i = 0; i < first.Length; i++)
                if (first[i] == tag[tag.Length-1] && IsBeginTag(first, i)) {
                    index = i;
                    break;
                }
            first = first.Remove(index, 1).Insert(index, "<i>");

            for (int i = 0; i < second.Length; i++)
                if (second[i] == tag[0] && IsEndTag(second, i)) {
                    index = i;
                    break;
                }

            second = second.Remove(index, 1).Insert(index, "</i>");
        }*/

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
