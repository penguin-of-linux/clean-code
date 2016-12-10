using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown {
    public class Md {
        public Settings settings = new Settings("", "");

        public void SetSettings(string baseURL, string CSSClass) {
            settings = new Settings(baseURL, CSSClass);
        }

        public string Render(string text) {
            if (IsUnderscoreString(text)) return text;

            var result = new StringBuilder();
            LineType oldLineType = LineType.Empty;
            foreach(var line in text.Split('\n')) {
                var type = GetLineType(line);
                if (type != oldLineType) {
                    result.Append(GetBlockEnd(oldLineType));
                    result.Append(GetBlockBegin(type));
                }

                if (line != "") result.Append(GetCutedLine(line, type) + "\n");
                oldLineType = type;
            }
            result.Append(GetBlockEnd(oldLineType));
            return result.Remove(result.Length - 1, 1).ToString();
        }

        private bool IsUnderscoreString(string text) {
            return text.All(s => s == '_');
        }

        private LineType GetLineType(string line) {
            if (IsCodeLine(line)) return LineType.Code;
            if (IsListLine(line)) return LineType.List;
            if (IsHeaderLine(line)) return LineType.Header;
            if (IsParagraphLine(line)) return LineType.Paragraph;
            if (line == string.Empty || string.IsNullOrWhiteSpace(line)) return LineType.Empty;
            throw new Exception("Undefined line type");
        }

        private string GetBlockBegin(LineType type) {
            switch (type) {
                case LineType.Code: return "<pre><code>\n";
                case LineType.List: return "<ol>\n";
                case LineType.Paragraph: return "<p>\n";
                default: return "";
            }
        }

        private string GetCutedLine(string line, LineType type) {
            switch (type) {
                case LineType.Code:
                    if (line[0] == ' ') return line.Substring(4);
                    if (line[0] == '\t') return line.Substring(1);
                    return line;
                case LineType.List:
                    var i = 0;
                    while (line[i] != '.') i++;
                    return $"<li>{line.Substring(++i)}</li>";
                case LineType.Header:
                    var headerLevel = 0;
                    while (line[headerLevel] == '#') headerLevel++;
                    i = line.Length - 1;
                    while (line[i] == '#') i--;
                    return MarkerConverter.ConvertLine(string.Format("<h{0}>{1}</h{0}>",
                        headerLevel,
                        line.Substring(headerLevel, i - headerLevel + 1)));
                default: return MarkerConverter.ConvertLine(line);
            }
        }

        private string GetBlockEnd(LineType type) {
            switch (type) {
                case LineType.Code: return "</code></pre>\n";
                case LineType.List: return "</ol>\n";
                case LineType.Paragraph: return "</p>\n";
                default: return "";
            }
        }

        private bool IsParagraphLine(string line) {
            return !string.IsNullOrWhiteSpace(line);
        }

        private bool IsListLine(string line) {
            return line.Length > 1 && char.IsDigit(line[0]) && line[1] == '.';
        }

        private bool IsCodeLine(string line) {
            var spacesCount = 0;
            for (int i = 0; i < line.Length && char.IsWhiteSpace(line[i]); i++) {
                if (line[i] == '\t') return true;
                spacesCount++;
                if (spacesCount == 4) return true;
            }
            return false;
        }

        private bool IsHeaderLine(string line) {
            return line.Length > 0 && line[0] == '#';
        }
    }
}
