using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public class BooleanArray : IList<bool>, IList
    {
        public BooleanArray(int length)
            : this(length, false) { }

        public BooleanArray(int length, bool defaultValue)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", length, "Must be at least 0");
            }
            _length = length;
            int dataLength = GetDataLength(length);
            _data = new byte[dataLength];
            if (defaultValue)
            {
                for (int i = 0; i < dataLength; i++)
                {
                    _data[i] = 255;
                }
            }
        }

        public BooleanArray(BooleanArray source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            _length = source._length;
            byte[] sourceData = source._data;
            int dataLength = sourceData.Length;
            _data = new byte[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                _data[i] = sourceData[i];
            }
        }

        public BooleanArray(IEnumerable<bool> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            bool[] array = source as bool[] ?? source.ToArray();

            int length = array.Length;
            _length = length;
            _data = new byte[GetDataLength(length)];

            int byteAlignedLength = (length >> 3) << 3;

            for (int i = 0; i < byteAlignedLength; i += 8)
            {
                byte value = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (array[i + j])
                    {
                        value |= (byte)(1 << j);
                    }
                }
                _data[i >> 3] = value;
            }

            if (length > byteAlignedLength)
            {
                byte value = 0;
                for (int j = 0; j < length - byteAlignedLength; j++)
                {
                    if (array[byteAlignedLength + j])
                    {
                        value |= (byte)(1 << j);
                    }
                }
                _data[byteAlignedLength >> 3] = value;
            }
        }

        private static int GetDataLength(int length)
        {
            int dataLength = length >> 3;
            if ((length % 8) > 0)
            {
                dataLength++;
            }
            return dataLength;
        }

        private readonly int _length;
        private readonly byte[] _data;

        public int Length
        {
            get
            {
                return _length;
            }
        }

        int IList<bool>.IndexOf(bool item)
        {
            int index = 0;
            foreach (bool x in this)
            {
                if (x == item)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        void IList<bool>.Insert(int index, bool item)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        void IList<bool>.RemoveAt(int index)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        public bool this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException("Must be at least 0");
                }
                else if (index >= _length)
                {
                    throw new IndexOutOfRangeException(string.Format("Must be less than {0}", _length));
                }

                int majorOffset = index >> 3;
                int minorOffset = index % 8;
                byte value = _data[majorOffset];
                byte check = (byte)(1 << minorOffset);
                return (value & check) != 0;
            }
            set
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException("Must be at least 0");
                }
                else if (index >= _length)
                {
                    throw new IndexOutOfRangeException(string.Format("Must be less than {0}", _length));
                }

                int majorOffset = index >> 3;
                int minorOffset = index % 8;
                byte current = _data[majorOffset];
                byte check = (byte)(1 << minorOffset);
                if (value)
                {
                    current |= check;
                }
                else
                {
                    current &= (byte)(~check);
                }
                _data[majorOffset] = current;
            }
        }

        void ICollection<bool>.Add(bool item)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        void ICollection<bool>.Clear()
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        bool ICollection<bool>.Contains(bool item)
        {
            return ((IList<bool>)this).IndexOf(item) != -1;
        }

        void ICollection<bool>.CopyTo(bool[] array, int arrayIndex)
        {
            foreach (bool item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        int ICollection<bool>.Count
        {
            get
            {
                return this.Length;
            }
        }

        bool ICollection<bool>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection<bool>.Remove(bool item)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        public IEnumerator<bool> GetEnumerator()
        {
            int byteAlignedLength = (_length >> 3) << 3;
            for (int i = 0; i < byteAlignedLength; i += 8)
            {
                byte value = _data[i >> 3];
                for (int j = 0; j < 8; j++)
                {
                    yield return (value & (1 << j)) != 0;
                }
            }

            if (_length > byteAlignedLength)
            {
                byte value = _data[byteAlignedLength >> 3];
                for (int j = 0; j < _length - byteAlignedLength; j++)
                {
                    yield return (value & (1 << j)) != 0;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        void IList.Clear()
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        bool IList.Contains(object value)
        {
            if (!(value is bool))
            {
                return false;
            }
            else
            {
                return ((ICollection<bool>)this).Contains((bool)value);
            }
        }

        int IList.IndexOf(object value)
        {
            if (!(value is bool))
            {
                return -1;
            }
            else
            {
                return ((IList<bool>)this).IndexOf((bool)value);
            }
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Remove(object value)
        {
            throw new NotSupportedException("Cannot change length of BooleanArray");
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (!(value is bool))
                {
                    throw new ArgumentException("Must be a Boolean", "value");
                }

                this[index] = (bool)value;
            }
        }

        int ICollection.Count
        {
            get
            {
                return this.Length;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            foreach (bool item in this)
            {
                array.SetValue(item, index++);
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        public ParallelQuery<bool> AsParallel()
        {
            return ParallelEnumerable.Range(0, _length)
                .Select(i => this[i]);
        }
    }
}
