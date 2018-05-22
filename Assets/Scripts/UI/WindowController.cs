using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowController : MonoBehaviour {

    public List<GameObject> Windows;
    
	public void OpenWindow(string name)
    {
        foreach(var go in Windows.Where((x) => x.activeSelf))
        {
            go.SetActive(false);
            go.SendMessage("OnWindowClosedHandler", SendMessageOptions.DontRequireReceiver);
        }

        var toOpen = Windows.First((x) => x.name.ToLower() == name);
        if(toOpen != null)
        {
            toOpen.SetActive(true);
            toOpen.SendMessage("OnWindowOpenHandle", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.LogError("Window not found " +  name);
        }
    }

    //binding
    public void OpenStoryWindow()
    {
        OpenWindow("story");
    }

    public void OpenTrainingWindow()
    {
        OpenWindow("training");
    }

    public void OpenStatsWindow()
    {
        OpenWindow("stats");
    }
}
