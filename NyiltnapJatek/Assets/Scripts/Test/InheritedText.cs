using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InheritedText : Text
{
    [HideInInspector] public LocalizationString localizationString = default;

    // Start is called before the first frame update
    protected override void Start()
    {
        RefreshText();
    }

    public void RefreshText()
    {
        m_Text = localizationString.current;
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects()]
[CustomEditor(typeof(InheritedText))]
public class InheritedTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

#warning Change this when adjusting the language list
        EditorGUILayout.PropertyField(serializedObject.FindProperty("localizationString").FindPropertyRelative("texts").GetArrayElementAtIndex(0), new GUIContent("Hungarian"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("localizationString").FindPropertyRelative("texts").GetArrayElementAtIndex(1), new GUIContent("English"));
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif