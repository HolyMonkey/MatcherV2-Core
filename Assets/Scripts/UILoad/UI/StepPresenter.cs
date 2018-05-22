using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StepPresenter : MonoBehaviour
{
    public Text Name;
    public Text Description;
    public Image Additional;
    public Image CheckMark;
    public Button Button;
    public Image Line;
    public TextMeshProUGUI MissionNumber;
    
    public Sprite OpenSprite, ClosedSprite, CompletedSprite;
    private Step _current;
     
    public void Visualize(Step step, int iterator)
    {
        _current = step;
        Name.text = step.Name;
        Description.text = step.Description;

        if (step.Image != null)
        {
            Additional.sprite = step.Image;
        }
        else
        {
            transform.GetComponent<RectTransform>()
                .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 175);
            Line.rectTransform.anchoredPosition = new Vector2(50, -140);
            Line.rectTransform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 110);
            Destroy(Additional.gameObject);
        }
        
        MissionNumber.text = iterator.ToString();

        switch (step.State)
        {
            case Step.StepState.Open:
                CheckMark.sprite = OpenSprite; 
                break;
            case Step.StepState.Closed:
                CheckMark.sprite = ClosedSprite;
                break;
            case Step.StepState.Completed:
                CheckMark.sprite = CompletedSprite;
                break;
        }

        Button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        _current.Run();
    }
}
