using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Scripts.UI.Animations
{
    class ExplosionAnimation : Animater
    {
        private float _speed;
        private float _duration;
        private Vector2 _from;
        private Dictionary<Transform, Vector2> _directions = new Dictionary<Transform, Vector2>();
        private Dictionary<Transform, float> _times = new Dictionary<Transform, float>();

        public ExplosionAnimation(float speed, float duration)
        {
            _speed = speed;
            _duration = duration;
        }

        public void SetFromPoint(Vector2 from)
        {
            _from = from;
        }

        public override void Animate(Transform transform, Action<Transform> onEnd = null)
        {
            base.Animate(transform, onEnd);
            if (!_directions.ContainsKey(transform))
            {
                _directions.Add(transform, ((Vector2)transform.position - _from).normalized);
            }

            if (!_times.ContainsKey(transform))
            {
                _times.Add(transform, _duration);
            }
        }

        public override void Update()
        {
            foreach(var t in _times.Keys.ToList())
            {
                _times[t] -= Time.deltaTime;
            }

            foreach (var t in animated)
            {
                t.Translate(_directions[t] * Time.deltaTime * _speed);
            }

            animated.RemoveWhere((x) =>
            {
                if (_times[x] <= 0)
                {
                    _directions.Remove(x);
                    _times.Remove(x);
                    OnEnd[x]?.Invoke(x);
                    OnEnd.Remove(x);
                    return true;
                }
                else return false;
            });
        }
    }

    class ExplosionWithInput : IAnimater
    {
        public ExplosionAnimation _animater;

        public ExplosionWithInput(ExplosionAnimation animater)
        {
            _animater = animater;
        }

        public void Animate(IEnumerable<Transform> transforms, Action<Transform> onEnd = null)
        {
            _animater.SetFromPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _animater.Animate(transforms, onEnd);
        }

        public void Animate(Transform transform, Action<Transform> onEnd = null)
        {
            _animater.SetFromPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _animater.Animate(transform, onEnd);
        }

        public void Update()
        {
            _animater.Update();
        }
    }
}
