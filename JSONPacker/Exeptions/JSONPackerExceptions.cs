using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPacker
{
    internal class JSONPackerExceptions:Exception
    {
        public JSONPackerExceptions(string message) : base("JSONPacker: "+message) { }
    }
}
