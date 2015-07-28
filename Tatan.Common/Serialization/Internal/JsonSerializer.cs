namespace Tatan.Common.Serialization.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;

    /// <summary>
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal sealed class JsonSerializer : AbstractSerializer
    {
        #region 单例

        private static readonly JsonSerializer _instance = new JsonSerializer(null, null);
        private static readonly Type _dictionaryType = typeof (IDictionary);
        private readonly IDictionary<Type, DataContractJsonSerializer> _typeMap;
        public static JsonSerializer Instance => _instance;

        #endregion

        public JsonSerializer(Func<object, string> serializeFunction, Func<string, object> deserializeFunction)
            : base(serializeFunction, deserializeFunction)
        {
            _typeMap = new Dictionary<Type, DataContractJsonSerializer>();
            var type = typeof(IDictionary<string, object>);
            _typeMap.Add(type,
                new DataContractJsonSerializer(type,
                    new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true }));
        }

        protected override void SerializeAction<T>(T obj, Type type, MemoryStream ms)
        {
            if (!_typeMap.ContainsKey(type))
            {
                lock (_typeMap)
                {
                    _typeMap.Add(type, new DataContractJsonSerializer(type));
                }
            }
            _typeMap[type].WriteObject(ms, obj);
        }

        protected override T DeserializeAction<T>(Type type, MemoryStream ms)
        {
            if (!_typeMap.ContainsKey(type))
            {
                lock (_typeMap)
                {
                    _typeMap.Add(type, new DataContractJsonSerializer(type,
                        new DataContractJsonSerializerSettings() {UseSimpleDictionaryFormat = true}));
                }
            }
            return (T) _typeMap[type].ReadObject(ms);
        }

        #region 自定义转换为JSON字符串的方法

        ///// <summary>
        ///// Json序列化类
        ///// </summary>
        //public static class CustomJSONSerialize
        //{
        //    /// <summary>
        //    /// 将任意对象转换成json串
        //    /// <para>T[]：任意数组类型，转换格式为：“["value1","value2",...]”</para>
        //    /// <para>IList(T)：任意列表类型，转换格式为：“["value1","value2",...]”</para>
        //    /// <para>IDictionary(K,V)：任意映射类型，转换格式为：“{"name1":"value1","name2":"value2",...}”</para>
        //    /// <para>DataRow：数据行类型，转换格式为：“{"name1":"value1","name2":"value2",...}”</para>
        //    /// <para>DataTable：数据表类型，转换格式为：“{"TableName":"?","Rows":[{"name1":"value1",...},...]}”</para>
        //    /// <para>DataSet：数据集类型，转换格式为：“{"DataSetName":"?","Tables":[{"TableName":"?","Rows":[{"name1":"value1",...},...]},...]}”</para>
        //    /// <para>Entity：实体类型，转换格式为：“{"name1":"value1","name2":"value2",...}”</para>
        //    /// </summary>
        //    /// <param name="o">对象</param>
        //    /// <returns>json串</returns>
        //    public static string From(object o)
        //    {
        //        string result = _FromObject(o);
        //        if (result.Length > 2 &&
        //            ((result[0] == '{' && result[result.Length - 1] == '}') ||
        //            (result[0] == '[' && result[result.Length - 1] == ']')))
        //        {
        //            return result;
        //        }
        //        return string.Empty;
        //    }

        //    /// <summary>
        //    /// 将Object转换成json串
        //    /// </summary>
        //    /// <param name="o">对象</param>
        //    /// <returns>json串</returns>
        //    private static string _FromObject(object o)
        //    {
        //        string result = "\"\"";
        //        if (o != null)
        //        {
        //            Type cls = o.GetType();
        //            if (o is string || o is StringBuilder || o is DateTime)
        //            {
        //                result = _FromString(o.ToString());
        //            }
        //            else if (cls.IsPrimitive || o is decimal)
        //            {
        //                result = o.ToString();
        //            }
        //            else if (cls.IsArray)
        //            {
        //                result = _FromArray((object[])o);
        //            }
        //            else if (o is IList && cls.IsGenericType)
        //            {
        //                result = _FromList((IList)o);
        //            }
        //            else if (o is IDictionary && cls.IsGenericType)
        //            {
        //                result = _FromMap((IDictionary)o);
        //            }
        //            else if (o is DataRow)
        //            {
        //                result = _FromDataRow((DataRow)o);
        //            }
        //            else if (o is DataTable)
        //            {
        //                result = _FromDataTable((DataTable)o);
        //            }
        //            else if (o is DataSet)
        //            {
        //                result = _FromDataSet((DataSet)o);
        //            }
        //            else
        //            {
        //                result = _FromEntity(o);
        //            }
        //        }
        //        return result;
        //    }

        //    /// <summary>
        //    /// 将String转换成json串
        //    /// </summary>
        //    /// <param name="s">字符串</param>
        //    /// <returns>json串</returns>
        //    private static string _FromString(string s)
        //    {
        //        string result = "\"\"";
        //        if (s != null)
        //        {
        //            StringBuilder sb = new StringBuilder(s.Length);
        //            sb.Append("\"");
        //            foreach (char c in s)
        //            {
        //                switch (c)
        //                {
        //                    case '\"':
        //                        sb.Append("\\\"");
        //                        break;
        //                    case '\\':
        //                        sb.Append("\\\\");
        //                        break;
        //                    case '\b':
        //                        sb.Append("\\b");
        //                        break;
        //                    case '\f':
        //                        sb.Append("\\f");
        //                        break;
        //                    case '\n':
        //                        sb.Append("\\n");
        //                        break;
        //                    case '\r':
        //                        sb.Append("\\r");
        //                        break;
        //                    case '\t':
        //                        sb.Append("\\t");
        //                        break;
        //                    case '/':
        //                        sb.Append("\\/");
        //                        break;
        //                    default:
        //                        if (c >= '\u0000' && c <= '\u001F')
        //                        {
        //                            sb.Append("\\u");
        //                            string ss = String.Format("{0:X}", c);
        //                            for (int i = 0; i < 4 - ss.Length; i++)
        //                            {
        //                                sb.Append('0');
        //                            }
        //                            sb.Append(ss.ToUpper());
        //                        }
        //                        else
        //                        {
        //                            sb.Append(c);
        //                        }
        //                        break;
        //                }
        //            }
        //            result = sb.Append("\"").ToString();
        //        }
        //        return result;
        //    }

        //    /// <summary>
        //    /// 将Array转换成json串
        //    /// </summary>
        //    /// <param name="list">列表</param>
        //    /// <returns>json串</returns>
        //    private static string _FromArray(object[] array)
        //    {
        //        StringBuilder sb = _GetStringBuilder(array.Length);
        //        sb.Append("[");
        //        foreach (object o in array)
        //        {
        //            sb.AppendFormat("{0},", _FromObject(o));
        //        }
        //        sb.Remove(sb.Length - 1, 1).Append("]");
        //        return sb.ToString();
        //    }

        //    /// <summary>
        //    /// 将List转换成json串
        //    /// </summary>
        //    /// <param name="list">列表</param>
        //    /// <returns>json串</returns>
        //    private static string _FromList(IList list)
        //    {
        //        StringBuilder sb = _GetStringBuilder(list.Count);
        //        sb.Append("[");
        //        foreach (object o in list)
        //        {
        //            sb.AppendFormat("{0},", _FromObject(o));
        //        }
        //        sb.Remove(sb.Length - 1, 1).Append("]");
        //        return sb.ToString();
        //    }

        //    /// <summary>
        //    /// 将Map转换成json串
        //    /// </summary>
        //    /// <param name="map">映射</param>
        //    /// <returns>json串</returns>
        //    private static string _FromMap(IDictionary map)
        //    {
        //        StringBuilder sb = _GetStringBuilder(map.Count);
        //        sb.Append("{");
        //        foreach (string key in map.Keys)
        //        {
        //            sb.AppendFormat("\"{0}\":{1},", key, _FromObject(map[key]));
        //        }
        //        sb.Remove(sb.Length - 1, 1).Append("}");
        //        return sb.ToString();
        //    }

        //    /// <summary>
        //    /// 将DataRow转换成json串
        //    /// </summary>
        //    /// <param name="dr">数据行</param>
        //    /// <returns>json串</returns>
        //    private static string _FromDataRow(DataRow dr)
        //    {
        //        StringBuilder sb = _GetStringBuilder(dr.Table.Columns.Count);
        //        sb.Append("{");
        //        for (int i = 0, count = dr.Table.Columns.Count; i < count; i++)
        //        {
        //            sb.AppendFormat("\"{0}\":{1},", dr.Table.Columns[i].ColumnName, _FromObject(dr[i]));
        //        }
        //        sb.Remove(sb.Length - 1, 1).Append("}");
        //        return sb.ToString();
        //    }

        //    /// <summary>
        //    /// 将DataTable转换成json串
        //    /// </summary>
        //    /// <param name="dt">数据表</param>
        //    /// <returns>json串</returns>
        //    private static string _FromDataTable(DataTable dt)
        //    {
        //        StringBuilder sb = _GetStringBuilder(dt.Columns.Count * dt.Rows.Count);
        //        sb.Append("{\"").Append(dt.TableName).Append("\":[");
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            sb.Append(_FromDataRow(dr)).Append(",");
        //        }
        //        sb.Remove(sb.Length - 1, 1).Append("]}");
        //        return sb.ToString();
        //    }

        //    /// <summary>
        //    /// 将DataSet转换成json串
        //    /// </summary>
        //    /// <param name="ds">数据集</param>
        //    /// <returns>json串</returns>
        //    private static string _FromDataSet(DataSet ds)
        //    {
        //        StringBuilder sb = _GetStringBuilder(ds.Tables.Count * 500); //假设一个Table平均长度为500
        //        sb.Append("{\"").Append(ds.DataSetName).Append("\":[");
        //        foreach (DataTable dt in ds.Tables)
        //        {
        //            sb.Append(_FromDataTable(dt)).Append(",");
        //        }
        //        sb.Remove(sb.Length - 1, 1).Append("]}");
        //        return sb.ToString();
        //    }

        //    /// <summary>
        //    /// 将实体类转换成json串
        //    /// </summary>
        //    /// <param name="entity">实体类</param>
        //    /// <returns>json串</returns>
        //    private static string _FromEntity(object entity)
        //    {
        //        string result = "{}";
        //        Type cls = entity.GetType();
        //        FieldInfo[] fields = cls.GetFields(BindingFlags.Instance | BindingFlags.Public);
        //        PropertyInfo[] properties = cls.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        //        StringBuilder sb = _GetStringBuilder(fields.Length + properties.Length);
        //        sb.Append("{");
        //        if (fields.Length > 0)
        //        {
        //            foreach (FieldInfo field in fields)
        //            {
        //                if (!field.IsStatic)
        //                {
        //                    sb.AppendFormat("\"{0}\":{1},", field.Name, _FromObject(field.GetValue(entity)));
        //                }
        //            }
        //        }
        //        if (properties.Length > 0)
        //        {
        //            foreach (PropertyInfo property in properties)
        //            {
        //                if (property.CanRead)
        //                {
        //                    sb.AppendFormat("\"{0}\":{1},", property.Name, _FromObject(property.GetValue(entity, null)));
        //                }
        //            }
        //        }
        //        if (sb.Length > 1)
        //        {
        //            result = sb.Remove(0, sb.Length - 1).Append("}").ToString();
        //        }
        //        return result;
        //    }

        //    /// <summary>
        //    /// 获取一个预计大小的StringBuilder
        //    /// </summary>
        //    /// <param name="count">元素个数</param>
        //    /// <returns>字符序列对象</returns>
        //    private static StringBuilder _GetStringBuilder(int count)
        //    {
        //        int avg = int.MaxValue / count;
        //        if (avg > 16) avg = 16; //16代表键值对的默认平均长度
        //        return new StringBuilder(count * avg);
        //    }
        //}

        #endregion
    }
}