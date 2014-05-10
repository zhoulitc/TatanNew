namespace Tatan.Common.Serialization.Internal
{
    using System;
    using System.IO;
    using System.Text;

    internal abstract class AbstractSerializer : ISerializer
    {
        protected Func<object, string> SerializeFunction;
        protected Func<string, object> DeserializeFunction;

        protected abstract void SerializeAction<T>(T obj, Type type, MemoryStream ms);
        protected abstract T DeserializeAction<T>(Type type, MemoryStream ms);

        protected AbstractSerializer(Func<object, string> serializeFunction, Func<string, object> deserializeFunction)
        {
            SerializeFunction = serializeFunction;
            DeserializeFunction = deserializeFunction;
        }

        public string Serialize(object obj)
        {
            if (obj == null)
                return string.Empty;
            if (SerializeFunction != null)
                return SerializeFunction(obj);

            using (var ms = new MemoryStream())
            {
                SerializeAction(obj, obj.GetType(), ms);
                return (Encoding.UTF8).GetString(ms.ToArray());
            }
        }

        public T Deserialize<T>(string text)
        {
            if (string.IsNullOrEmpty(text))
                return default(T);
            if (DeserializeFunction != null)
                return (T)DeserializeFunction(text);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(text)))
            {
                return DeserializeAction<T>(typeof(T), ms); 
            }
        }
    }
}