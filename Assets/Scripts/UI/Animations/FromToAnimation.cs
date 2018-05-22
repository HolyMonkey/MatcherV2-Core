using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Animations
{
    class FromToAnimation : Animater
    {
        private float _speed;
        private Vector3 _to;

        public FromToAnimation(float speed, Vector3 to)
        {
            _speed = speed;
            _to = to;
        }

        public override void Update()
        {
            foreach(var t in animated)
            {
                t.position = Vector3.MoveTowards(t.position, _to, Time.deltaTime * _speed);
            }

            IEnumerable<Transform> finished = animated.Where((x) => x.position == _to);
            foreach(var e in finished)
            {
                OnEnd[e]?.Invoke(e);
                OnEnd.Remove(e);
            }
            animated.RemoveWhere((x) => x.position == _to);
            
        }
    }
}
