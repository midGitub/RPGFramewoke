using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseManager<T> where T:BaseModel{

	private List<T> items;

    public List<T> Items {
        get { return items; }
    }

    public BaseManager()
    {
        items = new List<T>();
    }

    public void AddModel(T item) {
        if (item == null) {
            Debug.LogError("item is null!");
            return;
        }
        items.Add(item);
    }

    public T FindById(int id) {
        T t=items.Find(x => x.Id == id);
        if (t != null)
            return t;
        return default(T);
    }
}
