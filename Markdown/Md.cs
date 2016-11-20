using System;
using System.Collections.Generic;
using System.Linq;


namespace Markdown {
    public class Md {
        public string Render(string text) {
            var fields = GetAllFields(text);
            var tagFieldStack = new Stack<Tuple<Field, Tag>>();

            foreach (var field in fields) {
                foreach (var tag in field.Tags) {
                    if (IsEndTag(field.Text, tag.Pos)) {
                        if (tagFieldStack.Any(t => t.Item2.Type == tag.Type)) {
                            while (tagFieldStack.Peek().Item2.Type != tag.Type) tagFieldStack.Pop();
                            var otherPair = tagFieldStack.Pop();
                            ConvertTwoFieldsToHtmlTag(otherPair.Item1, field, otherPair.Item2, tag);
                            continue;
                        }
                    }
                    if (IsBeginTag(field.Text, tag.Pos)) {
                        tagFieldStack.Push(new Tuple<Field, Tag>(field, tag));
                    }
                }
            }

            return fields.Select(f => f.Text).Aggregate((x, y) => x + " " + y);
        }

        protected void ConvertTwoFieldsToHtmlTag(Field field1, Field field2, Tag tag1, Tag tag2) {
            string htmlTag = null;
            int tagLength = 0;
            switch (tag1.Type) {
                case TagType.Italic: htmlTag = "<i>"; tagLength = Tag.ItalicTagLength; break;
                case TagType.Strong: htmlTag = "<b>"; tagLength = Tag.StrongTagLength; break;
            }

            if (field1 == field2) tag2.Pos += 3 - tagLength;    //3 - length of html tag

            field1.Text = field1.Text.Remove(tag1.Pos, tagLength).Insert(tag1.Pos, htmlTag);
            field2.Text = field2.Text.Remove(tag2.Pos, tagLength).Insert(tag2.Pos, htmlTag.Insert(1, "/"));
        }

        private Field[] GetAllFields(string text) {
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
