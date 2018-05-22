using System;
using UnityEngine;
using System.Collections.Generic;
using Matcher.ExpressionGenerator;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Model
{
    public class Stage
    {
        private IMode _mode;
        private IExpressionGenerator _generator;

        private int _questionNumber;
        public int QuestedNumber => _questionNumber;

        public Stage(IMode mode)
        {
            if(mode == null)
            {
                throw new ArgumentNullException("mode");
            }
            _mode = mode;
            _generator = new ExpressionGenerator(mode);
            _questionNumber = _mode.Operand.Random;
        }

        public IEnumerable<IExpression> GetExpressions(int count, int goodCount)
        {
            List<IExpression> expressions = new List<IExpression>(count);

            while (count-- > 0)
            {
                int neededResult = _questionNumber;
                if (goodCount-- <= 0)
                {
                    neededResult = GetSpreaded(neededResult, _mode.WrongScater);
                }

                IExpression expression = _generator.Generate(neededResult);
                if(expression == null)
                {
                    throw new InvalidOperationException("not posible generate expression for on this mode: " + _mode.ToString());
                }
                expressions.Add(expression);
            }

            expressions.Sort((e1, e2) => Random.Range(-1, 2));
            return expressions;
        }

        public static int GetSpreaded(int value, int precent)
        {
            int spread = (int)Math.Round((value / 100f) * precent);
            if (StaticRandom.Next(0, 2) == 0)
            {
                return (value - StaticRandom.Next(1, 1 + spread));
            }
            else
            {
                return (value + StaticRandom.Next(1, 1 + spread));
            }
        }
    }
}
