namespace Tatan.Common.Extension.Reflect
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Data;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Exception;

    /// <summary>
    /// 反射相关扩展类
    /// <para>author:zhoulitcqq</para>
    /// </summary>
    public static class ReflectExtension
    {
        /// <summary>
        /// 创建一个委托
        /// </summary>
        /// <param name="method"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        public static void CreateDelegate<T>(this MethodInfo method, out T result)
            => result = (T)(object)Delegate.CreateDelegate(typeof(T), method);

        /// <summary>
        /// 获得一个类型的单例对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetSingleInstance<T>(this Type type) where T : class
        {
            var property = type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
            if (property != null)
            {
                return property.GetValue(null) as T;
            }
            var method = type.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public);
            if (method != null)
            {
                return method.Invoke(null, null) as T;
            }
            return null;
        }

        /// <summary>
        /// 获得一个类型的属性(不包括父类型)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetDeclaredOnlyProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// 调用当前类型的非继承公开方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static object InvokeDeclaredOnly(this object obj, string method, params object[] arguments)
        {
            Assert.ArgumentNotNull(nameof(method), method);
            Assert.ArgumentNotNull(nameof(arguments), arguments);

            var type = obj.GetType();
            var methodInfo = type.GetMethod(method,
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo == null)
            {
                var property = type.GetProperty(method,
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    return null;
                return property.GetValue(obj);
            }
            return methodInfo.Invoke(obj, arguments);
        }

        private static class Builder<T>
        {
            public static Func<IDataRecord, T, T> AsEntity;
        }

        private static readonly Type _objectType = typeof(object);

        private static readonly Type _stringType = typeof(string);

        private static readonly MethodInfo _getValueMethod = typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });

        private static readonly MethodInfo _isDBNullMethod = typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });

        private static readonly MethodInfo _toString = typeof(object).GetMethod("ToString", Type.EmptyTypes);

        private static Func<IDataRecord, T, T> Build<T>(IDataRecord record)
        {
            var type = typeof(T);
            var dynamicMethod = new DynamicMethod(Guid.NewGuid().ToString(), type, 
                new Type[] { typeof(IDataRecord), typeof(T) }, type, true);
            var generator = dynamicMethod.GetILGenerator();

            // T result;
            var result = generator.DeclareLocal(type);

            //result = entity;
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stloc, result);

            for (var i = 0; i < record.FieldCount; i++)
            {
                var propertyInfo = type.GetProperty(record.GetName(i));
                if (propertyInfo == null) continue;
                var setMethod = propertyInfo.GetSetMethod(true);
                if (setMethod == null) continue;

                var endIfLabel = generator.DefineLabel();

                //if (record.IsDBNull(i)) goto endIf;
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Callvirt, _isDBNullMethod);
                generator.Emit(OpCodes.Brtrue, endIfLabel);

                //result.Property = (Type)record[i];
                //result.Property = record[i] as Type;
                generator.Emit(OpCodes.Ldloc, result);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Callvirt, _getValueMethod);
                var fieldType = record.GetFieldType(i);
                if (fieldType.IsValueType)
                    generator.Emit(OpCodes.Unbox_Any, fieldType);
                else
                    generator.Emit(OpCodes.Castclass, fieldType);
                generator.Emit(OpCodes.Callvirt, setMethod);

                // Label endIf:
                generator.MarkLabel(endIfLabel);
            }

            // return result;
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            return (Func<IDataRecord, T, T>)dynamicMethod.CreateDelegate(typeof(Func<IDataRecord, T, T>));
        }

        /// <summary>
        /// 将数据记录转换到对应的实体
        /// </summary>
        /// <param name="record"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T AsEntity<T>(this IDataRecord record, T entity = null) where T : class
        {
            if (entity == null)
            {
                return null;
            }
            if (Builder<T>.AsEntity == null)
            {
                Builder<T>.AsEntity = Build<T>(record);
            }
            return Builder<T>.AsEntity(record, entity);
        }

        /// <summary>
        /// 获取Get属性的反射方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Func<object, object> GetGetMethod(this Type type, string propertyName)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            Assert.ArgumentNotNull(nameof(propertyName), propertyName);
            var property = type.GetProperty(propertyName);
            return GetGetMethod(type, property);
        }

        /// <summary>
        /// 获取Get属性的反射方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Func<object, object> GetGetMethod(this Type type, PropertyInfo property)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            Assert.ArgumentNotNull(nameof(property), property);
            if (!property.CanRead)
                return null;
            DynamicMethod getmethod = new DynamicMethod(Guid.NewGuid().ToString(), _objectType, new Type[] { _objectType }, type, true);
            var generator = getmethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, property.GetMethod);
            if (property.GetMethod.ReturnType.IsValueType)
                generator.Emit(OpCodes.Box, property.GetMethod.ReturnType);
            generator.Emit(OpCodes.Ret);
            return (Func<object, object>)getmethod.CreateDelegate(typeof(Func<object, object>));
        }

        /// <summary>
        /// 获取Set属性的反射方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Action<object, object> GetSetMethod(this Type type, string propertyName)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            Assert.ArgumentNotNull(nameof(propertyName), propertyName);
            var property = type.GetProperty(propertyName);
            return GetSetMethod(type, property);
        }

        /// <summary>
        /// 获取Set属性的反射方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Action<object, object> GetSetMethod(this Type type, PropertyInfo property)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            Assert.ArgumentNotNull(nameof(property), property);
            if (!property.CanWrite)
                return null;
            DynamicMethod setmethod = new DynamicMethod(Guid.NewGuid().ToString(), null, new Type[] { _objectType, _objectType }, type, true);
            var generator = setmethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            if (property.GetMethod.ReturnType.IsValueType)
                generator.Emit(OpCodes.Unbox_Any, property.GetMethod.ReturnType);
            generator.Emit(OpCodes.Callvirt, property.SetMethod);
            generator.Emit(OpCodes.Ret);
            return (Action<object, object>)setmethod.CreateDelegate(typeof(Action<object, object>));
        }

        /// <summary>
        /// 获取一个无参数的构造函数委托
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<TResult> GetConstructor<TResult>(this Type type)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            DynamicMethod constructor = new DynamicMethod(Guid.NewGuid().ToString(), typeof(TResult), Type.EmptyTypes, type, true);
            var generator = constructor.GetILGenerator();
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Ret);
            return (Func<TResult>)constructor.CreateDelegate(typeof(Func<TResult>));
        }

        /// <summary>
        /// 获取一个单参数的构造函数委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<T, TResult> GetConstructor<T, TResult>(this Type type)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            var types = new Type[] { typeof(T) };
            DynamicMethod constructor = new DynamicMethod(Guid.NewGuid().ToString(), typeof(TResult), types, type, true);
            var generator = constructor.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(types));
            generator.Emit(OpCodes.Ret);
            return (Func<T, TResult>)constructor.CreateDelegate(typeof(Func<T, TResult>));
        }

        /// <summary>
        /// 获取一个双参数的构造函数委托
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<T1, T2, TResult> GetConstructor<T1, T2, TResult>(this Type type)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            var types = new Type[] { typeof(T1), typeof(T2) };
            DynamicMethod constructor = new DynamicMethod(Guid.NewGuid().ToString(), typeof(TResult), types, type, true);
            var generator = constructor.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(types));
            generator.Emit(OpCodes.Ret);
            return (Func<T1, T2, TResult>)constructor.CreateDelegate(typeof(Func<T1, T2, TResult>));
        }

        /// <summary>
        /// 获取一个三参数的构造函数委托
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, TResult> GetConstructor<T1, T2, T3, TResult>(this Type type)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            var types = new Type[] { typeof(T1), typeof(T2), typeof(T3) };
            DynamicMethod constructor = new DynamicMethod(Guid.NewGuid().ToString(), typeof(TResult), types, type, true);
            var generator = constructor.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(types));
            generator.Emit(OpCodes.Ret);
            return (Func<T1, T2, T3, TResult>)constructor.CreateDelegate(typeof(Func<T1, T2, T3, TResult>));
        }

        /// <summary>
        /// 获取一个四参数的构造函数委托
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, T4, TResult> GetConstructor<T1, T2, T3, T4, TResult>(this Type type)
        {
            Assert.ArgumentNotNull(nameof(type), type);
            var types = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
            DynamicMethod constructor = new DynamicMethod(Guid.NewGuid().ToString(), typeof(TResult), types, type, true);
            var generator = constructor.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Ldarg_3);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(types));
            generator.Emit(OpCodes.Ret);
            return (Func<T1, T2, T3, T4, TResult>)constructor.CreateDelegate(typeof(Func<T1, T2, T3, T4, TResult>));
        }

        /// <summary>
        /// 将一个类型实例克隆成另一个类型实例(强类型)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CloneTo<TSource, T>(this TSource obj) 
            where T : class, new()
            where TSource: class
        {
            if (Cache<TSource, T>.Mapping == null)
            {
                Cache<TSource, T>.Mapping = GetMapping<TSource, T>();
            }
            return Cache<TSource, T>.Mapping(obj);
        }

        /// <summary>
        /// 将一个类型实例克隆成另一个类型实例(弱类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CloneTo<T>(this object obj)
            where T : class, new()
        {
            if (obj == null) return null;
            var type = obj.GetType();
            if (!Cache<T>.Mappings.ContainsKey(type))
            {
                Cache<T>.Mappings.Add(type, GetMapping<T>(type));
            }
            return Cache<T>.Mappings[type](obj);
        }

        private static Func<TSource, T> GetMapping<TSource, T>() 
            where T : class, new() 
            where TSource : class
        {
            var dynamicMethod = GetMapping(typeof(TSource), typeof(T));
            return (Func<TSource, T>)dynamicMethod.CreateDelegate(typeof(Func<TSource, T>));
        }

        private static Func<object, T> GetMapping<T>(Type sourceType)
            where T : class, new()
        {
            var dynamicMethod = GetMapping(sourceType, typeof(T), _objectType);
            return (Func<object, T>)dynamicMethod.CreateDelegate(typeof(Func<object, T>));
        }

        private static DynamicMethod GetMapping(Type sourceType, Type type, Type paramseterType = null)
        {
            var dynamicMethod = new DynamicMethod(Guid.NewGuid().ToString(), type,
                new Type[] { paramseterType ?? sourceType }, type, true);
            var generator = dynamicMethod.GetILGenerator();

            //T result;
            var result = generator.DeclareLocal(type);

            //result = new T();
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));//stack[obj]
            generator.Emit(OpCodes.Stloc, result);//stack[]

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                SetProperty(generator, result, property, sourceType);
            }

            //return result;
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }

        private static class Cache<TSource, T>
        {
            public static Func<TSource, T> Mapping { get; set; }
        }

        private static class Cache<T>
        {
            public static Dictionary<Type,Func<object, T>> Mappings { get; private set; } = new Dictionary<Type, Func<object, T>>();
        }

        private static readonly Dictionary<Type, OpCode> _typeMaps = new Dictionary<Type, OpCode>
        {
            [typeof(sbyte)] = OpCodes.Conv_Ovf_I1,
            [typeof(short)] = OpCodes.Conv_Ovf_I2,
            [typeof(int)] = OpCodes.Conv_Ovf_I4,
            [typeof(long)] = OpCodes.Conv_Ovf_I8,
            [typeof(byte)] = OpCodes.Conv_Ovf_U1,
            [typeof(ushort)] = OpCodes.Conv_Ovf_U2,
            [typeof(uint)] = OpCodes.Conv_Ovf_U4,
            [typeof(ulong)] = OpCodes.Conv_Ovf_U8,
            [typeof(char)] = OpCodes.Conv_Ovf_U2,
            [typeof(float)] = OpCodes.Conv_R4,
            [typeof(double)] = OpCodes.Conv_R8
        };

        private static readonly Dictionary<Type, MethodInfo> _parses = new Dictionary<Type, MethodInfo>
        {
            [typeof(sbyte)] = typeof(sbyte).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(short)] = typeof(short).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(int)] = typeof(int).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(long)] = typeof(long).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(byte)] = typeof(byte).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(ushort)] = typeof(ushort).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(uint)] = typeof(uint).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(ulong)] = typeof(ulong).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(char)] = typeof(char).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(float)] = typeof(float).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(double)] = typeof(double).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(decimal)] = typeof(decimal).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(bool)] = typeof(bool).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(DateTime)] = typeof(DateTime).GetMethod("Parse", new Type[] { typeof(string) }),
            [typeof(Guid)] = typeof(Guid).GetMethod("Parse", new Type[] { typeof(string) })
        };

        private static void SetProperty(ILGenerator generator, LocalBuilder result, PropertyInfo property, Type sourceType)
        {
            if (property.GetCustomAttribute<NotMappingAttribute>() != null) return;
            var attribute = property.GetCustomAttribute<MappingAttribute>();
            var name = attribute != null ? attribute.Name : property.Name;
            var sourceProperty = sourceType.GetProperty(name);
            if (sourceProperty == null || !sourceProperty.CanRead) return;
            var getMethod = sourceProperty.GetGetMethod();
            if (getMethod == null || !property.CanWrite) return;
            var setMethod = property.GetSetMethod();
            if (setMethod == null) return;
            var action = GetAction(property.PropertyType, sourceProperty.PropertyType);
            if (action == null) return;

            //result.Property = source.Property;
            generator.Emit(OpCodes.Ldloc, result); //stack[result]
            generator.Emit(OpCodes.Ldarg_0); //stack[result,source]
            generator.Emit(OpCodes.Callvirt, getMethod);//stack[result,value]
            action(generator, property.PropertyType, sourceProperty.PropertyType);
            generator.Emit(OpCodes.Callvirt, setMethod);//stack[]
        }

        private static Action<ILGenerator, Type, Type> GetAction(Type type, Type sourceType)
        {
            if (type != sourceType)
            {
                if (sourceType.IsValueType && !type.IsValueType)
                {
                    if (type == _objectType)
                    {
                        //result.Property = (object)source.Property; [Box]
                        return (g, t, s) => { g.Emit(OpCodes.Box, s); };
                    }
                    else if (type == _stringType)
                    {
                        //result.Property = source.Property.ToString(); [Box]
                        return (g, t, s) => 
                        {
                            g.Emit(OpCodes.Box, s);
                            g.Emit(OpCodes.Callvirt, _toString);
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (!sourceType.IsValueType && type.IsValueType)
                {
                    if (sourceType == _objectType)
                    {
                        //result.Property = (Type)source.Property; [Unbox]
                        return (g, t, s) => { g.Emit(OpCodes.Unbox_Any, t); };
                    }
                    else if (sourceType == _stringType && _parses.ContainsKey(type))
                    {
                        //result.Property = type.Parse(source.Property)
                        return (g, t, s) => { g.Emit(OpCodes.Call, _parses[type]); };
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (!sourceType.IsValueType && !type.IsValueType)
                {
                    if (type.IsAssignableFrom(sourceType))
                    {
                        //result.Property = source.Property as Type;
                        return (g, t, s) => { g.Emit(OpCodes.Castclass, s); };
                    }
                    else if (type == _stringType)
                    {
                        //result.Property = source.Property.ToString();
                        return (g, t, s) => { g.Emit(OpCodes.Callvirt, _toString); };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (_typeMaps.ContainsKey(type) && _typeMaps.ContainsKey(sourceType))
                    {
                        //result.Property = (Type)source.Property; [Value to Value]
                        return (g, t, s) => { g.Emit(_typeMaps[t]); };
                    }
                    else if (type.IsGenericType && type.Name.StartsWith("Nullable") && type.GenericTypeArguments[0] == sourceType)
                    {
                        //result.Property = (Type)source.Property; [Value to Value?]
                        return (g, t, s) => { g.Emit(OpCodes.Box, t); };
                    }
                    else if (sourceType.IsGenericType && sourceType.Name.StartsWith("Nullable") && sourceType.GenericTypeArguments[0] == type)
                    {
                        //result.Property = (Type)source.Property; [Value? to Value]
                        return (g, t, s) => 
                        {
                            g.Emit(OpCodes.Box, s);
                            g.Emit(OpCodes.Callvirt, s.GetMethod("get_Value"));
                        };
                    }
                    else 
                    {
                        return null;
                    }
                }
            }
            return (g, t, s) => { };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class MappingAttribute: Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MappingAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class NotMappingAttribute : Attribute
    {
    }
}