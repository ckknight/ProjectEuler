using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Ckknight.ProjectEuler.Collections
{
    public static class Point
    {
        public static Point<TValue> Create<TValue>(TValue x, TValue y) where TValue : IComparable<TValue>, IEquatable<TValue>
        {
            return new Point<TValue>(x, y);
        }
    }

    [Serializable]
    public struct Point<TValue> : IComparable<Point<TValue>>, IEquatable<Point<TValue>>, ISerializable where TValue : IComparable<TValue>, IEquatable<TValue>
    {
        public Point(TValue x, TValue y)
        {
            _x = x;
            _y = y;
        }

        private readonly TValue _x;
        private readonly TValue _y;

        public TValue X
        {
            get
            {
                return _x;
            }
        }

        public TValue Y
        {
            get
            {
                return _y;
            }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", _x, _y);
        }

        public override int GetHashCode()
        {
            int xCode = _x.GetHashCode();
            return (xCode << 4) ^ (xCode >> 4) ^ _y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Point<TValue>))
            {
                return false;
            }
            else
            {
                return Equals((Point<TValue>)obj);
            }
        }

        #region IEquatable<Point<TValue>> Members

        public bool Equals(Point<TValue> other)
        {
            return this._x.Equals(other._x) && this._y.Equals(other._y);
        }

        #endregion

        #region IComparable<Point<TValue>> Members

        public int CompareTo(Point<TValue> other)
        {
            int cmp = this._x.CompareTo(other._x);
            if (cmp != 0)
            {
                return cmp;
            }

            return this._y.CompareTo(other._y);
        }

        #endregion

        public static bool operator ==(Point<TValue> alpha, Point<TValue> bravo)
        {
            return alpha.Equals(bravo);
        }

        public static bool operator !=(Point<TValue> alpha, Point<TValue> bravo)
        {
            return !alpha.Equals(bravo);
        }

        public static bool operator <(Point<TValue> alpha, Point<TValue> bravo)
        {
            return alpha.CompareTo(bravo) < 0;
        }

        public static bool operator >(Point<TValue> alpha, Point<TValue> bravo)
        {
            return alpha.CompareTo(bravo) > 0;
        }

        public static bool operator <=(Point<TValue> alpha, Point<TValue> bravo)
        {
            return alpha.CompareTo(bravo) <= 0;
        }

        public static bool operator >=(Point<TValue> alpha, Point<TValue> bravo)
        {
            return alpha.CompareTo(bravo) >= 0;
        }
        
        #region ISerializable Members

        private Point(SerializationInfo info, StreamingContext context)
        {
            _x = (TValue)info.GetValue("x", typeof(TValue));
            _y = (TValue)info.GetValue("y", typeof(TValue));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", _x, typeof(TValue));
            info.AddValue("y", _y, typeof(TValue));
        }

        #endregion
    }
}
