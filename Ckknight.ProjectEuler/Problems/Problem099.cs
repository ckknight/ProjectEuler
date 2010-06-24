using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(99,
        @"Comparing two numbers written in index form like 211 and 37 is not
        difficult, as any calculator would confirm that 211 = 2048  37 = 2187.

        However, confirming that 632382^518061 > 519432^525806 would be much
        more difficult, as both numbers contain over three million digits.

        Using base_exp.txt (http://projecteuler.net/project/base_exp.txt), a
        22K text file containing one thousand lines with a base/exponent pair
        on each line, determine which line number has the greatest numerical
        value.

        NOTE: The first two lines in the file represent the numbers in the
        example given above.")]
    public class Problem099 : BaseProblem
    {
        public Problem099()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            return GetText()
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select((l, i) => l
                    .Trim()
                    .Split(new[] { ',' }, 2)
                    .Let(a => new
                    {
                        LineNumber = i + 1,
                        Base = int.Parse(a[0]),
                        Exponent = int.Parse(a[1]),
                    }))
                .WithMax(x => x.Exponent * Math.Log(x.Base))
                .LineNumber;
        }

        private readonly string Url = "http://projecteuler.net/project/base_exp.txt";
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
