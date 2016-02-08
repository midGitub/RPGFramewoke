using UnityEngine;
using UnityEditor;
using System.Collections;

public class CustomWindow : EditorWindow {
    private string content;
    public string Content {
        set { content = value; }
    }

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/CustomWindow")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomWindow));
    }

    void OnGUI()
    {
        if (GUILayout.Button("OK", GUILayout.Width(100), GUILayout.Height(30)))
        {
            Close();
        }
        ShowNotification(new GUIContent(content));
    }
}
