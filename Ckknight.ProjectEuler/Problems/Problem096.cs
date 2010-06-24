using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Ckknight.ProjectEuler.Collections;
using System.Collections.Specialized;
using System.IO;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(96,
        @"Su Doku (Japanese meaning number place) is the name given to a
        popular puzzle concept. Its origin is unclear, but credit must be
        attributed to Leonhard Euler who invented a similar, and much more
        difficult, puzzle idea called Latin Squares. The objective of Su Doku
        puzzles, however, is to replace the blanks (or zeros) in a 9 by 9 grid
        in such that each row, column, and 3 by 3 box contains each of the
        digits 1 to 9. Below is an example of a typical starting puzzle grid
        and its solution grid.

                0 0 3   0 2 0   6 0 0
                9 0 0   3 0 5   0 0 1
                0 0 1	8 0 6   4 0 0

                0 0 8   1 0 2   9 0 0
                7 0 0   0 0 0   0 0 8
                0 0 6   7 0 8   2 0 0

                0 0 2   6 0 9   5 0 0
                8 0 0   2 0 3   0 0 9
                0 0 5	0 1 0   3 0 0



                4 8 3   9 2 1   6 5 7
                9 6 7   3 4 5   8 2 1
                2 5 1	8 7 6   4 9 3

                5 4 8   1 3 2   9 7 6
                7 2 9   5 6 4   1 3 8
                1 3 6	7 9 8   2 4 5

                3 7 2   6 8 9   5 1 4
                8 1 4   2 5 3   7 6 9
                6 9 5	4 1 7	3 8 2

        A well constructed Su Doku puzzle has a unique solution and can be
        solved by logic, although it may be necessary to employ ""guess and
        test"" methods in order to eliminate options (there is much contested
        opinion over this). The complexity of the search determines the
        difficulty of the puzzle; the example above is considered easy because
        it can be solved by straight forward direct deduction.

        The 6K text file, sudoku.txt
        (http://projecteuler.net/project/sudoku.txt), contains fifty different
        Su Doku puzzles ranging in difficulty, but all with unique solutions
        (the first puzzle in the file is the example above).

        By solving all fifty puzzles find the sum of the 3-digit numbers found
        in the top left corner of each solution grid; for example, 483 is the
        3-digit number found in the top left corner of the solution grid above.")]
    public class Problem096 : BaseProblem
    {
        public Problem096()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            return GetText()
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Partition(10)
                .Select(g => g.Skip(1)
                    .Select(l => l
                        .Trim()
                        .ToCharArray()
                        .Select(c => c - '0')
                        .ToArray())
                    .ToArray())
                .AsParallel()
                .Select(g => new Grid(g))
                .Select(g => new
                {
                    Grid = g,
                    Solved = g.Solve()
                })
                .Aggregate(0, (sum, grid) =>
                {
                    if (grid.Solved)
                    {
                        return sum + grid.Grid[0, 0] * 100 + grid.Grid[1, 0] * 10 + grid.Grid[2, 0];
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                });
        }

        private readonly string Url = "http://projecteuler.net/project/sudoku.txt";
        private string _textCache;
        public string GetText()
        {
            if (_textCache == null)
            {
                WebClient client = new WebClient();
                _textCache = client.DownloadString(Url);
            }

            return _textCache;
        }

        private class Grid
        {
            public Grid(int[][] data)
            {
                _cells = new Cell[9, 9];
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        _cells[x, y] = new Cell { Value = data[y][x] };
                    }
                }
            }

            private readonly Cell[,] _cells;

            public int this[int x, int y]
            {
                get
                {
                    return _cells[x, y].Value;
                }
            }

            private void Erase()
            {
                var col = new bool[9, 10];
                var row = new bool[9, 10];
                var block = new bool[9, 10];
                for (int x = 0; x < 9; x++)
                {
                    foreach (var cell in GetColumn(x))
                    {
                        col[x, cell.Value] = true;
                    }
                }
                for (int y = 0; y < 9; y++)
                {
                    foreach (var cell in GetRow(y))
                    {
                        row[y, cell.Value] = true;
                    }
                }
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        block[(x / 3) + 3 * (y / 3), _cells[x, y].Value] = true;
                    }
                }

                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (_cells[x, y].Value == 0)
                        {
                            foreach (int z in AllValues)
                            {
                                if (!col[x, z] && !row[y, z] && !block[(x / 3) + 3 * (y / 3), z])
                                {
                                    _cells[x, y].PotentialValues.Add(z);
                                }
                            }
                        }
                    }
                }

                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        Cell cell = _cells[x, y];
                        if (cell.Value == 0 && cell.PotentialValues.Count == 1)
                        {
                            cell.Value = cell.PotentialValues.Single();
                            cell.PotentialValues.Clear();
                            CleanNeighboringCells(x, y);
                        }
                    }
                }
            }

            private void CleanNeighboringCells(int x, int y)
            {
                int value = _cells[x, y].Value;
                foreach (var cell in GetNeighboringCells(x, y))
                {
                    cell.PotentialValues.Remove(value);
                }
            }

            private static readonly int[] AllValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            private bool IsDone()
            {
                return !GetAllCells()
                    .Any(c => c.Value == 0)
                && GetAllAreas()
                    .All(area => !AllValues
                        .Except(area
                            .Select(c => c.Value))
                        .Any());
            }

            private int GetSolvedCount()
            {
                return GetAllCells()
                    .Count(c => c.Value != 0);
            }

            private IEnumerable<Cell> GetAllCells()
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        yield return _cells[x, y];
                    }
                }
            }

            private IEnumerable<IEnumerable<Cell>> GetAllAreas()
            {
                for (int i = 0; i < 9; i++)
                {
                    yield return GetRow(i);
                    yield return GetColumn(i);
                    yield return GetBlock(i);
                }
            }

            private IEnumerable<Cell> GetRow(int y)
            {
                for (int x = 0; x < 9; x++)
                {
                    yield return _cells[x, y];
                }
            }

            private IEnumerable<Cell> GetColumn(int x)
            {
                for (int y = 0; y < 9; y++)
                {
                    yield return _cells[x, y];
                }
            }

            private IEnumerable<Cell> GetBlock(int i)
            {
                int xStart = (i % 3) * 3;
                int yStart = (i / 3) * 3;

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        yield return _cells[x + xStart, y + yStart];
                    }
                }
            }

            private IEnumerable<Cell> GetBlock(int x, int y)
            {
                return GetBlock(x / 3 + (y / 3) * 3);
            }

            private IEnumerable<Cell> GetNeighboringCells(int x, int y)
            {
                return GetColumn(x)
                    .Concat(GetRow(y))
                    .Concat(GetBlock(x, y));
            }

            public bool Solve()
            {
                int lastSolvedCount = GetSolvedCount();
                while (!IsDone())
                {
                    Erase();
                    int solvedCount = GetSolvedCount();
                    if (solvedCount == lastSolvedCount)
                    {
                        return SolveByBruteForce();
                    }
                    else
                    {
                        lastSolvedCount = solvedCount;
                    }
                }
                return true;
            }

            private Point<int> FindUnsolvedPosition()
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (_cells[x, y].PotentialValues.Count > 1)
                        {
                            return new Point<int>(x, y);
                        }
                    }
                }
                return new Point<int>(-1, -1);
            }

            private bool SolveByBruteForce()
            {
                Point<int> point = FindUnsolvedPosition();
                if (point.X == -1)
                {
                    return false;
                }

                Cell[,] backup = new Cell[9, 9];
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        backup[x, y] = new Cell(_cells[x, y]);
                    }
                }

                foreach (int value in _cells[point.X, point.Y].PotentialValues.ToArray())
                {
                    _cells[point.X, point.Y].Value = value;
                    _cells[point.X, point.Y].PotentialValues.Clear();
                    CleanNeighboringCells(point.X, point.Y);
                    if (Solve())
                    {
                        return true;
                    }

                    for (int x = 0; x < 9; x++)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            _cells[x, y].Set(backup[x, y]);
                        }
                    }
                }
                return false;
            }

            private class Cell
            {
                public Cell()
                {
                    Value = 0;
                    _potentialValues = new HashSet<int>();
                }

                public Cell(Cell other)
                    : this()
                {
                    if (other == null)
                    {
                        throw new ArgumentNullException("other");
                    }

                    Set(other);
                }

                public void Set(Cell other)
                {
                    if (other == null)
                    {
                        throw new ArgumentNullException("other");
                    }

                    Value = other.Value;
                    _potentialValues.Clear();
                    _potentialValues.UnionWith(other.PotentialValues);
                }

                private readonly HashSet<int> _potentialValues;
                public HashSet<int> PotentialValues
                {
                    get
                    {
                        return _potentialValues;
                    }
                }
                public int Value { get; set; }

                public override string ToString()
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Value: ");
                    if (Value == 0)
                    {
                        sb.Append('?');
                        sb.AppendLine();
                        sb.Append("PotentialValues: ");
                        foreach (var item in PotentialValues)
                        {
                            sb.Append(item);
                            sb.Append(' ');
                        }
                    }
                    else
                    {
                        sb.Append(Value);
                    }
                    return sb.ToString();
                }

            }
        }
    }
}