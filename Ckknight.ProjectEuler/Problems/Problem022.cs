using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(22,
        @"Using names.txt (http://projecteuler.net/project/names.txt), a 46K
        text file containing over five-thousand first names, begin by sorting
        it into alphabetical order. Then working out the alphabetical value for
        each name, multiply this value by its alphabetical position in the list
        to obtain a name score.

        For example, when the list is sorted into alphabetical order, COLIN,
        which is worth 3 + 15 + 12 + 9 + 14 = 53, is the 938th name in the
        list. So, COLIN would obtain a score of 938 * 53 = 49714.

        What is the total of all the name scores in the file?")]
    public class Problem022 : BaseProblem
    {
        public Problem022()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            return GetText()
                .Split(',')
                .Select(name => name.Trim('"'))
                .OrderBy(name => name)
                .Select((name, i) => name
                    .ToCharArray()
                    .Select(c => c - 'A' + 1)
                    .Sum() * (i + 1))
                .Sum();
        }

        private readonly string Url = "http://projecteuler.net/project/names.txt";
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
