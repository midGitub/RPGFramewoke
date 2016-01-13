using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackController : MonoBehaviour {
    private ItemManager itemManager;
    private PackageManager packManager;
    public PackView packView;

    void Awake() {
        itemManager=ItemManager.getInstance();
        packManager = PackageManager.getInstance();
    }

    public void ShowPack() {
        List<BasePackage> packs=packManager.Items;
        foreach(BasePackage pack in packs){
            pack.item = itemManager.FindById(pack.GoodId);
        }
        packView.RenderViewToModel(packs);
    }
}
