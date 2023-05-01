using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCFx.Extender.DataType
{
    public class DataRecordFieldDictionary<TFirst, TSecond> : IDictionary<TFirst, TSecond>
    {
        private IDictionary<TFirst, TSecond> _firstToSecondDictionary;
        private IDictionary<TSecond, TFirst> _secondToFirstDictionary;

        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TFirst, TSecond> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TFirst, TSecond> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TFirst, TSecond> item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _firstToSecondDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _firstToSecondDictionary.IsReadOnly; }
        }

        public bool ContainsKey(TFirst key)
        {
            return _firstToSecondDictionary.ContainsKey(key);
        }

        public void Add(TFirst key, TSecond value)
        {
            _firstToSecondDictionary.Add(key, value);
            _secondToFirstDictionary.Add(value, key);
        }

        public bool Remove(TFirst key)
        {
            var value = _firstToSecondDictionary[key];

            return 
                _firstToSecondDictionary.Remove(key) &&
                _secondToFirstDictionary.Remove(value);
        }

        public bool TryGetValue(TFirst key, out TSecond value)
        {
            return _firstToSecondDictionary.TryGetValue(key, out value);
        }

        public TSecond this[TFirst key]
        {
            get { return _firstToSecondDictionary[key]; }
            set
            {
                _firstToSecondDictionary[key] = value;
                _secondToFirstDictionary[value] = key;
            }
        }

        public ICollection<TFirst> Keys
        {
            get { return _firstToSecondDictionary.Keys; }
        }

        public ICollection<TSecond> Values
        {
            get { return _firstToSecondDictionary.Values; }
        }
    }
}
