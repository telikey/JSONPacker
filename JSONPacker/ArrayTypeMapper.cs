using JSONPacker.Exeptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPacker
{

    internal class ArrayTypeMapper
    {
        public object MapTo(dynamic inArray, Type type)
        {
            if (!type.IsArray)
            {
                var constructor = type.GetConstructor(new Type[] { inArray.GetType() });
                if (constructor != null)
                {
                    return constructor.Invoke(new object[] { inArray });
                }
                else
                {
                    throw new NoConstructor(type);
                }
            }
            else
            {
                return inArray;
            }
        }

        public dynamic MapFrom(object inObject)
        {
            var objectType = inObject.GetType();
            var toArrayMethod = objectType.GetMethod("ToArray");
            if (toArrayMethod != null)
            {
                return (object[])toArrayMethod.Invoke(inObject, null);
            }
            else
            {
                if (objectType.IsArray)
                {
                    return inObject as dynamic;
                }
                else
                {
                    throw new NoToArrayMethod(objectType);
                }
            }
        }
    }
}
