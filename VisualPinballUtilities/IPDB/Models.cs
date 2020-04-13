using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualPinballUtilities.IPDB
{
    public class Models
    {
        public class ListsItem
        {
            public string Name { get; set; }
            public ListsValue Id { get; set; }
        }

        public enum ListsValue
        {
            ALL = 0,
            games,
            abbrev,
            top300,
            mfg1,
            mfg2,
            mpu,
            names,
            lastnames
        }
    }
}
