using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Modules.FastReflection
{
    /// <summary>
    /// Method（方法）快速反射接口（有返回值）
    /// </summary>
    public interface IObjectMethodInvoker
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        object Invoke(object instance, params object[] parameters);

        /// <summary>
        /// 方法信息
        /// </summary>
        MethodInfo Method { get; }
    }

    /// <summary>
    /// Method（方法）快速反射类（有返回值）
    /// </summary>
    public class ObjectMethodInvoker : IObjectMethodInvoker
    {
        private Func<object, object[], object> invoker;

        /// <summary>
        /// 方法信息
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public object Invoke(object instance, params object[] parameters)
        {
            return invoker(instance, parameters);
        }

        /// <summary>
        /// MethodInvoker构造函数
        /// </summary>
        /// <param name="method">方法信息</param>
        public ObjectMethodInvoker(MethodInfo method)
        {
            Method = method;
            InitializeInvoker();
        }

        private void InitializeInvoker()
        {
            if (Method.ReturnType == typeof(void))
            {
                throw new NotSupportedException("The method's returnType is void, please use VoidMethodInvoker");
            }
            else
            {
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");
                ParameterExpression parametersParam = Expression.Parameter(typeof(object[]), "parameters");
                ParameterInfo[] parameters = Method.GetParameters();
                List<Expression> parameterConverts = new List<Expression>();
                for (int i = 0; i < parameters.Length; i++)
                {
                    BinaryExpression parameterIndex = Expression.ArrayIndex(parametersParam, Expression.Constant(i));
                    UnaryExpression parameterConvert = Expression.Convert(parameterIndex, parameters[i].ParameterType);
                    parameterConverts.Add(parameterConvert);
                }
                UnaryExpression instanceConvert = Method.IsStatic ? null : Expression.Convert(instanceParam, Method.ReflectedType);
                MethodCallExpression call = Expression.Call(instanceConvert, Method, parameterConverts);
                invoker = Expression.Lambda<Func<object, object[], object>>(call, instanceParam, parametersParam).Compile();
            }
        }
    }

    /// <summary>
    /// Method（方法）快速反射接口（无返回值）
    /// </summary>
    public interface IVoidMethodInvoker
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="parameters">参数</param>
        void Invoke(object instance, params object[] parameters);

        /// <summary>
        /// 方法信息
        /// </summary>
        MethodInfo Method { get; }
    }

    /// <summary>
    /// Method（方法）快速反射类（无返回值）
    /// </summary>
    public class VoidMethodInvoker : IVoidMethodInvoker
    {
        private Action<object, object[]> invoker;

        /// <summary>
        /// 方法信息
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="parameters">参数</param>
        public void Invoke(object instance, params object[] parameters)
        {
            invoker(instance, parameters);
        }

        /// <summary>
        /// MethodInvoker构造函数
        /// </summary>
        /// <param name="method">方法信息</param>
        public VoidMethodInvoker(MethodInfo method)
        {
            Method = method;
            InitializeInvoker();
        }

        private void InitializeInvoker()
        {
            if (Method.ReturnType == typeof(void))
            {
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");
                ParameterExpression parametersParam = Expression.Parameter(typeof(object[]), "parameters");
                ParameterInfo[] parameters = Method.GetParameters();
                List<Expression> parameterConverts = new List<Expression>();
                for (int i = 0; i < parameters.Length; i++)
                {
                    BinaryExpression parameterIndex = Expression.ArrayIndex(parametersParam, Expression.Constant(i));
                    UnaryExpression parameterConvert = Expression.Convert(parameterIndex, parameters[i].ParameterType);
                    parameterConverts.Add(parameterConvert);
                }
                UnaryExpression instanceConvert = Method.IsStatic ? null : Expression.Convert(instanceParam, Method.ReflectedType);
                MethodCallExpression call = Expression.Call(instanceConvert, Method, parameterConverts);
                invoker = Expression.Lambda<Action<object, object[]>>(call, instanceParam, parametersParam).Compile();
            }
            else
            {
                throw new NotSupportedException("The method's returnType is not void, please use ObjectMethodInvoker");
            }
        }
    }

    /// <summary>
    /// Method（方法）快速反射类（有返回值）工厂类
    /// </summary>
    public class ObjectMethodInvokerFactory : FastReflectionFactory<MethodInfo, IObjectMethodInvoker>
    {
        /// <summary>
        /// 创建Method（方法）快速反射类（有返回值）
        /// </summary>
        /// <param name="key">方法属性</param>
        /// <returns>Method（方法）快速反射接口（有返回值）</returns>
        public override IObjectMethodInvoker Create(MethodInfo key)
        {
            return new ObjectMethodInvoker(key);
        }
    }

    /// <summary>
    /// Method（方法）快速反射类（无返回值）工厂类
    /// </summary>
    public class VoidMethodInvokerFactory : FastReflectionFactory<MethodInfo, IVoidMethodInvoker>
    {
        /// <summary>
        /// 创建Method（方法）快速反射类（无返回值）
        /// </summary>
        /// <param name="key">方法属性</param>
        /// <returns>Method（方法）快速反射接口（无返回值）</returns>
        public override IVoidMethodInvoker Create(MethodInfo key)
        {
            return new VoidMethodInvoker(key);
        }
    }
}
