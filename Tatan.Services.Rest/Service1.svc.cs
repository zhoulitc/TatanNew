using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Tatan.Net.Wcf;

namespace Tatan.Services.Rest
{
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class Service1 : IAction
    {
        private readonly List<Person> _data = new List<Person>
        {
            new Person{Name="zhouli",Age=26},
            new Person{Name="hedan",Age=22}
        };

        [Action(Method = HttpMethod.Get)]
        public object GetData(string name)
        {
            return _data.Find(p => p.Name == name);
        }

        [Action(Method = HttpMethod.Post)]
        public bool AddData(string name, int age)
        {
            foreach (var p in _data)
            {
                if (p.Name == name)
                    return false;
            }
            _data.Add(new Person { Name = name, Age = age });
            return true;
        }

        [Action(Method = HttpMethod.Delete)]
        public bool DeleteData(string name)
        {
            Person person = null;
            foreach (var p in _data.Where(p => p.Name == name))
            {
                person = p;
            }
            if (person == null) return false;
            _data.Remove(person);
            return true;
        }

        [Action(Method = HttpMethod.Put)]
        public bool EditData(string name, int age)
        {
            foreach (var p in _data.Where(p => p.Name == name))
            {
                p.Age = age;
                return true;
            }
            return false;
        }
    }
}
