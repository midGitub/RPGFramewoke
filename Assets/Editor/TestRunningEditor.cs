using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class TestRunningEditor
{
    static TestRunningEditor()
    {
        Debug.Log("Up and running");
        //EditorApplication.update += Update;
    }

    static void Update()
    {
        Debug.Log("Updating");
    }
}