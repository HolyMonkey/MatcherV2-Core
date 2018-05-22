using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Matcher.ExpressionGenerator;
using Assets.Scripts.Model;
using UnityEngine.EventSystems;
using Assets.Scripts.UI.Layout;

namespace Assets.Scripts
{
    public class BattleScreenController : MonoBehaviour
    {

        public Range ExpressionCount;
        public Range GoodCount;
        public GameObject WinWindow, FailWindow; 
        private Session _session;
        private IExpressionGroupController _group;
        private ILayouter _layouter = new StupidLayout();

        private float _timer;

        [Injection]
        public void Injection(Session session)
        {
            _session = session;
            _group = GetComponent<IExpressionGroupController>();

            _session.OnFinite += (s) =>
            {
                _group.Clear();
                WinWindow.SetActive(true);
            };
            _session.OnFailed += (s) =>
            {
                _group.Clear();
                FailWindow.SetActive(true);
            };

            _session.OnStageChange += (s) =>
            {
                _timer = 0;
                RecreatGroup();
            };
        }

        void Start()
        {
            RecreatGroup();
        }

        private void Update()
        {
            var pickedExpression = _group.Update();
            if(pickedExpression != null && !_session.IsEnded)
            {
                _session.Answer(pickedExpression.Match());
                RecreatGroup();
                _timer = 0;
            }

            if (!_session.IsEnded)
            {
                _session.SetStageTick(Time.deltaTime);
                _timer += Time.deltaTime;
                if(_timer >= 4)
                {
                    _timer = 0;
                    _session.ForceWrong();
                    RecreatGroup();
                }
            }
        }

        void RecreatGroup()
        {
            if (_session.IsEnded) return;

            _group.Clear();
            var expressions = _session.CurrentStage.GetExpressions(ExpressionCount.Random, GoodCount.Random);
            _group.CreateGroup(expressions, _layouter);
        }

    }
}