using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;
using System.Net;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(79,
        @"A common security method used for online banking is to ask the user
        for three random characters from a passcode. For example, if the
        passcode was 531278, they may ask for the 2nd, 3rd, and 5th characters;
        the expected reply would be: 317.

        The text file, keylog.txt, contains fifty successful login attempts.

        Given that the three characters are always asked for in order, analyse
        the file so as to determine the shortest possible secret passcode of
        unknown length.")]
    public class Problem079 : BaseProblem
    {
        public Problem079()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            DefaultDictionary<char, HashSet<char>> lefts = new DefaultDictionary<char, HashSet<char>>(c => new HashSet<char>());
            foreach (char[] code in GetText()
                    .Split(new[] { '\n' })
                    .Select(l => l.Trim())
                    .Where(l => l != string.Empty)
                    .Select(l => l.ToCharArray()))
            {
                for (int i = 0; i < code.Length; i++)
                {
                    lefts[code[i]].UnionWith(code.Take(i));
                }
            }

            List<char> password = new List<char>();
            while (lefts.Count > 0)
            {
                char c = lefts
                    .Where(p => p.Value.Count == 0)
                    .Select(p => p.Key)
                    .First();

                password.Add(c);
                lefts.Remove(c);
                foreach (var p in lefts)
                {
                    p.Value.Remove(c);
                }
            }
            return new string(password.ToArray());
        }

        private readonly string Url = "http://projecteuler.net/project/keylog.txt";
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
