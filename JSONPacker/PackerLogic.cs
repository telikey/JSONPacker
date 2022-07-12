using JSONPacker.Exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace JSONPacker
{

    public interface IJsonPackerLogic
    {
        TOutClass UnPack<TOutClass, TOutClassDto>(string inJson) where TOutClass : class, new() where TOutClassDto : class, new();
        string Pack<TInClass, TInClassDto>(TInClass inObject) where TInClass : class, new() where TInClassDto : class, new();

        TOutClassDto MapToDto<TOutClassDto, TInClass>(TInClass inObject) where TInClass : class, new() where TOutClassDto : class, new();
        TOutClass MapFromDto<TOutClass, TInClassDto>(TInClassDto inObject) where TInClassDto : class, new() where TOutClass : class, new();
    }

    //Rules 
    // Class name = _ name of DtoClass
    public class JsonPackerLogic : IJsonPackerLogic
    {
        private string _suffix { get; set; } = "";
        private string _prefix { get; set; } = "";
        private ArrayTypeMapper _arrayTypeProcess { get; set; }

        public JsonPackerLogic(string prefix,string suffix)
        {
            this._prefix= prefix;
            this._suffix= suffix;

            this._arrayTypeProcess = new ArrayTypeMapper();
        }

        public TOutClass UnPack<TOutClass, TOutClassDto>(string inJson) where TOutClass : class, new() where TOutClassDto : class, new()
        {
            var dto = JsonConvert.DeserializeObject<TOutClassDto>(inJson);
            return this.MapFromDto<TOutClass,TOutClassDto>(dto);

        }

        public string Pack<TInClass, TInClassDto>(TInClass inObject) where TInClass : class, new() where TInClassDto : class, new()
        {
            var dto = this.MapToDto<TInClassDto, TInClass>(inObject);
            return JsonConvert.SerializeObject(dto);
        }

        public TOutClassDto MapToDto<TOutClassDto, TInClass>(TInClass inObject) where TInClass : class, new() where TOutClassDto : class, new()
        {
            var classType = typeof(TInClass);
            var dtoType = typeof(TOutClassDto);

            var classFields = classType.GetFields(System.Reflection.BindingFlags.NonPublic| System.Reflection.BindingFlags.Instance);
            var dtoFields = dtoType.GetFields();

            var result = new TOutClassDto();

            foreach (var classField in classFields)
            {
                foreach (var dtoField in dtoFields)
                {
                    if (classField.Name == _prefix + dtoField.Name + _suffix)
                    {
                        if (dtoField.FieldType.IsArray)
                        {
                            var value = classField.GetValue(inObject);
                            var arrayValue = _arrayTypeProcess.MapFrom(value);
                            dtoField.SetValue(result, arrayValue);
                            break;
                        }
                        else
                        {
                            if (dtoField.FieldType.IsAssignableFrom(classField.FieldType))
                            {
                                var value = classField.GetValue(inObject);
                                dtoField.SetValue(result, value);
                                break;
                            }
                            else
                            {
                                throw new FieldTypesNotEqual(classField, dtoField, classType, dtoType);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public TOutClass MapFromDto<TOutClass, TInClassDto>(TInClassDto inObject) where TInClassDto : class, new() where TOutClass : class, new()
        {
            var dtoType = typeof(TInClassDto);
            var classType = typeof(TOutClass);

            var dtoFields = dtoType.GetFields();
            var classFields = classType.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var result = new TOutClass();

            foreach (var dtoField in dtoFields)
            {
                foreach (var classField in classFields)
                {
                    if (classField.Name == _prefix + dtoField.Name + _suffix)
                    {
                        if (dtoField.FieldType.IsArray)
                        {
                            var value = dtoField.GetValue(inObject);
                            var arrayValue = _arrayTypeProcess.MapTo(value,classField.FieldType);
                            classField.SetValue(result, arrayValue);
                        }
                        else
                        {
                            if (classField.FieldType.IsAssignableFrom(dtoField.FieldType))
                            {
                                var value = dtoField.GetValue(inObject);
                                classField.SetValue(result, value);
                            }
                            else
                            {
                                throw new FieldTypesNotEqual(classField, dtoField, dtoType, dtoType);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
