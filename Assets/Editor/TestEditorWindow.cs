using UnityEditor;
using System.Collections;
using UnityEngine;

public class TestEditorWindow : EditorWindow {
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    GameObject[] selectedObjects;
    GameObject useDefinedObject;

    [MenuItem("[EditorWindow]/Test Window")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(TestEditorWindow));
    }

    void OnGUI() {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field",myString);
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings",groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle",myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

        GUILayout.Space(30);
        GUILayout.Label("Objects",EditorStyles.boldLabel);


        EditorGUILayout.BeginVertical();
        selectedObjects=Selection.gameObjects;
        useDefinedObject=(GameObject)EditorGUILayout.ObjectField("User Defined Object:", useDefinedObject, typeof(GameObject), true);
        if (selectedObjects.Length > 0 && useDefinedObject) {
            if(GUILayout.Button("Replace Objects", GUILayout.Height(40),GUILayout.Width(120))){
                ReplaceObj();
            }
        }
        EditorGUILayout.LabelField("Selected Object Counts:"+selectedObjects.Length);
        EditorGUILayout.EndVertical();
        Repaint();
    }

    void ReplaceObj() {
        for (int i = 0; i < selectedObjects.Length; i++) {
            if (selectedObjects[i] != null) {
                GameObject.Instantiate(useDefinedObject, selectedObjects[i].transform.position, selectedObjects[i].transform.rotation);
                DestroyImmediate(selectedObjects[i]);//edit mode must use this method      
                //Destroy(selectedObjects[i]); 
            }
        }
    }
}
