using System.Collections.Generic;
using System.Reflection;

namespace Modules.FastReflection
{
    /// <summary>
    /// 快速反射类工厂类
    /// </summary>
    /// <typeparam name="TKey">MemberInfo</typeparam>
    /// <typeparam name="TValue">快速反射类接口</typeparam>
    public abstract class FastReflectionFactory<TKey, TValue> where TKey : MemberInfo
    {
        static Dictionary<TKey, TValue> dic;
        static FastReflectionFactory()
        {
            dic = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 获取MemberInfo对应的快速反射类（字典缓存）
        /// </summary>
        /// <param name="key">MemberInfo</param>
        /// <returns>快速反射接口</returns>
        public virtual TValue Get(TKey key)
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                value = Create(key);
                dic[key] = value;
            }
            return value;
        }

        /// <summary>
        /// 创建MemberInfo对应的快速反射类
        /// </summary>
        /// <param name="key">MemberInfo</param>
        /// <returns>快速反射类接口</returns>
        public abstract TValue Create(TKey key);
    }
}
