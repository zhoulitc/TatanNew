// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Common.Exception;

    /// <summary>
    /// 数据文档
    /// </summary>
    internal class DataDocument : IDataDocument
    {
        private readonly IList<IDataRecord> _records;

        private readonly IDictionary<string, int> _schema;

        internal DataDocument(IDataReader reader)
        {
            _records = new List<IDataRecord>();
            _schema = new Dictionary<string, int>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                _schema.Add(reader.GetName(i), i);
            }
            while (reader.Read())
            {
                var record = new DataRecord(_schema);
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    record[reader.GetName(i)] = reader.GetValue(i);
                }
                _records.Add(record);
            }
        }

        #region IDataDocument
        public int Count { get { return _records.Count; } }

        public IDataRecord this[int index]
        {
            get
            {
                ExceptionHandler.IndexOutOfRange(index, _records.Count);
                return _records[index];
            }
        }
        #endregion

        #region IEnumerable
        public IEnumerator<IDataRecord> GetEnumerator()
        {
            return _records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _records.GetEnumerator();
        }
        #endregion

        #region IObject
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _records.GetHashCode() ^ _schema.GetHashCode();
        }

        public override string ToString()
        {
            if (_records.Count == 0)
                return "[]";
            var sb = new StringBuilder(_records.Count * 10);
            sb.Append('[');
            foreach (var record in _records)
            {
                sb.AppendFormat("{0},", record.ToString());
            }
            sb[sb.Length - 1] = ']';
            return sb.ToString();
        }
        #endregion
    }
}