using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Model;
using Assets.Scripts.Utility;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class QuestedNumber : MonoBehaviour
{
    private CachedLink<TextMeshProUGUI> _text;
    private Session _session;

    [Injection]
    public void Injection(Session session)
    {
        _text = new CachedLink<TextMeshProUGUI>(gameObject);
        _session = session;
        _session.OnStageChange += StageChangeHandler;
        StageChangeHandler(session);
    }

    public void UpdateText(int number)
    {
        UpdateText(number.ToString());
    }

    public void UpdateText(string text)
    {
        _text.Component.text = text;
    }


    private void StageChangeHandler(Session session)
    {
        if (!session.IsEnded)
        {
            UpdateText(session.CurrentStage.QuestedNumber);
        }
        else
        {
            UpdateText("Конец");
        }
    }

    public void OnDestroy()
    {
        _session.OnStageChange -= StageChangeHandler;
    }
}
