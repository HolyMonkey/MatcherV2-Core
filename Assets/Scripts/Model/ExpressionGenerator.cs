using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Matcher.ExpressionGenerator
{
    public interface IExpressionGenerator
    {
        /// <summary>
        /// Генерирует выражение исходя из результата
        /// </summary>
        /// <param name="result">Результат генерироемого выражения</param>
        /// <returns>Возвращает выражение или null если сгенерировать не удалось</returns>
        IExpression Generate(int result); //4

        /// <summary>
        /// Помещает выражение во внутренний блэклист, и не генерирует его в следующий раз
        /// </summary>
        /// <param name="expression">Выражение которое попадает в блэклист</param>
        void PushInBlacklist(IExpression expression);
    }

    public interface IExpression
    {
        /// <summary>
        /// Форматирует выражение для отображения
        /// </summary>
        /// <param name="bracketed">Разделять ли блоки выражений скобками</param>
        /// <returns></returns>
        string ToString(bool bracketed = false);

        /// <summary>
        /// Вычисляет выражение
        /// </summary>
        /// <returns>Результат выражения</returns>
        int Match();
    }

    public interface IExpressionTemplate
    {
        int GetPlusCount();
        int GetMinusCount();
        int GetDivisionCount();
        int GetMultiplicationCount();

    }

    class ExpressionGenerator : IExpressionGenerator
    {
        private IMode _mode;
        private Tree _tree;

        public ExpressionGenerator(IMode mode)
        {
            _mode = mode;
            _tree = new Tree();
        }

        public IExpression Generate(int result)
        {
            var templates = _mode.ExpressionTemplates.ToList();
            var template = templates[UnityEngine.Random.Range(0, templates.Count)];

            var expression = _tree.Generate(template.GetPlusCount(), template.GetMinusCount(),
                                   template.GetDivisionCount(), template.GetMultiplicationCount(),
                                   result);

            return new StaticExpression(expression, result);
        }

        public void PushInBlacklist(IExpression expression)
        {
            throw new NotImplementedException();
        }
    }

    class StaticExpression : IExpression
    {
        private string _expression;
        private int _result;

        public StaticExpression(string expression, int result)
        {
            _expression = expression;
            _result = result;
        }

        public int Match()
        {
            return _result;
        }

        string IExpression.ToString(bool bracketed)
        {
            return _expression;
        }
    }

    [System.Serializable]
    public class Mode : IMode
    {
        [SerializeField]
        private List<StaticExpressionTemplate> _expressions;
        [SerializeField]
        private Range _goodResult, _operand;
        [SerializeField]
        private int _wrongScater;

        public IEnumerable<IExpressionTemplate> ExpressionTemplates => _expressions;

        public Mode(Range goodResult, Range operand, int wrongScater)
        {
            _goodResult = goodResult;
            _operand = operand;
            _wrongScater = wrongScater;
        }

        public Range GoodResult => _goodResult;
        public int WrongScater => _wrongScater;
        public Range Operand => _operand;
    }

    [System.Serializable]
    class StaticExpressionTemplate : IExpressionTemplate
    {
        [SerializeField]
        private int divisionCount, minusCount, mulCount, plusCount;

        public int GetDivisionCount()
        {
            return divisionCount;
        }

        public int GetMinusCount()
        {
            return minusCount;
        }

        public int GetMultiplicationCount()
        {
            return mulCount;
        }

        public int GetPlusCount()
        {
            return plusCount;
        }
    }

    public class Tree
    {
        private TreeNode Root;
        private static Random rnd = new Random();

        public void Destroy()
        {
            Root = new TreeNode();
        }

        public Tree()
        {
            Root = new TreeNode();
        }

        #region CreateTree(int[],int)

        private bool FindNotZero(int[] Mas)
        {
            bool result = false;
            int summa = 0;
            for (var i = 0; i < Mas.Length; i++) summa += Mas[i];
            if (summa > 0) result = true;
            return result;
        }


        public void CreateTree(int[] Mas, int result)
        {
            TreeNode root = Root;
            bool res = false;
            int i = 0;
            int kof = 1;
            int element;
            int Result = result / 2;
            if (result % 2 != 0) Result++;
            if ((Mas[2] == 0) && (Mas[3] == 0))
            {
                Result = result;
            }
            if ((Mas[0] == 0) && (Mas[1] == 0))
            {
                Result = result;
            }
            while (FindNotZero(Mas))
            {
                res = false;

                while (res != true)
                {
                    if ((Mas[2] != 0) || (Mas[3] != 0)) i = StaticRandom.Next(2, 4);
                    else i = StaticRandom.Next(0, 2);
                    if (Mas[i] != 0) res = true;
                }

                switch (i)
                {
                    case 0:
                        Mas[i]--;

                        if (Root.RightNode == null)
                        {
                            if (Root.LeftNode != null) Result = result / 2;
                            Root.RightNode = new TreeNode(Result);
                        }

                        root = Root.RightNode;
                        while (root.LeftNode != null) root = root.LeftNode;
                        root.LeftNode = new TreeNode('+');
                        root = root.LeftNode;
                        element = StaticRandom.Next(1, Result + 1);

                        if (Result != 1)
                        {
                            while ((element <= 0) || (Result - element == 0)) element = StaticRandom.Next(1, Result + 1);
                            root.LeftNode = new TreeNode(element);
                            root.RightNode = new TreeNode(Result - element);
                        }
                        else
                        {
                            root.LeftNode = new TreeNode(2);
                            root.RightNode = new TreeNode(-1);
                        }

                        Result = element;
                        break;

                    case 1:
                        Mas[i]--;

                        if (Root.RightNode == null)
                        {
                            if (Root.LeftNode != null) Result = result / 2;
                            Root.RightNode = new TreeNode(Result);
                        }

                        root = Root.RightNode;
                        while (root.LeftNode != null) root = root.LeftNode;

                        root.LeftNode = new TreeNode('-');
                        root = root.LeftNode;

                        element = StaticRandom.Next(Result, Result + 20);
                        while ((element <= 0) || (element == 0) || (element - Result == 0)) element = StaticRandom.Next(Result, Result + Result / 100 * 20 + 5);


                        root.LeftNode = new TreeNode(element);
                        root.RightNode = new TreeNode(element - Result);


                        Result = element;
                        break;

                    case 2:
                        Mas[i]--;

                        root = Root;
                        while (root.LeftNode != null) root = root.LeftNode;
                        root.LeftNode = new TreeNode('/');
                        root = root.LeftNode;

                        kof = 1;
                        element = StaticRandom.Next(Result, Result + Result + 200);

                        while ((element % kof != 0) || (element / kof != Result))
                        {
                            if ((kof > element) || (element / kof < Result))
                            {
                                element = StaticRandom.Next(Result, Result + Result + 200);
                                kof = 1;
                            }
                            kof++;
                        }


                        root.LeftNode = new TreeNode(element);
                        root.RightNode = new TreeNode(kof);

                        Result = element;


                        break;

                    case 3:
                        Mas[i]--;

                        root = Root;
                        while (root.LeftNode != null) root = root.LeftNode;
                        root.LeftNode = new TreeNode('*');
                        root = root.LeftNode;


                        kof = 1;
                        element = StaticRandom.Next(1, Result + 1);

                        while ((element * kof != Result))
                        {
                            kof++;
                            if (kof * element > Result)
                            {
                                element = StaticRandom.Next(1, Result + 1);
                                kof = 1;
                            }
                        }

                        if (kof > element)
                        {
                            root.LeftNode = new TreeNode(kof);
                            root.RightNode = new TreeNode(element);
                            Result = kof;
                        }
                        else
                        {
                            root.LeftNode = new TreeNode(element);
                            root.RightNode = new TreeNode(kof);
                            Result = element;
                        }

                        break;
                }

            }
        }
        #endregion

        #region Строка


        private string RowProcess(StringBuilder str)
        {
            var i = 0;
            while (i < str.Length)
            {
                if (((int)str[i] == 0) && (i + 1 < str.Length))
                {
                    if ((str[i + 1] != '-') && (i != 0)) str[i] = '+';
                    else str.Remove(i, 1);
                }
                else
                if ((str[i] == '-') && (str[i + 1] == '-'))
                {
                    str[i] = '+';
                    str.Remove(i + 1, 1);
                }
                else if (str[i] == '+' && str[i + 1] == '-') str.Remove(i, 1);
                else if (str[i] == '-' && str[i + 1] == '+') str.Remove(i + 1, 1);


                i++;
            }
            return Convert.ToString(str);
        }

        private StringBuilder ToString(TreeNode root)
        {
            StringBuilder str = new StringBuilder("");
            if (root != null)
            {
                if (root.LeftNode != null) str.Insert(str.Length, ToString(root.LeftNode));

                if (root.Sign != 'e') str.Insert(str.Length, root.Sign);
                else
                {
                    if (((root.LeftNode != null) && (root.RightNode == null)) || ((root.LeftNode == null) && (root.RightNode != null))) ;
                    else str.Insert(str.Length, root.Figure);
                }

                if (root.RightNode != null) str.Insert(str.Length, ToString(root.RightNode));
            }
            return str;
        }

        #endregion

        public string Generate(int pCount, int sCount, int dCount, int mCount, int result)
        {
            Destroy();
            Root.Figure = result;
            int[] Mas = new int[4] { pCount, sCount, dCount, mCount };
            CreateTree(Mas, result);
            string str = RowProcess(ToString(Root));
            // Console.WriteLine((int)str[4]);
            return str;
        }
    }

    public static class StaticRandom
    {
        private static Random random = new Random();
        private static object myLock = new object();

        /// <summary>
        /// Returns a random number within a specified range.
        ///
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned. </param><param name="max">The exclusive upper bound of the random number returned.
        ///             maxValue must be greater than or equal to minValue.
        ///             </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        ///             that is, the range of return values includes minValue but not maxValue.
        ///             If minValue equals maxValue, minValue is returned.
        ///
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">minValue is greater than maxValue.</exception>
        public static int Next(int min, int max)
        {
            lock (StaticRandom.myLock)
                return StaticRandom.random.Next(min, max);
        }


    }

    class TreeNode
    {
        private int figure;
        private char sign;
        private TreeNode leftNode;
        private TreeNode rightNode;

        public int Figure { set { figure = value; } get { return figure; } }
        public char Sign { set { sign = value; } get { return sign; } }
        public TreeNode LeftNode { set { leftNode = value; } get { return leftNode; } }
        public TreeNode RightNode { set { rightNode = value; } get { return rightNode; } }

        public TreeNode() { }

        public TreeNode(int figure)
        {
            Figure = figure;
            Sign = 'e';
        }

        public TreeNode(char sign)
        {
            Sign = sign;
        }

    }
}
