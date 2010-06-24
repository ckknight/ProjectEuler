using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(93,
        @"By using each of the digits from the set, {1, 2, 3, 4}, exactly once,
        and making use of the four arithmetic operations (+, , *, /) and
        brackets/parentheses, it is possible to form different positive integer
        targets.

        For example,

        8 = (4 * (1 + 3)) / 2
        14 = 4 * (3 + 1 / 2)
        19 = 4 * (2 + 3)  1
        36 = 3 * 4 * (2 + 1)

        Note that concatenations of the digits, like 12 + 34, are not allowed.

        Using the set, {1, 2, 3, 4}, it is possible to obtain thirty-one
        different target numbers of which 36 is the maximum, and each of the
        numbers 1 to 28 can be obtained before encountering the first non-
        expressible number.

        Find the set of four distinct digits, a < b < c < d, for which the
        longest set of consecutive positive integers, 1 to n, can be obtained,
        giving your answer as a string: abcd.")]
    public class Problem093 : BaseProblem
    {
        public override object CalculateResult()
        {
            var operations = new Func<Fraction, Fraction, Fraction>[]
            {
                (a, b) => a + b,
                (a, b) => a - b,
                (a, b) => a * b,
                (a, b) => a / b,
            };

            return new Range(1, 9, true)
                .GetCombinations(4)
                .Select(digits => digits
                    .GetPermutations()
                    .Select(p => new
                    {
                        a = p[0],
                        b = p[1],
                        c = p[2],
                        d = p[3],
                    })
                    .SelectMany(p => new Range(3)
                        .Aggregate(new[] { Enumerable.Empty<Func<Fraction, Fraction, Fraction>>() }.AsEnumerable(),
                            (a, i) =>
                                a.SelectMany(s => operations
                                    .Select(o => s.AppendItem(o))),
                            a => a
                                .Select(s => s
                                    .ToArray())
                                .ToArray())
                        .Select(o => new
                        {
                            x = o[0],
                            y = o[1],
                            z = o[2],
                        })
                        .SelectMany(o => new[]
                        {
                            o.x(o.y(o.z(p.a, p.b), p.c), p.d),
                            o.x(o.y(p.a, p.b), o.z(p.c, p.d)),
                            o.x(o.y(p.a, o.z(p.b, p.c)), p.d),
                            o.x(p.a, o.y(o.z(p.b, p.c), p.d)),
                            o.x(p.a, o.y(p.b, o.z(p.c, p.d)))
                        }))
                    .Let(r => new
                    {
                        Digits = digits,
                        Count = r
                            .Where(v => v > 0 && v.IsInteger)
                            .Distinct()
                            .OrderBy(v => v)
                            .ToMemorableEnumerable()
                            .TakeWhile(e => !e.HasPreviousValue || e.Value == e.PreviousValue + 1)
                            .Count()
                    }))
                .WithMax(x => x.Count)
                .Digits
                .StringJoin("");
        }
    }
}
