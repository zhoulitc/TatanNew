using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public class JSONSerializerTest
    {
        User user = new User()
        {
            Id = 1,
            Name = "wahaha",
            Info = new UserInfo()
            {
                Sex = true,
                Address = "\"我也不知道\"",
                Objs = new object[3]
                    {
                        33,
                        "wuyu",
                        false
                    }
            }
        };
        string textd = "{\"Id\":1,\"Info\":{\"Address\":\"\\\"我也不知道\\\"\",\"Objs\":[33,\"wuyu\",false],\"Sex\":true},\"Name\":\"wahaha\"}";
        string text = "{\"Id\":1,\"Name\":\"wahaha\",\"Info\":{\"Sex\":true,\"Address\":\"\\\"我也不知道\\\"\",\"Objs\":[33,\"wuyu\",false]}}";

        [TestMethod]
        public void JsonSerializeUsingExtension()
        {
            //使用第三方json序列化
            var json = Serializer.Json;
            //json.UsingExtension(
            //    (o) => JsonConvert.SerializeObject(o), 
            //    (s) => JsonConvert.DeserializeObject<User>(s));
            var s1 = json.Serialize(user);
            Assert.AreEqual(s1, text);

            var u = json.Deserialize<User>(s1);
            Assert.AreEqual(u, user);
        }

        [TestMethod]
        public void JsonDeserializeUsingExtension()
        {
            //使用第三方json序列化
            var json = Serializer.Json;
            //json.UsingExtension(
            //    (o) => JsonConvert.SerializeObject(o),
            //    (s) => JsonConvert.DeserializeObject<User>(s));
            var user1 = json.Deserialize<User>(text);
            Assert.AreEqual(user1, user);

            var s1 = json.Serialize(user1);
            Assert.AreEqual(s1, text);
        }

        [TestMethod]
        public void JsonSerializeUsingDefault()
        {
            var json = Serializer.Json;
            //json.UsingDefault();
            var s1 = json.Serialize(user);
            Assert.AreEqual(s1, textd);
        }

        [TestMethod]
        public void JsonDeserializeUsingDefault()
        {
            var json = Serializer.Json;
            //json.UsingDefault();
            var user1 = json.Deserialize<UserInfo>("{\"Address\":\"\\\"我也不知道\\\"\",\"Objs\":[33,\"wuyu\",false],\"Sex\":true}");
            Assert.AreEqual(user1.Address, user.Info.Address);
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
