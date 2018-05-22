
namespace Matcher.ExpressionGenerator
{
    [System.Serializable]
    public struct Range
    {

        public int Min, Max;
        public int Random => UnityEngine.Random.Range(Min, Max);

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
