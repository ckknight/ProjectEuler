using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ckknight.ProjectEuler.Problems
{
    /// <summary>
    /// Attribute to define properties of the problem.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProblemAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new ProblemAttribute.
        /// </summary>
        /// <param name="number">The problem number.</param>
        /// <param name="description">The description of the problem.</param>
        public ProblemAttribute(int number, string description)
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException("number", number, "Must be at least 1");
            }
            else if (description == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (description == string.Empty)
            {
                throw new ArgumentException("Must not be empty", "value");
            }

            _number = number;
            _description = CleanDescription(description);
        }

        private readonly int _number;
        /// <summary>
        /// Problem number
        /// </summary>
        public int Number
        {
            get
            {
                return _number;
            }
        }

        private static readonly string UrlFormat = "http://projecteuler.net/index.php?section=problems&id={0}";
        /// <summary>
        /// The URL for the problem on the projecteuler.net site.
        /// </summary>
        public string Url
        {
            get
            {
                return string.Format(UrlFormat, Number);
            }
        }

        private readonly string _description;
        /// <summary>
        /// Description of the problem.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
        }

        private static readonly char[] _lineSpliters = new[] { '\n' };
        private static readonly Regex _endingWhitespace = new Regex(@"\s+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex _startingWhitespace = new Regex(@"^\s*", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Dictionary<int, Regex> _whitespaceStartRegexes = new Dictionary<int, Regex>();
        private static Regex GetWhitespaceStartRegex(int length)
        {
            Regex result;
            if (!_whitespaceStartRegexes.TryGetValue(length, out result))
            {
                _whitespaceStartRegexes[length] = result = new Regex(@"^\s{1," + length + @"}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            }
            return result;
        }
        private static string CleanDescription(string text)
        {
            string[] lines = text.Split(_lineSpliters);

            int startingWhitespaceLength = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line = _endingWhitespace.Replace(line, string.Empty);
                if (line != string.Empty && i > 0)
                {
                    Match match = _startingWhitespace.Match(line);
                    if (match.Success && (startingWhitespaceLength == -1 || match.Length < startingWhitespaceLength))
                    {
                        startingWhitespaceLength = match.Length;
                    }
                }
                lines[i] = line;
            }

            if (startingWhitespaceLength > 0)
            {
                Regex whitespaceStartRegex = GetWhitespaceStartRegex(startingWhitespaceLength);
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = whitespaceStartRegex.Replace(lines[i], string.Empty);
                }
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}
