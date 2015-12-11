using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Lanaguage
{
    public class Language : ILanguage
    {
        public string Code { get; set; }
        public string NameEnglish { get; set; }
        public string NameNative { get; set; }
        public string ISO6392 { get; set; }
        public string ISO6391 { get; set; }
        public string Comments { get; set; }

    }
}