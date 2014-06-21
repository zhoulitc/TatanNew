using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Tatan.Services.Rest
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class Service1 : IService1
    {
        private readonly List<Person> _data = new List<Person>
        {
            new Person{Name="zhouli",Age=26},
            new Person{Name="hedan",Age=22}
        };

        public Person GetData(string name)
        {
            return _data.Find(p => p.Name == name);
        }

        public RestResult AddData(string name, string age)
        {
            foreach (var p in _data)
            {
                if (p.Name == name)
                    return RestResult.Fail("exists");
            }
            _data.Add(new Person{Name=name,Age=int.Parse(age)});
            return RestResult.Success();
        }

        public RestResult DeleteData(string name)
        {
            Person person = null;
            foreach (var p in _data.Where(p => p.Name == name))
            {
                person = p;
            }
            if (person == null) return RestResult.Fail("not exists");
            _data.Remove(person);
            return RestResult.Success();
        }

        public RestResult EditData(string name, string age)
        {
            foreach (var p in _data.Where(p => p.Name == name))
            {
                p.Age = int.Parse(age);
                return RestResult.Success();
            }
            return RestResult.Fail("not exists");
        }
    }
}
