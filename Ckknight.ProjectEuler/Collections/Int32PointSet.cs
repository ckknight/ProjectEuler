using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public class Int32PointSet : ISet<Point<int>>, ICollection<Point<int>>
    {
        public Int32PointSet(int width, int height)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException("width", width, "Must be at least 0");
            }
            else if (height < 0)
            {
                throw new ArgumentOutOfRangeException("height", height, "Must be at least 0");
            }
            _bucket = new BooleanMatrix(width, height);
        }

        public Int32PointSet(int width, int height, bool includeAllInitially)
            : this(width, height)
        {
            if (includeAllInitially)
            {
                _bucket.SetAll(true);
            }
        }

        public Int32PointSet(IEnumerable<Point<int>> sequence, int width, int height)
            : this(width, height)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            foreach (Point<int> item in sequence)
            {
                int x = item.X;
                int y = item.Y;
                if (x < 0 || x >= width)
                {
                    throw new ArgumentException("Item's X must be no less than 0 and not greater or equal to the provided width", "sequence");
                }
                else if (x < 0 || x >= height)
                {
                    throw new ArgumentException("Item's Y must be no less than 0 and not greater or equal to the provided width", "sequence");
                }
                _bucket[x, y] = true;
            }
        }

        private readonly BooleanMatrix _bucket;

        public int Width
        {
            get
            {
                return _bucket.Width;
            }
        }

        public int Height
        {
            get
            {
                return _bucket.Height;
            }
        }

        public IEnumerable<Point<int>> GetInverse()
        {
            int height = _bucket.Height;
            int width = _bucket.Width;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!_bucket[x, y])
                    {
                        yield return new Point<int>(x, y);
                    }
                }
            }
        }

        #region ICollection<int> Members

        void ICollection<Point<int>>.Add(Point<int> item)
        {
            Add(item);
        }

        public void Clear()
        {
            _bucket.SetAll(false);
        }

        public bool Contains(Point<int> item)
        {
            return Contains(item.X, item.Y);
        }

        public bool Contains(int x, int y)
        {
            if (x < 0 || y < 0 || x > Width || y > Height)
            {
                return false;
            }
            return _bucket[x, y];
        }

        void ICollection<Point<int>>.CopyTo(Point<int>[] array, int arrayIndex)
        {
            int height = _bucket.Height;
            int width = _bucket.Width;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (_bucket[x, y])
                    {
                        array[arrayIndex++] = new Point<int>(x, y);
                    }
                }
            }
        }

        int ICollection<Point<int>>.Count
        {
            get
            {
                int count = 0;
                int height = _bucket.Height;
                int width = _bucket.Width;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (_bucket[x, y])
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        bool ICollection<Point<int>>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool Remove(Point<int> item)
        {
            int x = item.X;
            int y = item.Y;
            return Remove(x, y);
        }
        
        public bool Remove(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return false;
            }

            if (_bucket[x, y])
            {
                _bucket[x, y] = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public ParallelQuery<Point<int>> AsParallel()
        {
            int width = _bucket.Width;
            return ParallelEnumerable.Range(0, _bucket.Height)
                .SelectMany(y => Enumerable.Range(0, width)
                    .Where(x => _bucket[x, y])
                    .Select(x => new Point<int>(x, y)));
        }

        #region IEnumerable<Point<int>> Members

        public IEnumerator<Point<int>> GetEnumerator()
        {
            int height = _bucket.Height;
            int width = _bucket.Width;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (_bucket[x, y])
                    {
                        yield return new Point<int>(x, y);
                    }
                }
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public bool Add(Point<int> point)
        {
            return Add(point.X, point.Y);
        }

        public bool Add(int x, int y)
        {
            if (x < 0)
            {
                throw new ArgumentOutOfRangeException("x", x, "Must be at least 0");
            }
            else if (y < 0)
            {
                throw new ArgumentOutOfRangeException("y", y, "Must be at least 0");
            }
            else if (x >= Width)
            {
                throw new ArgumentOutOfRangeException("x", x, string.Format("Must be at most {0}", Width - 1));
            }
            else if (y >= Height)
            {
                throw new ArgumentOutOfRangeException("y", y, string.Format("Must be at most {0}", Height - 1));
            }

            if (_bucket[x, y])
            {
                return false;
            }
            else
            {
                _bucket[x, y] = true;
                return true;
            }
        }

        public void ExceptWith(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            int width = Width;
            int height = Height;
            foreach (Point<int> item in other)
            {
                int x = item.X;
                int y = item.Y;
                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    _bucket[x, y] = false;
                }
            }
        }

        public void IntersectWith(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            ISet<Point<int>> otherSet = other as ISet<Point<int>> ?? other.ToHashSet();
            foreach (Point<int> item in this)
            {
                if (!otherSet.Contains(item))
                {
                    _bucket[item.X, item.Y] = false;
                }
            }
        }

        public bool IsProperSubsetOf(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ISet<Point<int>> otherSet = other as ISet<Point<int>> ?? other.ToHashSet();
            foreach (Point<int> item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return false;
                }
            }
            foreach (Point<int> item in otherSet)
            {
                if (!Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsProperSupersetOf(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ISet<Point<int>> otherSet = other as ISet<Point<int>> ?? other.ToHashSet();
            foreach (Point<int> item in otherSet)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }

            foreach (Point<int> item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSubsetOf(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ISet<Point<int>> otherSet = other as ISet<Point<int>> ?? other.ToHashSet();
            foreach (Point<int> item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            foreach (Point<int> item in other)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Overlaps(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            foreach (Point<int> item in other)
            {
                if (Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool SetEquals(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            var otherSet = other.ToHashSet();
            foreach (Point<int> item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return false;
                }
                otherSet.Remove(item);
            }
            return otherSet.Count == 0;
        }

        public void SymmetricExceptWith(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            int width = Width;
            int height = Height;
            foreach (Point<int> item in other.Distinct())
            {
                int x = item.X;
                int y = item.Y;
                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    _bucket[x, y] = !_bucket[x, y];
                }
            }
        }

        public void UnionWith(IEnumerable<Point<int>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            int width = Width;
            int height = Height;
            foreach (Point<int> item in other)
            {
                int x = item.X;
                int y = item.Y;
                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    _bucket[x, y] = true;
                }
            }
        }
    }
}
