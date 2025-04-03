using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVision.Shared
{
    // 定义一个有序字典类，支持泛型键和值
    public class OrderedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {

        #region  字段和属性
        // 内部使用普通字典来存储键值对，用于快速根据键获取值
        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        // 维护一个键的列表，用于保持插入顺序
        private readonly List<TKey> keyList = new List<TKey>();

        /// <summary>
        /// 获取键的列表
        /// </summary>
        public List<TKey> Keys
        {
            get => keyList;
        }

        /// <summary>
        /// 获取值的列表
        /// </summary>

        public List<TValue> Values
        {
            get
            {
                var list = new List<TValue>();
                foreach (var key in keyList)
                {
                    list.Add(dictionary[key]);
                }
                return list;
            }
        }

        #endregion


        #region  索引器
        public TValue this[TKey key]
        {
            get=> dictionary[key];
            set
            {
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, value);
                }
                else
                {
                    dictionary[key] = value;
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 是否包含指定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                keyList.Add(key);
            }
            else
            {
                dictionary[key] = value;
            }
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            dictionary.Remove(key);
            keyList.Remove(key);
        }
        #endregion


        #region 接口方法
        // 实现泛型的枚举器接口，用于遍历有序字典中的键值对
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            // 遍历键列表
            foreach (var key in keyList)
            {
                // 根据当前键从内部字典中获取值，并以键值对的形式通过迭代器返回
                yield return new KeyValuePair<TKey, TValue>(key, dictionary[key]);
            }
        }

      


        // 实现非泛型的枚举器接口，通过调用泛型枚举器接口来实现
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
