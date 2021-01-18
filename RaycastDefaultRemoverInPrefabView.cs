// Copyright (c) 2021 Hiroyuki Kako
// This software is released under the MIT License, see LICENSE.

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.SceneManagement.PrefabStageUtility;

public class RaycastDefaultRemoverInPrefabView : EditorWindow
{
    private static List<Graphic> _tempPrefabGraphics = new List<Graphic>();

    private static PrefabStage _recentPrefab;
    private static GameObject _instance;
    private static bool _isActive;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }


    [MenuItem("KEditorExtensions/RaycastDefaultRemoverInPrefabView")]
    private static void Open()
    {
        GetWindow<RaycastDefaultRemoverInPrefabView>("Toggle");
    }

    private void OnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            _isActive = EditorGUILayout.Toggle("Use RaycastDefaultRemoverInPrefabView", _isActive);
        }
    }

    private static void OnHierarchyChanged()
    {
        if (!_isActive) return;
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
#endif
