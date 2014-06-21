using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Data;
using Tatan.Permission.Entities;

namespace Tatan.Permission.UnitTest
{
    [TestClass]
    public class RelationTest
    {
        private IDataSource _source;

        [TestInitialize]
        public void Init()
        {
            string p = "System.Data.SQLite";
            string c = @"Data Source=Db\test.db3;Version=3;";
            _source = DataSource.Connect(p, c);
            //_source.Tables.Add(typeof(Tatan.Data.Relation.Fields));
            //_source.Tables.Add(typeof(Tatan.Data.Relation.Tables));
            //_source.UseSession("Fields", session => session.ExecuteAsync("TRUNCATE TABLE Fields"));
            //_source.UseSession("Tables", session => session.ExecuteAsync("TRUNCATE TABLE Tables"));
        }

        [TestMethod]
        public void TestUserGroups()
        {
            var user = new User(1);
            var group = new Group(3);
            user.Groups.Source = _source;

            Assert.IsFalse(user.Groups.Contains(group));
            Assert.IsTrue(user.Groups.Add(group));
            try
            {
                Assert.IsTrue(user.Groups.Add(group));
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "重复记录。");
            }
            Assert.IsTrue(user.Groups.Contains(group));
            var g = user.Groups.GetById(3);
            Assert.AreEqual(g.Id, 3);
            g.Clear();
            Assert.IsTrue(user.Groups.Remove(group));
            try
            {
                Assert.IsTrue(user.Groups.Remove(group));
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "不存在此记录。");
            }
            Assert.IsFalse(user.Groups.Contains(group));
        }

        [TestMethod]
        public void TestUserRoles()
        {
            var user = new User(1);
            var role = new Role(3);
            user.Roles.Source = _source;

            Assert.IsFalse(user.Roles.Contains(role));
            Assert.IsTrue(user.Roles.Add(role));
            Assert.IsTrue(user.Roles.Contains(role));
            var g = user.Roles.GetById(3);
            Assert.AreEqual(g.Id, 3);
            g.Clear();
            Assert.IsTrue(user.Roles.Remove(role));
            Assert.IsFalse(user.Roles.Contains(role));
        }

        [TestMethod]
        public void TestGroupUsers()
        {
            var group = new Group(1);
            var user = new User(3);
            group.Users.Source = _source;

            Assert.IsFalse(group.Users.Contains(user));
            Assert.IsTrue(group.Users.Add(user));
            Assert.IsTrue(group.Users.Contains(user));
            var g = group.Users.GetById(3);
            Assert.AreEqual(g.Id, 3);
            g.Clear();
            Assert.IsTrue(group.Users.Remove(user));
            Assert.IsFalse(group.Users.Contains(user));
        }

        [TestMethod]
        public void TestGroupRoles()
        {
            var group = new Group(1);
            var role = new Role(3);
            group.Roles.Source = _source;

            Assert.IsFalse(group.Roles.Contains(role));
            Assert.IsTrue(group.Roles.Add(role));
            Assert.IsTrue(group.Roles.Contains(role));
            var g = group.Roles.GetById(3);
            Assert.AreEqual(g.Id, 3);
            g.Clear();
            Assert.IsTrue(group.Roles.Remove(role));
            Assert.IsFalse(group.Roles.Contains(role));
        }

        [TestMethod]
        public void TestRoleUsers()
        {
            var role = new Group(1);
            var user = new User(3);
            role.Users.Source = _source;

            Assert.IsFalse(role.Users.Contains(user));
            Assert.IsTrue(role.Users.Add(user));
            Assert.IsTrue(role.Users.Contains(user));
            var g = role.Users.GetById(3);
            Assert.AreEqual(g.Id, 3);
            g.Clear();
            Assert.IsTrue(role.Users.Remove(user));
            Assert.IsFalse(role.Users.Contains(user));
        }

        [TestMethod]
        public void TestRoleGroups()
        {
            var role = new Role(1);
            var group = new Group(3);
            role.Groups.Source = _source;

            Assert.IsFalse(role.Groups.Contains(group));
            Assert.IsTrue(role.Groups.Add(group));
            Assert.IsTrue(role.Groups.Contains(group));
            var g = role.Groups.GetById(3);
            Assert.AreEqual(g.Id, 3);
            g.Clear();
            Assert.IsTrue(role.Groups.Remove(group));
            Assert.IsFalse(role.Groups.Contains(group));
        }

        [TestMethod]
        public void TestRolePermissions()
        {
            var role = new Role(1);
            var permission = new Entities.Permission(3);
            role.Permissions.Source = _source;

            Assert.IsFalse(role.Permissions.Contains(permission));
            Assert.IsTrue(role.Permissions.Add(permission));
            Assert.IsTrue(role.Permissions.Contains(permission));
            var g = role.Permissions.GetById(3);
            Assert.AreEqual(g.Id, 3);
            g.Clear();
            Assert.IsTrue(role.Permissions.Remove(permission));
            Assert.IsFalse(role.Permissions.Contains(permission));
        }

        [TestMethod]
        public void TestGroupPermissions()
        {
            var group = new Group(1);
            var role = new Role(2);
            var permission = new Entities.Permission(3);
            role.Permissions.Source = _source;
            role.Groups.Source = _source;
            group.Permissions.Source = _source;

            //测试从组的角色中寻找某权限
            Assert.IsTrue(role.Permissions.Add(permission));
            Assert.IsTrue(role.Groups.Add(group));
            Assert.IsTrue(group.Permissions.Contains(permission));
            var rg = group.Permissions.GetById(3);
            Assert.AreEqual(rg.Id, 3);
            Assert.IsTrue(role.Permissions.Remove(permission));
            Assert.IsTrue(role.Groups.Remove(group));

            Assert.IsFalse(group.Permissions.Contains(permission));
            Assert.IsTrue(group.Permissions.Add(permission));
            Assert.IsTrue(group.Permissions.Contains(permission));
            var g = group.Permissions.GetById(3);
            Assert.AreEqual(g.Id, 3);
            Assert.IsTrue(group.Permissions.Remove(permission));
            Assert.IsFalse(group.Permissions.Contains(permission));
        }

        [TestMethod]
        public void TestUserPermissions()
        {
            var user = new User(1);
            var group = new Group(2);
            var role = new Role(2);
            var permission = new Entities.Permission(3);
            var permission2 = new Entities.Permission(2);
            user.Groups.Source = _source;
            user.Roles.Source = _source;
            user.Permissions.Source = _source;
            role.Permissions.Source = _source;
            role.Groups.Source = _source;
            group.Permissions.Source = _source;

            //测试从用户的角色和组中寻找某权限
            Assert.IsTrue(user.Roles.Add(role));
            Assert.IsTrue(role.Permissions.Add(permission2));
            Assert.IsTrue(group.Permissions.Add(permission));
            Assert.IsTrue(user.Permissions.Contains(permission));
            Assert.IsTrue(user.Permissions.Contains(permission2));
            var rg = user.Permissions.GetById(3);
            Assert.AreEqual(rg.Id, 3);
            rg = user.Permissions.GetById(2);
            Assert.AreEqual(rg.Id, 2);
            Assert.IsTrue(role.Permissions.Remove(permission2));
            Assert.IsTrue(group.Permissions.Remove(permission));
            Assert.IsTrue(user.Roles.Remove(role));

            Assert.IsFalse(user.Permissions.Contains(permission));
            Assert.IsTrue(user.Permissions.Add(permission));
            Assert.IsTrue(user.Permissions.Contains(permission));
            var g = user.Permissions.GetById(3);
            Assert.AreEqual(g.Id, 3);
            Assert.IsTrue(user.Permissions.Remove(permission));
            Assert.IsFalse(user.Permissions.Contains(permission));
        }
    }
}
