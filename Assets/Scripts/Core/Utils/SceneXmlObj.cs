using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneXmlObj{
    private string sceneName;
    private List<XmlObj> objs;

    public string SceneName {
        get { return sceneName; }
        set { sceneName = value; }
    }

    public List<XmlObj> Objs
    {
        get { return objs; }
        set { objs = value; }
    }
}
public class XmlObj
{
    public string name;
    public string objPath;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}