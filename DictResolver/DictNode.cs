using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictResolver
{
    internal class DictNode
    {
        public DictNode Parent { get; set; }
        public Dictionary<string, string> Props { get; set; } = new Dictionary<string, string>();
    }
}
