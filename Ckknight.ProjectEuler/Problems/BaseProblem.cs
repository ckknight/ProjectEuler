using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler.Problems
{
    public abstract class BaseProblem
    {
        public ProblemAttribute ProblemAttribute
        {
            get
            {
                ProblemAttribute result = this.GetType().GetCustomAttributes(typeof(ProblemAttribute), false).SingleOrDefault() as ProblemAttribute;
                if (result == null)
                {
                    throw new InvalidOperationException("Every problem must specify a ProblemAttribute");
                }

                return result;
            }
        }

        public abstract object CalculateResult();
    }
}
