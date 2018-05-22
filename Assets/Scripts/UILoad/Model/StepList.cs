using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepList : MonoBehaviour
{
    public Transform Container;
    public GameObject Presenter;
    private int iterator;

    //public void Visualize()
    public void Visualize(List<Step> Steps)
    {
        iterator = 1;

        foreach (Transform child in Container)
        {
            Destroy(child.gameObject);
        }

        foreach (var step in Steps)
        {
            GameObject stepGo = Instantiate(Presenter, Container);
            stepGo.GetComponent<StepPresenter>().Visualize(step, iterator);
            iterator++;
        }
    }
}
