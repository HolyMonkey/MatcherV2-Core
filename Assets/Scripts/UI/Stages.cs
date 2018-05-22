using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Matcher.ExpressionGenerator;
using Assets.Scripts.Model;
using UnityEngine.EventSystems;
using Assets.Scripts;
using System;

public class Stages : MonoBehaviour {

    public GameObject StepElement, BetweenElement;
    private Dictionary<Stage, Toggle> _elements = new Dictionary<Stage, Toggle>();
    private Dictionary<Stage, Slider> _betweens = new Dictionary<Stage, Slider>();
    private Toggle _lastToggle;
    private Session _session;
    private Transform _transform;
    private Stage Current;

    [Injection]
    public void Injection(Session session)
    {
        _session = session;
        _transform = transform;

        List<Stage> stages =  _session.GetStages().ToList();

        var stagesWidth = StepElement.GetComponent<RectTransform>().rect.width;
        var width = GetComponent<RectTransform>().rect.width;
        var betweenWidth = width - stagesWidth * stages.Count;

        for (int i = 0; i < stages.Count; i++)
        {
            var stepGo = Instantiate(StepElement, _transform);
            var toggle = stepGo.GetComponent<Toggle>();
            toggle.isOn = false;
            _elements.Add(stages[i], toggle);


            var betweenGo = Instantiate(BetweenElement, _transform);
            _betweens.Add(stages[i], betweenGo.GetComponent<Slider>());         
        }

        _lastToggle = Instantiate(StepElement, _transform).GetComponent<Toggle>();

        _session.OnStageChange += StageChangeHandler;
        _session.OnStageTick += StageTickHandler;
        _session.OnFinite += SessionFiniteHandler;
        StageChangeHandler(session);
    }


    private void StageChangeHandler(Session session)
    {
         _elements[session.CurrentStage].isOn = true;  
    }

    private void StageTickHandler(Stage stage, float totalTick)
    {
        if (_betweens.ContainsKey(stage))
        {
            _betweens[stage].value = totalTick;
        }
    }

    private void SessionFiniteHandler(Session session)
    {
        _lastToggle.isOn = true;
    }

    private void OnDestroy()
    {
        _session.OnStageChange -= StageChangeHandler;
        _session.OnStageTick -= StageTickHandler;
        _session.OnFinite -= SessionFiniteHandler;
    }
}
