using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;

public class MenuWindowsTest {

    [Test]
	public void IsListContainsNeedWindows()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");
        var windowController = Editor.FindObjectOfType<WindowController>();
        windowController.OpenStoryWindow();
        windowController.OpenStatsWindow();
        windowController.OpenTrainingWindow();
        //

        windowController.OpenTrainingWindow();
    }
}
