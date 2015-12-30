using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestMediator : BaseMediator {
    private GameObject updateResBtn, updateScriptBtn, loadConfigBtn, testPoolBtn,startBtn;
    private GameObject infoText;

    protected override void AddEvent()
    {
        updateResBtn=transform.Find("Btn1").gameObject;
        EventTriggerListener.Get(updateResBtn).PointerClick += HotUpdateResource;
        updateScriptBtn = transform.Find("Btn2").gameObject;
        EventTriggerListener.Get(updateScriptBtn).PointerClick += HotUpdateScript;
        loadConfigBtn = transform.Find("Btn3").gameObject;
        EventTriggerListener.Get(loadConfigBtn).PointerClick += LoadTxt;
        testPoolBtn = transform.Find("Btn4").gameObject;
        EventTriggerListener.Get(testPoolBtn).PointerClick += TestObjectPoll;
        infoText = transform.Find("Text").gameObject;
        startBtn = transform.Find("Start").gameObject;
        EventTriggerListener.Get(startBtn).PointerClick += StartLogin;
    }

    void HotUpdateResource(PointerEventData eventData)
    {
        AssetBundleService.getInstance().LoadAsset("prefabs/object/cube.unity3d", "cube", testPrefabs);
    }

    void HotUpdateScript(PointerEventData eventData)
    {
        object[] results = LuaManager.getInstance().CallMethod("GameManager", "hello");
        infoText.GetComponent<Text>().text = results[0].ToString();
    }

    void LoadTxt(PointerEventData eventData)
    {
        CSkillLoader skillLoader = CStaticDownload<CSkillLoader>.getInstance().GetStaticInfo(100);
        GLog.Log(skillLoader.GetSkillName());
    }

    private int index = 0;

    void TestObjectPoll(PointerEventData eventData)
    {
        index++;
        if (index > 5)
        {
            GameObject sphere = ObjectPool.instance.GetObjByName("Prefabs/Sphere");
            int range3 = Random.Range(-1, 1);
            sphere.transform.position = new Vector3(range3, range3, range3);
        }
        if (index < 5 || index > 10)
        {
            GameObject capsule = ObjectPool.instance.GetObjByName("Prefabs/Capsule");
            int range1 = Random.Range(-2, 0);
            capsule.transform.position = new Vector3(range1, range1, range1);
        }

        GameObject cube = ObjectPool.instance.GetObjByName("Prefabs/Cube");
        int range2 = Random.Range(0, 2);
        cube.transform.position = new Vector3(range2, range2, range2);
    }

    void StartLogin(PointerEventData eventData) {
        SingletonObject<TestMediator>.getInstance().Close();
        SingletonObject<LoginMediator>.getInstance().Open();
        ObjectPool.instance.clearPool();
    }

    private void testPrefabs(string assetName, UnityEngine.Object obj)
    {
        GameObject go =GameObject.Instantiate(obj as GameObject);
        go.transform.position = Vector3.zero;
    }
}
