using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Animations
{
    abstract class Animater : IAnimater
    {
        protected HashSet<Transform> animated;
        protected Dictionary<Transform, Action<Transform>> OnEnd;

        public Animater()
        {
            animated = new HashSet<Transform>();
            OnEnd = new Dictionary<Transform, Action<Transform>>();
        }

        public void Animate(IEnumerable<Transform> transforms, Action<Transform> onEnd = null)
        {
            foreach(var t in transforms)
            {
                Animate(t, onEnd);
            }
        }
        public virtual void Animate(Transform transform, Action<Transform> onEnd = null)
        {
            animated.Add(transform);

            if (!OnEnd.ContainsKey(transform))
            {
                OnEnd.Add(transform, onEnd);
            }
        }
        public abstract void Update();
    }
}
