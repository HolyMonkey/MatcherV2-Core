using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Animations
{
    interface IAnimater
    {
        void Animate(IEnumerable<Transform> transforms, Action<Transform> onEnd = null);
        void Animate(Transform transform, Action<Transform> onEnd = null);
        void Update();
    }
}
