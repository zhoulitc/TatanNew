using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tatan.Common.UnitTest
{
    using System;
    using Serialization;
    using Tatan.Common.Extension.Object;
    using Tatan.Common.Extension.String.Convert;

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserInfo Info { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is User)
            {
                var user = (User)obj;
                return user.Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class UserInfo
    {
        public bool Sex { get; set; }
        public string Address { get; set; }
        public object[] Objs { get; set; }
    }

    [TestClass]
    public class JsonSerializerTest
    {
        readonly User _user = new User
        {
            Id = 1,
            Name = "wahaha",
            Info = new UserInfo
            {
                Sex = true,
                Address = "\"我也不知道\"",
                Objs = new object[]
                    {
                        33,
                        "wuyu",
                        false
                    }
            }
        };

        private const string _textd = "{\"Id\":1,\"Info\":{\"Address\":\"\\\"我也不知道\\\"\",\"Objs\":[33,\"wuyu\",false],\"Sex\":true},\"Name\":\"wahaha\"}";
        private const string _text = "{\"Id\":1,\"Name\":\"wahaha\",\"Info\":{\"Sex\":true,\"Address\":\"\\\"我也不知道\\\"\",\"Objs\":[33,\"wuyu\",false]}}";

        [TestMethod]
        public void JsonSerializeUsingExtension()
        {
            //使用第三方json序列化
            Component.ComponentManager.Register(new JsonSerializerAdapter(new NewtonsoftJsonSerializers()));

            var s1 = _user.ToJsonString();
            Assert.AreEqual(s1, _text);

            var u = s1.AsObject<User>();
            Assert.AreEqual(u, _user);
        }

        [TestMethod]
        public void JsonDeserializeUsingExtension()
        {
            //使用第三方json序列化
            Component.ComponentManager.Register(new JsonSerializerAdapter(new NewtonsoftJsonSerializers()));
            var user1 = _text.AsObject<User>();
            Assert.AreEqual(user1, _user);

            var s1 = user1.ToJsonString();
            Assert.AreEqual(s1, _text);
        }

        [TestMethod]
        public void JsonSerializeUsingDefault()
        {
            Component.ComponentManager.Register(new JsonSerializerAdapter());
            //json.UsingDefault();
            var s1 = _user.ToJsonString();
            Assert.AreEqual(s1, _textd);
        }

        [TestMethod]
        public void JsonDeserializeUsingDefault()
        {
            Component.ComponentManager.Register(new JsonSerializerAdapter());
            //json.UsingDefault();
            var user1 = "{\"Address\":\"\\\"我也不知道\\\"\",\"Objs\":[33,\"wuyu\",false],\"Sex\":true}".AsObject<UserInfo>();
            Assert.AreEqual(user1.Address, _user.Info.Address);
        }

        [TestMethod]
        public void JsonSerializeEmpty()
        {
            Component.ComponentManager.Register(new JsonSerializerAdapter());
            //使用第三方json序列化
            object oo = null;
            var s1 = oo.ToJsonString();
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void JsonDeserializeEmpty()
        {
            Component.ComponentManager.Register(new JsonSerializerAdapter());
            //使用第三方json序列化
            string oo = null;
            var user1 = oo.AsObject<User>();
            Assert.AreEqual(user1, null);
        }

        public class NewtonsoftJsonSerializers : ISerializer
        {
            public T Deserialize<T>(string text)
            {
                return JsonConvert.DeserializeObject<T>(text);
            }

            public string Serialize(object obj)
            {
                return JsonConvert.SerializeObject(obj);
            }
        }
    }
}
