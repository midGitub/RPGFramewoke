using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackageManager : BaseManager<BasePackage>{
    private PackageManager() : base() {
        AddModel(new BasePackage(1,2,2));
        AddModel(new BasePackage(2, 5,3));
        AddModel(new BasePackage(3));
        AddModel(new BasePackage(4));
    }

    private static PackageManager instanse;

    public static PackageManager getInstance() {
        if (instanse == null)
            instanse = new PackageManager();
        return instanse;
    }
}
