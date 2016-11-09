using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public class Md {
        public int currentPosition = 0;
        public string Render(string mdText) {
            /*
             * 1) Разбиваем на поля по пробелам
             * 2) Для каждого поля определяем типы поля
             * 3) С помощью стека бежим по всем полям и определяем, куда сватить теги
             * 4) ставим теги с помощью TagsConvertion
             */
            return "";
        }

        protected string[] GetAllFields(string text) {
            return new string[] { };
        }

        protected FieldType[] GetFieldTypes(string field) {
            return new FieldType[0];
        }
    }
}
