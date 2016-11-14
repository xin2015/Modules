using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Modules.FastReflection
{
    /// <summary>
    /// Constructor（构造函数）快速反射接口
    /// </summary>
    public interface IConstructorInvoker
    {
        /// <summary>
        /// 调用构造函数
        /// </summary>
        /// <param name="parameters">构造函数参数</param>
        /// <returns>实例</returns>
        object Invoke(params object[] parameters);

        /// <summary>
        /// 构造函数信息
        /// </summary>
        ConstructorInfo Constructor { get; }
    }

    /// <summary>
    /// Constructor（构造函数）快速反射类
    /// </summary>
    public class ConstructorInvoker : IConstructorInvoker
    {
        private Func<object[], object> invoker;

        /// <summary>
        /// 构造函数信息
        /// </summary>
        public ConstructorInfo Constructor { get; private set; }

        /// <summary>
        /// 调用构造函数
        /// </summary>
        /// <param name="parameters">构造函数参数</param>
        /// <returns>实例</returns>
        public object Invoke(params object[] parameters)
        {
            return invoker(parameters);
        }

        /// <summary>
        /// ConstructorInvoker构造函数
        /// </summary>
        /// <param name="constructor">构造函数信息</param>
        public ConstructorInvoker(ConstructorInfo constructor)
        {
            Constructor = constructor;
            InitializeInvoker();
        }

        private void InitializeInvoker()
        {
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            List<Expression> parameterConverts = new List<Expression>();
            ParameterInfo[] parameters = Constructor.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                BinaryExpression parameterIndex = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                parameterConverts.Add(Expression.Convert(parameterIndex, parameters[i].ParameterType));
            }
            NewExpression instanceNew = Expression.New(Constructor, parameterConverts);
            UnaryExpression instanceConvert = Expression.Convert(instanceNew, typeof(object));
            invoker = Expression.Lambda<Func<object[], object>>(instanceConvert, parametersParameter).Compile();
        }
    }

    /// <summary>
    /// Constructor（构造函数）快速反射类工厂类
    /// </summary>
    public class ConstructorInvokerFactory : FastReflectionFactory<ConstructorInfo, IConstructorInvoker>
    {
        /// <summary>
        /// 创建Constructor（构造函数）快速反射类
        /// </summary>
        /// <param name="key">构造函数信息</param>
        /// <returns>Constructor（构造函数）快速反射接口</returns>
        public override IConstructorInvoker Create(ConstructorInfo key)
        {
            return new ConstructorInvoker(key);
        }
    }
}
