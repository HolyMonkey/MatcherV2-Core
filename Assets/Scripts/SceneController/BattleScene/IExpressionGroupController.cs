using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Matcher.ExpressionGenerator;
using Assets.Scripts.UI.Layout;

public interface IExpressionGroupController
{
    void CreateGroup(IEnumerable<IExpression> expressions, ILayouter layouter);
    void Clear();
    IExpression Update();
}
