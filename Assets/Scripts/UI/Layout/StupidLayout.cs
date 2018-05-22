using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UI.Layout
{
    class StupidLayout : ILayouter
    {
        public Queue<Vector3> GetLayout(int count)
        {
            Queue<Vector3> layout = new Queue<Vector3>();

            int positionInRow = 0;
            int currentRow = 0;
            int countPerRow = 3;
            float size = (5f / countPerRow);

            for (int i = 0; i < count; i++)
            {
                layout.Enqueue(new Vector3((positionInRow * size) + Random.Range(0, size - 1.5f) - 1.5f, currentRow * size));
                if (++positionInRow == countPerRow)
                {
                    positionInRow = 0;
                    currentRow++;
                }

            }

            return layout;
        }
    }
}
