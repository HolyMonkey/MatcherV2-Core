using UnityEngine;
using System.Collections;
using Matcher.ExpressionGenerator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using Assets.Scripts.UI.Animations;
using Assets.Scripts.UI.Layout;
using TMPro;

public class AnimatedGroupController : MonoBehaviour, IExpressionGroupController
{
    public GameObject ExpressionPrefab;
    [Range(0, 10)]
    public float Speed;

    public Vector3 StartPoint, MiddlePoint;
    private IAnimater InAnimater, OutAnimater;
    private Dictionary<GameObject, IExpression> _expressionsOnScene = new Dictionary<GameObject, IExpression>();
    private EventTrigger.Entry _onInterectionEntry = null;
    public EventTrigger.Entry OnInterectionEnrty
    {
        get
        {
            if(_onInterectionEntry == null)
            {
                _onInterectionEntry = new EventTrigger.Entry();
                _onInterectionEntry.eventID = EventTriggerType.PointerClick;
                _onInterectionEntry.callback.AddListener(ExpressionInterectionHandler);
            }
            return _onInterectionEntry;
        }
    }

    private GameObject _selectedObject;

    public void Awake()
    {
        InAnimater = new FromToAnimation(Speed, MiddlePoint);
        OutAnimater = new ExplosionWithInput(new ExplosionAnimation(Speed, 2));
    }

    public void Clear()
    {
        _selectedObject = null;
        OutAnimater.Animate(_expressionsOnScene.Keys.Select((x) => {
            x.transform.parent = null;
            return x.transform; 
            }), (e) => Destroy(e.gameObject));
        _expressionsOnScene.Clear();
    }

    public void CreateGroup(IEnumerable<IExpression> expressions, ILayouter layouter)
    {
        var parent = GetGroupParent();
        parent.position = StartPoint;
        var layout = layouter.GetLayout(expressions.Count());

        foreach (var expression in expressions)
        {
            var go = Instantiate(ExpressionPrefab, parent);
            go.name = string.Format("{0} = {1}", expression.ToString(), expression.Match());
 
            go.transform.localPosition = layout.Dequeue();
            go.transform.Find("Label").GetComponent<TextMeshPro>().text = expression.ToString();

            go.GetComponent<EventTrigger>().triggers.Add(OnInterectionEnrty);

            _expressionsOnScene.Add(go, expression);     
        }

        InAnimater.Animate(parent);
    }

    private Transform GetGroupParent()
    {
        foreach(var parent in GameObject.FindObjectsOfType<GameObject>().Where((x) => x.name == "Group Parent"))
        {
            if (parent.transform.childCount == 0) return parent.transform;
        }
        return new GameObject("Group Parent").transform;
    }

    private void ExpressionInterectionHandler(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData)
        {
            var pointerData = (PointerEventData)baseEventData;

            if (_expressionsOnScene.ContainsKey(pointerData.pointerPress))
            {
                _selectedObject = pointerData.pointerPress;
            }
        }
    }
   
    IExpression IExpressionGroupController.Update()
    {
        InAnimater.Update();
        OutAnimater.Update();
        return _selectedObject ? _expressionsOnScene[_selectedObject] : null;   
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(StartPoint, 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(MiddlePoint, 1);
    }

 }


#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(AnimatedGroupController)), UnityEditor.CanEditMultipleObjects]
public class AnimatedGroupControllerEditor : UnityEditor.Editor
{
    protected virtual void OnSceneGUI()
    {
        AnimatedGroupController group = (AnimatedGroupController)target;

        UnityEditor.EditorGUI.BeginChangeCheck();
        Vector3 newStartPosition = UnityEditor.Handles.PositionHandle(group.StartPoint, Quaternion.identity);
        Vector3 newMidlePosition = UnityEditor.Handles.PositionHandle(group.MiddlePoint, Quaternion.identity);
        if (UnityEditor.EditorGUI.EndChangeCheck())
        {
            UnityEditor.Undo.RecordObject(group, "Change points");
            group.StartPoint = newStartPosition;
            group.MiddlePoint = newMidlePosition;
        }
    }
}
#endif