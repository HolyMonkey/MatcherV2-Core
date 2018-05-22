using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class Precenter : MonoBehaviour
    {
        public PrecenterSegment Good, Bad;
        private Session _session;
        private RectTransform _transform;

        void Start()
        {
            _transform = GetComponent<RectTransform>();
            UpdateElements(50, 50);
        }

        [Injection]
        public void Injection(Session session)
        {
            _session = session;
            _session.OnPrecentesChange += UpdateWidth;
        }

        public void UpdateWidth(Session session = null)
        {
            if (session == null) session = _session;

            UpdateElements(session.GoodPrecent, session.BadPrecent);
        }

        public void UpdateElements(float goodPrecent, float badPrecent)
        {
            var onePrecent = _transform.rect.width / 100;

            Good.SetSizeFromPrecent(onePrecent, goodPrecent);
            Bad.SetSizeFromPrecent(onePrecent, badPrecent);
        }

        private void OnDestroy()
        {
            if(_session != null)
                _session.OnPrecentesChange -= UpdateWidth;
        }
    }

    [System.Serializable]
    public class PrecenterSegment
    {
        public Text Label;
        public RectTransform Transform;

        public void SetSizeFromPrecent(float unit, float precent)
        {
            Label.text = precent.ToString();
            Transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, unit * precent);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Precenter)), UnityEditor.CanEditMultipleObjects]
    public class PrecenterEditor : UnityEditor.Editor
    {
        private float goodPrecent;

        protected virtual void OnSceneGUI()
        {
            Precenter precenter = (Precenter)target;    
        }

        public override void OnInspectorGUI()
        {
            Precenter precenter = (Precenter)target;
            base.DrawDefaultInspector();
            if (GUILayout.Button("Recanculate"))
            {
                precenter.UpdateElements(36, 70);
            }
        }
    }
#endif
}
