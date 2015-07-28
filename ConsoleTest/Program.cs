
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;

namespace ConsoleTest
{
    using System.Runtime.Serialization;

    /// <summary>
    /// 服务请求对象
    /// </summary>
    [DataContract]
    public class ServiceRequest
    {
        /// <summary>
        /// 在服务器端保存的服务对象全名
        /// </summary>
        [DataMember]
        public string Service { get; set; }

        /// <summary>
        /// 服务的行为名，默认为Call
        /// </summary>
        [DataMember]
        public string Action { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public object[] Arguments { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            int? a = 1;
            int aa = (int)a;

            //string s;
            //int s1 = 1;
            //s = s1.ToString();

            //var i = 1;
            //decimal s = (decimal)i;
            ////bool s1 = (bool)"true";
            ////DateTime s2 = (DateTime)"true";
            //var l = s.ToString();
            //var ss = int.Parse(l);

            //var f1 = typeof(object).GetMethod("ToString", Type.EmptyTypes);
            //var f2 = typeof(int).GetMethod("Parse", new Type[] { typeof(string) });

            //var func1 = GetMapping<AA, AA1>();
            //var func2 = GetMapping<AA, AA2>();
            //var func3 = GetMapping<AA, AA3>();
            //var func4 = GetMapping<AA4, AA>();
            //var func5 = GetMapping<AA, AA5>();
            //var func6 = GetMapping<AA6, AA>();
            //var aa = new AA
            //{
            //    Name = "wahaha",
            //    Age = 18,
            //    Sex = SexType.Nan
            //};
            //var aa1 = func1(aa);
            //var aa2 = func2(aa);
            //var aa3 = func3(aa);
            //var aa4 = new AA4
            //{
            //    Name = "wahaha",
            //    Age = 18,
            //    Sex = SexType.Nan
            //};
            //var aa5 = func5(aa);
            //var aaa = func4(aa4);
            //var aa6 = new AA6
            //{
            //    Name = 1999,
            //    Age = 18,
            //    Sex = SexType.Nan
            //};
            //var aaaa = func6(aa6);
        }

        public void te()
        {

        }

        public static AA4 F(AA a)
        {
            var a4 = new AA4();
            a4.Age = a.Age.ToString();
            return a4;
        }

        /// <summary>
        /// 创建实体映射到另一个实体的委托
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<TSource, T> GetMapping<TSource, T>() where T : new()
        {
            var sourceType = typeof(TSource);
            var type = typeof(T);
            var typeMaps = new Dictionary<Type, OpCode>
            {
                [typeof(sbyte)] = OpCodes.Conv_I1,
                [typeof(short)] = OpCodes.Conv_I2,
                [typeof(int)] = OpCodes.Conv_I4,
                [typeof(long)] = OpCodes.Conv_I8,
                [typeof(byte)] = OpCodes.Conv_U1,
                [typeof(ushort)] = OpCodes.Conv_U2,
                [typeof(uint)] = OpCodes.Conv_U4,
                [typeof(ulong)] = OpCodes.Conv_U8,
                [typeof(char)] = OpCodes.Conv_U2,
                [typeof(float)] = OpCodes.Conv_R4,
                [typeof(double)] = OpCodes.Conv_R8
            };

            //source, dest

            var dynamicMethod = new DynamicMethod(Guid.NewGuid().ToString(), type,
                new Type[] { sourceType }, type, true);
            var generator = dynamicMethod.GetILGenerator();

            // TResult result;
            var result = generator.DeclareLocal(type);

            //result = new TResult();
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));//stack[obj]
            generator.Emit(OpCodes.Stloc, result);//stack[]

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<MappingAttribute>();
                var name = attribute != null ? attribute.Name : property.Name;
                var sourceProperty = sourceType.GetProperty(name);
                if (sourceProperty == null) continue;
                var getMethod = sourceProperty.GetGetMethod();
                if (getMethod == null) continue;
                var setMethod = property.GetSetMethod();
                if (setMethod == null) continue;

                //result.Property = source.Property;
                generator.Emit(OpCodes.Ldloc, result); //stack[result]
                generator.Emit(OpCodes.Ldarg_0); //stack[result,source]
                generator.Emit(OpCodes.Callvirt, getMethod);//stack[result,value]
                if (property.PropertyType != sourceProperty.PropertyType)
                {
                    //result.Property = (Type)source.Property; [Box]
                    if (sourceProperty.PropertyType.IsValueType && 
                        !property.PropertyType.IsValueType)
                    {
                        generator.Emit(OpCodes.Box, sourceProperty.PropertyType);
                    }
                    //result.Property = (Type)source.Property; [Unbox]
                    else if (!sourceProperty.PropertyType.IsValueType && 
                        property.PropertyType.IsValueType)
                    {
                        generator.Emit(OpCodes.Unbox_Any, property.PropertyType);
                    }
                    //result.Property = source.Property as Type;
                    else if (!sourceProperty.PropertyType.IsValueType && 
                        !property.PropertyType.IsValueType)
                    {
                        generator.Emit(OpCodes.Castclass, sourceProperty.PropertyType);
                    }
                    else
                    {
                        if (typeMaps.ContainsKey(property.PropertyType))
                        {
                            generator.Emit(typeMaps[property.PropertyType]);
                        }
                    }
                }
                generator.Emit(OpCodes.Callvirt, setMethod);//stack[]
            }

            //return result;
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            return (Func<TSource, T>)dynamicMethod.CreateDelegate(typeof(Func<TSource, T>));
        }

    }

    public class AA
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public SexType Sex { get; set; }
    }

    public partial class AA1
    {
        public object Name { get; set; }

        public int Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA2
    {
        public string Name { get; set; }

        public object Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA3
    {
        [Mapping(Name = "Name")]
        public string FName { get; set; }

        public int Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA4
    {
        public string Name { get; set; }

        public string Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA5
    {
        public string Name { get; set; }

        public string Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public partial class AA6
    {
        public long? Name { get; set; }

        public long Age { get; set; }

        public SexType Sex { get; set; }

        public string Other { get; set; }
    }

    public enum SexType
    {
        Nv,

        Nan
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class MappingAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
