using UnityEngine;
using System.Collections;

public class BaseItem : BaseModel {
    private string name;
    private string iconPath;

    public BaseItem() { }

    public BaseItem(int id, string name, string iconPath) {
        this.Id = id;
        this.name = name;
        this.iconPath = iconPath;
    }

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public string IconPath {
        get { return iconPath; }
        set { iconPath = value; }
    }
}
