using System;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils
{

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private TKey[] keys = Array.Empty<TKey>();

        [SerializeField]
        private TValue[] values = Array.Empty<TValue>();

        static SerializableDictionary()
        {
            if (!typeof(TKey).IsSerializable)
            {
                throw new System.Exception(
                    string.Format("error:  {0} is not serializable!", typeof(TKey).FullName)
                );
            }
            if (!typeof(TValue).IsSerializable)
            {
                throw new System.Exception(
                    string.Format("error:  {0} is not serializable!", typeof(TValue).FullName)
                );
            }
        }

        public void OnBeforeSerialize()
        {
            int length = this.Count;
            Array.Resize<TKey>(ref keys, length);
            Array.Resize<TValue>(ref values, length);
            int index = 0;
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys[index] = pair.Key;
                values[index] = pair.Value;
                index++;
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Length != values.Length)
                throw new System.Exception(
                    string.Format("error:  {0} keys but {1} values after deserialization.", keys.Length, values.Length)
                );

            for (int i = 0, length = keys.Length; i < length; ++i)
            {
                this.Add(keys[i], values[i]);
            }
        }
    }
}
