using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tatan.Common.UnitTest
{
    using Serialization;

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
            var json = Serializer.CreateJsonSerializer(
                JsonConvert.SerializeObject,
                JsonConvert.DeserializeObject<User>
                );
            var s1 = json.Serialize(_user);
            Assert.AreEqual(s1, _text);

            var u = json.Deserialize<User>(s1);
            Assert.AreEqual(u, _user);
        }

        [TestMethod]
        public void JsonDeserializeUsingExtension()
        {
            //使用第三方json序列化
            var json = Serializer.CreateJsonSerializer(
                JsonConvert.SerializeObject,
                JsonConvert.DeserializeObject<User>
                );
            var user1 = json.Deserialize<User>(_text);
            Assert.AreEqual(user1, _user);

            var s1 = json.Serialize(user1);
            Assert.AreEqual(s1, _text);
        }

        [TestMethod]
        public void JsonSerializeUsingDefault()
        {
            var json = Serializer.Json;
            //json.UsingDefault();
            var s1 = json.Serialize(_user);
            Assert.AreEqual(s1, _textd);
        }

        [TestMethod]
        public void JsonDeserializeUsingDefault()
        {
            var json = Serializer.Json;
            //json.UsingDefault();
            var user1 = json.Deserialize<UserInfo>("{\"Address\":\"\\\"我也不知道\\\"\",\"Objs\":[33,\"wuyu\",false],\"Sex\":true}");
            Assert.AreEqual(user1.Address, _user.Info.Address);
        }

        [TestMethod]
        public void JsonSerializeEmpty()
        {
            //使用第三方json序列化
            var json = Serializer.Json;
            var s1 = json.Serialize(null);
            Assert.AreEqual(s1, string.Empty);
        }

        [TestMethod]
        public void JsonDeserializeEmpty()
        {
            //使用第三方json序列化
            var json = Serializer.Json;
            var user1 = json.Deserialize<User>(null);
            Assert.AreEqual(user1, null);
        }
    }
}
