using Assets.Scripts;
using Assets.Scripts.Model;
using Matcher.ExpressionGenerator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Step
{ 
    public string Name;
    public string Description;
    public Sprite Image;

    public List<Mode> Modes;

    public StepState State;

    public enum StepState
    {
        Closed,
        Open,
        Completed
    }

    public void Run()
    {
        var activator = new SessionActivator(new Session(
            Modes.Select((mode) => new Stage(mode)).ToArray()
        ));
        activator.Run();
    }
}
