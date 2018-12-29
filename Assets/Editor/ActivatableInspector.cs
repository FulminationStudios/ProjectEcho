using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Activatable), true)]
public class ActivatableInspector : Editor {
    bool showPowerSources = true;
    List<bool> showSources = new List<bool>();
    public override void OnInspectorGUI() {
        Activatable subject = (Activatable)target;
        showPowerSources = EditorGUILayout.Foldout(showPowerSources, "Power Sources");
        if (showPowerSources) {
            int index = 0;
            GUIStyle indented = new GUIStyle(GUI.skin.label);
            indented.padding.left = 10;
            foreach (PowerSources source in subject.powerSources) {
                EditorGUILayout.BeginVertical(indented);
                showSources.Add(true);
                showSources[index] = EditorGUILayout.Foldout(showSources[index], "Source " + index);
                if (showSources[index]) {
                    Object this_source = source.interactable;
                    source.interactable = (Interactable)EditorGUILayout.ObjectField(this_source, typeof(Interactable));
                    source.isInverse = EditorGUILayout.ToggleLeft("Is Inverse", source.isInverse);
                    EditorGUILayout.LabelField("Output: " + source.output);
                    if (GUILayout.Button("Remove Source")) {
                        if (source.interactable != null) {
                            source.interactable.activatableObjects.Remove(subject);
                        }
                        subject.powerSources.Remove(source);
                        EditorUtility.SetDirty(target);
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
                index++;
            }
        }
        if (GUILayout.Button("Add Source")) {
            subject.powerSources.Add(new PowerSources(null,false));
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        if (GUILayout.Button("Set Up Sources")) {
            SetUpSources(subject, target);
        }

        base.OnInspectorGUI();

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            SetUpSources(subject, target);
        }
    }

    void SetUpSources(Activatable subject, Object target) {
        foreach (PowerSources source in subject.powerSources) {
            //Debug.Log("ITERATING!");
            if (source.interactable != null) {
                if (!source.interactable.activatableObjects.Contains(subject)) {
                    source.interactable.activatableObjects.Add(subject);
                    EditorUtility.SetDirty(source.interactable);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
        }
        EditorUtility.SetDirty(target);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
