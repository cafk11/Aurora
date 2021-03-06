﻿using System;
using System.ComponentModel;

namespace Aurora.Profiles
{
    public class Node
    {
        protected Newtonsoft.Json.Linq.JObject _ParsedData;

        internal Node(string json_data)
        {
            if (String.IsNullOrWhiteSpace(json_data))
            {
                json_data = "{}";
            }
            _ParsedData = Newtonsoft.Json.Linq.JObject.Parse(json_data);
        }

        internal string GetString(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value))
                return value.ToString();
            else
                return "";
        }

        internal int GetInt(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value))
                return Convert.ToInt32(value.ToString());
            else
                return -1;
        }

        internal float GetFloat(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;

            if (_ParsedData.TryGetValue(Name, out value))
                return Convert.ToSingle(value.ToString());
            else
                return -1.0f;
        }

        internal long GetLong(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;

            if (_ParsedData.TryGetValue(Name, out value))
                return Convert.ToInt64(value.ToString());
            else
                return -1;
        }

        internal T GetEnum<T>(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value) && !String.IsNullOrWhiteSpace(value.ToString()))
            {
                var type = typeof(T);
                if (!type.IsEnum) throw new InvalidOperationException();
                foreach (var field in type.GetFields())
                {
                    var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute != null)
                    {
                        if (attribute.Description.ToLowerInvariant().Equals(value.ToString().ToLowerInvariant()))
                            return (T)field.GetValue(null);
                    }

                    if (field.Name.ToLowerInvariant().Equals(value.ToString().ToLowerInvariant()))
                        return (T)field.GetValue(null);
                }

                return (T)Enum.Parse(typeof(T), "Undefined", true);
            }
            else
                return (T)Enum.Parse(typeof(T), "Undefined", true);
        }

        internal bool GetBool(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value) && value.ToObject<bool>())
                return value.ToObject<bool>();
            else
                return false;
        }

        internal T[] GetArray<T>(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;

            if (_ParsedData.TryGetValue(Name, out value))
                return value.ToObject<T[]>();
            else
                return new T[] { };
        }
    }
}
