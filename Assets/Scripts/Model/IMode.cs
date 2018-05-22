using System.Collections.Generic;

namespace Matcher.ExpressionGenerator
{
    public interface IMode
    {
        IEnumerable<IExpressionTemplate> ExpressionTemplates { get; }
        Range GoodResult { get; }
        int WrongScater { get; }
        Range Operand { get; }
    }
}
