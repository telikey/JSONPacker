using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSONPacker.Exeptions
{
    internal class NoConstructor : JSONPackerExceptions
    {
        public NoConstructor(Type classType) 
            :base("Тип "+classType.Name+" не имеет конструктора для массива")
        {

        }
    }
}
