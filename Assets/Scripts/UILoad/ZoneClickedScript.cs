using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneClickedScript : MonoBehaviour
{
    public GameObject ZoneChooserCanvas;
    public List<Step> Levels;
    public StepList List;

    public void ZoneClicked()
    {
        ZoneChooserCanvas.SetActive(false);
        List.Visualize(Levels);
    }
}
