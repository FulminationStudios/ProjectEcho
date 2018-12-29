using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Scaleable),true)]
public class ScaleableInspector : ActivatableInspector {

    public override void OnInspectorGUI() {
        Scaleable subject = (Scaleable)target;
        EditorGUI.BeginChangeCheck();
        subject.dimensions = EditorGUILayout.Vector2Field("Dimensions",subject.dimensions);
        if (EditorGUI.EndChangeCheck()) {
            subject.UpdateDimensions(subject.dimensions);
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        base.OnInspectorGUI();
    }

}
