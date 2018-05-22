using Assets.Scripts;
using Assets.Scripts.Model;
using Matcher.ExpressionGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Level
{
    public List<Mode> Modes;
    public string Name;

    public void Run()
    {
        var activator = new SessionActivator(new Session(
            Modes.Select((mode) => new Stage(mode)).ToArray()
        ));
        activator.Run();
    }
}
