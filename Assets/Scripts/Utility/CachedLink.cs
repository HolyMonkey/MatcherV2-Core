using UnityEngine;

namespace Assets.Scripts.Utility
{
    class CachedLink<T>
    {
        private GameObject _go;
        private T _component;
        public T Component
        {
            get
            {
                if (_component == null)
                    _component = _go.GetComponent<T>();
                return _component;
            }
        }

        public CachedLink(GameObject go)
        {
            _go = go;
        }
    }
}
