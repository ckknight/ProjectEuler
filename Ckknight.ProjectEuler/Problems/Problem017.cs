using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(17,
        @"If the numbers 1 to 5 are written out in words: one, two, three,
        four, five, then there are 3 + 3 + 5 + 4 + 4 = 19 letters used in
        total.

        If all the numbers from 1 to 1000 (one thousand) inclusive were written
        out in words, how many letters would be used?


        NOTE: Do not count spaces or hyphens. For example, 342 (three hundred
        and forty-two) contains 23 letters and 115 (one hundred and fifteen)
        contains 20 letters. The use of ""and"" when writing out numbers is in
        compliance with British usage.")]
    public class Problem017 : BaseProblem
    {
        private static Regex _invalidCharactersRegex = new Regex(@"[^a-z]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        public override object CalculateResult()
        {
            return new Range(1, 1000, true)
                .Select(n => NumberToWords(n))
                .Select(w => _invalidCharactersRegex.Replace(w, string.Empty))
                .Sum(w => w.Length);
        }

        private static readonly Dictionary<int, string> _reservedNumbers = new Dictionary<int, string>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
            { 4, "four" },
            { 5, "five" },
            { 6, "six" },
            { 7, "seven" },
            { 8, "eight" },
            { 9, "nine" },
            { 10, "ten" },
            { 11, "eleven" },
            { 12, "twelve" },
            { 13, "thirteen" },
            { 14, "fourteen" },
            { 15, "fifteen" },
            { 16, "sixteen" },
            { 17, "seventeen" },
            { 18, "eighteen" },
            { 19, "nineteen" },
            { 20, "twenty" },
            { 30, "thirty" },
            { 40, "forty" },
            { 50, "fifty" },
            { 60, "sixty" },
            { 70, "seventy" },
            { 80, "eighty" },
            { 90, "ninety" },
        };
        public string NumberToWords(int number)
        {
            if (number < 1 || number > 1000)
            {
                throw new ArgumentOutOfRangeException("number", number, "Must be at least 1 and at most 1000");
            }

            if (number == 1000)
            {
                return "one thousand";
            }

            string prefix = string.Empty;
            if (number >= 100)
            {
                string hundredNumber = _reservedNumbers[number / 100] + " hundred";
                if ((number % 100) == 0)
                {
                    return hundredNumber;
                }
                prefix = hundredNumber + " and ";
                number %= 100;
            }

            if (_reservedNumbers.ContainsKey(number))
            {
                return prefix + _reservedNumbers[number];
            }

            return prefix + _reservedNumbers[(number / 10) * 10] + "-" + _reservedNumbers[number % 10];
        }
    }
}
