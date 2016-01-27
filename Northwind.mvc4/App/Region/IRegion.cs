using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Region
{
    public interface IRegion
    {
        int RegionID { get; set; }
        string RegionDescription { get; set; }

        bool IsEmpty();
    }
}
