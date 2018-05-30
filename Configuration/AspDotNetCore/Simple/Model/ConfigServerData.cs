using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Model
{
    public class ConfigServerData
    {
        public Info Info { get; set; }
    }

    public class Info
    {
        public string Profile { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
