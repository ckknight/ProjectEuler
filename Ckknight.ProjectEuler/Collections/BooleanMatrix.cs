using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public class BooleanMatrix
    {
        public BooleanMatrix(int width, int height)
            : this(width, height, false) { }

        public BooleanMatrix(int width, int height, bool defaultValue)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException("width", width, "Must be at least 0");
            }
            else if (height < 0)
            {
                throw new ArgumentOutOfRangeException("height", height, "Must be at least 0");
            }
            else if ((long)width * (long)height > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("height", height, "width * height must no greater than int.MaxValue");
            }

            _width = width;
            _height = height;

            _data = new byte[GetDataLength(width, height)];
            if (defaultValue)
            {
                SetAll(true);
            }
        }

        public BooleanMatrix(BooleanMatrix source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            _width = source._width;
            _height = source._height;

            byte[] sourceData = source._data;
            int length = sourceData.Length;
            _data = new byte[length];
            for (int i = 0; i < length; i++)
            {
                _data[i] = sourceData[i];
            }
        }

        public BooleanMatrix(bool[,] source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            _width = source.GetLength(0);
            _height = source.GetLength(1);
            if ((long)_width * (long)_height > int.MaxValue)
            {
                throw new ArgumentException("width * height cannot be greater than int.MaxValue", "source");
            }

            int length = GetDataLength(_width, _height);
            _data = new byte[length];

            int majorOffset = 0;
            int minorOffset = 0;
            byte current = 0;
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (minorOffset == 8)
                    {
                        _data[majorOffset] = current;
                        current = 0;
                        minorOffset = 0;
                        majorOffset++;
                    }

                    if (source[x, y])
                    {
                        current |= (byte)(1 << minorOffset);
                    }

                    minorOffset++;
                }
            }

            _data[majorOffset] = current;
        }

        public void SetAll(bool value)
        {
            int length = _data.Length;
            byte byteValue = value ? byte.MaxValue : (byte)0;
            for (int i = 0; i < length; i++)
            {
                _data[i] = byteValue;
            }
        }

        private static int GetDataLength(int width, int height)
        {
            int length = width * height;
            int dataLength = length >> 3;
            if ((length % 8) > 0)
            {
                dataLength++;
            }
            return dataLength;
        }

        private readonly int _width;
        private readonly int _height;
        private readonly byte[] _data;

        public int Width
        {
            get
            {
                return _width;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
        }

        public bool this[int x, int y]
        {
            get
            {
                if (x < 0)
                {
                    throw new IndexOutOfRangeException("x Must be at least 0");
                }
                else if (y < 0)
                {
                    throw new IndexOutOfRangeException("y Must be at least 0");
                }
                else if (x >= _width)
                {
                    throw new IndexOutOfRangeException(string.Format("x Must be less than {0}", _width));
                }
                else if (y >= _height)
                {
                    throw new IndexOutOfRangeException(string.Format("y Must be less than {0}", _height));
                }

                int index = y * _width + x;

                int majorOffset = index >> 3;
                int minorOffset = index % 8;
                byte value = _data[majorOffset];
                byte check = (byte)(1 << minorOffset);
                return (value & check) != 0;
            }
            set
            {
                if (x < 0)
                {
                    throw new IndexOutOfRangeException("x Must be at least 0");
                }
                else if (y < 0)
                {
                    throw new IndexOutOfRangeException("y Must be at least 0");
                }
                else if (x >= _width)
                {
                    throw new IndexOutOfRangeException(string.Format("x Must be less than {0}", _width));
                }
                else if (y >= _height)
                {
                    throw new IndexOutOfRangeException(string.Format("y Must be less than {0}", _height));
                }

                int index = y * _width + x;

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
    }
}
