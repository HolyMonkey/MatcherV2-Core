using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Layout
{
    public interface ILayouter
    {
        Queue<Vector3> GetLayout(int count);
    }
}
