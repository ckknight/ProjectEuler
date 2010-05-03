using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Problems;
using System.Diagnostics;

namespace Ckknight.ProjectEuler
{
    class Program
    {
        static void Main(string[] args)
        {
            RunProblem(12);
        }

        public static void RunProblem(int number)
        {
            BaseProblem problem = GetProblemByNumber(number);
            Console.WriteLine("Problem #{0}", problem.ProblemAttribute.Number);
            Console.WriteLine("URL: {0}", problem.ProblemAttribute.Url);
            Console.WriteLine("Description:");
            Console.WriteLine(problem.ProblemAttribute.Description);
            Console.WriteLine();

            Stopwatch watch = Stopwatch.StartNew();
            object result = problem.CalculateResult();
            watch.Stop();
            Console.WriteLine("Result:");
            Console.WriteLine();
            Console.WriteLine(result);
            Console.WriteLine();
            Console.WriteLine("Done in {0}.", watch.Elapsed);
            Console.ReadLine();
        }

        private static IDictionary<int, Type> _problemTypes;
        public static IDictionary<int, Type> GetAllProblemTypes()
        {
            if (_problemTypes == null)
            {
                _problemTypes = typeof(BaseProblem).Assembly
                    .GetTypes()
                    .Where(t => typeof(BaseProblem).IsAssignableFrom(t) && t != typeof(BaseProblem))
                    .Select(t => new { Type = t, Attribute = (ProblemAttribute)t.GetCustomAttributes(typeof(ProblemAttribute), false).SingleOrDefault() })
                    .Where(x => x.Attribute != null)
                    .ToDictionary(x => x.Attribute.Number, x => x.Type);
            }
            return _problemTypes;
        }

        public static Type GetProblemTypeByNumber(int number)
        {
            Type type;
            if (!GetAllProblemTypes().TryGetValue(number, out type))
            {
                throw new ArgumentOutOfRangeException("number", number, "No problem found");
            }

            return type;
        }

        public static BaseProblem GetProblemByNumber(int number)
        {
            Type type = GetProblemTypeByNumber(number);

            BaseProblem result = Activator.CreateInstance(type) as BaseProblem;
            if (result == null)
            {
                throw new InvalidOperationException(string.Format("Cannot create instance of problem {0}", number));
            }

            return result;
        }
    }
}
