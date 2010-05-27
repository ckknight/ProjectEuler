using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ckknight.ProjectEuler.Collections;
using System.Text.RegularExpressions;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(59,
        @"Each character on a computer is assigned a unique code and the
        preferred standard is ASCII (American Standard Code for Information
        Interchange). For example, uppercase A = 65, asterisk (*) = 42, and
        lowercase k = 107.

        A modern encryption method is to take a text file, convert the bytes to
        ASCII, then XOR each byte with a given value, taken from a secret key.
        The advantage with the XOR function is that using the same encryption
        key on the cipher text, restores the plain text; for example,
        65 XOR 42 = 107, then 107 XOR 42 = 65.

        For unbreakable encryption, the key is the same length as the plain
        text message, and the key is made up of random bytes. The user would
        keep the encrypted message and the encryption key in different
        locations, and without both ""halves"", it is impossible to decrypt the
        message.

        Unfortunately, this method is impractical for most users, so the
        modified method is to use a password as a key. If the password is
        shorter than the message, which is likely, the key is repeated
        cyclically throughout the message. The balance for this method is using
        a sufficiently long password key for security, but short enough to be
        memorable.

        Your task has been made easy, as the encryption key consists of three
        lower case characters. Using cipher1.txt
        (http://projecteuler.net/project/cipher1.txt), a file containing the
        encrypted ASCII codes, and the knowledge that the plain text must
        contain common English words, decrypt the message and find the sum of
        the ASCII values in the original text.")]
    public class Problem059 : BaseProblem
    {
        public Problem059()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            int[] values = GetText()
                .Split(',')
                .Select(x => int.Parse(x.Trim()))
                .ToArray();
            
            int[][] secretCodes = new Range('a', 'z', true)
                .SelectMany(a => new Range('a', 'z', true)
                    .SelectMany(b => new Range('a', 'z', true)
                        .Select(c => new[] { a, b, c })))
                .ToArray();

            string[] commonWords = new[] { "the", "of", "to", "and", "a", "in", "is", "it", "you", "that", "he", "was", "for", "on", "are", "with" };

            Regex punctuationRegex = new Regex(@"[^\w\s]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            Regex whitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var possibilities = secretCodes
                .Select(x => new {
                    Code = x,
                    Result = new string(values
                        .Select((c, i) => c ^ x[i % x.Length])
                        .Select(c => (char)c)
                        .ToArray())
                })
                .Select(x => new {
                    x.Code,
                    x.Result,
                    Words = whitespaceRegex.Split(punctuationRegex.Replace(x.Result, string.Empty))
                        .Where(w => w != string.Empty)
                        .ToMultiHashSet(StringComparer.InvariantCultureIgnoreCase),
                })
                .Select(x => new {
                    x.Code,
                    x.Result,
                    x.Words,
                    Value = commonWords
                        .Select(w => x.Words.GetCount(w))
                        .Sum()
                })
                .ToArray();

            var bestPossibility = possibilities
                .Aggregate((a, b) => a.Value > b.Value ? a : b);

            return bestPossibility
                .Result
                .Sum(c => (int)c);
        }

        private readonly string Url = "http://projecteuler.net/project/cipher1.txt";
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
