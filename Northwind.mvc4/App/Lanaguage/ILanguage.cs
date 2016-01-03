using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Lanaguage
{
    public interface ILanguage
    {
        string Code { get; set; }
        string Name_EN { get; set; }
        string Name_Native { get; set; }
        string ISO6392 { get; set; }
        string ISO6391 { get; set; }
        string Comments { get; set; }
    }
}
