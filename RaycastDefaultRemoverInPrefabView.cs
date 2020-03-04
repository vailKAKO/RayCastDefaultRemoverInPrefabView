using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class RaycastDefaultRemoverInPrefabView
{
    private static GameObject _tempPrefab;
    private static List<Graphic> _tempPrefabGraphics = new List<Graphic>();

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        if (Application.isPlaying) return;

        if (PrefabStageUtility.GetCurrentPrefabStage() == null) return;

        var instance = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot;

        Debug.Log($"{_tempPrefabGraphics.Count.ToString()} graphics existed");
        Debug.Log($"Object {instance.name} is loaded");

        if (_tempPrefabGraphics.Count != 0 || (_tempPrefab == null || instance.name != _tempPrefab.name))
        {
            var currentGraphic = instance.GetComponentsInChildren<Graphic>();

            foreach (var tmp in currentGraphic)
            {
                if (_tempPrefabGraphics.Contains(tmp)) continue;
                Debug.Log($"{tmp.name} is new Object");
                tmp.raycastTarget = tmp.GetComponent<IEventSystemHandler>() != null;
            }
        }
        else
        {
            Debug.Log("this may different prefab from recent. load next.");
        }

        _tempPrefab = instance;

        var tmpList = _tempPrefab.GetComponentsInChildren<Graphic>();
        _tempPrefabGraphics = new List<Graphic>();
        foreach (var content in tmpList)
        {
            _tempPrefabGraphics.Add(content);
        }
    }
}
