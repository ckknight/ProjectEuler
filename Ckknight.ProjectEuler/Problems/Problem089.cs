using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ckknight.ProjectEuler.Collections;
using System.IO;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(89,
        @"The rules for writing Roman numerals allow for many ways of writing
        each number (see FAQ: Roman Numerals -
        http://projecteuler.net/index.php?section=faq&ref=roman_numerals).
        However, there is always a ""best"" way of writing a particular number.

        For example, the following represent all of the legitimate ways of
        writing the number sixteen:

        IIIIIIIIIIIIIIII
        VIIIIIIIIIII
        VVIIIIII
        XIIIIII
        VVVI
        XVI

        The last example being considered the most efficient, as it uses the
        least number of numerals.

        The 11K text file, roman.txt
        (http://projecteuler.net/project/roman.txt), contains one thousand
        numbers written in valid, but not necessarily minimal, Roman numerals;
        that is, they are arranged in descending units and obey the subtractive
        pair rule (see FAQ for the definitive rules for this problem).

        Find the number of characters saved by writing each of these in their
        minimal form.

        Note: You can assume that all the Roman numerals in the file contain no
        more than four consecutive identical units.")]
    public class Problem089 : BaseProblem
    {
        public Problem089()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            return GetText()
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(r => new
                {
                    Value = FromRoman(r),
                    PreviousRoman = r,
                })
                .Select(x => new
                {
                    x.Value,
                    x.PreviousRoman,
                    BetterRoman = ToRoman(x.Value),
                })
                .Sum(x => x.PreviousRoman.Length - x.BetterRoman.Length);
        }

        private readonly Tuple<string, int>[] RomanNumeralData =
        {
	        Tuple.Create("M",  1000),
	        Tuple.Create("CM", 900),
	        Tuple.Create("D",  500),
	        Tuple.Create("CD", 400),
	        Tuple.Create("C",  100),
	        Tuple.Create("XC", 90),
	        Tuple.Create("L",  50),
	        Tuple.Create("XL", 40),
	        Tuple.Create("X",  10),
	        Tuple.Create("IX", 9),
	        Tuple.Create("V",  5),
	        Tuple.Create("IV", 4),
	        Tuple.Create("I",  1),
        };

        public int FromRoman(string romanNumeral)
        {
            int value = 0;
            foreach (var v in RomanNumeralData)
            {
                while (romanNumeral.StartsWith(v.Item1))
                {
                    romanNumeral = romanNumeral.Substring(v.Item1.Length);
                    value += v.Item2;
                    if (romanNumeral.Length == 0)
                    {
                        return value;
                    }
                }
            }
            return value;
        }

        public string ToRoman(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException("Must be at least 1");
            }

            StringBuilder sb = new StringBuilder();

            foreach (var v in RomanNumeralData)
            {
                while (value >= v.Item2)
                {
                    sb.Append(v.Item1);
                    value -= v.Item2;
                }
            }

            return sb.ToString();
        }

        private readonly string Url = "http://projecteuler.net/project/roman.txt";
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
