using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : BaseManager<BaseItem>{
    private ItemManager() :base() {
        AddModel(new BaseItem(1, "aa", "Female"));
        AddModel(new BaseItem(2, "bb", "Man"));
    }

    private static ItemManager instanse;

    public static ItemManager getInstance()
    {
        if (instanse == null)
            instanse = new ItemManager();
        return instanse;
    }
}
