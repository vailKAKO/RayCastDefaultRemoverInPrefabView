using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.SceneManagement.PrefabStageUtility;

public static class RaycastDefaultRemoverInPrefabView
{
    private static List<Graphic> _tempPrefabGraphics = new List<Graphic>();

    private static PrefabStage _recentPrefab;
    private static GameObject _instance;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        if (Application.isPlaying) return;
        var tmpCurrentStageId = GetCurrentPrefabStage();
        if (tmpCurrentStageId == null) return;


        if (_recentPrefab == null || _recentPrefab != tmpCurrentStageId || _instance == null)
        {
            _recentPrefab = tmpCurrentStageId;
            _instance = tmpCurrentStageId.prefabContentsRoot;
            Debug.Log("this may new or different from current opened prefab. load next.");
            AddGraphicComponent();
            return;
        }

        var currentGraphic = _instance.GetComponentsInChildren<Graphic>();

        foreach (var tmp in currentGraphic)
        {
            if (_tempPrefabGraphics.Contains(tmp)) continue;
            Debug.Log($"{tmp.name} is new Object");
            tmp.raycastTarget = tmp.GetComponent<IEventSystemHandler>() != null;
        }

        AddGraphicComponent();
    }

    private static void AddGraphicComponent()
    {
        var tmpList = _instance.GetComponentsInChildren<Graphic>();
        _tempPrefabGraphics = new List<Graphic>();
        foreach (var content in tmpList)
        {
            _tempPrefabGraphics.Add(content);
        }
    }
}
