using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {

    bool showDataVariables = true;
    public override void OnInspectorGUI() {
        GameManager subject = (GameManager)target;
        GUIStyle indented = new GUIStyle(GUI.skin.label);
        indented.padding.left = 10;
        showDataVariables = EditorGUILayout.Foldout(showDataVariables, "Game Data");
        if (showDataVariables) {
            foreach (System.Reflection.FieldInfo field in typeof(GameData).GetFields()) {
                EditorGUILayout.LabelField(field.Name + ": " + field.GetValue(null), indented);
            }
        }

        base.OnInspectorGUI();
    }

}
