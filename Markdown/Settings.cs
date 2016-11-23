using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown {
    public struct Settings {
        public readonly string baseURL;
        public readonly string CSSClass;

        public Settings(string baseURL, string CSSClass) {
            this.baseURL = baseURL;
            this.CSSClass = CSSClass;
        }
    }
}
