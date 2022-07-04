using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSONPacker.Exeptions
{
    internal class FieldTypesNotEqual:JSONPackerExceptions
    {
        public FieldTypesNotEqual(FieldInfo classField, FieldInfo dtoField, Type classType, Type dtoType) 
            :base("Тип "+classType.Name+"."+classField.Name+"("+classField.FieldType.Name+")"+ " не производный от " + 
                 dtoType.Name + "." + dtoField.Name + "(" + dtoField.FieldType.Name + ")")
        {

        }
    }
}
