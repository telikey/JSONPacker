using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSONPacker.Exeptions
{
    internal class NoToArrayMethod : JSONPackerExceptions
    {
        public NoToArrayMethod(Type classType) 
            :base("Тип "+classType.Name+" не имеет метод ToArray()")
        {

        }
    }
}
