using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public class Md {
        public int currentPosition = 0;
        public string Render(string mdText) {
            return "";
        }

        protected string[] GetAllFields(string text) {
            return new string[] { };
        }

        protected FieldType GetFieldType(string field) {
            return FieldType.ItalicBegin;
        }
    }
}
