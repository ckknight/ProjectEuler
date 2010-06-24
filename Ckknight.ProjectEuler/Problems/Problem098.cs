using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(98,
        @"By replacing each of the letters in the word CARE with 1, 2, 9, and 6
        respectively, we form a square number: 1296 = 36^2. What is remarkable
        is that, by using the same digital substitutions, the anagram, RACE,
        also forms a square number: 9216 = 96^2. We shall call CARE (and RACE) a
        square anagram word pair and specify further that leading zeroes are
        not permitted, neither may a different letter have the same digital
        value as another letter.

        Using words.txt (http://projecteuler.net/project/words.txt), a 16K text
        file containing nearly two-thousand common English words, find all the
        square anagram word pairs (a palindromic word is NOT considered to be
        an anagram of itself).

        What is the largest square number formed by any member of such a pair?

        NOTE: All anagrams formed must be contained in the given text file.")]
    public class Problem098 : BaseProblem
    {
        public Problem098()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            return GetText()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim('"'))
                .OrderByDescending(w => w.Length)
                .OrderedGroupBy(w => w.Length)
                .Select(l => l
                    .GroupBy(w => w, AnagramComparer.Instance)
                    .SelectMany(g => g.GetCombinations(2))
                    .Select(a => new KeyValuePair<string, string>(a[0], a[1]))
                    .ToArray())
                .Where(l => l.Any())
                .Select(l =>
                {
                    var length = l.First().Key.Length;
                    int minSquare = (int)Math.Pow(10, length - 1);
                    int maxSquare = minSquare * 10;
                    var sq = new Range(1, int.MaxValue)
                        .Select(n => n * n)
                        .SkipWhile(n => n < minSquare)
                        .TakeWhile(n => n < maxSquare)
                        .ToHashSet();

                    var result = l.Aggregate(0, (max, s) =>
                    {
                        var first = s.Key.ToCharArray();
                        var second = s.Value.ToCharArray();

                        var mappings = new Range(length)
                            .Aggregate(new Dictionary<int, int>(), (m, j) =>
                            {
                                m.Add(new Range(length).First(i => second[j] == first[i] && !m.ContainsKey(i)), j);
                                return m;
                            });

                        return Math.Max(max, sq.Max(i =>
                        {
                            var si = MathUtilities.ToDigits(i, true);
                            if (IsValid(s.Key, si))
                            {
                                int[] sn = new int[length];
                                for (int j = 0; j < length; j++)
                                {
                                    sn[mappings[j]] = si[j];
                                }
                                if (sn[0] != 0)
                                {
                                    int q = (int)MathUtilities.FromDigits(sn, true);
                                    if (sq.Contains(q) && q != i)
                                    {
                                        return Math.Max(i, q);
                                    }
                                }
                            }

                            return 0;
                        }));
                    });
                    return result;
                })
                .First(r => r > 0);
        }

        private static bool IsValid(string word, int[] num)
        {
            Dictionary<int, char> map = new Dictionary<int, char>();
            int l = word.Length;
            for (int i = 0; i < l; i++)
            {
                char c = word[i];
                int n = num[i];

                if (!map.ContainsKey(n))
                {
                    map.Add(n, c);
                }
                else
                {
                    if (map[n] != c) return false;
                }
            }
            return true;
        }

        private class AnagramComparer : IEqualityComparer<string>
        {
            private AnagramComparer() { }
            private static readonly AnagramComparer _instance = new AnagramComparer();
            public static AnagramComparer Instance
            {
                get
                {
                    return _instance;
                }
            }

            #region IEqualityComparer<string> Members

            public bool Equals(string x, string y)
            {
                if (object.Equals(x, y))
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                if (x.Length != y.Length)
                {
                    return false;
                }

                return x.OrderBy(c => c).SequenceEqual(y.OrderBy(c => c));
            }

            public int GetHashCode(string obj)
            {
                if (obj == null)
                {
                    return 0;
                }

                return obj.OrderBy(c => c).Aggregate(obj.Length, (current, c) => current ^ (current << 3) ^ c);
            }

            #endregion
        }

        private readonly string Url = "http://projecteuler.net/project/words.txt";
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
    }
}
