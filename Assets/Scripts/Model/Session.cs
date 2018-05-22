using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Session
    {

        private Stage[] _stages;
        private Queue<Stage> _stagesQueue;
        private int _goodAnswers, _badAnswers;
        private int GoodAnswers
        {
            get
            {
                return _goodAnswers;
            }

            set
            {
                _goodAnswers = value;
                OnGoodChoice?.Invoke();
                OnPrecentesChange?.Invoke(this);
            }
        }
        private int BadAnswers
        {
            get
            {
                return _badAnswers;
            }

            set
            {
                _badAnswers = value;
                OnBadChoice?.Invoke();
                OnPrecentesChange?.Invoke(this);
            }
        }
        private Dictionary<Stage, float> _stagesTicks = new Dictionary<Stage, float>();
        private float _maxStageTick = 15;

        public event Action<Session> OnPrecentesChange, OnFinite, OnStageChange, OnFailed;
        public event Action<Stage, float> OnStageTick;
        public event Action OnGoodChoice, OnBadChoice;

        public int GoodPrecent
        {
            get
            {
                return Precents(GoodAnswers, BadAnswers).Item1;
            }
        }
        public int BadPrecent
        {
            get
            {
                return Precents(GoodAnswers, BadAnswers).Item2;
            }
        }
        public bool IsFinite { get; private set; }
        public bool IsFailed { get; private set; }
        public bool IsEnded {
            get
            {
                return IsFinite || IsFailed;
            }
        }
        public Stage CurrentStage {
            get {
                if (_stagesQueue.Count > 0)
                {
                    return _stagesQueue.Peek();
                }
                else
                {
                    return null;
                }
            }
        }
        public IEnumerable<Stage> Stages => _stages;

        public Session(params Stage[] stages)
        {
            if (stages.Length == 0)
            {
                throw new ArgumentException("Need stages", "stages");
            }

            _stages = stages;
            _stagesQueue = new Queue<Stage>(_stages);

            foreach(var stage in _stages)
            {
                _stagesTicks.Add(stage, 0);
            }
        }

        public void Answer(int answer)
        {
            if (IsEnded)
            {
                throw new InvalidOperationException();
            }

            if (answer == CurrentStage.QuestedNumber)
            {
                GoodAnswers++;
            }
            else
            {
                BadAnswers++;
            }
        }

        public void ForceWrong()
        {
            if (IsEnded)
            {
                throw new InvalidOperationException();
            }

            BadAnswers++;
        }

        public bool NextStage()
        {
            if (IsEnded)
            {
                return false;
            }

            if(GoodPrecent < BadPrecent)
            {
                IsFailed = true;
                OnFailed?.Invoke(this);
                return false;
            }

            if (_stagesQueue.Count > 1)
            {
                _stagesQueue.Dequeue();
                OnStageChange?.Invoke(this);
            }
            else
            {
                IsFinite = true;
                OnStageChange?.Invoke(this);
                OnFinite?.Invoke(this);
            }
            
            return true;
        }

        public IEnumerable<Stage> GetStages()
        {
            return _stagesQueue;
        }

        public void SetStageTick(float tick)
        {
            _stagesTicks[CurrentStage] += tick;
            if (_stagesTicks[CurrentStage] >= _maxStageTick)
            {
                NextStage();
            }

            OnStageTick?.Invoke(CurrentStage,
                Mathf.Clamp(_stagesTicks[CurrentStage] / _maxStageTick, 0, 1));
        }

        private static Tuple<int, int> Precents(int val1, int val2)
        {
            if (val1 == 0 && val2 == 0)
                return new Tuple<int, int>(50, 50);
            if(val1 == 0)
                return new Tuple<int, int>(0, 100);
            if(val2 == 0)
                return new Tuple<int, int>(100, 0);

            float total = val1 + val2;
            return new Tuple<int, int>(Mathf.RoundToInt(val1 / total * 100), Mathf.RoundToInt(val2 / total * 100));
        }
    }
}
