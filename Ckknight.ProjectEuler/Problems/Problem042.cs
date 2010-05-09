using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(42,
        @"The nth term of the sequence of triangle numbers is given by,
        t(n) = 1/2n(n+1); so the first ten triangle numbers are:
        
                1, 3, 6, 10, 15, 21, 28, 36, 45, 55, ...

        By converting each letter in a word to a number corresponding to its
        alphabetical position and adding these values we form a word value. For
        example, the word value for SKY is 19 + 11 + 25 = 55 = t(10). If the word
        value is a triangle number then we shall call the word a triangle word.

        Using words.txt (http://projecteuler.net/project/words.txt), a 16K text
        file containing nearly two-thousand common English words, how many are
        triangle words?")]
    public class Problem042 : BaseProblem
    {
        public Problem042()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            var wordValues = GetText()
                .Split(',')
                .Select(w => w.Trim('"')
                    .ToCharArray()
                    .Select(c => c - 'A' + 1)
                    .Sum())
                .ToArray();

            var triangles = GetTriangles()
                .TakeWhile(t => t <= wordValues.Max())
                .ToHashSet();

            return wordValues
                .Where(w => triangles.Contains(w))
                .Count();
        }

        public IEnumerable<int> GetTriangles()
        {
            int n = 0;
            while (true)
            {
                n++;

                yield return (n * (n + 1)) / 2;
            }
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
