using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : SingletonObject<UIManager>
{
    private GameObject uiRoot;
    private Camera mCamera;
    private GameObject canvasObj;
    //default resolution
    private Vector2 defaultResolution = new Vector2(750, 1334);
    private Dictionary<string, BaseMediator> mediators = new Dictionary<string, BaseMediator>();


    public void Init() {
        uiRoot = new GameObject("UIRoot");
        uiRoot.layer = LayerMask.NameToLayer("UI");
        GameObject.DontDestroyOnLoad(uiRoot);
        mCamera=uiRoot.AddComponent<Camera>();
        mCamera.clearFlags = CameraClearFlags.Depth;
        mCamera.cullingMask = LayerMask.GetMask("UI");
        //Canvas
        canvasObj = new GameObject("Canvas");
        canvasObj.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.pixelPerfect = true;
        canvas.worldCamera = mCamera;      
        canvasObj.transform.SetParent(uiRoot.transform);
        //CanvasScaler
        CanvasScaler scaler= canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;       
        scaler.referenceResolution = defaultResolution;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        //GraphicRaycaster
        canvasObj.AddComponent<GraphicRaycaster>();

        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
        eventSystem.AddComponent<TouchInputModule>();
        eventSystem.transform.parent = uiRoot.transform;
    }

    public void AddMediator(BaseMediator baseMediator)
    {
        string name = baseMediator.PanelName;
        mediators.Add(name, baseMediator);
    }

    public void AddPanel(GameObject go) {
        go.transform.SetParent(canvasObj.transform);
    }

    public void OnDestroy()
    {
        mediators.Clear();
        uiRoot = null;
    }
}
