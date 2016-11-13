using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Modules.FastReflection
{
    /// <summary>
    /// Property（属性）快速反射接口
    /// </summary>
    public interface IPropertyAccessor
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <returns></returns>
        object GetValue(object instance);

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="value">属性值</param>
        void SetValue(object instance, object value);

        /// <summary>
        /// 属性信息
        /// </summary>
        PropertyInfo Property { get; }
    }

    /// <summary>
    /// Property（属性）快速反射类
    /// </summary>
    public class PropertyAccessor : IPropertyAccessor
    {
        private Func<object, object> getter;
        private Action<object, object> setter;

        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <returns></returns>
        public object GetValue(object instance)
        {
            if (getter == null)
            {
                throw new NotSupportedException("Get method is not defined for this property.");
            }
            return getter(instance);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="value">属性值</param>
        public void SetValue(object instance, object value)
        {
            if (setter == null)
            {
                throw new NotSupportedException("Set method is not defined for this property.");
            }
            setter(instance, value);
        }

        /// <summary>
        /// PropertyAccessor构造函数
        /// </summary>
        /// <param name="property">属性信息</param>
        public PropertyAccessor(PropertyInfo property)
        {
            Property = property;
            InitializeGet();
            InitializeSet();
        }

        private void InitializeGet()
        {
            if (Property.CanRead)
            {
                MethodInfo getMethod = Property.GetGetMethod();
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");
                UnaryExpression instanceConvert = getMethod.IsStatic ? null : Expression.Convert(instanceParam, Property.ReflectedType);
                MethodCallExpression call = Expression.Call(instanceConvert, getMethod);
                UnaryExpression valueConvert = Expression.Convert(call, typeof(object));
                getter = Expression.Lambda<Func<object, object>>(valueConvert, instanceParam).Compile();
            }
        }

        private void InitializeGetToo()
        {
            if (Property.CanRead)
            {
                MethodInfo getMethod = Property.GetGetMethod();
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");
                UnaryExpression instanceConvert = getMethod.IsStatic ? null : Expression.Convert(instanceParam, Property.ReflectedType);
                MemberExpression property = Expression.Property(instanceConvert, Property);
                UnaryExpression propertyConvert = Expression.Convert(property, typeof(object));
                getter = Expression.Lambda<Func<object, object>>(propertyConvert, instanceParam).Compile();
            }
        }

        private void InitializeSet()
        {
            if (Property.CanWrite)
            {
                MethodInfo setMethod = Property.GetSetMethod();
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");
                ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");
                UnaryExpression instanceConvert = setMethod.IsStatic ? null : Expression.Convert(instanceParam, Property.ReflectedType);
                UnaryExpression valueConvert = Expression.Convert(valueParam, Property.PropertyType);
                MethodCallExpression call = Expression.Call(instanceConvert, setMethod, valueConvert);
                setter = Expression.Lambda<Action<object, object>>(call, instanceParam, valueParam).Compile();
            }
        }

        private void InitializeSetToo()
        {
            if (Property.CanWrite)
            {
                MethodInfo setMethod = Property.GetSetMethod();
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");
                ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");
                UnaryExpression instanceConvert = setMethod.IsStatic ? null : Expression.Convert(instanceParam, Property.ReflectedType);
                UnaryExpression valueConvert = Expression.Convert(valueParam, Property.PropertyType);
                MemberExpression instancePropery = Expression.Property(instanceConvert, Property);
                BinaryExpression instanceAssign = Expression.Assign(instancePropery, valueConvert);
                setter = Expression.Lambda<Action<object, object>>(instanceAssign, instanceParam, valueParam).Compile();
            }
        }
    }

    /// <summary>
    /// Property（属性）快速反射类工厂类
    /// </summary>
    public class PropertyAccessorFactory : FastReflectionFactory<PropertyInfo, IPropertyAccessor>
    {
        /// <summary>
        /// 创建Property（属性）快速反射类
        /// </summary>
        /// <param name="key">属性信息</param>
        /// <returns>Property（属性）快速反射接口</returns>
        public override IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }
    }
}
