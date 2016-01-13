using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePackage : BaseModel {
    private int count;
    public int Count {
        get { return count; }
        set { count = value; }
    }

    private int goodId;
    public int GoodId {
        get { return goodId; }
        set { goodId = value; }
    }

    public BaseItem item;

    public BasePackage() { }

    public BasePackage(int id, int count, int goodId)
    {
        this.id = id;
        this.count = count;
        this.goodId = goodId;
    }

    public BasePackage(int id)
    {
        this.id = id;
        this.count = 0;
        this.goodId = -1;
    }
}
