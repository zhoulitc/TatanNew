namespace Tatan.Common.Serialization.Internal
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Xml = System.Xml.Serialization.XmlSerializer;

    /// <summary>
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    internal sealed class XmlSerializer : AbstractSerializer
    {
        #region 单例

        private static readonly XmlSerializer _instance = new XmlSerializer(null, null);
        private readonly IDictionary<Type, Xml> _typeMap;
        public static XmlSerializer Instance => _instance;

        #endregion

        public XmlSerializer(Func<object, string> serializeFunction, Func<string, object> deserializeFunction)
            : base(serializeFunction, deserializeFunction)
        {
            _typeMap = new Dictionary<Type, Xml>();
        }

        protected override void SerializeAction<T>(T obj, Type type, MemoryStream ms)
        {
            if (!_typeMap.ContainsKey(type))
            {
                lock (_typeMap)
                {
                    _typeMap.Add(type, new Xml(type));
                }
            }
            _typeMap[type].Serialize(ms, obj);
        }

        protected override T DeserializeAction<T>(Type type, MemoryStream ms)
        {
            if (!_typeMap.ContainsKey(type))
            {
                lock (_typeMap)
                {
                    _typeMap.Add(type, new Xml(type));
                }
            }
            return (T) _typeMap[type].Deserialize(ms);
        }

        #region 自定义XML转换方法

        //internal static class CustomXMLSerialize
        //{
        //    public static readonly string LIST_FLAG = "_";
        //    public static readonly string XML_ROOT = "root";
        //    public static readonly string XML_HEADER_FORMAT = "<?xml version=\"1.0\" encoding=\"utf-8\"?><{0}>{1}</{2}>";

        //    public static string From(object o)
        //    {
        //        return From(o, XML_ROOT);
        //    }

        //    public static string From(object o, string root)
        //    {
        //        string result = _FromObject(o);
        //        if (result.Length > 1 && result[0] == '<' && result[result.Length - 1] == '>')
        //        {
        //            return string.Format(XML_HEADER_FORMAT, root, result, root);
        //        }
        //        return string.Empty;
        //    }

        //    private static string _FromObject(object o)
        //    {
        //        string result = string.Empty;
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

        //    private static string _FromString(string s)
        //    {
        //        return s;
        //    }

        //    private static string _FromArray(object[] array)
        //    {
        //        StringBuilder sb = _GetStringBuilder(array.Length);
        //        foreach (object o in array)
        //        {
        //            sb.AppendFormat("<{0}>{1}</{2}>", LIST_FLAG, _FromObject(o), LIST_FLAG);
        //        }
        //        return sb.ToString();
        //    }

        //    private static string _FromList(IList list)
        //    {
        //        StringBuilder sb = _GetStringBuilder(list.Count);
        //        foreach (object o in list)
        //        {
        //            sb.AppendFormat("<{0}>{1}</{2}>", LIST_FLAG, _FromObject(o), LIST_FLAG);
        //        }
        //        return sb.ToString();
        //    }

        //    private static string _FromMap(IDictionary map)
        //    {
        //        StringBuilder sb = _GetStringBuilder(map.Count);
        //        foreach (string key in map.Keys)
        //        {
        //            sb.AppendFormat("<{0}>{1}</{2}>", key, _FromObject(map[key]), key);
        //        }
        //        return sb.ToString();
        //    }

        //    private static string _FromDataRow(DataRow dr)
        //    {
        //        StringBuilder sb = _GetStringBuilder(dr.Table.Columns.Count);
        //        sb.Append("<Row>");
        //        for (int i = 0, count = dr.Table.Columns.Count; i < count; i++)
        //        {
        //            sb.AppendFormat("<{0}>{1}</{2}>", dr.Table.Columns[i].ColumnName, _FromObject(dr[i]), dr.Table.Columns[i].ColumnName);
        //        }
        //        sb.Append("</Row>");
        //        return sb.ToString();
        //    }

        //    private static string _FromDataTable(DataTable dt)
        //    {
        //        StringBuilder sb = _GetStringBuilder(dt.Columns.Count * dt.Rows.Count);
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            sb.AppendFormat("<{0}>{1}</{2}>", LIST_FLAG, _FromDataRow(dr), LIST_FLAG);
        //        }
        //        return sb.ToString();
        //    }

        //    private static string _FromDataSet(DataSet ds)
        //    {
        //        StringBuilder sb = _GetStringBuilder(ds.Tables.Count * 800); //假设一个Table平均长度为800
        //        foreach (DataTable dt in ds.Tables)
        //        {
        //            sb.AppendFormat("<{0}>{1}</{2}>", LIST_FLAG, _FromDataTable(dt), LIST_FLAG);
        //            sb.Append(_FromDataTable(dt));
        //        }
        //        return sb.ToString();
        //    }

        //    private static string _FromEntity(object entity)
        //    {
        //        string result = string.Empty;
        //        Type cls = entity.GetType();
        //        FieldInfo[] fields = cls.GetFields(BindingFlags.Instance | BindingFlags.Public);
        //        PropertyInfo[] properties = cls.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        //        StringBuilder sb = _GetStringBuilder(fields.Length + properties.Length);
        //        if (fields.Length > 0)
        //        {
        //            foreach (FieldInfo field in fields)
        //            {
        //                if (!field.IsStatic)
        //                {
        //                    sb.AppendFormat("<{0}>{1}</{2}>", field.Name, _FromObject(field.GetValue(entity)), field.Name);
        //                }
        //            }
        //        }
        //        if (properties.Length > 0)
        //        {
        //            foreach (PropertyInfo property in properties)
        //            {
        //                if (property.CanRead)
        //                {
        //                    sb.AppendFormat("<{0}>{1}</{2}>", property.Name, _FromObject(property.GetValue(entity, null)), property.Name);
        //                }
        //            }
        //        }
        //        return result;
        //    }

        //    private static StringBuilder _GetStringBuilder(int count)
        //    {
        //        int avg = int.MaxValue / count;
        //        if (avg > 40) avg = 40; //40代表键值对的默认平均长度
        //        return new StringBuilder(count * avg);
        //    }
        //}

        #endregion
    }
}