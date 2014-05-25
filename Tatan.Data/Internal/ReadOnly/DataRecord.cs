// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Common.Exception;

    /// <summary>
    /// 数据记录
    /// </summary>
    internal class DataRecord : IDataRecord
    {
        private readonly object[] _values;

        private readonly IDictionary<string, int> _schema;

        public DataRecord(IDictionary<string, int> schema)
        {
            _schema = schema;
            _values = new object[schema.Count];
            Id = Common.Guid.New();
        }

        #region IDentifiable
        public string Id { get; internal set; }
        #endregion

        #region IDataRecord
        public int Count { get { return _values.Length; } }

        public object this[string key]
        {
            get
            {
                ExceptionHandler.ArgumentNull("key", key);
                ExceptionHandler.KeyNotFound(_schema, key);
                return this[_schema[key]];
            }
            internal set
            {
                ExceptionHandler.ArgumentNull("key", key);
                ExceptionHandler.KeyNotFound(_schema, key);
                _values[_schema[key]] = value;
            }
        }

        public object this[int index]
        {
            get
            {
                ExceptionHandler.IndexOutOfRange(index);
                return _values[index];
            }
            internal set
            {
                ExceptionHandler.IndexOutOfRange(index);
                _values[index] = value;
            }
        }
        #endregion

        #region IEnumerable
        public IEnumerator<string> GetEnumerator()
        {
            return _schema.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _schema.Keys.GetEnumerator();
        }
        #endregion

        #region Object
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _schema.GetHashCode() + _values.GetHashCode();
        }

        public override string ToString()
        {
            if (_schema.Count == 0 || _values.Length == 0)
                return string.Empty;
            var sb = new StringBuilder(_schema.Count * _values.Length);
            sb.Append('{');
            foreach (var pair in _schema)
            {
                sb.Append("\"").Append(pair.Key).Append("\":").Append(this[pair.Value]).Append(",");
            }
            sb[sb.Length - 1] = '}';
            return sb.ToString();
        }
        #endregion
    }
}