using UnityEngine;
using System.Collections;

public abstract class BaseMediator
{
    public enum eBaseMediatorState
    {
        Open,
        Close,
        Loading,
        NotLoad
    }
    protected string panelName;
    protected string path;
    protected Transform transform;
    protected bool isFirstOpen=true;
    protected bool isOpen;
    protected eBaseMediatorState state = eBaseMediatorState.NotLoad;

    public virtual void LoadPanel() {
        if (string.IsNullOrEmpty(panelName) || string.IsNullOrEmpty(path))
            return;
        AssetBundleService.getInstance().LoadAsset(path, panelName, LoadPanelComplete);
    }
    
    public void LoadPanelComplete(string name, UnityEngine.Object asset) {
        GameObject o = (GameObject)UnityEngine.Object.Instantiate(asset);
        o.name = name;
        transform = o.transform;
        UIManager.getInstance().AddPanel(o);
        RectTransform rect = o.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        o.SetActive(false);
        if (isOpen)
            DirectOpen();
        else
            state = eBaseMediatorState.Close;
    }

    public virtual void Open() {
        if (isOpen) {
            UpdatePanel();
            return;
        } 
        isOpen = true;
        if (state == eBaseMediatorState.NotLoad)
            LoadPanel();
        else
            DirectOpen();
    }

    public virtual void Close() {
        if (!isOpen)
            return;
        isOpen = false;
        state = eBaseMediatorState.Close;
        transform.gameObject.SetActive(false);
    }

    //第一次打开
    protected virtual void FirstOpen() { 
    
    }

    //添加事件
    protected virtual void AddEvent()
    {

    }

    //更新面板
    protected virtual void UpdatePanel() { 
    
    }

    private void DirectOpen() {
        state = eBaseMediatorState.Open;
        transform.gameObject.SetActive(true);
        if (isFirstOpen) {
            isFirstOpen = false;
            AddEvent();
            FirstOpen();
        }
        UpdatePanel();
    }

    public string PanelName {
        get { return panelName; }
        set { panelName = value; }
    }

    public eBaseMediatorState State {
        get { return state; }
    }

    public string Path {
        get { return path; }
        set { path = value; }
    }
}
